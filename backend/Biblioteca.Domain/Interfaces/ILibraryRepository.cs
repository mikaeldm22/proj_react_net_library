

using Biblioteca.Domain.Entities;

namespace Biblioteca.Domain.Interfaces
{
    public interface ILibraryRepository
    {
        Task<IEnumerable<BookEntity>> GetAllBooksAsync();
        Task<BookEntity?> GetBookByCodeAsync(string code);
        Task<BookEntity> AddBookAsync(BookEntity book);
        Task<BookEntity> UpdateBookAsync (BookEntity book);
        Task DeleteBookAsync(string code);
        Task<int> GetAvailableBooksAsync();
    }
}
