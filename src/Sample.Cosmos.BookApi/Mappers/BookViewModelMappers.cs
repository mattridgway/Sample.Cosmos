using Sample.Cosmos.BookApi.Models;
using Sample.Cosmos.BookApi.ViewModels;

namespace Sample.Cosmos.BookApi.Mappers
{
    internal static class BookViewModelMappers
    {
        internal static Book ToModel(this UpdateBookViewModel viewModel, string id)
        {
            return new Book {
                Id = id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                ISBN = viewModel.ISBN
            };
        }
    }
}
