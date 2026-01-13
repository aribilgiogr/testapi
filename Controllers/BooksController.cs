using Microsoft.AspNetCore.Mvc;
using TestAPI.Models;
using TestAPI.Services;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService service;

        public BooksController(IBooksService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> Get() => await service.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await service.GetAsync(id);
            if (book is null) return NotFound();
            return book;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await service.CreateAsync(newBook);
            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Book updatedBook)
        {
            var book = await service.GetAsync(id);
            if (book is null) return NotFound();

            updatedBook.Id = book.Id;
            await service.UpdateAsync(id, updatedBook);
            return NoContent();
        }
        
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await service.GetAsync(id);
            if (book is null) return NotFound();

            await service.RemoveAsync(id);
            return NoContent();
        }
    }
}