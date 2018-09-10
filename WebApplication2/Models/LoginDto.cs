using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class LoginDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public int userId { get; internal set; }
    }
}
