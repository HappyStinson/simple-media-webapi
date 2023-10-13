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
        var controller = new FilesController(fileStorageMock.Object);
        var file = new Mock<IFormFile>();

        // Act
        var response = await controller.UploadFile(file.Object);

        // Assert
        Assert.NotNull(response);
        var objectResponse = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(StatusCodes.Status200OK, objectResponse.StatusCode);
    }

    [Fact]
    public async Task UploadFile_ReturnsOkResult()
    {
        // Arrange
        var fileStorageMock = new Mock<IFileStorage>();
        fileStorageMock.Setup(storage => storage.SaveFileAsync(It.IsAny<IFormFile>())).ReturnsAsync(true);
        var controller = new FilesController(fileStorageMock.Object);
        var file = new Mock<IFormFile>();

        // Act
        var response = await controller.UploadFile(file.Object);

        // Assert
        Assert.NotNull(response);
        var objectResponse = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(StatusCodes.Status200OK, objectResponse.StatusCode);
    }

    // Add more test methods for other scenarios
}