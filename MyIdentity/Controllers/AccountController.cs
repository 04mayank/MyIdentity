using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyIdentity.BusinessLayer;
using MyIdentity.Email;
using MyIdentity.EntityLayer;

namespace MyIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountLogic _businessLayer;
        private IEmailSender _emailsender;

        public AccountController(AccountLogic businessLayer, IEmailSender emailsender)
        {
            _businessLayer = businessLayer;
            _emailsender = emailsender;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var result = await _businessLayer.Register(model);
            if (result[0] == "Registered Successfully")
            {
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { UserId = model.Email, Code = result[1] }, protocol: HttpContext.Request.Scheme);

                await _emailsender.SendEmailAsync(model.Email, "MyIdentity.com - Confirm Your Email", "Please confirm your e-mail by clicking this link: <a href=\"" + callbackUrl + "\">click here</a>");

                return Ok(new
                {
                    username = model.UserName,
                    email = model.Email,
                    status = 1,
                    message = result[0]
                });
            }
            return BadRequest(new JsonResult(result));
        }


        //[HttpGet("api/token")]
        //[Authorize]
        //public IActionResult GetToken()
        //{
        //    return Ok(GenerateToken(User.Identity.Name));
        //}

        //private string GenerateToken(string userId)
        //{
        //    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["JwtKey"]));

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, userId)
        //    };

        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken("signalrdemo", "signalrdemo", claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


        //[HttpPost("[action]")]
        //public async Task<IActionResult> Login([FromBody] Login model)
        //{

        //    //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password,
        //    //model.RememberMe, false);
        //    var user = await userManager.FindByEmailAsync(model.Email);
        //    var roles = await userManager.GetRolesAsync(user);
        //    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret));
        //    double tokenExpiryTime = Convert.ToDouble(appSettings.ExpireTime);
        //    //if (result.Succeeded)
        //    //{
        //    //    return Ok();
        //    //}
        //    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(new Claim[]
        //            {
        //            new Claim(JwtRegisteredClaimNames.Sub, model.Email),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            new Claim(ClaimTypes.NameIdentifier, user.Id),
        //            new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
        //            new Claim("LoggedOn", DateTime.Now.ToString())
        //        }),

        //            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
        //            Issuer = appSettings.Site,
        //            Audience = appSettings.Audience,
        //            Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTime)
        //        };

        //        var token = tokenHandler.CreateToken(tokenDescriptor);

        //        return Ok(new { token = tokenHandler.WriteToken(token), expiration = token.ValidTo, email = user.Email, userRole = roles.FirstOrDefault(), userId = user.Id });
        //    }
        //    ModelState.AddModelError("", "User Name or Password not found!");
        //    return Unauthorized(new { LoginError = "Please check Login Credentials - In valid User Name or Password" });
        //}


        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

           var result = await _businessLayer.ConfirmEmail(email, code);
           return new JsonResult(result);
        }


        [HttpPost, Route("LogOut/")]
        public async Task<IActionResult> LogOut()
        {
            await _businessLayer.LogOut();
            return Ok();
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}