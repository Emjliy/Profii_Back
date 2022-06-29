using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Profii.ViewModels
{
    public class RegisterVM
    {

        [Required]
        public string FullName { get; set; }
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password),Compare(nameof(Password),ErrorMessage="Password doesn't match")]
        public string CheckPassword { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
