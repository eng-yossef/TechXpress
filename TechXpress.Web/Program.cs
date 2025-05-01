using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Models.Contexts;
using TechXpress.Data.Repositories.Category;
using TechXpress.Data.Repositories.Order;
using TechXpress.Data.Repositories.OrederDetails;
using TechXpress.Data.Repositories.Product;
using TechXpress.Data.Repositories.Review;
using TechXpress.Data.UnitOfWork;
using TechXpress.Services.Product;

namespace TechXpress.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //add  Services Here
            //builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddDbContext<TechXpressDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                   );
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //.Services.AddScoped<IOrderRepository, OrderRepository>();


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
