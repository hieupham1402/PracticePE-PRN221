using Q2.Hubs;

namespace Q2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();
            builder.Services.AddSession();
            builder.Services.AddMemoryCache();
            builder.Services.AddSignalR();

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();
            app.UseSession();
            app.MapRazorPages();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.MapHub<ProductsHub>("/productHubs");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}