using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;
using MyMvcApp.Services;

namespace MyMvcApp.Controllers;

public class BlogController : Controller
{
    private readonly BlogService _blogService;

    public BlogController(BlogService blogService)
    {
        _blogService = blogService;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _blogService.GetAllPosts();
        return View(posts);
    }

    public async Task<IActionResult> Post(int id)
    {
        var post = await _blogService.GetPostById(id);
        if (post is null) return NotFound();
        return View(post);
    }

    [Authorize(Roles = Roles.Admin)]
    public IActionResult Create()
    {
        return View(new BlogPost { PublishedOn = DateTime.Today });
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogPost post)
    {
        if (!ModelState.IsValid) return View(post);

        await _blogService.AddPost(post);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _blogService.GetPostById(id);
        if (post is null) return NotFound();
        return View(post);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = Roles.Admin)]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _blogService.DeletePost(id);
        return RedirectToAction(nameof(Index));
    }
}
