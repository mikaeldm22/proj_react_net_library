using Biblioteca.Application.DTOs;

namespace Biblioteca.Application.Interfaces;

public interface ILibraryService
{
    Task<IEnumerable<BookDto>> GetAllBooksAsync();
    Task<BookDto?> GetBookByCodeAsync(string code);
    Task<BookStatsDto> GetBookStatsAsync();
    Task<BookDto> CreateBookAsync(CreateBookDto createBookDto);
    Task DeleteBookAsync(string code);
    Task<BookDto> BorrowBookAsync(string code, BorrowBookDto borrowBookDto);
    Task<BookDto> ReturnBookAsync(string code);
}