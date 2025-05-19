using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner_API.Models;
using static WorkoutPlanner_API.Data.DataStore;

namespace WorkoutPlanner_API.Controllers;

/// <summary>
/// Basic CRUD operations for User resources
/// </summary>
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    [HttpGet]
    public IEnumerable<User> GetAll()
    {
        return Users;
    }


    [HttpGet("{id:int}")]
    public ActionResult<User> Get(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            return Ok(user);
        }

        return NotFound();
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <remarks>
    /// The email must be unique but this demo does not validate it.
    /// </remarks>
    [HttpPost]
    public ActionResult<User> Create(User user)
    {
        user.Id = Users.Count + 1;
        Users.Add(user);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, User user)
    {
        var initial = Users.FirstOrDefault(u => u.Id == id);

        if (initial == null)
        {
            return NotFound();
        }

        initial.Name = user.Name;
        initial.Email = user.Email;
        initial.PasswordHash = user.PasswordHash;

        return NoContent();
    }


    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        Users.Remove(user);

        return NoContent();
    }

}
