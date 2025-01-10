using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using admin_sportyfe.Models;
using admin_sportyfe.Services;

namespace admin_sportyfe.Controllers
{
    public class ProductController : Controller
    {
        private readonly FirebaseService _firebaseService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IWebHostEnvironment webHostEnvironment)
        {
            _firebaseService = new FirebaseService();
            _webHostEnvironment = webHostEnvironment;
        }

        // Hiển thị danh sách sản phẩm
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _firebaseService.GetProductsAsync();
            return View(products);
        }

        // Hiển thị form thêm sản phẩm
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Add(Product product)
{
    // Xóa các trường không cần kiểm tra khỏi ModelState
    ModelState.Remove(nameof(product.Id));
    ModelState.Remove(nameof(product.image));

    // Kiểm tra trạng thái ModelState
    if (!ModelState.IsValid)
    {
        foreach (var key in ModelState.Keys)
        {
            foreach (var error in ModelState[key].Errors)
            {
                Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
            }
        }
        return View(product); // Trả về view với lỗi để hiển thị
    }

    try
    {
        product.Id = Guid.NewGuid().ToString(); // Tạo ID tự động

        // Xử lý upload file ảnh
        if (product.ImageFile != null && product.ImageFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(fileStream);
            }

            product.image = "/images/" + uniqueFileName; // Gán đường dẫn ảnh
        }

        await _firebaseService.AddProductAsync(product);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex.Message}");
        ModelState.AddModelError("", "An error occurred while processing your request.");
    }

    return View(product);
}

        // Hiển thị form sửa sản phẩm
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var product = await _firebaseService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(); // Sản phẩm không tồn tại
            }

            return View(product);
        }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(Product product)
{
    // Xóa các trường không cần kiểm tra khỏi ModelState
    ModelState.Remove(nameof(product.Id));
    ModelState.Remove(nameof(product.image));

    if (!ModelState.IsValid)
    {
        foreach (var key in ModelState.Keys)
        {
            foreach (var error in ModelState[key].Errors)
            {
                Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
            }
        }
        return View(product);
    }

    try
    {
        // Xử lý upload ảnh nếu có
        if (product.ImageFile != null && product.ImageFile.Length > 0)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(fileStream);
            }

            product.image = "/images/" + uniqueFileName; // Gán đường dẫn ảnh mới
        }
        else
        {
            // Giữ nguyên ảnh cũ nếu không upload file mới
            var existingProduct = await _firebaseService.GetProductByIdAsync(product.Id);
            if (existingProduct != null)
            {
                product.image = existingProduct.image;
            }
        }

        await _firebaseService.UpdateProductAsync(product.Id, product);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception: {ex.Message}");
        ModelState.AddModelError("", "An error occurred while processing your request.");
    }

    return View(product);
}

        // Xóa sản phẩm
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await _firebaseService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(); // Sản phẩm không tồn tại
            }

            try
            {
                await _firebaseService.DeleteProductAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                ModelState.AddModelError("", $"Error deleting product: {ex.Message}");
            }

            return RedirectToAction("Index");
        }
    }
}
