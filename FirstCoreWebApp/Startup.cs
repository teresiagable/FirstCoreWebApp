using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FirstCoreWebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(10); //.FromSeconds(60);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddMvc(); //add MVC so we can use it
            //services.AddControllersWithViews(); //when not all MVC features are needed
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseDefaultFiles();  // looks for index.html or default.html in wwwroot
            app.UseStaticFiles();   //default opens up the wwwroot folder to be accessed
            app.UseSession();
            //app.UseHttpContextItemsMiddleware();  //core 3 update messed this one up
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "ReviewRoute",              //name to route rule
                    pattern: "TheReviews", //url to match
                    defaults: new { controller = "Reviews", action = "Index" }  //what controller & action to call
                    );

                endpoints.MapControllerRoute(
                     name: "CreateReviewRoute",              //name to route rule
                     pattern: "WriteYourReview", //url to match
                     defaults: new { controller = "Reviews", action = "Create" }  //what controller & action to call
                     );

                // custom/special routes should be added before default
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello W�rld!");
                //});
            });
        }
    }
}