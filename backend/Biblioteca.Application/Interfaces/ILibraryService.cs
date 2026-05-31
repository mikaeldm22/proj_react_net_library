using Biblioteca.Application.DTOs;

namespace Biblioteca.Application.Interfaces;

public interface ILibraryService
{
    Task<IEnumerable<BookDto>> GetAllBooksAsync();
    Task<BookDto> GetBookByIdAsync(int id);
    Task<BookStatsDto> GetBookStatsAsync();
    Task<BookDto> AddBookAsync(CreateBookDto createBookDto);
    Task<BookDto> UpdateBookAsync(int id, CreateBookDto updateBookDto);
    Task<bool> DeleteBookAsync(int id);
    Task<BookDto> BorrowBookAsync(int id, BorrowBookDto borrowBookDto);
    Task<BookDto> ReturnBookAsync(int id);
}