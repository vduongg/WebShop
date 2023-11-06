using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace KaiserStore.Areas.Admin.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Area("Admin")]
        [Route("/Admin/Inventory")]
        [Authorize]
        public async Task<IActionResult> InventoryAsync()
        {
            var product = _context.products.
                Include(s => s.sizes).
                ToList();

            return View(product);
        }
        [Area("Admin")]
        [Route("/Admin/Inventory/AddSize/{id}")]
        [Authorize]
        public IActionResult AddSize(int id)
        {
            ViewData["id"] = id;
            return View();
        }
        [Area("Admin")]
        [Route("/Admin/Inventory/ImportHistory/{id}")]
        [Authorize]
        public IActionResult ImportHistory(int id)
        {
            var history = _context.importDetails.Where(s => s.ProductId == id).ToList();
            return View(history);
        }
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Inventory/AddSize/{id}")]
        [Authorize]
        public async Task<IActionResult> AddSize(SizeItem size, int id)
        {
          
            var sizeId = await _context.sizes.Where(s=> s.ProductId == size.ProductId).ToListAsync();
            bool  check = false;
            foreach (var item in sizeId)
            {
                if(item.Name == size.Name) 
                {
                    check = true;
                }
            }
            if( check == false)
            {
                if(ModelState.IsValid)
                {
               
                    _context.Add(size);
                    _context.SaveChanges();
                    return RedirectToAction("Inventory");
                }
                
            }

            return View();
        }
        [Area("Admin")]
        [Route("/Admin/Inventory/Import/{id}")]
        [Authorize]
        public IActionResult Import(int id)
        {
            var listSize = _context.sizes.Where(s=> s.ProductId == id).ToList();
            ViewData["ListSize"] = listSize;
            var product = _context.products.Find(id);
            ViewData["product"] = product;
            return View();
        }
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Inventory/Import/{id}")]
        [Authorize]
        public IActionResult Import(ImportDetails details, int id)
        {
            var listSize = _context.sizes.Where(s => s.ProductId == id).ToList();
            ViewData["ListSize"] = listSize;
            var product = _context.products.Find(id);
            ViewData["product"] = product;

            details.Id = 0;
            details.DateTime = DateTime.Now;
            if( ModelState.IsValid)
            {
                
                _context.Add(details);
                _context.SaveChanges();
                var size = _context.sizes.Where(s => s.ProductId == details.ProductId).Where(s => s.Name == details.Size).FirstOrDefault();
                size.Quantity = size.Quantity + details.quantity;
                _context.SaveChanges();
                return RedirectToAction("Inventory");

            }
            return View();
        }
    }
}
