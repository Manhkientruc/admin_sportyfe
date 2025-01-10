using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using admin_sportyfe.Models;

namespace admin_sportyfe.Services
{
    public class FirebaseService
    {
        private const string FirebaseUrl = "https://sportyfe-55a35-default-rtdb.firebaseio.com/";
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient(FirebaseUrl);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var products = await _firebaseClient
                .Child("products")
                .OnceAsync<Product>();

            return products.Select(p => new Product
            {
                Id = p.Key,
                tittle = p.Object.tittle,
                price = p.Object.price,
                image = p.Object.image
            }).ToList();
        }

        public async Task AddProductAsync(Product product)
{
    await _firebaseClient
        .Child("products")
        .PostAsync(new
        {
            product.Id,
            product.tittle,
            product.price,
            product.image,
            product.description,
            product.category,
            product.status
        });
}

        public async Task UpdateProductAsync(string id, Product product)
{
    await _firebaseClient
        .Child("products")
        .Child(id)
        .PutAsync(new
        {
            product.Id,
            product.tittle,
            product.price,
            product.image,
            product.description,
            product.category,
            product.status
        });
}

        public async Task DeleteProductAsync(string id)
        {
            await _firebaseClient
                .Child("products")
                .Child(id)
                .DeleteAsync();
        }
        //        public async Task<Product> GetProductByIdAsync(string id)
        public async Task<Product> GetProductByIdAsync(string id)
        {
            try
            {
                var product = await _firebaseClient
                    .Child("products")
                    .Child(id)
                    .OnceSingleAsync<Product>();

                if (product == null)
                {
                    Console.WriteLine($"Product with ID {id} not found in Firebase.");
                    return null;
                }

                // Trả về đối tượng Product
                return new Product
                {
                    Id = id,
                    tittle = product.tittle,
                    price = product.price,
                    image = product.image
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching product by ID {id}: {ex.Message}");
                return null;
            }
        }
    }
}
