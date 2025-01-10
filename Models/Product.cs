using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace admin_sportyfe.Models
{
    public class Product
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter a title")]
        public string tittle { get; set; }

        [Required(ErrorMessage = "Please enter a price")]
        public decimal price { get; set; }

        public string image { get; set; }

        public string description { get; set; }

        public string category { get; set; }

        public string status { get; set; }

        [JsonIgnore] // Bỏ qua khi serialize/deserialized với JSON
        public IFormFile ImageFile { get; set; }
    }
}
