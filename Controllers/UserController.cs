using Microsoft.AspNetCore.Mvc;
using tasks.Models;
using tasks.Interfaces;

namespace tasks.Controllers;


[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{


    IUserService UsersService;
    public UsersController(IUserService UsersService)
    {
        this.UsersService = UsersService;
    }

    [HttpGet]
    public ActionResult<List<User>> Get()
    {
        return UsersService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var user = UsersService.GetById(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    public ActionResult Post(User newUser)
    {
        var newId = UsersService.Add(newUser);

        return CreatedAtAction("Post", 
            new {id = newId}, UsersService.GetById(newId));
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id,User newUser)
    {
        var result = UsersService.Update(id, newUser);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var result = UsersService.Delete(id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }
}