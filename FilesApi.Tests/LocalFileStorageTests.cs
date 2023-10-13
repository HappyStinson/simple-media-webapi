using Microsoft.AspNetCore.Http;
using Moq;

public class LocalFileStorageTests
{
    [Fact]
    public async Task SaveFileAsync_WhenFileTooLarge_ReturnsFalse()
    {
        // Arrange
        var storage = new LocalFileStorage("media-test");
        var file = new Mock<IFormFile>();
        file.Setup(f => f.Length).Returns(600 * 1048576); // Simulate a file larger than 500MB.

        // Act
        var result = await storage.SaveFileAsync(file.Object);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SaveFileAsync_WhenFileExistsAndNotUpdated_ReturnsFalse()
    {
        // Arrange
        var storagePath = "media-test";
        var fileName = "existingFile.txt";
        var filePath = Path.Combine(storagePath, fileName);

        // Create a file with the same name and length as the input file
        File.WriteAllText(filePath, "Some content");
        
        var storage = new LocalFileStorage(storagePath);
        var file = new Mock<IFormFile>();
        file.Setup(f => f.FileName).Returns(fileName);
        file.Setup(f => f.Length).Returns(new FileInfo(filePath).Length);

        // Act
        var result = await storage.SaveFileAsync(file.Object);

        // Assert
        Assert.False(result);

        // Clean up the file created during the test
        File.Delete(filePath);
    }

    [Fact]
    public async Task SaveFileAsync_WhenFileDoesNotExist_UploadsFileAndReturnsTrue()
    {
        // Arrange
        var storagePath = "media-test";
        var fileName = "newFile.txt";
        var filePath = Path.Combine(storagePath, fileName);

        var storage = new LocalFileStorage(storagePath);
        var file = new Mock<IFormFile>();
        file.Setup(f => f.FileName).Returns(fileName);
        file.Setup(f => f.Length).Returns(100); // File size within the limit

        // Act
        var result = await storage.SaveFileAsync(file.Object);

        // Assert
        Assert.True(result);

        // Verify that the file was created
        Assert.True(File.Exists(filePath));

        // Clean up the file created during the test
        File.Delete(filePath);
    }

    [Fact]
    public void GetFileList_EmptyDirectoryShouldWork()
    {
        // Arrange
        LocalFileStorage storage = new("media-test");
        List<FileMetadata> expected = new();

        // Act
        var actual = storage.GetFileList();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetFileList_DirectoryShouldReturnList()
    {
        // Arrange
        LocalFileStorage storage = new();
        List<FileMetadata> expected = new()
        {
            new FileMetadata
            {
                Name = "test1.jpeg",
                Size = 1234,
                Date = DateTime.Now
            }
        };

        // Act
        var actual = storage.GetFileList();

        // Assert
        Assert.Equal(expected.Count, actual.Count);
    }
}