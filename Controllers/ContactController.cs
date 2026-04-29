using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Send(ContactMessage form)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), form);
        }

        TempData["ContactSuccess"] = $"Thanks {form.Name}, we'll be in touch at {form.Email}.";
        return RedirectToAction(nameof(Index));
    }
}
