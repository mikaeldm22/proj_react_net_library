namespace Biblioteca.Application.DTOs;

public class BookStatsDto
{
    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public int BorrowedBooks { get; set; }
}