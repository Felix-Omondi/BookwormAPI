using BookwormAPI.Data;
using BookwormAPI.Dtos;
using BookwormAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookwormAPI.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _context;
    private readonly IUserService _userService;

    public BookService(AppDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<Book> CreateBookAsync(CreateBookDto bookDto)
    {
        var newBook = new Book
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            Genre = bookDto.Genre,
            Status = bookDto.Status,
            UserId = bookDto.UserId,
        };

        _context.Books.Add(newBook);
        await _context.SaveChangesAsync();
        return newBook;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book is null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        return book;
    }

    public async Task<List<Book>> GetBooksAsync()
    {
        var books = await _context.Books.ToListAsync();
        return books;
    }

    public async Task<List<Book>> GetUserBooks(int userId)
    {
        List<Book> books = [];

        var user = await _userService.GetUserByIdAsync(userId);
        if (user is not null)
        {
            books.AddRange(user.Books);
        }
        return books;
    }

    public async Task<Book?> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
    {
        var book = await _context.Books.FindAsync(id);

        if (book is not null)
        {
            book.Title = updateBookDto.Title ?? book.Title;
            book.Author = updateBookDto.Author ?? book.Author;
            book.Genre = updateBookDto.Genre ?? book.Genre;
            book.Status = updateBookDto.Status ?? book.Status;
            book.UpdatedAt = DateTime.UtcNow;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
        return book;
    }
}
