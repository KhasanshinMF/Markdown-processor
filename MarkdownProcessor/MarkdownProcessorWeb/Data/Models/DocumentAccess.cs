namespace MarkdownProcessorWeb.Models;

public class DocumentAccess
{
    public int DocumentId { get; set; }
    
    public Document Document { get; set; }
    
    public int UserId { get; set; }
    
    public User User { get; set; }
    
    public AccessLevel AccessLevel { get; set; }
}

public enum AccessLevel
{
    Reader,
    Editor
}