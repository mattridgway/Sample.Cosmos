using System.Threading.Tasks;
using Sample.Cosmos.BookApi.Models;

namespace Sample.Cosmos.BookApi.Repositories
{
    internal class CosmosBookRepository : IBookRepository
    {
        public Task<Book> CreateBookAsync(string name, string description, string ISBN)
        {
            return Task.FromResult(default(Book));
        }
    }
}
