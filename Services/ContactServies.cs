using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Services;

public class ContactService
{
    private readonly AppDbContext _context;

    public ContactService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ContactMessage> SaveAsync(ContactMessage message)
    {
        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<ContactMessage>> GetAllAsync()
    {
        return await _context.ContactMessages
            .AsNoTracking()
            .OrderByDescending(m => m.Id)
            .ToListAsync();
    }
}
