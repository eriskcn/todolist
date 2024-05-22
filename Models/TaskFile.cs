using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using todolist.Models;

namespace todolist.Models
{
    [Table("TaskFiles")]
    public class TaskFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileID { get; set; }

        [Required]
        [ForeignKey("TaskItem")]
        public int TaskID { get; set; }
        public TaskItem TaskItem { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
