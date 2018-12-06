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
            services.Configure<CosmosConfiguration>(options =>
            {
                options.Key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
                options.Endpoint = "https://localhost:8081";
            });
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
