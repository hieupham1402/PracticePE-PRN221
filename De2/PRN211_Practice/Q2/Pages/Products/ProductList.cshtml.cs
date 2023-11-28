using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q2.Models;
using System.Text.Json;

namespace Q2.Pages.Products
{
    public class ProductListModel : PageModel
    {
        private readonly LuyenOnThiDBContext context;

        public ProductListModel()
        {
            context = new LuyenOnThiDBContext();
        }

        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public int CategoryIdSeleced { get; set; }

        public void OnGet(int idCategory = 0)
        {
            if (idCategory == 0)
            {
                Products = context.Products.ToList();
            }
            else
            {
                CategoryIdSeleced = idCategory;
                Products = context.Products.Where(x => x.CategoryId == idCategory).ToList();
            }
            Categories = context.Categories.ToList();
        }
        //ham add to cart
        public IActionResult OnGetAddToCart(int productId, int idCategory)
        {

            List<OrderDetail> orders = new List<OrderDetail>();
            if (HttpContext.Session.GetString("cart") != null)
            {
                string data = HttpContext.Session.GetString("cart");
                orders = JsonSerializer.Deserialize<List<OrderDetail>>(data);
            }
            else
            {
                orders = new List<OrderDetail>();
            }
            OrderDetail order = orders.FirstOrDefault(x => x.ProductId == productId);
            if(order != null)
            {
                order.Quantity++;
            }
            else
            {
                order = new OrderDetail();
                order.ProductId = productId;
                order.Quantity = 1;
                orders.Add(order);
            }
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(orders));
            CategoryIdSeleced = idCategory;
            return RedirectToPage("", new {idCategory = idCategory}); //chuyển hướng đến trang giỏ hàng sau khi thêm
        }

    }
}
