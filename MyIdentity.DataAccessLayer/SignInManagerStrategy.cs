using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyIdentity.EntityLayer;

namespace MyIdentity.DataAccessLayer
{
    public class SignInManagerStrategy
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInManagerStrategy(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
