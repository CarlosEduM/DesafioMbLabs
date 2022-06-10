using DesafioMbLabs.Models;
using DesafioMbLabs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioMbLabs.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly ITokenService _tockenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tockenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Login(UserBase loginData)
        {
            var user = await _userService.GetUserAsync(loginData.Email, loginData.Password);

            if (user == null)
                return NotFound(new { message = "Invalid email or password" });

            var userTocken = _tockenService.GenerateTocken(user);

            return new
            {
                tocken = userTocken,
            };
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User>> GetUser()
        {
            var user = await _userService.GetUserAsync(HttpContext.User.Identity.Name);

            if (user == null)
                return NotFound();

            user.Password = "";

            return user;
        }

        [HttpGet("ticket")]
        [Authorize]
        public async Task<ActionResult<List<Ticket>>> GetTickets()
        {
            var user = await _userService.GetUserAsync(HttpContext.User.Identity.Name);

            if (user == null)
                return NotFound();

            return user.Tickets;
        }

        [HttpGet("ticket/{id}")]
        [Authorize]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var user = await _userService.GetUserAsync(HttpContext.User.Identity.Name);

            if (user == null)
                return NotFound();

            Ticket ticket = user.Tickets.FirstOrDefault(t => t.Id == id);

            if (ticket == null)
                return NotFound();

            return ticket;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> NewUser(User user)
        {
            if (null != await _userService.GetUserAsync(user.Email))
                return BadRequest(new { message = "Username already exists" });

            await _userService.CreateUserAsync(user);

            return CreatedAtAction(nameof(NewUser), new { id = user.Id });
        }

        [HttpPost("eventManager")]
        [AllowAnonymous]
        public async Task<IActionResult> NewEventManager(EventManager user)
        {
            if (null != await _userService.GetUserAsync(user.Email))
                return BadRequest(new { message = "Username already exists" });

            await _userService.CreateUserAsync(user);

            return CreatedAtAction(nameof(NewUser), new { id = user.Id });
        }


        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Update(JsonPatchDocument<User> newUser)
        {
            var user = await _userService.GetUserAsync(HttpContext.User.Identity.Name);

            if (user == null)
                NotFound();

            newUser.ApplyTo(user, ModelState);

            foreach (var ops in newUser.Operations)
            {
                if (ops.path == "/tickets"
                    || ops.path.Contains("/transactions")
                    || ops.path == "/rule"
                    || ops.path == "/events")
                    return BadRequest(new { message = $"{ops.path} is not changeble for this path" });
            }

            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    message = "Some update invalid"
                });

            await _userService.UpdateUserAsync(user);

            user.Password = "";

            return new ObjectResult(user);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            User user = await _userService.GetUserAsync(HttpContext.User.Identity.Name);

            if (user == null)
                return NotFound();

            await _userService.DeleteUserAsync(user);

            return NoContent();
        }
    }
}
