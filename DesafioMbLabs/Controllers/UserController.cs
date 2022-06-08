using DesafioMbLabs.Models;
using DesafioMbLabs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace DesafioMbLabs.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Login(UserBase loginData)
        {
            var user = await _userService.GetUserAsync(loginData.Email, loginData.Password);

            if (user == null)
                return NotFound(new { message = "Invalid email or password" });

            var userTocken = TokenService.GenerateTocken(user, Encoding.ASCII.GetBytes(_configuration["JwtSecret"]));

            return new
            {
                tocken = userTocken,
            };
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User>> Get()
        {
            var user = await _userService.GetUserAsync(HttpContext.User.Identity.Name);

            if (user == null)
                return NotFound();

            user.Password = "";

            return user;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> NewUser(User user)
        {
            if (null != await _userService.GetUserAsync(user.Name))
                return BadRequest(new { message = "Username already exists" });

            await _userService.CreateUserAsync(user);

            return CreatedAtAction(nameof(NewUser), new { id = user.Id });
        }

        [HttpPost]
        [Route("eventManager")]
        [AllowAnonymous]
        public async Task<IActionResult> NewEventManager(EventManager user)
        {
            if (null != await _userService.GetUserAsync(user.Name))
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
                switch (ops.path)
                {
                    case "/tickets":
                        continue;
                    case "/transactions":
                        continue;
                    case "/rule":
                        continue;
                    case "/events":
                        continue;
                    default:
                        return BadRequest(new { message = $"{ops.path} is not changeble for this path" });
                }
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

            if (user != null)
                return NotFound();

            await _userService.DeleteUserAsync(user);

            return NoContent();
        }
    }
}
