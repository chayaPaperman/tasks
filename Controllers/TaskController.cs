using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tasks.Interfaces;
using tasks.Models;

namespace tasks.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "User")]
public class TasksController : ControllerBase
{
    private int userId;
    ITasksService TasksService;

    public TasksController(ITasksService TasksService, IHttpContextAccessor httpContextAccessor)
    {
        this.TasksService = TasksService;
        string id = httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
        this.userId = int.Parse(id != null ? id : "0");
    }

    [HttpGet]
    public ActionResult<List<Task1>> Get()
    {
        return TasksService.GetAll(this.userId);
    }

    [HttpGet("{id}")]
    public ActionResult<Task1> Get(int id)
    {
        var task = TasksService.GetById(this.userId, id);
        if (task == null)
            return NotFound();
        return task;
    }

    [HttpPost]
    public ActionResult Post(Task1 newTask)
    {
        newTask.UserId = this.userId;
        var newId = TasksService.Add(newTask);

        return CreatedAtAction(
            "Post",
            new { id = newId },
            TasksService.GetById(this.userId, newId)
        );
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Task1 newTask)
    {
        newTask.UserId = this.userId;
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
        var result = TasksService.Delete(this.userId, id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }
}
