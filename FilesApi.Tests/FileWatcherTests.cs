namespace FilesApi.Tests;

public class FileWatcherTests
{
    [Fact]
    public void FileWatcher_WhenFileCreated_EventRaised()
    {
        // Arrange
        var tempDirectory = Path.Combine(Path.GetTempPath(), "FileWatcherTest");
        Directory.CreateDirectory(tempDirectory);

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        using (var fileWatcher = new FileWatcher(tempDirectory))
        {
            string filename = "testfile.txt";

            // Act
            File.Create(Path.Combine(tempDirectory, filename)).Close();
            // Sleep to give the file watcher some time to detect the change
            Thread.Sleep(500);

            // Assert
            var output = consoleOutput.ToString();
            Assert.Contains($"File \"{filename}\" has been uploaded.", output);
        }

        // Clean up
        consoleOutput.Close();
        Directory.Delete(tempDirectory, true);
    }

    [Fact]
    public void FileWatcher_WhenFileChanged_EventRaised()
    {
        // Arrange
        var tempDirectory = Path.Combine(Path.GetTempPath(), "FileWatcherTest");
        Directory.CreateDirectory(tempDirectory);

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Create the file
        string filename = "testfile.txt";
        string filePath = Path.Combine(tempDirectory, filename);
        File.WriteAllText(filePath, "Initial content");
        
        using (var fileWatcher = new FileWatcher(tempDirectory))
        {
            // Act
            File.WriteAllText(filePath, "Modified content");

            Thread.Sleep(1000);

            // Assert
            var output = consoleOutput.ToString();
            Assert.Contains($"File \"{filename}\" was updated.", output);
        }

        // Clean up
        consoleOutput.Close();
        Directory.Delete(tempDirectory, true);
    }
}