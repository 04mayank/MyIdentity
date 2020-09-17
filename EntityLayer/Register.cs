using System;
using System.ComponentModel.DataAnnotations;

namespace MyIdentity.EntityLayer
{
    public class Register
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }

        public string PhoneNo { get; set; }

        public string Role { get; set; }
    }
}
