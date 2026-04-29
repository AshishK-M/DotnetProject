using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class BlogController : Controller
{
    private static readonly List<BlogPost> Posts = new()
    {
        new BlogPost { Id = 1, Title = "Welcome to the Blog", Author = "Team", PublishedOn = new DateTime(2026, 1, 15), Body = "This is our very first post. Stay tuned for more updates." },
        new BlogPost { Id = 2, Title = "Shipping Faster with MVC", Author = "Team", PublishedOn = new DateTime(2026, 2, 10), Body = "A few patterns we use to keep iteration speed high." },
        new BlogPost { Id = 3, Title = "Designing Clean Views", Author = "Team", PublishedOn = new DateTime(2026, 3, 22), Body = "Tips for keeping Razor templates simple and maintainable." }
    };

    public IActionResult Index()
    {
        return View(Posts.OrderByDescending(p => p.PublishedOn).ToList());
    }

    public IActionResult Post(int id)
    {
        var post = Posts.FirstOrDefault(p => p.Id == id);
        if (post is null)
        {
            return NotFound();
        }
        return View(post);
    }
}
