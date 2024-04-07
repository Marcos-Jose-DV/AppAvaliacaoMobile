
using Mobile.Models;

namespace Mobile.Services.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetMovies();
    Task<Movie> GetMovieById(int id);
    Task<Movie> PostMovie(Movie movie);
    Task<Movie> PutMovie(int id, Movie movie);
    Task<Movie> DeleteMovie(int id);
}
