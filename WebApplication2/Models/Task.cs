using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class Task
    {
       
        public int TaskId { get; set; }
        [MaxLength(100)]
        [Required]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        [Required]
        public Boolean Status { get; set; }
        public int UserId { get; set; }
        public User Users { get; set; }
        public Guid GuserId { get; set; }
        
    }
}
