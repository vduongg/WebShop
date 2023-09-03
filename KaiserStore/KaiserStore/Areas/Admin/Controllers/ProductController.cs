using Microsoft.AspNetCore.Mvc;

namespace KaiserStore.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        [Area("Admin")]
        [Route("/Admin/Product")]
        public IActionResult Product()
        {
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/Product/Add")]
        public IActionResult Add()
        {
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/Product/Edit")]
        public IActionResult Edit()
        {
            return View();
        }

        [Area("Admin")]
        [Route("/Admin/Product/Delete")]
        public IActionResult Delete()
        {
            return View();
        }
    }
}
