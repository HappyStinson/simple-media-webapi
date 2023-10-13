using Microsoft.AspNetCore.Mvc;

namespace FilesApi.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IFileStorage _fileStorage;

    public FilesController(IFileStorage fileStorage)
    {
        _fileStorage = fileStorage;
    }

    [HttpPost("upload")]
    // [RequestSizeLimit(524288000)] // 500MB limit
    // public async Task<IActionResult> UploadFiles(List<IFormFile> files)
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file) // improve to handle multiple files
    {
        try
        {
            var result = await _fileStorage.SaveFileAsync(file);
            if (result)
            {
                return Ok("File uploaded successfully");
            }
            else
            {
                return Ok("File not updated");
            }
        }
        // catch (FileSizeLimitExceededException ex)
        // {
        //     return BadRequest(ex.Message);
        // }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    // OPTION 2 - handles multiple files
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile()
    {
        var files = Request.Form.Files;

        foreach (var file in files)
        {
            var success = await _fileStorage.SaveFileAsync(file);
            if (!success)
            {
                return BadRequest("File size exceeds 500MB limit.");
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