

namespace Biblioteca.Domain.Entities
{
    public class BookEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty; 
        public string Author { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? BorrewedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public string? BorrowerName { get; set; }
    }
}
