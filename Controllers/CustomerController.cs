using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace admin_sportyfe.Controllers
{
    public class CustomerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Dữ liệu giả lập cho danh sách khách hàng
            var customers = new List<Customer>
            {
                new Customer { Name = "Phuong Tuan", Email = "trinhtranpt@gmail.com", Phone = "0985629903", City = "Ha Noi", Orders = 1, TotalSpent = 1500000 },
                new Customer { Name = "Phuong Tuan", Email = "trinhtranpt@gmail.com", Phone = "0985629903", City = "Ha Noi", Orders = 5, TotalSpent = 7500000 },
                new Customer { Name = "Phuong Tuan", Email = "trinhtranpt@gmail.com", Phone = "0985629903", City = "Ha Noi", Orders = 3, TotalSpent = 4500000 },
            };

            return View(customers);
        }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public int Orders { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
