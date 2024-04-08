﻿using Mobile.Constans;
using Mobile.Models;
using Mobile.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Mobile.Services;

public class MovieService : IMovieService
{
    HttpClient _client;
    JsonSerializerOptions _jsonOptions;

    public MovieService()
    {
        _client = new HttpClient();
        _jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
    }
    public async Task<IEnumerable<Movie>> GetMovies()
    {
        IEnumerable<Movie>? movie = null;
        try
        {
            var response = await _client.GetAsync($"{Configurations.Url}/movies");
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                movie = await JsonSerializer.DeserializeAsync<IEnumerable<Movie>>(responseStream, _jsonOptions);
            }

        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return movie;

    }

    public Task<Movie> GetMovieById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Movie> PostMovie(Movie movie)
    {
        throw new NotImplementedException();
    }

    public async Task<Movie> PutMovie(int id, Movie obj)
    {
        Movie? movieUpdate = null;
        try
        {

            string jsonRespponse = JsonSerializer.Serialize<object>(obj, _jsonOptions);

            StringContent content = new StringContent(jsonRespponse, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{Configurations.Url}/movie/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                movieUpdate = await JsonSerializer.DeserializeAsync<Movie>(responseStream, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return movieUpdate;

    }
    public async Task<Movie> DeleteMovie(int id)
    {
        Movie? movie = null;
        try
        {
            var response = await _client.DeleteAsync($"{Configurations.Url}/movie/{id}");
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                movie = await JsonSerializer.DeserializeAsync<Movie>(responseStream, _jsonOptions);
            }

        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return movie;
    }
}
