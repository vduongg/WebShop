using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KaiserStore.Areas.Admin.Controllers
{
    public class SlideController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SlideController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Area("Admin")]
        [Route("/Admin/Slide")]
        [Authorize]
        public async Task<IActionResult> Slide()
        {
            var slide = await _context.slides.ToListAsync();
            return View(slide);

        }
        [Area("Admin")]
        [Route("/Admin/Slide/Add")]
        [Authorize]
        public async Task<IActionResult> Add()
        {

         
            return View();

        }
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Slide/Add")]
        [Authorize]
        public async Task<IActionResult> Add(Slide slide)
        {
            var file = slide.file;
            if (file != null)
            {
                var s = slide;
                using (var target = new MemoryStream())
                {
                    file.CopyTo(target);
                    s.dataimage = target.ToArray();
                }
                if (ModelState.IsValid)
                {
                    slide.Id = 0;
                    _context.slides.Add(s);
                    _context.SaveChanges();
                    return RedirectToAction("Slide");
                }

            }
           

            return View();

        }
        [Area("Admin")]
        [Route("/Admin/Slide/Delete/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [Area("Admin")]
        [Route("/Admin/Slide/Delete/{id}")]

        public IActionResult Delete(GetStatus get, int id)
        {
            var slide = _context.slides.Find(id);
            slide.status = get.status;
            _context.SaveChanges();
            return RedirectToAction("Slide");
        }
        [Area("Admin")]
        [Route("/Admin/Slide/Restore/{id}")]
        [Authorize]
        public IActionResult Restore(int id)
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [Area("Admin")]
        [Route("/Admin/Slide/Restore/{id}")]

        public IActionResult Restore(GetStatus get, int id)
        {
            var slide = _context.slides.Find(id);
            slide.status = get.status;
            _context.SaveChanges();
            return RedirectToAction("Slide");
        }

    }
}
