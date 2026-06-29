using Microsoft.AspNetCore.Mvc;

namespace Sprint14_MVC.Controllers;

[ApiController]
[Route("api/v2/[controller]")]
public class MoviesApiController : ControllerBase
{
    private static readonly List<Movie> Movies = new()
    {
        new() { Id = 1, Title = "Inception", Year = 2010, Genre = "sci-fi", Rating = 8.8, Director = "Christopher Nolan" },
        new() { Id = 2, Title = "The Dark Knight", Year = 2008, Genre = "action", Rating = 9.0, Director = "Christopher Nolan" },
        new() { Id = 3, Title = "Interstellar", Year = 2014, Genre = "sci-fi", Rating = 8.7, Director = "Christopher Nolan" },
        new() { Id = 4, Title = "Pulp Fiction", Year = 1994, Genre = "crime", Rating = 8.9, Director = "Quentin Tarantino" },
        new() { Id = 5, Title = "The Godfather", Year = 1972, Genre = "crime", Rating = 9.2, Director = "Francis Ford Coppola" }
    };
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var movie = Movies.FirstOrDefault(m=> m.Id == id);
        return movie != null ? Ok(movie) : NotFound();
    }
    
    [HttpGet("genre/{genre}")]
    public IActionResult GetByGenre(string genre)
    {
        var results = Movies.Where(m => m.Genre == genre).ToList();
        return Ok(results);
    }

    public IActionResult GetTopRated(int count)
    {
        var results = Movies.OrderByDescending(m => m.Rating).Take(count).ToList();
        return Ok(results);
    }
}