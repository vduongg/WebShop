using KaiserStore.Data;
using KaiserStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var ListSize = await _context.sizes.ToListAsync();
            ViewData["ListSize"] = ListSize;
            var product = _context.products.ToList();

            return View(product);
        }
        [Area("Admin")]
        [Route("/Admin/Inventory/AddSize")]
        [Authorize]
        public IActionResult AddSize(int id)
        {
            ViewData["id"] = id;
            return View();
        }
        [HttpPost]
        [Area("Admin")]
        [Route("/Admin/Inventory/AddSize")]
        [Authorize]
        public async Task<IActionResult> AddSize(SizeItem size)
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
                _context.Add(size);
                _context.SaveChanges();
                return RedirectToAction("Inventory");
            }

            return View();
        }
        [Area("Admin")]
        [Route("/Admin/Inventory/Import")]
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
        [Route("/Admin/Inventory/Import")]
        [Authorize]
        public async Task<IActionResult> Import(ImportDetails details)
        {
           
            details.Id = 0;
            details.DateTime = DateTime.Now;
            _context.Add(details);
            _context.SaveChanges();
            var size = _context.sizes.Where(s => s.ProductId == details.ProductId).Where(s => s.Name == details.Size).FirstOrDefault();
            if(size.Quantity > 0)
            {
                size.Quantity = size.Quantity + details.quantity;
            }
            else
            {
                size.Quantity = details.quantity;
            }
            _context.SaveChanges();
            return RedirectToAction("Inventory");
        }
    }
}
