using System.ComponentModel.DataAnnotations;

namespace PostManager.Domain.Model
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
