using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using todolist.Models; 

namespace todolist.Models
{
    [Table("Tasks")]
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskID { get; set; }

        [ForeignKey("UserID")]
        public int UserID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime DueDate { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }

}