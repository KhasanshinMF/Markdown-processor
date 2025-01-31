using Microsoft.EntityFrameworkCore;
using MarkdownProcessorWeb.Models;

namespace Web.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentAccess> DocumentAccesses { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>()
            .HasOne(d => d.Author)
            .WithMany(u => u.AuthoredDocuments)
            .HasForeignKey(d => d.AuthorId);
        
        modelBuilder.Entity<DocumentAccess>()
            .HasKey(da => new { da.DocumentId, da.UserId });
        
        modelBuilder.Entity<DocumentAccess>()
            .HasOne(da => da.Document)
            .WithMany(d => d.DocumentAccesses)
            .HasForeignKey(da => da.DocumentId);

        modelBuilder.Entity<DocumentAccess>()
            .HasOne(da => da.User)
            .WithMany()
            .HasForeignKey(da => da.UserId);
        
        base.OnModelCreating(modelBuilder);
    }
}