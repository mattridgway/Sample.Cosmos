using Microsoft.AspNetCore.Mvc;

namespace Sample.Cosmos.BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
