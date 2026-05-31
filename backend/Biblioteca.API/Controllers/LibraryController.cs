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
    public IActionResult Get()
    {
        return Ok("Welcome to the Library API!");
    }
}