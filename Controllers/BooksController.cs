using Microsoft.AspNetCore.Mvc;
using RASP_Redis.Models;
using RASP_Redis.Services.MongoDB;
using RASP_Redis.Services.Redis;

namespace RASP_Redis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;
        private readonly BookStoreRedisService _cache;

        public BooksController(BooksService booksService, BookStoreRedisService isbnCache)
        {
            _booksService = booksService;
            _cache = isbnCache;
        }

        [HttpPost("cache")]
        public async Task<IActionResult> CacheDocIdsAsync()
        {
            try
            {
                var books = await _booksService.GetAsync();

                if (books == null || !books.Any())
                {
                    return NotFound(new { Message = "No books found to cache." });
                }

                int newCacheCount = 0; 
                foreach (var book in books)
                {
                    if (!string.IsNullOrEmpty(book.ISBN) && !string.IsNullOrEmpty(book.Id))
                    {
                        var cachedDocId = await _cache.GetCachedDocIdAsync(book.ISBN);

                        if (string.IsNullOrEmpty(cachedDocId))
                        {
                            await _cache.CacheISBNAsync(book.ISBN, book.Id);
                            newCacheCount++;
                        }
                    }
                }

                return Ok(new
                {
                    Message = "Books cached successfully",
                    TotalBooks = books.Count,
                    NewlyCachedBooks = newCacheCount
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in CacheDocIdsAsync method: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An unexpected error occurred while caching books." });
            }
        }

        [HttpGet]
        public async Task<List<Book>> Get() =>
            await _booksService.GetAsync();

        [HttpGet("{isbn}")]
        public async Task<ActionResult<Book>> Get(string isbn)
        {
            try
            {
                var docId = await _cache.GetCachedDocIdAsync(isbn);

                if (string.IsNullOrEmpty(docId))
                {
                    return NotFound(new { Message = $"Book with ISBN {isbn} not found in the cache." });
                }

                var document = await _booksService.GetAsync(docId);

                return document == null
                    ? NotFound(new { Message = $"Document with ID {docId} not found." })
                    : Ok(document);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Get method: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An unexpected error occurred while processing your request." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book newBook)
        {
            if (newBook == null || string.IsNullOrEmpty(newBook.ISBN))
            {
                return BadRequest("Invalid book data");
            }

            try
            {
                var cachedDocId = await _cache.GetCachedDocIdAsync(newBook.ISBN);
                if (!string.IsNullOrEmpty(cachedDocId))
                {
                    return Conflict(new { Message = $"ISBN {newBook.ISBN} already exists." });
                }

                await _booksService.CreateAsync(newBook);

                await _cache.CacheISBNAsync(newBook.ISBN, newBook.Id);

                return CreatedAtAction(nameof(Get), new { isbn = newBook.ISBN }, newBook);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating book: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while creating the book. Please try again later." });
            }
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> Delete(string isbn)
        {
            try
            {
                var docId = await _cache.GetCachedDocIdAsync(isbn);

                if (string.IsNullOrEmpty(docId))
                {
                    return NotFound(new { Message = $"Book with ISBN {isbn} not found in the cache." });

                }

                var document = await _booksService.GetAsync(docId);

                if (document is null)
                {
                    return NotFound(new { Message = $"Document with ID {docId} not found." });
                }

                await _booksService.RemoveAsync(docId);
                await _cache.RemoveCachedISBNAsync(isbn);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Get method: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An unexpected error occurred while processing your request." });
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Book updatedBook)
        {
            if (updatedBook == null || string.IsNullOrEmpty(updatedBook.ISBN))
            {
                return BadRequest("Invalid book data");
            }

            var cachedDocId = await _cache.GetCachedDocIdAsync(updatedBook.ISBN);

            if (string.IsNullOrEmpty(cachedDocId))
            {
                return Conflict(new { Message = $"ISBN {updatedBook.ISBN} does not exist." });
            }

            updatedBook.Id = cachedDocId;

            await _booksService.UpdateAsync(cachedDocId, updatedBook);

            return NoContent();
        }
    }
}
