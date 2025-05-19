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
        foreach (var user in Users)
        {
            if (user.Id == id)
            {
                return Ok(user);
            }
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
        int idx = -1;
        for (int i = 0; i < Users.Count; i++)
        {
            if (Users[i].Id == id)
            {
                idx = i;
                break;
            }
        }

        if (idx == -1)
        {
            return NotFound();
        }

        user.Id = id;
        Users[idx] = user;
        return NoContent();
    }


    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        for (int i = 0; i < Users.Count; i++)
        {
            if (Users[i].Id == id)
            {
                Users.RemoveAt(i);
                return NoContent();
            }
        }

        return NotFound();
    }

}
