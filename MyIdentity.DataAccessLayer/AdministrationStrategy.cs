using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyIdentity.EntityLayer;

namespace MyIdentity.DataAccessLayer
{
    public class AdministrationStrategy
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public AdministrationStrategy(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }


        public async Task<List<string>> CreateRole(CreateRole model)
        {
            List<string> errors = new List<string>();

            IdentityRole identityRole = new IdentityRole
            {
                Name = model.RoleName
            };

            IdentityResult result = await roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return new List<string>(){ "Registered Successfully" };
            }
            foreach (IdentityError error in result.Errors)
            {
                errors.Add(error.Description);
            }
            return errors;
        }


        //public IActionResult GetRoles()
        //{
        //    var roles = roleManager.Roles;
        //    return Ok(roles);
        //}
    }
}
