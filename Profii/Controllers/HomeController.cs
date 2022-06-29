using Microsoft.AspNetCore.Mvc;
using Profii.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Profii.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context { get; }
        public HomeController(AppDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View(_context.Members.ToList());
        }
    }
}
