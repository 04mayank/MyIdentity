using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MyIdentity.EntityLayer
{
    public class ApplicationUser : IdentityUser
    {
        //public string Address { get; set; }
        public virtual List<TokenModel> Tokens { get; set; }
    }
}
