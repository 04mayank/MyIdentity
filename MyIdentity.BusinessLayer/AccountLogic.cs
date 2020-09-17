using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyIdentity.DataAccessLayer;
using MyIdentity.EntityLayer;

namespace MyIdentity.BusinessLayer
{
    public class AccountLogic
    {
        private readonly UserManagerStrategy _userManager;
        private readonly SignInManagerStrategy _signInManager;

        public AccountLogic(UserManagerStrategy userManager, SignInManagerStrategy signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<List<string>> Register(Register model)
        {
            List<string> response = new List<string>();
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNo,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateUser(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRole(user, model.Role);
                //await SignInManager.SignInAsync(user, isPersistent: false);

                // Sending Confirmation Email

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                response.Add("Registered Successfully");
                response.Add(code);
                return response;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    response.Add(error.Description);
                }
            }
            return response;
        }


        public async Task<List<string>> ConfirmEmail(string email, string code)
        {
            List<string> response = new List<string>();

            var user = await _userManager.FindByEmail(email);

            if (user == null)
            {
                response.Add("No User");
                return response;
            }

            if (user.EmailConfirmed)
            {
                response.Add("AlreadyConfirmed");
                return response;
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                response.Add("EmailConfirmed");
                response.Add(code);
                return response;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    response.Add(error.ToString());
                }
                return response;
            }
        }


        public async Task<string> LogOut()
        {
            await _signInManager.SignOut();
            return "Successfull";
        }


        public void Delete(int id)
        {
        }
    }
}
