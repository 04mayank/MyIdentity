using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyIdentity.DataAccessLayer;
using MyIdentity.EntityLayer;

namespace MyIdentity.BusinessLayer
{
    public class AdministrationLogic
    {
        private readonly RoleManagerStrategy _roleManager; 

        public AdministrationLogic(RoleManagerStrategy roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<string>> CreateRole(CreateRole model)
        {
            List<string> errors = new List<string>();

            IdentityRole identityRole = new IdentityRole
            {
                Name = model.RoleName
            };

            IdentityResult result = await _roleManager.CreateRole(identityRole);
            if (result.Succeeded)
            {
                return new List<string>() { "Registered Successfully" };
            }
            foreach (IdentityError error in result.Errors)
            {
                errors.Add(error.Description);
            }
            return errors;
        }


        public List<IdentityRole> GetRoles()
        {
            var roles = _roleManager.GetRole().ToList();
            return roles;
        }
    }
}
