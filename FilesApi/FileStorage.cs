namespace FilesApi;

public interface IFileStorage
{
    Task<bool> SaveFileAsync(IFormFile file);
    List<FileMetadata> GetFileList();
    string GetStorageDirectoryPath();
    void EnsureStorageDirectoryExists();
}

// Implement the file storage interface to save files on the local disk
public class LocalFileStorage : IFileStorage
{
    private string _storagePath = "media";

    public LocalFileStorage() => EnsureStorageDirectoryExists();

    public LocalFileStorage(string storagePath)
    {
        _storagePath = storagePath;
        EnsureStorageDirectoryExists();
    }

    public void EnsureStorageDirectoryExists()
    {
        var storageDirectory = GetStorageDirectoryPath();

        if (!Directory.Exists(storageDirectory))
        {
            Directory.CreateDirectory(storageDirectory);
            Console.WriteLine($"Creating the media directory at: {storageDirectory}");
        }
    }

    public async Task<bool> SaveFileAsync(IFormFile file)
    {
        var filePath = Path.Combine(_storagePath, file.FileName);

        // Update existing file if content is newer
        if (File.Exists(filePath))
        {
            var existingFile = new FileInfo(filePath);
            if (existingFile.Length == file.Length)
            {
                return false; // File is not updated
            }
        }

        // Create a new file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return true;
    }

    public List<FileMetadata> GetFileList()
    {
        var files = Directory.GetFiles(_storagePath);
        var fileMetadataList = new List<FileMetadata>();

        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            fileMetadataList.Add(new FileMetadata
            {
                Name = fileInfo.Name,
                Size = fileInfo.Length,
                Date = fileInfo.LastWriteTime
            });
        }
     
        return fileMetadataList;
    }

    public string GetStorageDirectoryPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), _storagePath);
    }
}

public class FileMetadata
{
    public string? Name { get; set; }
    public long Size { get; set; }
    public DateTime Date { get; set; }
}