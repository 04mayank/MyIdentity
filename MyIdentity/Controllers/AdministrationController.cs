using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyIdentity.BusinessLayer;
using MyIdentity.EntityLayer;

namespace MyIdentity.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly AdministrationLogic _businessLayer;

        public AdministrationController(AdministrationLogic businessLayer)
        {
            _businessLayer = businessLayer;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> CreateRole(CreateRole model)
        {
            
            var result = await _businessLayer.CreateRole(model);
            if (result[0] == "Registered Successfully")
            {
                return Ok(new { messsage = result[0] });
            }
            return BadRequest(new JsonResult(result));
        }


        [HttpGet("[action]")]
        public IActionResult GetRoles()
        {
            var roles = _businessLayer.GetRoles();
            return Ok(roles);
        }
    }
}