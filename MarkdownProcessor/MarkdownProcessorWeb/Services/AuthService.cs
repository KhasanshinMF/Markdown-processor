using MarkdownProcessorWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Data;

namespace MarkdownProcessorWeb.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<User> RegisterAsync(string email, string username, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Email == email || u.Username == username))
            throw new Exception("Пользователь с таким email или username уже существует.");

        var user = new User
        {
            Email = email,
            Username = username,
            PasswordHash = _passwordHasher.HashPassword(null, password)
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> LoginAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
            throw new Exception("Пользователь не найден");

        var result = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password);

        if (result != PasswordVerificationResult.Success)
            throw new Exception("Неверный пароль");

        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}