using MarkdownProcessorWeb.Models;

namespace MarkdownProcessorWeb.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(string email, string username, string password);

    Task<User> LoginAsync(string email, string password);
    
    Task<User> GetUserByEmailAsync(string email);
}