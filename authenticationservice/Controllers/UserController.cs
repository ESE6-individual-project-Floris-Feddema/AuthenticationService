using System;
using System.Threading.Tasks;
using authenticationservice.Services;
using authenticationservice.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authenticationservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.Get());
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] UserRegisterView view)
        {
            try
            {
                var user = await _service.Insert(view.Name, view.Email, view.Password);
                return Ok(new {user.Name, user.Email});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> InsertGoogle([FromBody] GoogleUserView view)
        {
            try
            {
                var user = await _service.InsertGoogle(view.TokenId);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginView view)
        {
            try
            {
                var user = await _service.Login(view.Email, view.Password);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("login/google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleUserView view)
        {
            try
            {
                var user = await _service.LoginGoogle(view.TokenId);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("fill")]
        public async Task<IActionResult> Fill()
        {
            await _service.Fill();
            return Ok();
        }

    }
}