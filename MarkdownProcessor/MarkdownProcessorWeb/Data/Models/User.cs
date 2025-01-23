using System.ComponentModel.DataAnnotations;

namespace MarkdownProcessorWeb.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }

    public List<Document> AuthoredDocuments { get; set; } = new();
    
    public List<DocumentAccess> DocumentAccesses { get; set; } = new();
}