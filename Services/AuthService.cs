using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Services;

public class AuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> ValidateAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null) return null;
        return user.PasswordHash == HashPassword(password) ? user : null;
    }

    public async Task<User?> RegisterAsync(string username, string password, string role = Roles.User)
    {
        if (await _db.Users.AnyAsync(u => u.Username == username))
            return null;

        var user = new User
        {
            Username = username,
            PasswordHash = HashPassword(password),
            Role = role,
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public static string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }
}
