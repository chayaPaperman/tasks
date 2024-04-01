using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tasks.Models;
using tasks.Interfaces;
using tasks.Services;

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
    [Authorize(Policy = "Admin")]
    public ActionResult<List<User>> Get()
    {
        return UsersService.GetAll();
    }

    [HttpGet]
    [Route("/user")]
    [Authorize(Policy = "User")]
    public ActionResult<User> getUser()
    {
        var id=int.Parse(User.FindFirst("id")?.Value!);
        var user = UsersService.GetById(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult Post(User newUser)
    {
        var newId = UsersService.Add(newUser);

        return CreatedAtAction("Post", 
            new {id = newId}, UsersService.GetById(newId));
    }

    [HttpPut]
    [Authorize(Policy = "User")]
    public ActionResult Put(User newUser)
    {
        var id=int.Parse(User.FindFirst("id")?.Value!);
        newUser.Id=id;
        var result = UsersService.Update(newUser);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public ActionResult Delete(int id)
    {
        var result = UsersService.Delete(id);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

        [HttpPost]
    [Route("/login")]
    public ActionResult<String> Login([FromBody] User u)
    {
        var dt = DateTime.Now;

        User user=UsersService.getUser(u.Name,u.Password);

        if(user==null){
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new Claim("id",user.Id.ToString()),
        };

        if(user.Password=="12345678"){
            claims.Add(new Claim("type", "Admin"));
        }
        else{
            claims.Add(new Claim("type", "User"));
        }

        var token = LoginService.GetToken(claims);

        return new OkObjectResult(LoginService.WriteToken(token));
    }

    [Route("/ifAdmin")]
    [HttpGet]
    [Authorize(Policy = "Admin")]
    public ActionResult<String> GetIfAdmin()
    {
        return new OkObjectResult("true");
    }
}