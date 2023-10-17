namespace FilesApi;

public class FileWatcher : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private bool _disposed = false;

    public FileWatcher(string path)
    {
        // Console.WriteLine($"Watching for changes in {path} directory.");

        _watcher = new FileSystemWatcher(path)
        {
            EnableRaisingEvents = true
        };

        _watcher.Created += OnFileCreated;
        _watcher.Changed += OnFileChanged;
    }

    private void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"File \"{e.Name}\" has been uploaded.");
    }

    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"File \"{e.Name}\" was updated.");
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _watcher.Dispose();
            }

            _disposed = true;
        }
    }
}