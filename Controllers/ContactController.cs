using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;
using MyMvcApp.Services;

namespace MyMvcApp.Controllers;

public class ContactController : Controller
{
    private readonly ContactService _contactService;

    public ContactController(ContactService contactService)
    {
        _contactService = contactService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(ContactMessage form)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), form);
        }

        await _contactService.SaveAsync(form);

        TempData["ContactSuccess"] = $"Thanks {form.Name}, we'll be in touch at {form.Email}.";
        return RedirectToAction(nameof(Index));
    }
}
