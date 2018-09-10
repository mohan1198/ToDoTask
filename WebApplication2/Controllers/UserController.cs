using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Route("api/ToDoList")]
    public class UserController : Controller
    {
        private ToDoContext context;
        private Output output;

        public UserController()
        {
            //create an instance of the ToDoContext for accessing the database.
            context = new ToDoContext();
            //stores the output of the apis.
            output = new Output();
        }

        [HttpPost("Register")]
        public IActionResult RegisterUser([FromBody]RegisterDto register)
        {
            //validates if the json file is sent 
            if (register == null)
            {
                output.status = "Failure";
                output.description = "Send a json file";
            }
            else
            {
                try
                {
                    if (Regex.Match(input: register.email, pattern: @".+@[A-Za-z]+[.][A-Za-z]+").Success)
                    {
                        if (Regex.Match(input: register.password, pattern: ".{8,}").Success)
                        {
                            var enc = new Encrypt();
                            var password = enc.Encryption(register.password);
                            var key = enc.GetKey();
                            var IV = enc.GetIV();
                            User user = new User
                            {
                                Email = register.email,
                                Password = password,
                                GuserId = Guid.NewGuid(),
                                Key = key,
                                IV = IV,
                                Date = DateTime.Now
                            };
                            context.users.Add(user);
                            context.SaveChanges();
                            output.status = "success";
                            output.description = "user registered successfully";
                        }
                        else
                        {
                            output.status = "Error";
                            output.description = "Invalid password. Should be minimum 8 characters";
                        }

                    }
                    else
                    {
                        output.status = "Error";
                        output.description = "entered email id is not valid. Valid email type: aaa@gmail.com";
                    }

                }
                catch (Exception e)
                {
                    output.status = "Error";
                    output.description = e.Message;
                }
            }
            return new JsonResult(output);
        }

        [HttpPost("login")]
        public IActionResult ValidateUser([FromBody]LoginDto login)
        {
            if (login == null)
            {
                output.status = "Failure";
                output.description = "Send a json file";
            }
            else
            {
                try
                {
                    //compares the email id sent by the currnet user and the email id stored in the database
                    var user = context.users.Where(e => e.Email.Equals(login.email.Trim())).FirstOrDefault();
                    if (user == null)
                    {
                        output.status = "Error";
                        output.description = "user is not registered";
                    }
                    //encrypts the password sent by the current user for validation
                    Encrypt encrypt = new Encrypt();
                    var passwordCheck = encrypt.Encryption(login.password, user.Key, user.IV);
                    var id = from e in context.users
                         where e.Email.Equals(login.email.Trim()) && e.Password.Equals(passwordCheck)
                         select e.UserId;
                    if (id == null || id.Count() == 0)
                    {
                        output.status = "error";
                        output.description = "invalid password";
                    }   
                    else
                    {
                        var token = TokenController.GenerateToken(login.email);
                        return new JsonResult(new { AccessToken = token, UserId = user.GuserId });
                    }
                }
                catch (Exception e)
                {
                    output.status = "error";
                    output.description = e.Message;
                }
            }
            return new JsonResult(output);
        }
    }
}
