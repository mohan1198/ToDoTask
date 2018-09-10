using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class DeleteTask
    {
        public int taskId { get; set; }
        public Guid guserId { get; set; }
        
    }
}
