using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MyIdentity.DataAccessLayer
{
    public class RoleManagerStrategy
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerStrategy(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRole(IdentityRole identityRole)
        {
            return await _roleManager.CreateAsync(identityRole);
        }

        public IQueryable<IdentityRole> GetRole()
        {
            return _roleManager.Roles;
        }
    }
}
