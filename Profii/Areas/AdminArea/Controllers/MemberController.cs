using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Profii.DAL;
using Profii.Helpers;
using Profii.Models;
using Profii.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Profii.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class MemberController : Controller
    {
        private AppDbContext _context { get; }
        private IWebHostEnvironment _env;

        public MemberController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        // GET: MemberController
        public ActionResult Index()
        {
            return View(_context.Members);
        }

        // GET: MemberController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: MemberController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!member.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Max size must be 200");
                return View();
            }
            if (!member.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "type must be photo");
                return View();
            }
            member.Url = await member.Photo.SaveFileAsync(_env.WebRootPath, "assets/images");
            await _context.AddAsync(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: MemberController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Member dbMember = _context.Members.Find(id);
            if (dbMember == null)
            {
                return NotFound();
            }
            return View(dbMember);
        }

        // POST: MemberController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Member member)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Member dbMember = _context.Members.Find(id);
            if (dbMember == null)
            {
                return NotFound();
            }
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            //if (!member.Photo.CheckFileSize(200))
            //{
            //    ModelState.AddModelError("Photo", "Max size must be 200");
            //    return View();
            //}
            //if (!member.Photo.CheckFileType("image/"))
            //{
            //    ModelState.AddModelError("Photo", "File type must be photo");
            //    return View();
            //}
            member.Url = await member.Photo.SaveFileAsync(_env.WebRootPath, "assets/images");
            var path = Helper.GetPath(_env.WebRootPath, "assets/images", dbMember.Url);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            dbMember.Url = member.Url;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id==null)
            {
                return BadRequest();
            }
            var dbMember =  _context.Members.Find(id);
            if (dbMember == null)
            {
                return NotFound();
            }
            var path = Helper.GetPath(_env.WebRootPath, "images", dbMember.Url);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Members.Remove(dbMember);  
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
