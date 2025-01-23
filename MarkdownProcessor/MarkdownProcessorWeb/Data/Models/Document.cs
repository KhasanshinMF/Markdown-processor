using System.ComponentModel.DataAnnotations;

namespace MarkdownProcessorWeb.Models;

public class Document
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Content { get; set; }
    
    public bool IsPublic { get; set; }

    public int AuthorId { get; set; }
    
    public User Author { get; set; }

    public List<DocumentAccess> DocumentAccesses { get; set; } = new();
}