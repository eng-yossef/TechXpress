using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Models;
using TechXpress.Data.Models.Contexts;
using TechXpress.Data.Repositories.CartItemRepo;
using TechXpress.Data.Repositories.CategoryRepo;
using TechXpress.Data.Repositories.GenericRepository;
using TechXpress.Data.Repositories.OrderDetailRepo;
using TechXpress.Data.Repositories.OrderRepo;
using TechXpress.Data.Repositories.ProductRepo;
using TechXpress.Data.Repositories.ReviewRepo;
using TechXpress.Data.UnitOfWork;
using TechXpress.Services.CartItemsService;
using TechXpress.Services.CategoriesService;
using TechXpress.Services.OrdersService;
using TechXpress.Services.ProductsService;
using TechXpress.Services.ReviewsService;
using TechXpress.Services.OrdersDetailsService;
using TechXpress.Services.ShoppingCartsService;
using TechXpress.Data.Repositories.ShoppingCartRepo;
using TechXpress.Web.Filters;
using TechXpress.Web.Services.Interfaces;
using TechXpress.Web.Services.Implementations;

namespace TechXpress.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Updated DbContext registration with migrations assembly
            builder.Services.AddDbContext<TechXpressDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("TechXpress.Data")));

            // Updated Identity configuration with custom ApplicationUser
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<TechXpressDbContext>()
            .AddDefaultTokenProviders();

            // Add Repositories Here
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

            // Add Services Here
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartItemService, CartItemService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
            //IUserService
            builder.Services.AddScoped<IUserService,UserService>();
            // Fix for CS0311: Ensure AIAssistantService implements IAIAssistantService interface
            // Register the HttpClient service
            builder.Services.AddHttpClient();

            // Register AIAssistantService with dependency injection
            builder.Services.AddTransient<IAIAssistantService, AIAssistantService>();
            builder.Services.AddTransient<IAICommerceService, AICommerceAssistantService>();
            //Add filters 
            builder.Services.AddScoped<UpdateCartItemCountFilter>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            //app.UseMiddleware<CartItemCountMiddleware>();

            // Add response caching services
            builder.Services.AddResponseCaching();

            // Use response caching middleware
            app.UseResponseCaching();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
