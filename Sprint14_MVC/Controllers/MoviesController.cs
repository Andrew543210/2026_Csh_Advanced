using Microsoft.AspNetCore.Mvc;

namespace Sprint14_MVC.Controllers;

public class MoviesController : Controller
{
    
    private static readonly List<Movie> Movies = new()
    {
        new() { Id = 1, Title = "Inception", Year = 2010, Genre = "sci-fi", Rating = 8.8, Director = "Christopher Nolan" },
        new() { Id = 2, Title = "The Dark Knight", Year = 2008, Genre = "action", Rating = 9.0, Director = "Christopher Nolan" },
        new() { Id = 3, Title = "Interstellar", Year = 2014, Genre = "sci-fi", Rating = 8.7, Director = "Christopher Nolan" },
        new() { Id = 4, Title = "Pulp Fiction", Year = 1994, Genre = "crime", Rating = 8.9, Director = "Quentin Tarantino" },
        new() { Id = 5, Title = "The Godfather", Year = 1972, Genre = "crime", Rating = 9.2, Director = "Francis Ford Coppola" }
    };
    public IActionResult FilterByYearAndGenre(int year, string genre)
    {
        var filtered = Movies.Where(m => m.Year == year && (genre == "all" || m.Genre == genre));
        
        if (!filtered.Any())
            return Content($"No movies found for {year} and genre {genre}");
        
        return Content($"Movies found: {filtered.Count()}, Movies: {string.Join("\n", filtered.Select(m => $"{m.Title} ({m.Year}) - {m.Genre}, Rating: {m.Rating}, Director: {m.Director}"))}");
    }
}