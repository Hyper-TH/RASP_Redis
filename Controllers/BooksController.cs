using Microsoft.AspNetCore.Mvc;
using RASP_Redis.Services;
using RASP_Redis.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.OutputCaching;

namespace RASP_Redis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;
        private readonly ISBNsService _isbnsService;

        public BooksController(BooksService booksService, ISBNsService isbnsService, IDistributedCache cache)
        {
            _booksService = booksService;
            _isbnsService = isbnsService;
        }

        [HttpPost("cache")]
        public async Task<IActionResult> CacheDocIdsAsync()
        {
            try
            {
                // Retrieve all books from MongoDB
                var books = await _booksService.GetAsync();

                if (books == null || !books.Any())
                {
                    return NotFound(new { Message = "No books found to cache." });
                }

                int newCacheCount = 0; // Counter for newly cached items
                foreach (var book in books)
                {
                    if (!string.IsNullOrEmpty(book.ISBN) && !string.IsNullOrEmpty(book.Id))
                    {
                        // Check if the ISBN is already cached
                        var cachedDocId = await _isbnsService.GetCachedDocIdAsync(book.ISBN);

                        if (string.IsNullOrEmpty(cachedDocId))
                        {
                            // Cache only if not already present
                            await _isbnsService.CacheISBNAsync(book.ISBN, book.Id);
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
                var docId = await _isbnsService.GetCachedDocIdAsync(isbn);

                if (string.IsNullOrEmpty(docId))
                {
                    return NotFound(new { Message = $"Book with ISBN {isbn} not found in the cache." });
                }

                // Retrieve the document using the docId
                var document = await _booksService.GetAsync(docId);

                if (document == null)
                {
                    return NotFound(new { Message = $"Document with ID {docId} not found." });
                }

                return Ok(document);
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
                var cachedDocId = await _isbnsService.GetCachedDocIdAsync(newBook.ISBN);
                if (!string.IsNullOrEmpty(cachedDocId))
                {
                    return Conflict(new { Message = $"ISBN {newBook.ISBN} already exists." });
                }

                await _booksService.CreateAsync(newBook);

                await _isbnsService.CacheISBNAsync(newBook.ISBN, newBook.Id);

                return CreatedAtAction(nameof(Get), new { isbn = newBook.ISBN }, newBook);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating book: {ex.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An error occurred while creating the book. Please try again later." });
            }
        }


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book updatedBook)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            updatedBook.Id = book.Id;

            await _booksService.UpdateAsync(id, updatedBook);

            return NoContent();
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> Delete(string isbn)
        {
            try
            {
                var docId = await _isbnsService.GetCachedDocIdAsync(isbn);

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
                await _isbnsService.RemovedCachedISBNAsync(isbn);

                return CreatedAtAction(nameof(Get), new { isbn });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error in Get method: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "An unexpected error occurred while processing your request." });
            }

        }
    }
}
