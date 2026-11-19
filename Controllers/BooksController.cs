using BookwormAPI.Dtos;
using BookwormAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookwormAPI.Controllers
{
    [Authorize]
    [Route("api/v1/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(CreateBookDto createBookDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdBook = await _bookService.CreateBookAsync(createBookDto);
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book is null) return NotFound();
            return Ok(book);
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookService.GetBooksAsync();
            return Ok(books);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetBooks(int userId)
        {
            var books = await _bookService.GetUserBooks(userId);
            return Ok(books);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = await _bookService.GetBookByIdAsync(id);
            if (book is null) return NotFound();

            var updatedBook = await _bookService.UpdateBookAsync(id, updateBookDto);
            return Ok(updatedBook);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var deleteResult = await _bookService.DeleteBookAsync(id);
            if (!deleteResult) return NotFound();
            return NoContent();
        }
    }
}
