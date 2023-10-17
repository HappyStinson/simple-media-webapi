using FilesApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FilesApi.Tests;

public class FilesControllerTests
{
    [Fact]
    public async Task UploadFile_ValidFile_ReturnsOkResult()
    {
        // Arrange
        var fileStorageMock = new Mock<IFileStorage>();
        fileStorageMock.Setup(storage => storage.SaveFileAsync(It.IsAny<IFormFile>())).ReturnsAsync(true);
        var controller = new FilesController(fileStorageMock.Object);

        var files = new List<IFormFile>
        {
            new Mock<IFormFile>().Object
        };

        // Act
        var result = await controller.UploadFile(files);

        // Assert
        Assert.NotNull(result);
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
    }

    [Fact]
    public async Task UploadFile_WhenFileExists_ReturnsStatus500()
    {
        // Arrange
        var fileStorageMock = new Mock<IFileStorage>();
        fileStorageMock.Setup(storage => storage.SaveFileAsync(It.IsAny<IFormFile>())).ReturnsAsync(false);
        var controller = new FilesController(fileStorageMock.Object);

        var files = new List<IFormFile>
        {
            new Mock<IFormFile>().Object
        };

        // Act
        var result = await controller.UploadFile(files);

        // Assert
        Assert.NotNull(result);
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    public async Task UploadFile_WhenFileTooLarge_ReturnsBadRequest()
    {
        // Arrange
        var largeFile = new Mock<IFormFile>();
        largeFile.Setup(f => f.Length).Returns(600 * 1048576); // Simulate a file larger than 500MB.

        var files = new List<IFormFile>
        {
            largeFile.Object
        };

        var fileStorageMock = new Mock<IFileStorage>();
        var controller = new FilesController(fileStorageMock.Object);

        // Act
        var result = await controller.UploadFile(files);

        // Assert
        Assert.NotNull(result);
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.Contains("exceeds 500MB limit", objectResult?.Value?.ToString());
    }

    [Fact]
    public async Task UploadFile_NoFileProvided_ReturnsBadRequest()
    {
        // Arrange
        var fileStorageMock = new Mock<IFileStorage>();
        var controller = new FilesController(fileStorageMock.Object);

        // Act
        var result = await controller.UploadFile(new List<IFormFile>());

        // Assert
        Assert.NotNull(result);
        var objectResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
    }
}