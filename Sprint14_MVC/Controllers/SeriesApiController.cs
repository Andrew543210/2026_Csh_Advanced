using Microsoft.AspNetCore.Mvc;
using Sprint14_MVC.Models;

namespace Sprint14_MVC.Controllers;

[ApiController]
[Route("api/v1/[controller]")] 
public class SeriesApiController : ControllerBase
{
    private static readonly List<TvSeries> Shows = new()
    {
        new() { Id = 1, Title = "Breaking Bad", Slug = "breaking-bad", ReleaseYear = 2008, Rating = 9.5 },
        new() { Id = 2, Title = "Better Call Saul", Slug = "better-call-saul", ReleaseYear = 2015, Rating = 8.9 }
    };
    
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var show = Shows.FirstOrDefault(s => s.Id == id);
        return show != null ? Ok(show) : NotFound();
    }
    
    [HttpGet("released/{year:int:range(1900,2026)}")]
    public IActionResult GetByYear(int year)
    {
        var results = Shows.Where(s => s.ReleaseYear == year);
        return Ok(results);
    }
}