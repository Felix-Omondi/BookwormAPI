using BookwormAPI.Dtos;
using BookwormAPI.Models;

namespace BookwormAPI.Services;

public interface IBookService
{
    Task<Book> CreateBookAsync(CreateBookDto bookDto);
    Task<Book?> UpdateBookAsync(int id, UpdateBookDto updateBookDto);
    Task<bool> DeleteBookAsync(int id);
    Task<Book?> GetBookByIdAsync(int id);
    Task<List<Book>> GetBooksAsync();
    Task<List<Book>> GetUserBooks(int userId);
}
