﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace PostManager.API.DTO;

public partial class PostDTO
{
    public int Id { get; set; }

    public string Descricao { get; set; }

    public DateTime Data { get; set; }

    public int UsuarioId { get; set; }

    public virtual UsuarioDTO Usuario { get; set; }
}