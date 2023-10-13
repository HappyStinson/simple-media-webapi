public interface IFileWatcher
{
    void Start();
}

public class FileWatcher : IFileWatcher
{
    public void Start()
    {
        FileSystemWatcher watcher = new("media");
        watcher.EnableRaisingEvents = true;
        
        watcher.Changed += (sender, e) =>
        {
            Console.WriteLine($"File {e.Name} was updated.");
        };

        watcher.Created += (sender, e) =>
        {
            Console.WriteLine($"File {e.Name} has been uploaded.");
        };
    }
}