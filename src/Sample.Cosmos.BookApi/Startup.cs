using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sample.Cosmos.BookApi.Repositories;

namespace Sample.Cosmos.BookApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<IBookRepository, CosmosBookRepository>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Remember the API endpoint is /api/books");
            });
        }
    }
}
