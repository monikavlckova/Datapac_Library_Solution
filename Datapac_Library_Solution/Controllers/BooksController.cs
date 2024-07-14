using Datapac_Library_Solution.Data;
using Datapac_Library_Solution.Models;
using Microsoft.AspNetCore.Mvc;

namespace Datapac_Library_Solution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryDbContext dbContext;

    public BooksController(LibraryDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var book = await dbContext.Books.FindAsync(id);

        if (book == null) return NotFound();

        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Book addBook)
    {
        if (addBook is null) throw new ArgumentNullException(nameof(addBook));
        if (string.IsNullOrEmpty(addBook.Author)) return BadRequest("Missing author");
        if (string.IsNullOrEmpty(addBook.Name)) return BadRequest("Missing name");

        var book = new Book
        {
            Id = 0,
            Name = addBook.Name,
            Author = addBook.Author
        };

        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        return Ok(book);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, Book updateBook)
    {
        if (updateBook is null) throw new ArgumentNullException(nameof(updateBook));
        if (string.IsNullOrEmpty(updateBook.Author)) return BadRequest("Missing author");
        if (string.IsNullOrEmpty(updateBook.Name)) return BadRequest("Missing name");

        var book = await dbContext.Books.FindAsync(id);

        if (book == null) return NotFound();

        book.Name = updateBook.Name;
        book.Author = updateBook.Author;

        await dbContext.SaveChangesAsync();

        return Ok(book);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var book = await dbContext.Books.FindAsync(id);

        if (book == null) return NotFound();

        dbContext.Remove(book);
        await dbContext.SaveChangesAsync();

        return Ok(book);
    }
}