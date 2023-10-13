using FilesApi.Controllers;

public class LocalFileStorageTests
{
    // Task<bool> SaveFileAsync(IFormFile file);

    [Fact]
    public void GetFileList_EmptyDirectoryShouldWork()
    {
        // Arrange
        LocalFileStorage storage = new("media-empty");
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