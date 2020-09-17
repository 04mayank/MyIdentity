using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyIdentity.BusinessLayer;
using MyIdentity.EntityLayer;

namespace MyIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenLogic _businessLayer;

        public TokenController(ITokenLogic businessLayer)
        {
            _businessLayer = businessLayer;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Auth([FromBody] TokenRequestModel model) // granttype = "refresh_token"
        {
            // We will return Generic 500 HTTP Server Status Error
            // If we receive an invalid payload
            if (model == null)
            {
                return new StatusCodeResult(500);
            }

            var result = await _businessLayer.Auth(model);
            if (result == null)
            {
                return new BadRequestResult();
                //Unauthorized(new { LoginError = "Please Check the Login Credentials - Ivalid Username/Password was entered" });
            }
            return Ok(result);
        }
    }
}