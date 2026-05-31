using Biblioteca.Domain.Enums;

namespace Biblioteca.Application.DTOs;

public class BookDto
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int Year { get; set; }
    public BookStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? BorrowedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public string? BorrowerName { get; set; }


}