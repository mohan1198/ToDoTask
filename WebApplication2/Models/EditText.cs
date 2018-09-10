using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class EditText
    {
        public int taskId { get; set; }
        public Guid guserId { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
    }
}
