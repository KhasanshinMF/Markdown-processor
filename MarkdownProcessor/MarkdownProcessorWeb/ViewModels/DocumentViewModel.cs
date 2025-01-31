using System.ComponentModel.DataAnnotations;

namespace MarkdownProcessorWeb.ViewModels;

public class DocumentViewModel
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    public bool IsPublic { get; set; }
}