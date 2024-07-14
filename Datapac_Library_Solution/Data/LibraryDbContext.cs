using Datapac_Library_Solution.Models;
using Microsoft.EntityFrameworkCore;

namespace Datapac_Library_Solution.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Borrowing>()
            .HasOne<Book>()
            .WithMany()
            .HasForeignKey(v => v.BookId)
            .IsRequired();
        //.OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Borrowing>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(v => v.CustomerId)
            .IsRequired();
        //.OnDelete(DeleteBehavior.Restrict);
    }
}