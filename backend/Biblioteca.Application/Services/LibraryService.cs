

using Biblioteca.Application.DTOs;
using Biblioteca.Application.Interfaces;

namespace Biblioteca.Application.Services
{
    public class LibraryService : ILibraryService
    {

        private readonly ILibraryRepository _repository;
        public Task<BookDto> AddBookAsync(CreateBookDto createBookDto)
        {
            throw new NotImplementedException();
        }

        public Task<BookDto> BorrowBookAsync(int id, BorrowBookDto borrowBookDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BookDto> GetBookByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BookStatsDto> GetBookStatsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BookDto> ReturnBookAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BookDto> UpdateBookAsync(int id, CreateBookDto updateBookDto)
        {
            throw new NotImplementedException();
        }
    }
}
