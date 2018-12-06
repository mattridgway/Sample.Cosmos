using Microsoft.AspNetCore.Mvc;
using Sample.Cosmos.BookApi.Mappers;
using Sample.Cosmos.BookApi.Repositories;
using Sample.Cosmos.BookApi.ViewModels;
using System.Threading.Tasks;

namespace Sample.Cosmos.BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpPost]
        public async Task<ActionResult<BookViewModel>> Create(CreateBookViewModel viewModel)
        {
            var book = await _bookRepository.CreateBookAsync(viewModel.Name, viewModel.Description, viewModel.ISBN);
            return book.ToViewModel();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookViewModel>> Read(string id)
        {
            try
            {
                var book = await _bookRepository.GetBookAsync(id);
                return book.ToViewModel();
            }
            catch(BookNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookViewModel>> Update(string id, UpdateBookViewModel viewModel)
        {
            try { 
                var book = await _bookRepository.UpdateBookAsync(viewModel.ToModel(id));
                return book.ToViewModel();
            }
            catch (BookNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidBookException)
            {
                return BadRequest();
            }
        }
    }
}
