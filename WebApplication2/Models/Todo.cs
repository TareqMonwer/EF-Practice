using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CompletedAt { get; set; }
    }
}
