using Sample.Cosmos.BookApi.Models;
using Sample.Cosmos.BookApi.ViewModels;

namespace Sample.Cosmos.BookApi.Mappers
{
    internal static class BookMappers
    {
        internal static BookViewModel ToViewModel(this Book model)
        {
            return new BookViewModel {
                Description = model.Description,
                Id = model.Id,
                ISBN = model.ISBN,
                Name = model.Name
            };
        }
    }
}
