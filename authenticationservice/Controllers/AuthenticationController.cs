using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using authenticationservice.Helpers;
using authenticationservice.Services;
using authenticationservice.Views;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace authenticationservice.Controllers

{    [Route("api/v1/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            this._authService = authService;
        }
        
        [AllowAnonymous]
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody]UserView userView)
        {
            try
            {
                //SimpleLogger.Log("userView = " + userView.tokenId);
                var payload = GoogleJsonWebSignature.ValidateAsync(userView.TokenId, new GoogleJsonWebSignature.ValidationSettings()).Result;
                var user = await _authService.Authenticate(payload);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, Security.Encrypt(AppSettings.Settings.JwtEmailEncryption,user.Email)),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.Settings.JwtSecret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(string.Empty,
                    string.Empty,
                    claims,
                    expires: DateTime.Now.AddSeconds(55*60),
                    signingCredentials: creds);
                
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
            return BadRequest();
        }
    }
}