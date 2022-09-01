using System.ComponentModel.DataAnnotations;

namespace PersonProcesses.Entities.Base
{
    public class BaseEntity
    {
        [Key]
        [Required]
        public Guid UUID { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
