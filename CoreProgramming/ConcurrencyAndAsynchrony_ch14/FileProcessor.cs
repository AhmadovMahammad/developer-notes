namespace ConcurrencyAndAsynchrony_ch14
{
    public class FileProcessor
    {
        public async Task ProcessFilesAsync(IEnumerable<string> filePaths, CancellationToken cancellationToken)
        {
            Console.WriteLine("Beginning file processing...");

            foreach (var filePath in filePaths)
            {
                try
                {
                    await ProcessFileAsync(filePath, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine($"\nProcessing cancelled for {filePath}.");
                    break;
                }
            }

            Console.WriteLine("File processing operation completed.");
        }

        private async Task ProcessFileAsync(string filePath, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Starting processing of {filePath}");

            for (int i = 0; i < 5; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Console.WriteLine($"Processing stage {i} for {filePath}...");
                await Task.Delay(500, cancellationToken); // each stage takes hal
            }

            Console.WriteLine($"Completed processing of {filePath}.");
        }
    }
}