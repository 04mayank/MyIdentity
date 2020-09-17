using System;
using System.ComponentModel.DataAnnotations;

namespace MyIdentity.EntityLayer
{
    public class CreateRole
    {
        [Required]
        public string RoleName { get; set; }
    }
}
