using System.Globalization;
using Datapac_Library_Solution.Data;
using Datapac_Library_Solution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Datapac_Library_Solution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BorrowingsController : ControllerBase
{
    private readonly LibraryDbContext dbContext;

    public BorrowingsController(LibraryDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    [HttpGet]
    [Route("getByCustomerId/{customerId:int}")]
    public async Task<IActionResult> GetActiveCustomersBorrowings([FromRoute] int customerId)
    {
        var result = dbContext.Borrowings.Where(x => x.CustomerId == customerId && x.ReturnedDate == null);

        return Ok(await result.ToListAsync());
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Borrowing addBorrowing)
    {
        if (addBorrowing is null) throw new ArgumentNullException(nameof(addBorrowing));
        
        var customer = await dbContext.Customers.FindAsync(addBorrowing.CustomerId);
        if (customer == null) return BadRequest($"Customer with id {addBorrowing.CustomerId} not exist");
        var book = await dbContext.Books.FindAsync(addBorrowing.BookId);
        if (book == null) return BadRequest($"Book with id {addBorrowing.BookId} not exist");
        
        var borrowing = new Borrowing
        {
            Id = 0, 
            BookId = addBorrowing.BookId,
            CustomerId = addBorrowing.CustomerId,
            BorrowedDate = addBorrowing.BorrowedDate,
            ReturnedDate = addBorrowing.ReturnedDate,
            Deadline = addBorrowing.Deadline
        };

        await dbContext.Borrowings.AddAsync(borrowing);
        await dbContext.SaveChangesAsync();

        return Ok(borrowing);
    }
    
    [HttpPut]
    [Route("returnById/{id:int}")]
    public async Task<IActionResult> Return([FromRoute] int id)
    {
        var borrowing = await dbContext.Borrowings.FindAsync(id);

        if (borrowing == null) return NotFound();

        borrowing.ReturnedDate = DateTime.Now;

        await dbContext.SaveChangesAsync();

        return Ok(borrowing);
    }
    
}