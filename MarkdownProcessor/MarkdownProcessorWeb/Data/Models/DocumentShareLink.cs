using System.ComponentModel.DataAnnotations;

namespace MarkdownProcessorWeb.Models;

public class DocumentShareLink
{
    [Key]
    public int Id { get; set; }
    
    public string Token { get; set; }
    
    public int DocumentId { get; set; }
    
    public Document Document { get; set; }
}