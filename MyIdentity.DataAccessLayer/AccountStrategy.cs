using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyIdentity.EntityLayer;

namespace MyIdentity.DataAccessLayer
{
    public class AccountStrategy
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountStrategy(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        public async Task<string> Register(Register model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNo,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, model.Role);
                return "Registered Successfully";
            }
            return "Unsuccessfull";
        }


        public async Task<string> LogOut()
        {
            await signInManager.SignOutAsync();
            return "Successfull";
        }


        public void Delete(int id)
        {
        }
    }
}
