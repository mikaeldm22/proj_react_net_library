

using Biblioteca.Domain.Enums;
using System.Data;

namespace Biblioteca.Domain.Entities
{
    public class BookEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty; public string Author { get; set; } = string.Empty;
        public int Year { get; set; }
        public BookStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? BorrewedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public string? BorrowerName { get; set; }
    }
}
