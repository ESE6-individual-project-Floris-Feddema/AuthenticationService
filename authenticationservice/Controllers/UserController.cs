using System;
using System.Threading.Tasks;
using authenticationservice.Helpers;
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
        
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] UserRegisterView view)
        {
            try
            {
                var user = await _service.Insert(view.Name, view.Email, view.Password);
                return Ok(user.ToPrivateDto());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("google")]
        public async Task<IActionResult> InsertGoogle([FromBody] GoogleUserView view)
        {
            try
            {
                var user = await _service.InsertGoogle(view.TokenId);
                return Ok(user.ToPrivateDto());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginView view)
        {
            try
            {
                var user = await _service.Login(view.Email, view.Password);
                return Ok(user.ToPrivateDto());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login/google")]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleUserView view)
        {
            try
            {
                var user = await _service.LoginGoogle(view.TokenId);
                return Ok(user.ToPrivateDto());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountView account)
        {
            try
            {
                var result = await _service.Update(id, account.Name);
                return Ok(result.ToPrivateDto());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public async Task<IActionResult> GetUser(string email)
        {
            try
            {
                var result = await _service.Get(email);
                return Ok(result.ToPublicDto());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}