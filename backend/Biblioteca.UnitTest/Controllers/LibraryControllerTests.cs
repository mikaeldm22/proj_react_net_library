using Biblioteca.API.Controllers;
using Biblioteca.Application.DTOs;
using Biblioteca.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Biblioteca.UnitTest.Controllers;

[TestClass]
public class LibraryControllerTests
{
    private Mock<ILibraryService> _mockLibraryService = null!;
    private Mock<ILogger<LibraryController>> _mockLogger = null!;
    private LibraryController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockLibraryService = new Mock<ILibraryService>();
        _mockLogger = new Mock<ILogger<LibraryController>>();
        _controller = new LibraryController(_mockLogger.Object, _mockLibraryService.Object);
    }

    #region GetAllBooks Tests

    [TestMethod]
    public async Task GetAllBooks_Should_Return_Ok_With_All_Books()
    {
        // Arrange
        var books = new List<BookDto>
        {
            new BookDto
            {
                Code = "001",
                Title = "Test Book 1",
                Author = "Author 1",
                Publisher = "Publisher 1",
                Genre = "Fiction",
                Year = 2020,
                CreatedAt = DateTime.UtcNow
            },
            new BookDto
            {
                Code = "002",
                Title = "Test Book 2",
                Author = "Author 2",
                Publisher = "Publisher 2",
                Genre = "Non-Fiction",
                Year = 2021,
                CreatedAt = DateTime.UtcNow
            }
        };

        _mockLibraryService.Setup(s => s.GetAllBooksAsync()).ReturnsAsync(books);

        // Act
        var result = await _controller.GetAllBooks();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedBooks = okResult.Value as IEnumerable<BookDto>;
        Assert.IsNotNull(returnedBooks);
        Assert.AreEqual(2, returnedBooks.Count());

        _mockLibraryService.Verify(s => s.GetAllBooksAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetAllBooks_Should_Return_Ok_With_Empty_List()
    {
        // Arrange
        var books = new List<BookDto>();
        _mockLibraryService.Setup(s => s.GetAllBooksAsync()).ReturnsAsync(books);

        // Act
        var result = await _controller.GetAllBooks();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedBooks = okResult.Value as IEnumerable<BookDto>;
        Assert.IsNotNull(returnedBooks);
        Assert.AreEqual(0, returnedBooks.Count());
    }

    #endregion

    #region GetBookByCode Tests

    [TestMethod]
    public async Task GetBookByCode_Should_Return_Ok_When_Book_Found()
    {
        // Arrange
        var code = "001";
        var book = new BookDto
        {
            Code = code,
            Title = "Test Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2020,
            CreatedAt = DateTime.UtcNow
        };

        _mockLibraryService.Setup(s => s.GetBookByCodeAsync(code)).ReturnsAsync(book);

        // Act
        var result = await _controller.GetBookByCode(code);

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedBook = okResult.Value as BookDto;
        Assert.IsNotNull(returnedBook);
        Assert.AreEqual(code, returnedBook.Code);
        Assert.AreEqual("Test Book", returnedBook.Title);

        _mockLibraryService.Verify(s => s.GetBookByCodeAsync(code), Times.Once);
    }

    [TestMethod]
    public async Task GetBookByCode_Should_Return_NotFound_When_Book_Not_Exists()
    {
        // Arrange
        var code = "999";
        _mockLibraryService.Setup(s => s.GetBookByCodeAsync(code)).ReturnsAsync((BookDto?)null);

        // Act
        var result = await _controller.GetBookByCode(code);

        // Assert
        var notFoundResult = result.Result as NotFoundResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);

        _mockLibraryService.Verify(s => s.GetBookByCodeAsync(code), Times.Once);
    }

    #endregion

    #region CreateBook Tests

    [TestMethod]
    public async Task CreateBook_Should_Return_CreatedAtAction_When_Book_Created()
    {
        // Arrange
        var createBookDto = new CreateBookDto
        {
            Code = "001",
            Title = "New Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2024
        };

        var createdBookDto = new BookDto
        {
            Code = "001",
            Title = "New Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2024,
            CreatedAt = DateTime.UtcNow
        };

        _mockLibraryService.Setup(s => s.CreateBookAsync(It.IsAny<CreateBookDto>()))
            .ReturnsAsync(createdBookDto);

        // Act
        var result = await _controller.CreateBook(createBookDto);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual(201, createdResult.StatusCode);
        Assert.AreEqual(nameof(LibraryController.GetBookByCode), createdResult.ActionName);
        Assert.AreEqual(createBookDto.Code, ((dynamic?)createdResult.RouteValues?["code"])?.ToString());

        _mockLibraryService.Verify(s => s.CreateBookAsync(It.IsAny<CreateBookDto>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateBook_Should_Return_BadRequest_When_ModelState_Invalid()
    {
        // Arrange
        var createBookDto = new CreateBookDto();
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.CreateBook(createBookDto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);

        _mockLibraryService.Verify(s => s.CreateBookAsync(It.IsAny<CreateBookDto>()), Times.Never);
    }

    #endregion

    #region BorrowBook Tests

    [TestMethod]
    public async Task BorrowBook_Should_Return_Ok_When_Book_Borrowed_Successfully()
    {
        // Arrange
        var code = "001";
        var borrowDto = new BorrowBookDto { BorrowerName = "John Doe" };

        var borrowedBook = new BookDto
        {
            Code = code,
            Title = "Test Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2020,
            BorrowerName = "John Doe",
            BorrowedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _mockLibraryService.Setup(s => s.BorrowBookAsync(code, borrowDto))
            .ReturnsAsync(borrowedBook);

        // Act
        var result = await _controller.BorrowBook(code, borrowDto);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedBook = okResult.Value as BookDto;
        Assert.IsNotNull(returnedBook);
        Assert.AreEqual("John Doe", returnedBook.BorrowerName);

        _mockLibraryService.Verify(s => s.BorrowBookAsync(code, borrowDto), Times.Once);
    }

    [TestMethod]
    public async Task BorrowBook_Should_Return_NotFound_When_Book_Not_Found()
    {
        // Arrange
        var code = "999";
        var borrowDto = new BorrowBookDto { BorrowerName = "John Doe" };

        _mockLibraryService.Setup(s => s.BorrowBookAsync(code, borrowDto))
            .ThrowsAsync(new KeyNotFoundException($"O Livro com o código {code} não foi encontrado."));

        // Act
        var result = await _controller.BorrowBook(code, borrowDto);

        // Assert
        var notFoundResult = result as NotFoundResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);

        _mockLibraryService.Verify(s => s.BorrowBookAsync(code, borrowDto), Times.Once);
    }

    [TestMethod]
    public async Task BorrowBook_Should_Return_BadRequest_When_Book_Already_Borrowed()
    {
        // Arrange
        var code = "001";
        var borrowDto = new BorrowBookDto { BorrowerName = "Jane Doe" };
        var errorMessage = $"O Livro com o código {code} já está emprestado.";

        _mockLibraryService.Setup(s => s.BorrowBookAsync(code, borrowDto))
            .ThrowsAsync(new InvalidOperationException(errorMessage));

        // Act
        var result = await _controller.BorrowBook(code, borrowDto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);

        _mockLibraryService.Verify(s => s.BorrowBookAsync(code, borrowDto), Times.Once);
    }

    #endregion

    #region ReturnBook Tests

    [TestMethod]
    public async Task ReturnBook_Should_Return_Ok_When_Book_Returned_Successfully()
    {
        // Arrange
        var code = "001";

        var returnedBook = new BookDto
        {
            Code = code,
            Title = "Test Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2020,
            BorrowerName = null,
            ReturnedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _mockLibraryService.Setup(s => s.ReturnBookAsync(code))
            .ReturnsAsync(returnedBook);

        // Act
        var result = await _controller.ReturnBookAsync(code);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedBookDto = okResult.Value as BookDto;
        Assert.IsNotNull(returnedBookDto);
        Assert.IsNull(returnedBookDto.BorrowerName);

        _mockLibraryService.Verify(s => s.ReturnBookAsync(code), Times.Once);
    }

    [TestMethod]
    public async Task ReturnBook_Should_Return_NotFound_When_Book_Not_Found()
    {
        // Arrange
        var code = "999";

        _mockLibraryService.Setup(s => s.ReturnBookAsync(code))
            .ThrowsAsync(new KeyNotFoundException($"O Livro com o código {code} não foi encontrado."));

        // Act
        var result = await _controller.ReturnBookAsync(code);

        // Assert
        var notFoundResult = result as NotFoundResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);

        _mockLibraryService.Verify(s => s.ReturnBookAsync(code), Times.Once);
    }

    [TestMethod]
    public async Task ReturnBook_Should_Return_BadRequest_When_Book_Already_Available()
    {
        // Arrange
        var code = "001";
        var errorMessage = $"O Livro com o código {code} já foi devolvido.";

        _mockLibraryService.Setup(s => s.ReturnBookAsync(code))
            .ThrowsAsync(new InvalidOperationException(errorMessage));

        // Act
        var result = await _controller.ReturnBookAsync(code);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);

        _mockLibraryService.Verify(s => s.ReturnBookAsync(code), Times.Once);
    }

    #endregion

    #region GetLibraryStats Tests

    [TestMethod]
    public async Task GetLibraryStats_Should_Return_Ok_With_Stats()
    {
        // Arrange
        var stats = new BookStatsDto
        {
            TotalBooks = 10,
            AvailableBooks = 7,
            BorrowedBooks = 3
        };

        _mockLibraryService.Setup(s => s.GetBookStatsAsync()).ReturnsAsync(stats);

        // Act
        var result = await _controller.GetLibraryStats();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedStats = okResult.Value as BookStatsDto;
        Assert.IsNotNull(returnedStats);
        Assert.AreEqual(10, returnedStats.TotalBooks);
        Assert.AreEqual(7, returnedStats.AvailableBooks);
        Assert.AreEqual(3, returnedStats.BorrowedBooks);

        _mockLibraryService.Verify(s => s.GetBookStatsAsync(), Times.Once);
    }

    #endregion
}
