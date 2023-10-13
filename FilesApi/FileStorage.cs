public interface IFileStorage
{
    Task<bool> SaveFileAsync(IFormFile file);
    List<FileMetadata> GetFileList();
}

// Implement the file storage interface to save files on the local disk
public class LocalFileStorage : IFileStorage
{
    private string _storagePath = "MyPath";

    public async Task<bool> SaveFileAsync(IFormFile file)
    {
        if (file.Length > 500 * 1024 * 1024) // Check file size
        {
            // good idea with a custom exception ?
            return false;
            // throw new FileSizeLimitExceededException("File size exceeds 500MB.");
        }

        var filePath = Path.Combine(_storagePath, file.FileName);

        if (File.Exists(filePath))
        {
            var existingFile = new FileInfo(filePath);
            if (existingFile.Length == file.Length)
            {
                return false; // File is not updated
            }
        }

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
}

public class FileMetadata
{
    public string Name { get; set; }
    public long Size { get; set; }
    public DateTime Date { get; set; }
}