using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyMvcApp.Data;
using MyMvcApp.Models;

namespace MyMvcApp.Services;

public class BlogService
{
    private readonly AppDbContext _context;

    public BlogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BlogPost>> GetAllPosts()
    {
        return await _context.BlogPosts
            .FromSqlRaw("EXEC sp_GetBlogPosts")
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<BlogPost?> GetPostById(int id)
    {
        var results = await _context.BlogPosts
            .FromSqlRaw("EXEC sp_GetBlogPostById @Id",
                new SqlParameter("@Id", id))
            .AsNoTracking()
            .ToListAsync();
        return results.FirstOrDefault();
    }

    public async Task AddPost(BlogPost post)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_AddBlogPost @Title, @Author, @PublishedOn, @Body",
            new SqlParameter("@Title", post.Title),
            new SqlParameter("@Author", post.Author),
            new SqlParameter("@PublishedOn", post.PublishedOn),
            new SqlParameter("@Body", post.Body));
    }

    public async Task DeletePost(int id)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "EXEC sp_DeleteBlogPost @Id",
            new SqlParameter("@Id", id));
    }
}
