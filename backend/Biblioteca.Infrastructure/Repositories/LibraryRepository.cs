

using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Biblioteca.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Infrastructure.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly BibliotecaDbContext _context;

        public LibraryRepository(BibliotecaDbContext context)
        {
            _context = context;
        }
        public async Task<BookEntity> AddBookAsync(BookEntity book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task DeleteBookAsync(string code)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Code == code);
            if (book is not null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BookEntity>> GetAllBooksAsync()
        {
            return await _context.Books.OrderByDescending(b => b.CreatedAt).ToListAsync();
        }

        public async Task<int> GetAvailableBooksAsync()
        {
            return await _context.Books.CountAsync(b => b.Status == "DISPONIVEL");
        }

       
        public async Task<BookEntity?> GetBookByCodeAsync(string code)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Code == code);
        }

        public async Task<BookEntity> UpdateBookAsync(BookEntity book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }
    }
}
