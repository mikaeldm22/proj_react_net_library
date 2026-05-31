using Biblioteca.Application.DTOs;
using Biblioteca.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase
{
    private readonly ILogger<LibraryController> _logger;
    private readonly ILibraryService _libraryService;

    public LibraryController(ILogger<LibraryController> logger, ILibraryService libraryService)
    {
        _logger = logger;
        _libraryService = libraryService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
    {
        var books = await _libraryService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{code}")]
    public async Task<ActionResult<BookDto>> GetBookByCode(string code)
    {
        var book = await _libraryService.GetBookByCodeAsync(code);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _libraryService.CreateBookAsync(createBookDto);
        return CreatedAtAction(nameof(GetBookByCode), new { code = createBookDto.Code }, createBookDto);
    }

    [HttpPatch("{code}/borrow")]
    public async Task<ActionResult> BorrowBook(string code, [FromBody] BorrowBookDto borrowBookDto)
    {
        try
        {
            var book = await _libraryService.BorrowBookAsync(code, borrowBookDto);
            return Ok(book);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new {message = ex.Message });
        }
    }

    [HttpPatch("{code}/return")]
    public async Task<ActionResult> ReturnBookAsync(string code)
    {
        try
        {
            var book = await _libraryService.ReturnBookAsync(code);
            return Ok(book);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("stats")]
    public async Task<ActionResult<BookStatsDto>> GetLibraryStats()
    {
        var stats = await _libraryService.GetBookStatsAsync();
        return Ok(stats);
    }
}