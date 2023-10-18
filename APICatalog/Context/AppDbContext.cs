using APICatalog.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalog.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<CategoryModel>? Categories { get; set; }

    public DbSet<ProductModel>? Products { get; set; }    
}
