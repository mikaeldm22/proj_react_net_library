

using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Interfaces
{
    public interface ILibraryRepository
    {
        Task<IEnumerable<BookEntity>> GetAllBooksAsync();
        Task<BookEntity?> GetBookByCodeAsync(int code);
        Task<BookEntity> AddBookAsync(BookEntity book);
        Task<BookEntity> UpdateBookAsync (BookEntity book);
        Task DeleteBookAsync(int code);
        Task<int> GetAvailableBooksByTitleAsync(string title);
        Task<int> GetBooksByTitleAsync(string title);
    }
}
