using Microsoft.AspNetCore.Mvc;
using RASP_Redis.Services;
using RASP_Redis.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace RASP_Redis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;
        private readonly IDistributedCache _cache;

        public BooksController(BooksService booksService, IDistributedCache cache)
        {
            _booksService = booksService;
            _cache = cache;
        }

        [HttpGet]
        public async Task<List<Book>> Get() =>
            await _booksService.GetAsync();

        //[HttpGet("{id:length(24)}")]
        //public async Task<ActionResult<Book>> Get(string id)
        //{
        //    var book = await _booksService.GetAsync(id);

        //    if (book is null)
        //    {
        //        return NotFound();
        //    }

        //    return book;
        //}

        [HttpGet("{isbn}")]
        public async Task<ActionResult<Book>> Get(string isbn)
        {
            // Check if ISBN is in the cache
            var cachedDocId = await _cache.GetStringAsync(isbn);

            string docId;

            if (cachedDocId == null)
            {
                var book = await _booksService.GetByISBNAsync(isbn);

                if (book == null)
                {
                    return NotFound();
                }

                docId = book.Id;

                await _cache.SetStringAsync(isbn, docId, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }
            else
            {
                docId = cachedDocId;
            }

            var document = await _booksService.GetAsync(docId);

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            await _booksService.CreateAsync(newBook);

            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
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

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _booksService.RemoveAsync(id);

            return NoContent();

        }
    }
}
