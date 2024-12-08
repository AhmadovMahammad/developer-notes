namespace ConcurrencyAndAsynchrony_ch14;

public class FileInstaller
{
    public async Task InstallFileAsync(string url, string destinationPath, IProgress<int> progress, CancellationToken cancellationToken)
    {
        Console.WriteLine($"--- Installing file from [{url}] to [{destinationPath}]");
        int bytesDownloaded = 0;
        int totalBytes = 1000;

        while (bytesDownloaded < totalBytes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            bytesDownloaded += 100;
            await Task.Delay(500, cancellationToken); // each 100 bytes take half second to install for example

            int percentComplete = (bytesDownloaded * 100) / totalBytes;
            progress.Report(percentComplete);
        }

        // simulate saving the file
        await Task.Delay(500);
    }
}
