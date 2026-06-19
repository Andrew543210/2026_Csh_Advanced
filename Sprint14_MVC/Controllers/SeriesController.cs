using Microsoft.AspNetCore.Mvc;
using Sprint14_MVC.Models;

namespace Sprint14_MVC.Controllers;

public class SeriesController : Controller
{
    private static readonly List<TvSeries> Shows = new()
    {
        new() { Id = 1, Title = "Breaking Bad", Slug = "breaking-bad", ReleaseYear = 2008, Rating = 9.5 },
        new() { Id = 2, Title = "Better Call Saul", Slug = "better-call-saul", ReleaseYear = 2015, Rating = 8.9 }
    };

   
    public IActionResult DetailsBySlug(string slug)
    {
        var show = Shows.FirstOrDefault(s => s.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));
        if (show == null) return NotFound("Серіал не знайдено :(");
        
        return Content($"Успіх! Традиційний роутинг знайшов серіал: {show.Title} ({show.ReleaseYear} рік). Рейтинг: {show.Rating}");
    }
}