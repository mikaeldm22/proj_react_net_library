using Biblioteca.Application.DTOs;
using Biblioteca.Application.Services;
using Biblioteca.Domain.Entities;
using Biblioteca.Domain.Interfaces;
using Moq;

namespace Biblioteca.UnitTest.Services;

[TestClass]
public class LibraryServiceTests
{
    private Mock<ILibraryRepository> _mockRepository = null!;
    private LibraryService _libraryService = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ILibraryRepository>();
        _libraryService = new LibraryService(_mockRepository.Object);
    }

    #region GetAllBooksAsync Tests

    [TestMethod]
    public async Task GetAllBooksAsync_Should_Return_All_Books()
    {
        // Arrange
        var books = new List<BookEntity>
        {
            new BookEntity
            {
                Id = Guid.NewGuid(),
                Code = "001",
                Title = "Test Book 1",
                Author = "Author 1",
                Publisher = "Publisher 1",
                Genre = "Fiction",
                Year = 2020,
                Status = "DISPONIVEL",
                CreatedAt = DateTime.UtcNow
            },
            new BookEntity
            {
                Id = Guid.NewGuid(),
                Code = "002",
                Title = "Test Book 2",
                Author = "Author 2",
                Publisher = "Publisher 2",
                Genre = "Non-Fiction",
                Year = 2021,
                Status = "EMPRESTADO",
                CreatedAt = DateTime.UtcNow
            }
        };

        _mockRepository.Setup(r => r.GetAllBooksAsync()).ReturnsAsync(books);

        // Act
        var result = await _libraryService.GetAllBooksAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        _mockRepository.Verify(r => r.GetAllBooksAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetAllBooksAsync_Should_Return_Empty_List_When_No_Books()
    {
        // Arrange
        var books = new List<BookEntity>();
        _mockRepository.Setup(r => r.GetAllBooksAsync()).ReturnsAsync(books);

        // Act
        var result = await _libraryService.GetAllBooksAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count());
        _mockRepository.Verify(r => r.GetAllBooksAsync(), Times.Once);
    }

    #endregion

    #region GetBookByCodeAsync Tests

    [TestMethod]
    public async Task GetBookByCodeAsync_Should_Return_Book_When_Found()
    {
        // Arrange
        var code = "001";
        var book = new BookEntity
        {
            Id = Guid.NewGuid(),
            Code = code,
            Title = "Test Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2020,
            Status = "DISPONIVEL",
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetBookByCodeAsync(code)).ReturnsAsync(book);

        // Act
        var result = await _libraryService.GetBookByCodeAsync(code);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(code, result?.Code);
        Assert.AreEqual("Test Book", result?.Title);
        _mockRepository.Verify(r => r.GetBookByCodeAsync(code), Times.Once);
    }

    [TestMethod]
    public async Task GetBookByCodeAsync_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        var code = "999";
        _mockRepository.Setup(r => r.GetBookByCodeAsync(code)).ReturnsAsync((BookEntity?)null);

        // Act
        var result = await _libraryService.GetBookByCodeAsync(code);

        // Assert
        Assert.IsNull(result);
        _mockRepository.Verify(r => r.GetBookByCodeAsync(code), Times.Once);
    }

    #endregion

    #region CreateBookAsync Tests

    [TestMethod]
    public async Task CreateBookAsync_Should_Create_Book_With_Available_Status()
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

        var createdBook = new BookEntity
        {
            Id = Guid.NewGuid(),
            Code = createBookDto.Code,
            Title = createBookDto.Title,
            Author = createBookDto.Author,
            Publisher = createBookDto.Publisher,
            Genre = createBookDto.Genre,
            Year = createBookDto.Year,
            Status = "DISPONIVEL",
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.AddBookAsync(It.IsAny<BookEntity>())).ReturnsAsync(createdBook);

        // Act
        var result = await _libraryService.CreateBookAsync(createBookDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(createBookDto.Code, result.Code);
        Assert.AreEqual(createBookDto.Title, result.Title);
        Assert.AreEqual("DISPONIVEL", result.Status);
        _mockRepository.Verify(r => r.AddBookAsync(It.IsAny<BookEntity>()), Times.Once);
    }

    #endregion

    #region BorrowBookAsync Tests

    [TestMethod]
    public async Task BorrowBookAsync_Should_Update_Book_Status_To_Borrowed()
    {
        // Arrange
        var code = "001";
        var borrowerName = "John Doe";
        var borrowDto = new BorrowBookDto { BorrowerName = borrowerName };

        var book = new BookEntity
        {
            Id = Guid.NewGuid(),
            Code = code,
            Title = "Test Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2020,
            Status = "DISPONIVEL",
            CreatedAt = DateTime.UtcNow
        };

        var updatedBook = new BookEntity
        {
            Id = book.Id,
            Code = code,
            Title = "Test Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2020,
            Status = "EMPRESTADO",
            BorrewedAt = DateTime.UtcNow,
            BorrowerName = borrowerName,
            CreatedAt = book.CreatedAt
        };

        _mockRepository.Setup(r => r.GetBookByCodeAsync(code)).ReturnsAsync(book);
        _mockRepository.Setup(r => r.UpdateBookAsync(It.IsAny<BookEntity>())).ReturnsAsync(updatedBook);

        // Act
        var result = await _libraryService.BorrowBookAsync(code, borrowDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("EMPRESTADO", result.Status);
        Assert.AreEqual(borrowerName, result.BorrowerName);
        _mockRepository.Verify(r => r.GetBookByCodeAsync(code), Times.Once);
        _mockRepository.Verify(r => r.UpdateBookAsync(It.IsAny<BookEntity>()), Times.Once);
    }

    [TestMethod]
    public async Task BorrowBookAsync_Should_Throw_KeyNotFoundException_When_Book_Not_Found()
    {
        // Arrange
        var code = "999";
        var borrowDto = new BorrowBookDto { BorrowerName = "John Doe" };

        _mockRepository.Setup(r => r.GetBookByCodeAsync(code)).ReturnsAsync((BookEntity?)null);

        // Act & Assert
        try
        {
            await _libraryService.BorrowBookAsync(code, borrowDto);
            Assert.Fail("Expected KeyNotFoundException to be thrown");
        }
        catch (KeyNotFoundException)
        {
            // Expected behavior
            _mockRepository.Verify(r => r.GetBookByCodeAsync(code), Times.Once);
            _mockRepository.Verify(r => r.UpdateBookAsync(It.IsAny<BookEntity>()), Times.Never);
        }
    }

    [TestMethod]
    public async Task BorrowBookAsync_Should_Throw_InvalidOperationException_When_Book_Already_Borrowed()
    {
        // Arrange
        var code = "001";
        var borrowDto = new BorrowBookDto { BorrowerName = "Jane Doe" };

        var book = new BookEntity
        {
            Id = Guid.NewGuid(),
            Code = code,
            Title = "Test Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2020,
            Status = "EMPRESTADO",
            BorrowerName = "John Doe",
            BorrewedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetBookByCodeAsync(code)).ReturnsAsync(book);

        // Act & Assert
        try
        {
            await _libraryService.BorrowBookAsync(code, borrowDto);
            Assert.Fail("Expected InvalidOperationException to be thrown");
        }
        catch (InvalidOperationException)
        {
            // Expected behavior
            _mockRepository.Verify(r => r.GetBookByCodeAsync(code), Times.Once);
            _mockRepository.Verify(r => r.UpdateBookAsync(It.IsAny<BookEntity>()), Times.Never);
        }
    }

    #endregion

    #region ReturnBookAsync Tests

    [TestMethod]
    public async Task ReturnBookAsync_Should_Update_Book_Status_To_Available()
    {
        // Arrange
        var code = "001";
        var book = new BookEntity
        {
            Id = Guid.NewGuid(),
            Code = code,
            Title = "Test Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2020,
            Status = "EMPRESTADO",
            BorrowerName = "John Doe",
            BorrewedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        var returnedBook = new BookEntity
        {
            Id = book.Id,
            Code = code,
            Title = "Test Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2020,
            Status = "DISPONIVEL",
            BorrowerName = null,
            ReturnedAt = DateTime.UtcNow,
            CreatedAt = book.CreatedAt
        };

        _mockRepository.Setup(r => r.GetBookByCodeAsync(code)).ReturnsAsync(book);
        _mockRepository.Setup(r => r.UpdateBookAsync(It.IsAny<BookEntity>())).ReturnsAsync(returnedBook);

        // Act
        var result = await _libraryService.ReturnBookAsync(code);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("DISPONIVEL", result.Status);
        Assert.IsNull(result.BorrowerName);
        _mockRepository.Verify(r => r.GetBookByCodeAsync(code), Times.Once);
        _mockRepository.Verify(r => r.UpdateBookAsync(It.IsAny<BookEntity>()), Times.Once);
    }

    [TestMethod]
    public async Task ReturnBookAsync_Should_Throw_KeyNotFoundException_When_Book_Not_Found()
    {
        // Arrange
        var code = "999";
        _mockRepository.Setup(r => r.GetBookByCodeAsync(code)).ReturnsAsync((BookEntity?)null);

        // Act & Assert
        try
        {
            await _libraryService.ReturnBookAsync(code);
            Assert.Fail("Expected KeyNotFoundException to be thrown");
        }
        catch (KeyNotFoundException)
        {
            // Expected behavior
            _mockRepository.Verify(r => r.GetBookByCodeAsync(code), Times.Once);
            _mockRepository.Verify(r => r.UpdateBookAsync(It.IsAny<BookEntity>()), Times.Never);
        }
    }

    [TestMethod]
    public async Task ReturnBookAsync_Should_Throw_InvalidOperationException_When_Book_Already_Available()
    {
        // Arrange
        var code = "001";
        var book = new BookEntity
        {
            Id = Guid.NewGuid(),
            Code = code,
            Title = "Test Book",
            Author = "Author",
            Publisher = "Publisher",
            Genre = "Fiction",
            Year = 2020,
            Status = "DISPONIVEL",
            CreatedAt = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetBookByCodeAsync(code)).ReturnsAsync(book);

        // Act & Assert
        try
        {
            await _libraryService.ReturnBookAsync(code);
            Assert.Fail("Expected InvalidOperationException to be thrown");
        }
        catch (InvalidOperationException)
        {
            // Expected behavior
            _mockRepository.Verify(r => r.GetBookByCodeAsync(code), Times.Once);
            _mockRepository.Verify(r => r.UpdateBookAsync(It.IsAny<BookEntity>()), Times.Never);
        }
    }

    #endregion

    #region GetBookStatsAsync Tests

    [TestMethod]
    public async Task GetBookStatsAsync_Should_Return_Correct_Stats()
    {
        // Arrange
        var books = new List<BookEntity>
        {
            new BookEntity { Id = Guid.NewGuid(), Code = "001", Status = "DISPONIVEL", CreatedAt = DateTime.UtcNow },
            new BookEntity { Id = Guid.NewGuid(), Code = "002", Status = "EMPRESTADO", CreatedAt = DateTime.UtcNow },
            new BookEntity { Id = Guid.NewGuid(), Code = "003", Status = "DISPONIVEL", CreatedAt = DateTime.UtcNow }
        };

        _mockRepository.Setup(r => r.GetAllBooksAsync()).ReturnsAsync(books);
        _mockRepository.Setup(r => r.GetAvailableBooksAsync()).ReturnsAsync(2);

        // Act
        var result = await _libraryService.GetBookStatsAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.TotalBooks);
        Assert.AreEqual(2, result.AvailableBooks);
        Assert.AreEqual(1, result.BorrowedBooks);
        _mockRepository.Verify(r => r.GetAllBooksAsync(), Times.Once);
        _mockRepository.Verify(r => r.GetAvailableBooksAsync(), Times.Once);
    }

    #endregion

    #region DeleteBookAsync Tests

    [TestMethod]
    public async Task DeleteBookAsync_Should_Call_Repository_Delete()
    {
        // Arrange
        var code = "001";
        _mockRepository.Setup(r => r.DeleteBookAsync(code)).Returns(Task.CompletedTask);

        // Act
        await _libraryService.DeleteBookAsync(code);

        // Assert
        _mockRepository.Verify(r => r.DeleteBookAsync(code), Times.Once);
    }

    #endregion
}
