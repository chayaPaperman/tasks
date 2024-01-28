using Microsoft.AspNetCore.Mvc;
using tasks.Models;
using tasks.Interfaces;

namespace tasks.Controllers;


[ApiController]
[Route("[controller]")]
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
        return TasksService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Task1> Get(int id)
    {
        var task = TasksService.GetById(id);
        if (task == null)
            return NotFound();
        return task;
    }

    [HttpPost]
    public ActionResult Post(Task1 newTask1)
    {
        var newId = TasksService.Add(newTask1);

        return CreatedAtAction("Post", 
            new {id = newId}, TasksService.GetById(newId));
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id,Task1 newTask)
    {
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
        var result = TasksService.Delete(id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }
}
