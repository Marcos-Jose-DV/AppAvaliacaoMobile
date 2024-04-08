using Mobile.Models;

namespace Mobile.Services.Interfaces;

public interface IBookService
{
    Task<IEnumerable<Book>> GetBook();
    Task<Book> GetbookById(int id);
    Task<Book> PostBook(Book book);
    Task<Book> PutBook(int id, Book book);
    Task<Book> DeleteBook(int id);
}
