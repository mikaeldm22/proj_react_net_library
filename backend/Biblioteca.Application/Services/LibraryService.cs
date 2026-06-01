

using Biblioteca.Application.DTOs;
using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;

namespace Biblioteca.Application.Services
{
    public class LibraryService : ILibraryService
    {

        private readonly ILibraryRepository _repository;

        public LibraryService(ILibraryRepository repository)
        {
            _repository = repository;
        }

        public async Task<BookDto> BorrowBookAsync(string code, BorrowBookDto borrowBookDto)
        {
            var book = await _repository.GetBookByCodeAsync(code);

            if (book == null)
            {
                throw new KeyNotFoundException($"O Livro com o código {code} não foi encontrado.");
            }

            if (book.Status == "EMPRESTADO")
            {
                throw new InvalidOperationException($"O Livro com o código {code} já está emprestado.");
            }

            book.Status = "EMPRESTADO";
            book.BorrewedAt = DateTime.UtcNow;
            book.BorrowerName = borrowBookDto.BorrowerName;

            var updatedBook = await _repository.UpdateBookAsync(book);
            return MapToBookDto(updatedBook);

        }

        public async Task<BookDto> CreateBookAsync(CreateBookDto createBookDto)
        {
            var book = new BookEntity
            {
                Code = createBookDto.Code,
                Title = createBookDto.Title,
                Author = createBookDto.Author,
                Publisher = createBookDto.Publisher,
                Genre = createBookDto.Genre,
                Year = createBookDto.Year,
                Status = "DISPONIVEL",
                CreatedAt = DateTime.UtcNow
            };

            var createdBook = await _repository.AddBookAsync(book);
            return MapToBookDto(createdBook);
        }

        public async Task DeleteBookAsync(string code)
        {
            await _repository.DeleteBookAsync(code);
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _repository.GetAllBooksAsync();
            return books.Select(MapToBookDto);
        }

        public async Task<BookDto?> GetBookByCodeAsync(string code)
        {
            var book = await _repository.GetBookByCodeAsync(code);
            return book is null ? null : MapToBookDto(book);
        }

        public async Task<BookStatsDto> GetBookStatsAsync()
        {
            var books = await _repository.GetAllBooksAsync();
            var availableBooks = await _repository.GetAvailableBooksAsync();

            return new BookStatsDto
            {
                TotalBooks = books.Count(),
                AvailableBooks = availableBooks,
                BorrowedBooks = books.Count() - availableBooks
            };
        }


        public async Task<BookDto> ReturnBookAsync(string code)
        {
            var book = await _repository.GetBookByCodeAsync(code);

            if (book == null)
            {
                throw new KeyNotFoundException($"O Livro com o código {code} não foi encontrado.");
            }

            if (book.Status == "DISPONIVEL")
            {
                throw new InvalidOperationException($"O Livro com o código {code} já foi devolvido.");
            }

            book.Status = "DISPONIVEL";
            book.ReturnedAt = DateTime.UtcNow;
            book.BorrowerName = null;

            var updatedBook = await _repository.UpdateBookAsync(book);
            return MapToBookDto(updatedBook);
        }


        private static BookDto MapToBookDto(BookEntity book)
        {
            return new BookDto
            {
                Code = book.Code,
                Title = book.Title,
                Author = book.Author,
                Year = book.Year,
                Status = book.Status,
                Genre = book.Genre,
                Publisher = book.Publisher,
                BorrowedAt = book.BorrewedAt,
                BorrowerName = book.BorrowerName,
                CreatedAt = book.CreatedAt,
                ReturnedAt = book.ReturnedAt
            };
        }

    }
}
