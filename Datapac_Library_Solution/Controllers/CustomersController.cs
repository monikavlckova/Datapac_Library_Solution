using Datapac_Library_Solution.Data;
using Datapac_Library_Solution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Datapac_Library_Solution.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly LibraryDbContext dbContext;

    public CustomersController(LibraryDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var customer = await dbContext.Customers.FindAsync(id);

        if (customer == null) return NotFound();

        return Ok(customer);
    }
    
    //Vráti zoznam používateľov, ktorí majú deň pred uplynutím termínu na vrátenie knihy
    [HttpGet]
    [Route("getByDate")]
    public async Task<IActionResult> GetCustomersDayBeforeDeadline()
    {
        var result = from c in dbContext.Customers.AsQueryable()
            join b in dbContext.Borrowings.Where(x => x.ReturnedDate == null && x.Deadline.Date == DateTime.Today.AddDays(1))
                on c.Id equals b.CustomerId
            select new { c.Id, c.FirstName, c.LastName, c.Email };

        return Ok(await result.ToListAsync());
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Customer addCustomer)
    {
        if (addCustomer is null) throw new ArgumentNullException(nameof(addCustomer));
        if (string.IsNullOrEmpty(addCustomer.FirstName)) return BadRequest("Missing first name");
        if (string.IsNullOrEmpty(addCustomer.LastName)) return BadRequest("Missing last name");
        if (string.IsNullOrEmpty(addCustomer.Email)) return BadRequest("Missing email");

        var customer = new Customer
        {
            Id = 0,
            FirstName = addCustomer.FirstName,
            LastName = addCustomer.LastName,
            Email = addCustomer.Email
        };

        await dbContext.Customers.AddAsync(customer);
        await dbContext.SaveChangesAsync();

        return Ok(customer);
    }
    
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, Customer updateCustomer)
    {
        var customer = await dbContext.Customers.FindAsync(id);
        if (string.IsNullOrEmpty(updateCustomer.FirstName)) return BadRequest("Missing first name");
        if (string.IsNullOrEmpty(updateCustomer.LastName)) return BadRequest("Missing last name");
        if (string.IsNullOrEmpty(updateCustomer.Email)) return BadRequest("Missing email");

        if (customer == null) return NotFound();

        customer.FirstName = updateCustomer.FirstName;
        customer.LastName = updateCustomer.LastName;
        customer.Email = updateCustomer.Email;

        await dbContext.SaveChangesAsync();

        return Ok(customer);
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var customer = await dbContext.Customers.FindAsync(id);

        if (customer == null) return NotFound();

        dbContext.Remove(customer);
        await dbContext.SaveChangesAsync();

        return Ok(customer);
    }
}