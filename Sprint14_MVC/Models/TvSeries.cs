namespace Sprint14_MVC.Models;

public class TvSeries
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty; 
    public int ReleaseYear { get; set; }
    public double Rating { get; set; }
}