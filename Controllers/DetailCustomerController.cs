using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace admin_sportyfe.Controllers
{
    public class DetailCustomerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // Dữ liệu giả lập cho chi tiết khách hàng
            var customerDetail = new CustomerDetail
            {
                Name = "Phuong Tuan",
                Email = "trinhtranpt@gmail.com",
                Phone = "0985629903",
                Address = "K54 số 1000, ngõ 2388, Đet An, Văn Quán, Hà Đông, Hà Nội",
                MembershipDuration = "3 tháng",
                Notes = "Ghi chú khách hàng",
                Orders = new List<Order>
                {
                    new Order { Id = "18802", Date = "18-08-2024, 10:09", Status = "Paid", Amount = 1500000, Invoice = "Hóa đơn 1", Note = "Ghi chú 1" },
                    new Order { Id = "18803", Date = "19-08-2024, 11:00", Status = "Pending", Amount = 2000000, Invoice = "Hóa đơn 2", Note = "Ghi chú 2" }
                }
            };

            return View(customerDetail);
        }
    }

    public class CustomerDetail
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string MembershipDuration { get; set; }
        public string Notes { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Invoice { get; set; }
        public string Note { get; set; }
    }
}
