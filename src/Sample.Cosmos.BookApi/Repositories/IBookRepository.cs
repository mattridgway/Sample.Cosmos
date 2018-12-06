using Sample.Cosmos.BookApi.Models;
using System.Threading.Tasks;

namespace Sample.Cosmos.BookApi.Repositories
{
    public interface IBookRepository
    {
        Task<Book> CreateBookAsync(string name, string description, string ISBN);
        Task<Book> GetBookAsync(string id);
        Task<Book> UpdateBookAsync(Book book);
    }
}
