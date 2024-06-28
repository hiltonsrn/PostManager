using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PostManager.API.DTO;
using PostManager.API.HUBs;
using PostManager.Domain.Model;
using PostManager.Infra.Data.Repository;
using PostManager.Service;
using PostManager.Service.Services;
using Swashbuckle.Swagger;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<LocalDBMSSQLLocalDBContext>(options =>
{

    var _connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(_connectionString, opt =>
    {
        opt.CommandTimeout(180);
        opt.EnableRetryOnFailure(5);
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API de gerenciamento de postagens", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
#region Injeção repositórios
builder.Services.AddScoped<IBaseRepository<Usuario>, BaseRepository<Usuario>>();
builder.Services.AddScoped<IBaseRepository<Post>, BaseRepository<Post>>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
#endregion

#region Injeção services
builder.Services.AddScoped<IBaseService<Usuario>, BaseService<Usuario>>();
builder.Services.AddScoped<IBaseService<Post>, BaseService<Post>>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPostService, PostService>();
#endregion

#region Mapeamentos
builder.Services.AddSingleton(new MapperConfiguration(config =>
{
    config.CreateMap<Usuario, UsuarioDTO>().ForMember(o => o.Posts, option => option.Ignore());
    config.CreateMap<PostDTO, Post>();

    config.CreateMap<UsuarioDTO, Usuario>().ForMember(o => o.Posts, option => option.Ignore());
    config.CreateMap<Post, PostDTO>();
}).CreateMapper());
#endregion
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,

         ValidIssuer = builder.Configuration["jwt:issuer"],
         ValidAudience = builder.Configuration["jwt:audience"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:secretkey"]))
     };
 }
);
builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                                .AllowAnyMethod()
                                                                                 .AllowAnyHeader()));

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "API de gerenciamento de postagens v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PostHub>("/notificacoes/{email}");
app.Run();

