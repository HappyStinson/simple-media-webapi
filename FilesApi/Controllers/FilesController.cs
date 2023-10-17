using Microsoft.AspNetCore.Mvc;

namespace FilesApi.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IFileStorage _fileStorage;
    private readonly FileWatcher _fileWatcher;
    private const int _maxFileSize = 500 * 1048576;


    public FilesController(IFileStorage fileStorage)
    {
        // get the directory to watch
        _fileStorage = fileStorage;
        _fileWatcher = new(Path.Combine(Directory.GetCurrentDirectory(), "media"));
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] List<IFormFile> files)
    {
        if (files.Count < 1)
        {
            return BadRequest($"No files were provided.");
        }

        foreach (var file in files)
        {
            if (file.Length > _maxFileSize)
            {
                return BadRequest($"{file.FileName} exceeds 500MB limit.");
            }

            if (!await _fileStorage.SaveFileAsync(file))
            {
                return StatusCode(500, $"Failed to save {file.FileName}.");
            }
        }

        return Ok("Files uploaded successfully.");
    }

    [HttpGet("list")]
    public IActionResult GetFileList()
    {
        try
        {
            var fileList = _fileStorage.GetFileList();

            return Ok(fileList);
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during file listing
            return StatusCode(500, $"An error occurred while retrieving the file list: {ex.Message}");
        }
    }
}