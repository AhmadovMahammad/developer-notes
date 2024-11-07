using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        /* Stream Architecture
         
        The .NET stream architecture has three main concepts: 
        1. backing stores, 2. decorators, and 3. adapters. 
        
        These elements are designed to handle data efficiently and flexibly, 
        whether it’s coming from or going to a file, network, memory, or other data sources. 
        Let’s go over each concept and its role in the architecture.

        */

        /* Backing Stores
         
        Backing stores are the foundational elements in .NET’s stream system. 
        They represent data sources or destinations, such as files or network connections, 
        that make input and output meaningful by providing a way to read or write data sequentially.

        Purpose of Backing Stores: Backing stores can be:
        1. A source from which data can be read
        2. A destination to which data can be written

        Backing stores don’t load all data into memory at once.
        Instead, they handle data sequentially, meaning that 
        you can read or write data in chuncks rather than all at oncee.
        This sequential approach is essential for managing large data sources with a fixed, limited amount of memory.

        */

        /* Decorator Streams
         
        Decorators are specialized streams that take an existing stream (often a backing store) and add extra functionality. 
        Instead of reading or writing data directly, a decorator providing a layer of transformation or enhancement.

        Purpose of Decorators:
        Decorators allow adding features like encryption, compression, or 
        buffering to a backing store without changing the underlying data source’s functionality.

        Examples:
        DeflateStream: Compresses or Decompresses data as it passes through.
        CryptoStream: Encrypt or Decrypt data for secure transmission.

        Chaining Decorators: 
        You can combine decorators, like a CryptoStream that encrypts data which is then compressed by a DeflateStream. 
        This allows complex data transformations to be composed in a single chain.

        string filePath = "data.bin";
        string originalText = "Hello, secure and compressed world!";

        // Writing encrypted and compressed data to the file
        using FileStream fileStream = new FileStream(filePath, FileMode.Create);
        using Aes aes = Aes.Create();
        using CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        using DeflateStream deflateStream = new DeflateStream(cryptoStream, CompressionMode.Compress);
        using StreamWriter writer = new StreamWriter(deflateStream, Encoding.UTF8);
        // Writing original text to the deflated (compressed) and encrypted file stream
        writer.WriteLine(originalText);


        */

        /* Adapters
         
        Adapters connect streams to higher-level data structures, like text or XML, which many applications work with. 
        They provide a way to connect the raw byte data in formats that are more usable in applications.

        Adapters offer methods designed for specific data types. 
        For instance, a text adapter might provide ReadLine to read a full line of text, 
        which would be challenging with just raw bytes.

        Whereas decorators enhance or modify data while maintaining the byte-oriented interface, 
        adapters usually hide the low-level byte operations, 
        exposing only the methods necessary for working with a specific data type.

        Summary: 
        * Backing Store Streams: Handle raw data sources like files or network connections.
        * Decorator Streams: Add functionality like encryption or compression while maintaining a standard stream interface.
        * Adapters: Convert raw byte data into structured, higher-level data types, like text or XML, with methods suited to those types.

        */

        /* Using Streams.
         
        The abstract Stream class is the base for all streams.
        It defines methods and properties for three fundamental operations: reading, writing, and seeking, 
        as well as for administrative tasks such as closing, flushing, and configuring timeouts.

        Code Walkthrough (Synchronous I/O)

        using Stream s = new FileStream("test.txt", FileMode.OpenOrCreate);

        // Check stream capabilities:
        Console.WriteLine(s.CanRead);   // True (FileStream supports reading)
        Console.WriteLine(s.CanWrite);  // True (FileStream supports writing)
        Console.WriteLine(s.CanSeek);   // True (FileStream supports seeking)

        // Writing individual bytes:
        s.WriteByte(101); // Write a single byte (ASCII 'e')
        s.WriteByte(102); // Write another byte (ASCII 'f')

        // Writing a block of bytes:
        byte[] block = new byte[] { 1, 2, 3, 4, 5 };
        s.Write(block, 0, block.Length); // Write entire block of bytes

        Console.WriteLine(s.Length);    // 7 (total bytes written so far)
        Console.WriteLine(s.Position);  // 7 (current position at the end)

        // Seeking: Move back to the start of the stream
        s.Position = 0;

        // Reading bytes:
        Console.WriteLine(s.ReadByte());
        Console.WriteLine(s.ReadByte());

        // Reading a block of bytes:
        Console.WriteLine(s.Read(block, 0, block.Length)); // Returns total number of bytes read into the buffer. so it is 5.

        // Attempting to read again, expecting 0 as the end has been reached:
        Console.WriteLine(s.Read(block, 0, block.Length)); // 0


        -----NOTES:
        CanRead, CanWrite, CanSeek: These properties confirm the capabilities of the stream. For FileStream, all are true.

        Asynchronous versions (ReadAsync, WriteAsync) allow non-blocking operations. 
        These are especially useful for slower I/O, such as network data.

        static async Task AsyncDemo()
        {
            using Stream s = new FileStream("test.txt", FileMode.OpenOrCreate);

            byte[] block = new byte[] { 1, 2, 3, 4, 5 };
            await s.WriteAsync(block);

            s.Position = 0;
            Console.WriteLine(await s.ReadAsync(block));
        }

        The asynchronous methods make it easy to write responsive and scalable applications that 
        work with potentially slow streams (particularly network streams), without tying up a thread.

        */

        /* Reading and Writing
         
        A stream can support reading, writing, or both. 
        If CanWrite returns false, the stream is read-only; if CanRead returns false, the stream is write-only.

        Read receives a block of data from the stream into an array. 
        When reading data from a stream, Read might not fill the entire buffer in a single call, 
        especially with large buffers or slow data sources (e.g., network streams).
        
        Read will return the actual number of bytes read, which can vary. 
        To ensure that you read a fixed amount of data, you need to loop until all the bytes are read. 
        Here's how to do it:

        static async Task Read()
        {
            using Stream s = new FileStream("test.txt", FileMode.OpenOrCreate);

            byte[] block = new byte[] { 1, 2, 3, 4, 5 };
            await s.WriteAsync(block);

            byte[] data = new byte[100];
            int bytesRead = 0;
            int chunk = 0;

            do
            {
                chunk = s.Read(data, bytesRead, data.Length - bytesRead);
                bytesRead += chunk;
            }
            while (bytesRead < data.Length && chunk > 0);
        }

        data: A byte array buffer with a length of 1,000 bytes.
        bytesRead: Keeps track of the total number of bytes read into data.
        chunkSize: Stores the number of bytes read in each Read call.

        Each loop iteration attempts to read bytes into the remaining portion of data. 
        When Read reaches the end of the stream, chunkSize becomes 0, and the loop exits.

        -----Simplifying with BinaryReader.

        If you need a fixed number of bytes (like 1,000), 
        BinaryReader offers a more concise solution with its ReadBytes method. 
        
        This method returns an array filled with the exact number of bytes requested, or 
        fewer if the stream ends before reaching that count.

        byte[] data = new BinaryReader(s).ReadBytes(1000);

        If s has fewer than 1,000 bytes, data will contain only as many bytes as are available.
        You can replace 1000 with (int)s.Length to read the entire stream if it’s seekable (supports Length).

        byte[] data = new BinaryReader(s).ReadBytes((int)s.Length);
        
        */

        /* What is Seeking?
         
        Seeking refers to the ability to change the current position in a stream. 
        This means you can move to different points within the stream without needing 
        to read or write all the data from the start.

        Seeking allows:

        1. Random Access: 
        You can go directly to any part of a stream, 
        which is helpful for applications that don’t process data linearly.
        
        2. Length Modification: 
        You can adjust the length of a seekable stream by setting a new length (truncating or expanding).

        3. Optimized Performance: 
        Seeking prevents the need to read unnecessary parts of the stream if only specific segments are needed.

        -----Stream Properties Related to Seeking

        1) Position: 
        The Position property lets you read or set the current position within a stream. 
        It's an int or long (depending on the stream type) that represents the byte index from the start of the stream.

        2) Seek Method:
        Seek is more flexible than Position alone because it allows you to move the position in multiple ways:

        a) From the Beginning: You can set an absolute position within the stream.
        b) From the Current Position: You can move forward or backward relative to your current position.
        c) From the End: You can go to a position that’s relative to the end of the stream.

        3) Length and SetLength:
        The Length property shows the total size of a stream, while SetLength can change it. 
        a) Reducing the length truncates the data beyond the new end position, while b) expanding it adds empty bytes.


        -----Seekable vs. Non-Seekable Streams.

        Some streams, like FileStream, are seekable, while others, 
        like NetworkStream and encryption streams (e.g., CryptoStream), are non-seekable.

        Seekable Streams allow direct access to any part of the data. 
        You can use Seek, Position, Length, and SetLength to manipulate the stream's data location.

        For Non-Seekable Streams, CanSeek returns false, and Length and Position properties may throw exceptions if accessed. 
        With non-seekable streams, you typically read or write data sequentially and cannot move backwards.

        */

        /* Closing and Flushing
         

        */

        #region code example

        //using Stream s = new FileStream("test.txt", FileMode.OpenOrCreate);

        //// Check stream capabilities:
        //Console.WriteLine(s.CanRead);   // True (FileStream supports reading)
        //Console.WriteLine(s.CanWrite);  // True (FileStream supports writing)
        //Console.WriteLine(s.CanSeek);   // True (FileStream supports seeking)

        //// Writing individual bytes:
        //s.WriteByte(101); // Write a single byte (ASCII 'e')
        //s.WriteByte(102); // Write another byte (ASCII 'f')

        //// Writing a block of bytes:
        //byte[] block = new byte[] { 1, 2, 3, 4, 5 };
        //s.Write(block, 0, block.Length); // Write entire block of bytes

        //Console.WriteLine(s.Length);    // 7 (total bytes written so far)
        //Console.WriteLine(s.Position);  // 7 (current position at the end)

        //// Seeking: Move back to the start of the stream
        //s.Position = 0;

        //// Reading bytes:
        //Console.WriteLine(s.ReadByte());
        //Console.WriteLine(s.ReadByte());

        //// Reading a block of bytes:
        //Console.WriteLine(s.Read(block, 0, block.Length)); // Returns total number of bytes read into the buffer. so it is 5.

        //// Attempting to read again, expecting 0 as the end has been reached:
        //Console.WriteLine(s.Read(block, 0, block.Length)); // 0

        //-------------------------------------

        // Create a file and write initial bytes: A, B, C (ASCII values 65, 66, 67)
        //using (FileStream fs = new("test.txt", FileMode.Create))
        //{
        //    fs.WriteByte(65); // ASCII 'A'
        //    fs.WriteByte(66); // ASCII 'B'
        //    fs.WriteByte(67); // ASCII 'C'

        //    //Console.WriteLine("Position: {0}", fs.Position); // 3
        //}

        //using (FileStream fs = new("test.txt", FileMode.Open))
        //{
        //    fs.Seek(0, SeekOrigin.Begin); // Move to the beginning
        //    fs.WriteByte(88); // ASCII 'X'
        //}

        //using (FileStream fs = new("test.txt", FileMode.Open))
        //{
        //    Console.WriteLine(fs.ReadByte()); // 88 (X)
        //    Console.WriteLine(fs.ReadByte()); // 66 (B)
        //    Console.WriteLine(fs.ReadByte()); // 67 (C)
        //}

        #endregion
    }

    static async Task Read()
    {
        using Stream s = new FileStream("test.txt", FileMode.OpenOrCreate);

        byte[] block = new byte[] { 1, 2, 3, 4, 5 };
        await s.WriteAsync(block);

        // solution 1

        //byte[] data = new byte[100];
        //int bytesRead = 0;
        //int chunk = 0;

        //do
        //{
        //    chunk = s.Read(data, bytesRead, data.Length - bytesRead);
        //    bytesRead += chunk;
        //}
        //while (bytesRead < data.Length && chunk > 0);

        // solution 2

        byte[] data = new BinaryReader(s).ReadBytes((int)s.Length);
    }

    static async Task AsyncDemo()
    {
        using Stream s = new FileStream("test.txt", FileMode.OpenOrCreate);

        byte[] block = new byte[] { 1, 2, 3, 4, 5 };
        await s.WriteAsync(block);

        s.Position = 0;
        Console.WriteLine(await s.ReadAsync(block));
    }
}