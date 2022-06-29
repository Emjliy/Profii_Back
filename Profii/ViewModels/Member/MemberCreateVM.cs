using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Profii.ViewModels
{
    public class MemberCreateVM
    {
        public string Url { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        [Required]
        public string Name { get; set; }
        [NotMapped, Required]
        public IFormFile Photo { get; set; }
    }
}
