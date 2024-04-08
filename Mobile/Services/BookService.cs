using Mobile.Constans;
using Mobile.Models;
using Mobile.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mobile.Services;

public class BookService : IBookService
{
    HttpClient _client;
    JsonSerializerOptions _jsonOptions;

    public BookService()
    {
        _client = new HttpClient();
        _jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
    }


    public async Task<IEnumerable<Book>> GetBook()
    {
        IEnumerable<Book>? book = null;
        try
        {
            var response = await _client.GetAsync($"{Configurations.Url}/book");
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                book = await JsonSerializer.DeserializeAsync<IEnumerable<Book>>(responseStream, _jsonOptions);
            }

        }
        catch (Exception ex)
        {
            ex.ToString();
        }

        return book;
    }

    public Task<Book> GetbookById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Book> PostBook(Book book)
    {
        throw new NotImplementedException();
    }

    public Task<Book> PutBook(int id, Book book)
    {
        throw new NotImplementedException();
    }

    public Task<Book> DeleteBook(int id)
    {
        throw new NotImplementedException();
    }
}
