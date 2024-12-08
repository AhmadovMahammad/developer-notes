using System.Collections.Concurrent;

namespace Disposal_GarbageCollection_ch12;

public class TempFileManager
{
    public static readonly ConcurrentQueue<TempFileManager> FailedDeletions = new ConcurrentQueue<TempFileManager>();
    public readonly string FilePath = string.Empty;
    public Exception? DeletionError { get; private set; }

    public TempFileManager(string filePath)
    {
        FilePath = filePath;
        Console.WriteLine($"Temporary file created: {FilePath}");
    }

    ~TempFileManager()
    {
        try
        {
            File.Delete(FilePath);
            Console.WriteLine($"Temporary file deleted: {FilePath}");
        }
        catch (Exception exception)
        {
            DeletionError = exception;
            FailedDeletions.Enqueue(this);
            Console.WriteLine($"Failed to delete file: {FilePath}. Error: {exception.Message}");
        }
    }
}