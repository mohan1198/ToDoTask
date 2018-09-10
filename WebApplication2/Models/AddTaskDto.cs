using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class AddTaskDto
    {
        public string title { get; set; }
        public string subTitle { get; set; }
        public Boolean status { get; set; }
        public Guid guserId { get; set; }
    }
}
