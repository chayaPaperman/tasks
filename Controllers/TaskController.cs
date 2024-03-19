using Microsoft.AspNetCore.Mvc;
using tasks.Models;
using tasks.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace tasks.Controllers;


[ApiController]
[Route("[controller]")]
[Authorize(Policy = "User")]
public class TasksController : ControllerBase
{
    ITasksService TasksService;
    public TasksController(ITasksService TasksService)
    {
        this.TasksService = TasksService;
    }

    [HttpGet]
    public ActionResult<List<Task1>> Get()
    {
        return TasksService.GetAll(int.Parse(User.FindFirst("id")?.Value!));
    }

    [HttpGet("{id}")]
    public ActionResult<Task1> Get(int id)
    {
        var task = TasksService.GetById(int.Parse(User.FindFirst("id")?.Value!),id);
        if (task == null)
            return NotFound();
        return task;
    }

    [HttpPost]
    public ActionResult Post(Task1 newTask)
    {
        newTask.UserId=int.Parse(User.FindFirst("id")?.Value!);
        var newId = TasksService.Add(newTask);

        return CreatedAtAction("Post", 
            new {id = newId}, TasksService.GetById(int.Parse(User.FindFirst("id")?.Value!),newId));
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id,Task1 newTask)
    {
        newTask.UserId=int.Parse(User.FindFirst("id")?.Value!);
        var result = TasksService.Update(id, newTask);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var result = TasksService.Delete(int.Parse(User.FindFirst("id")?.Value!),id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }
}
