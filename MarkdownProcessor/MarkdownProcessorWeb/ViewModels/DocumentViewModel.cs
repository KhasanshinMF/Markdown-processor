using System.ComponentModel.DataAnnotations;

namespace MarkdownProcessorWeb.ViewModels;

public class DocumentViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Название документа обязательно")]
    [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    public bool IsPublic { get; set; }
}