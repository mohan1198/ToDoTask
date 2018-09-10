using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Models;
using Task = ToDoList.Models.Task;


namespace ToDoList.Controllers
{


    [Route("api/ToDoList")]
    public class ToDoController : Controller
    {
        ToDoContext context;
        Output output;
        public ToDoController()
        {
            //create an instance of the ToDoContext for accessing the database.
            context = new ToDoContext();
            //stores the output of the apis.
            output = new Output();
        }

        // api for adding new task for the existing user
        [HttpPost("AddTask")]
        [Authorize]
        public IActionResult AddTask([FromBody]AddTaskDto addTask)
        {
            if (addTask == null)
            {
                output.status = "Error";
                output.description = "send a json file";
            }
            else
            {
                Task task = new Task
                {
                    Title = addTask.title,
                    SubTitle = addTask.subTitle,
                    Status = addTask.status
                };
                try
                {
                    var user = context.users.Where(e => e.GuserId == addTask.guserId).FirstOrDefault();

                    if (user != null)
                    {
                        task.GuserId = addTask.guserId;
                        task.UserId = user.UserId;
                    }
                    else
                    {
                        output.status = "Error";
                        output.description = "the user for which you are entering task does not exists";
                    }
                    context.tasks.Add(task);
                    context.SaveChanges();
                    output.status = "success";
                    output.description = "task added successfully";
                }
                catch (Exception e)
                {
                    output.status = "Error";
                    output.description = e.Message;
                }
            }
            return new JsonResult(output);
        }

        //api for deleting task
        [HttpDelete("DeleteTask")]
        [Authorize]
        public IActionResult DeleteTask([FromBody]DeleteTask deleteTask)
        {
            if (deleteTask == null)
            {
                output.status = "Error";
                output.description = "send a json file";
            }
            else
            {
                try
                {
                    var delete = context.tasks.Where(e => 
                            e.GuserId == deleteTask.guserId && e.TaskId == deleteTask.taskId && !e.Status).FirstOrDefault();
                    if (delete == null)
                    {
                        output.status = "Error";
                        output.description = "no task to delete";
                    }
                    else
                    {
                        //this line deletes the row from the database
                        //context.tasks.Remove(del);
                        //status is updated to indicate that the task is completed
                        delete.Status = true;
                        context.SaveChanges();
                        output.status = "success";
                        output.description = "task deleted successfully";
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


        [HttpPatch("Edit")]
        [Authorize]
        public IActionResult EditTask([FromBody]EditText editTask)
        {
            if (editTask == null)
            {
                output.status = "Error";
                output.description = "send a json file";
            }
            else
            {
                try
                {
                    Task edit = context.tasks.Where(e => 
                         e.GuserId == editTask.guserId && e.TaskId == editTask.taskId && !e.Status).FirstOrDefault();
                    if (edit == null)
                        return new JsonResult(new List<object>()
                {
                        new { Error="no task to edit", Description="task is either completed or does not exists"}
                });
                    edit.Title = editTask.title;
                    edit.SubTitle = editTask.subTitle;
                    context.SaveChanges();
                    output.status = "success";
                    output.description = "task edited successfully";
                }
                catch (Exception e)
                {
                    output.status = "Error";
                    output.description = e.Message;
                }
            }
            return new JsonResult(output);
        }



        [HttpGet("DisplayTask/{UserId}")]
        [Authorize]
        public IActionResult DisplayTask(Guid userId)
        {
           try
            {
                var userTasks = context.tasks.Where(m => m.GuserId == userId && !m.Status).ToList();
                if (userTasks == null || userTasks.Count() == 0)
                {
                    output.status = "error";
                    output.description = "user has no task";
                }
                else
                {
                    return new JsonResult(userTasks);
                }
            }
            catch (Exception e)
            {
                output.status = "Error";
                output.description = e.Message;
            }
            return new JsonResult(output);
        }
    }
}
