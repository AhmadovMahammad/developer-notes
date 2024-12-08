using System.Text;

namespace Disposal_GarbageCollection_ch12;

public class FileManager : IDisposable
{
    private readonly FileStream _fileStream; // Unmanaged resource (file handle)
    private bool _disposed;

    public FileManager()
    {
        _fileStream = new FileStream("example.txt", FileMode.OpenOrCreate);
        Console.WriteLine("File Opened.");
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing) // it is called by Dispose method and you should clean up managed resources
            {
                Console.WriteLine("Releasing managed resources (if any).");
                // For example, closing a database connection or clearing a cache would go here.
            }

            // Release unmanaged resources (FileStream here)
            if (_fileStream is not null)
            {
                _fileStream.Close();
                Console.WriteLine("File stream closed (unmanaged resource).");
            }
        }

        _disposed = true;
    }

    ~FileManager()
    {
        Dispose(false);
        // Finalizer calls Dispose to clean up unmanaged resources
        // Because it handled managed resources itself by CLR
    }

    public void WriteToFile(string content)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }

        byte[] bytes = Encoding.UTF8.GetBytes(content);
        _fileStream.Write(bytes, 0, bytes.Length);
        Console.WriteLine("Content written to file.");
    }
}