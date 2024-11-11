using StreamArchitecture_ch15;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using System.Xml;

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
         
        Streams must be disposed after use to release underlying resources 
        such as file and socket handles. 
        A simple way to guarantee this is by instantiating streams within using blocks.

        NOTE: 
        When working with a chain of decorator streams (e.g., a CryptoStream layered over a FileStream), 
        closing the outermost decorator automatically closes 
        both it and the underlying backing store stream.

        Example: 
        If you have a CryptoStream that writes encrypted data into a FileStream, 
        closing the CryptoStream also closes the FileStream.

        This simplifies resource management, 
        as you don’t need to close each stream in the chain individually.

        ---Buffering

        Many streams (especially file streams) internally use buffering. 
        This means that data you write may not immediately be saved to the backing store (like a disk file), 
        instead sitting in a buffer temporarily.

        The Flush method forces any buffered data to be written to the backing store immediately. 
        However, Flush is called automatically when you close the stream, 
        so you don’t need to call Flush before Close.

        */

        /* Timeouts
        Timeouts are essential for network-related streams but are typically unnecessary for file or memory streams.
        
        Timeout Properties:

        1. ReadTimeout: Sets the maximum wait time for reading from the stream before an exception is thrown.
        2. WriteTimeout: Sets the maximum wait time for writing to the stream.

        Generally, only network streams (like NetworkStream) support timeouts. 
        File and memory streams process data quickly without these delays, so they don't have timeouts.

        Asynchronous methods like ReadAsync and WriteAsync do not use timeouts directly. 
        Instead, you can pass a cancellation token to control the duration of an async operation.

        try
        {
            using TcpClient client = new TcpClient("127.0.0.1", 5672);
            using NetworkStream stream = client.GetStream();

            stream.WriteTimeout = 2 * 1000;
            stream.ReadTimeout = 2 * 1000;

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            Console.WriteLine($"Bytes read: {bytesRead}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"An IO error occurred (likely a timeout): {ex.Message}");
        }

        */

        //==================================================================================

        /* FileStream
         
        The File class provides convenient static methods for quickly creating common FileStream instances, 
        which simplifies the process of setting up a stream for basic read or write operations.

        1. File.OpenRead():
        _ Opens a file in read-only mode.
        _ The file must exist; otherwise, an exception is thrown.
        _ Suitable for situations where data needs to be read without modifying it.

        FileStream fs = File.OpenRead("test.txt");
        now fs is ready for reading data from file stream.

        2. File.OpenWrite():
        _ Opens a file in write-only mode.
        _ If the file does not exist, it creates a new one.
        _ If the file exists, it leaves the existing content intact but starts writing from the beginning.
        _ If you write fewer bytes than were already present in the file, this leaves a mixture of old and new content.

        Previous data in test.txt:
        Hello World. This data has been written to test File.OpenWrite() Method.

        FileStream fs = File.OpenWrite("test.txt");
        fs.Write(new byte[] { 1, 2, 3, 4, 5 });
        fs.Flush();

        After:
         World. This data has been written to test File.OpenWrite() Method.

        3. File.Create():
        _ Creates or overwrites a file, truncating any existing content.
        _ Opens the file with read and write permissions.
        _ This is ideal when you want to start with a clean slate and avoid any remnants of previous content.

        Previous:
         World. This data has been written to test File.OpenWrite() Method.

        After: does not overwrite, instead open new page
        Data: This is a clean beginning

        FileStream fs = File.Create("test.txt");
        fs.Write(Encoding.UTF8.GetBytes("This is a clean beginning"));
        fs.Flush();

        Summary between OpenWrite and Create:
        
        1. File.Create always overwrites an existing file, starting with a fresh empty file.
        2. File.OpenWrite keeps any existing content but begins writing at the start of the file.
           This behavior can lead to old content remaining after new content 
           if you don’t overwrite all previous data.

        ---------------------------

        You can also directly instantiate a FileStream. Its constructors provide access to every feature, 
        allowing you to specify a filename or low-level file handle, file creation and access modes, 
        and options for sharing, buffering, and security.

        1_ File Name: The name of the file to open.
        2_ File Mode: Specifies the action to take on the file (e.g., create new, open, append).

        public enum FileMode
        {
            CreateNew = 1, Create = 2,
            Open = 3, OpenOrCreate = 4,
            Truncate = 5, Append = 6
        }

        FileMode.Create: 
        Creates a new file. If the file already exists, it is overwritten (truncated to zero length). 
        This mode is often used when you need to start fresh with new data.
        
        FileMode.CreateNew: 
        Creates a new file. If the file already exists, an IOException is thrown. 
        This is useful when you want to avoid overwriting an existing file.
        
        FileMode.Open: 
        Opens an existing file. If the file does not exist, an FileNotFoundException is thrown. 
        This is used when you want to read or modify existing data.
        
        FileMode.OpenOrCreate: 
        Opens the file if it exists; otherwise, it creates a new file.
        This is useful when you want to work with a file that may or may not exist yet.
        
        FileMode.Append: 
        Opens the file if it exists and positions the write cursor at the end. 
        If the file does not exist, it creates a new file. 
        This mode is specifically for appending data and disallows reading 
        (attempting to read will throw a NotSupportedException).
        
        FileMode.Truncate: 
        Opens an existing file and truncates its size to zero (erasing all content). 
        If the file does not exist, a FileNotFoundException is thrown. 
        This is used when you want to clear out an existing file but keep its name/path.

        3_ File Access: Determines if the file is opened for reading, writing or both.
        
        4_ File Share:
        FileShare defines how the file can be accessed concurrently by other processes or threads 
        when one process is already using it. 
        
        This is crucial for managing scenarios where multiple parts of a system 
        may try to read from or write to the same file. 
        Here's how it works:

        Sharing Modes: The FileShare enumeration offers various modes, 
        which define what other processes can or cannot do with the file once it’s opened:

        FileShare.None
        FileShare.Read
        FileShare.Write
        FileShare.ReadWrite
        FileShare.Delete

        When a file is opened with a specific FileShare mode, 
        the OS enforces these permissions at the file system level.
        
        If an operation violates the FileShare setting (e.g., attempting to open a file with FileShare.None by another process), 
        the OS will throw an exception.

        5_ Buffer size.

        The buffer is an intermediate memory space that temporarily holds data while 
        it’s being read from or written to a file. 
        This allows FileStream to optimize performance by reducing the frequency of direct disk operations.

        ---How It Works:

        When reading data, it is loaded from disk into the buffer. 
        If the program requests data that is already in the buffer, 
        it’s served directly from memory, which is much faster than reading from disk again.

        This buffering mechanism reduces the number of I/O operations, 
        which can greatly enhance performance, especially for repetitive reads and writes.

        If not specified, FileStream uses a default buffer size 
        (typically 4KB to 8KB, depending on the platform).

        */

        /* MemoryStream

        A MemoryStream is a type of Stream that uses an in-memory byte array as its backing store 
        instead of a file or network connection. 
        This means that all data is loaded into memory, making it fast but also potentially memory-intensive.

        ---Why Use MemoryStream?
        a) Since the data is in memory, you can perform random access operations (seek, read, write) efficiently.
        b) Faster than disk-based streams as it avoids the latency of disk I/O

        ---MemoryStream Key Methods and Properties
        
        1. Copying to MemoryStream
        A MemoryStream can be populated by copying data from another stream (e.g., a file or network stream) 
        when the data size is manageable:

        using FileStream sourceStream = new FileStream("test.txt", FileMode.OpenOrCreate);
        using MemoryStream memoryStream = new MemoryStream();

        // copy data from source stream into memory stream.
        sourceStream.CopyTo(memoryStream);

        // seek back to the start of the memory stream for reading.
        memoryStream.Position = 0;

        Here, sourceStream.CopyTo(memoryStream); transfers data to MemoryStream, 
        letting you process it entirely in memory.

        2. Converting MemoryStream to a Byte Array
        To retrieve all data from a MemoryStream, you can use the ToArray method, 
        which creates a new byte array containing the data.

        byte[] data = memoryStream.ToArray();

        */

        /* Text Adapters
         
        Text adapters in .NET, specifically the TextReader and TextWriter classes, 
        are designed for reading and writing text (characters and strings) as opposed to raw bytes. 
        
        These abstract base classes allow for a higher-level, more convenient way of working with text data, 
        providing various concrete implementations to handle different underlying data sources (like files, memory, etc.).

        -----TextReader
        TextReader is an abstract base class used to read characters from an input stream. 
        The key features of TextReader are its methods for reading characters, lines, and blocks of text, and 
        it offers functionality for handling different character encodings and character positions in the stream.

        -Key Methods of TextReader

        a) Reading One Character:

        a.1) Peek(): Returns the next character in the stream without advancing the position. 
        It returns -1 when the end of the stream is reached, and you cast the result to a char.

        a.2) Read(): Reads the next character and advances the position. 
        This method also returns -1 at the end of the stream, and you can cast the result to a char.

        b) Reading Many Character:

        b.1) Read(char[] buffer, int index, int count): 
        Reads multiple characters into the specified char[] buffer, starting from the index and reading up to count characters.
        
        b.2) ReadBlock(char[] buffer, int index, int count): 
        Similar to Read, but guarantees that it will fill the buffer or throw an exception if it cannot do so 
        (useful for blocking operations).

        c) Reading Lines and Entire Stream:

        c.1) ReadLine(): Reads characters until a line break (\n or \r\n), 
        returning the line as a string and discarding the newline characters. 
        It returns null if at the end of the stream.

        c.2) ReadToEnd(): Reads the entire remaining text from the current position to the end of the stream and returns it as a string.

        d) Other Useful Members:

        d.1) Null: 
        
        A static readonly instance of TextReader that performs no actions (useful for placeholders).
        public static readonly TextReader Null;

        4.2) Synchronized(): 

        Returns a thread-safe version of a TextReader, which can be helpful for multi-threaded environments.
        public static TextReader Synchronized (TextReader reader);

        NOTE: The Peek() method and Read() returning -1 when the end of the stream is reached is one way to detect the end of data. 
        The ReadLine() method also helps to handle end-of-stream conditions, 
        as it returns null when no more lines are available.

        -----TextWriter

        TextWriter is the counterpart to TextReader and is used for writing characters to an output stream. 
        It is designed to write text-based data, with support for encoding and formatting.

        1) Write(char value): Writes a single character to the stream.
        2) Write(string value): Writes an entire string to the stream.
        3) Write(char[] buffer, int index, int count): Writes a char[] buffer to the stream, 
           starting from the given index and writing up to the specified count.
        4) WriteLine(string value): Writes a string followed by the newline character(s) defined by Environment.NewLine. 
           This is particularly useful for line-based data formats.
        5) NewLine: This property holds the newline character(s) for the current environment, 
           which defaults to \r\n on Windows and \n on Unix-based systems. 
           You can change it for interoperability with different platforms.

        6) FormatProvider: This property allows you to define how formatting is applied when writing data. 
           It can be customized to control how various data types (like numbers or dates) are formatted.

        */

        /* StreamReader and StreamWriter
         
        StreamReader and StreamWriter are classes in the System.IO namespace that 
        are used to read and write character data (text) to streams. 
        
        The key difference between these classes and other I/O types like FileStream or BinaryReader is that 
        these classes operate with text data, which may require encoding/decoding from raw bytes into characters.

        The default encoding used in StreamReader and StreamWriter is UTF-8. 
        However, you can specify a different encoding like UTF-16, ASCII, etc.

        ------------------------------------------------------------------------------
        In the following example, a StreamWriter writes two lines of text to a file, 
        and then a StreamReader reads the file back:

        using (FileStream fileStream = File.OpenWrite("test.txt"))
        using (TextWriter writer = new StreamWriter(fileStream))
        {
            writer.WriteLine("Line1");
            writer.WriteLine("Line2");
        }

        using (FileStream fileStream = File.OpenRead("test.txt"))
        using (TextReader reader = new StreamReader(fileStream))
        {
            Console.WriteLine(reader.ReadLine());
            Console.WriteLine(reader.ReadLine());
        }

        Because text adapters are so often coupled with files, 
        the File class provides the static methods CreateText, AppendText, and OpenText to shortcut the process:

        using (TextWriter writer = File.CreateText("test.txt"))
        {
            writer.WriteLine("Line1");
            writer.WriteLine("Line2");
        }

        using (TextWriter writer = File.AppendText("test.txt"))
        {
            writer.WriteLine("Line3");
        }

        using TextReader reader = File.OpenText("test.txt");

        string? readline = string.Empty;
        while ((readline = reader.ReadLine()) != null)
        {
            Console.WriteLine(readline);
        }

        while (reader.Peek() > -1)
        {
            Console.WriteLine(reader.ReadLine());
        }

        */

        /* Character Encodings
         
        Character encodings are essential for representing text as bytes when working with streams, 
        especially since streams are inherently byte-oriented. 
        In C#, StreamReader and StreamWriter allow us to read and write text by converting between characters and bytes. 
        
        How characters are stored and read as bytes is determined by the encoding used, which is managed by the Encoding class in C#.

        ---Default Encoding (UTF-8)
        
        By default, UTF-8 encoding is used in StreamReader and StreamWriter. 
        UTF-8 is efficient for characters in the ASCII range (0-127), using only one byte per character. 
        
        However, for characters outside the ASCII range—such as special symbols and characters from non-Western languages,
        UTF-8 uses variable-length encoding, which may require two to four bytes per character.

        using (FileStream fs = new FileStream("test.txt", FileMode.Create))
        using (TextWriter writer = new StreamWriter(fs))
        {
            writer.WriteLine("test");
        }

        using (FileStream fileStream = File.OpenRead("test.txt"))
        {
            for (int b; (b = fileStream.ReadByte()) > -1;)
            {
                Console.Write((char)b + " ");
            }
        }

        // Output: 116 101 115 116 45 226 128 148 13 10
        The carriage return and line feed (13 and 10) indicate the end of the line.

        --UTF-16 Encoding

        UTF-16 encoding generally uses two bytes (16 bits) per character, 
        making it efficient for characters that fall within the Unicode Basic Multilingual Plane (BMP). 
        Characters outside the BMP may require four bytes. 
        
        This encoding is useful when working with text requiring broad Unicode support, 
        as UTF-16 provides fixed-width encoding for most characters.

        using (FileStream fs = new FileStream("test.txt", FileMode.Create))
        using (TextWriter writer = new StreamWriter(fs, Encoding.Unicode))
        {
            writer.WriteLine("test-");
        }

        using (FileStream fileStream = File.OpenRead("test.txt"))
        {
            for (int b; (b = fileStream.ReadByte()) > -1;)
            {
                Console.Write((char)b + " ");
            }
        }

        // Output: 255 254 116 0 101 0 115 0 116 0 45 0 13 0 10 0

        255 254 is the BOM (Byte Order Mark), which identifies the file as UTF-16 little-endian.

        */

        /* Notes
         
        -- 1) Why all the letters include second ZERO in utf16 encoding?

        In UTF-16 encoding, each character is represented by two bytes (16 bits), 
        even if it only requires one byte in UTF-8. 
        
        The 0 in 116 0, 101 0, etc., represents the second byte of each character in UTF-16 encoding, 
        where the second byte is 0 for ASCII characters because they fit within the first byte.

        -- 2) Handling BOM in UTF-8 Encoding

        UTF-8 encoding may include a Byte Order Mark (BOM) at the start of the file to signal the encoding format. 
        By default, StreamWriter with UTF-8 encoding will add a BOM unless otherwise specified. 
        However, this BOM is often unnecessary, and you can disable it using UTF8Encoding.

        using (FileStream fs = new FileStream("test.txt", FileMode.Create))
        using (TextWriter writer = new StreamWriter(fs, new UTF8Encoding(false)))
        {
            writer.WriteLine("test-");
        }

        // Output: 116 101 115 116 45 13 10


        -- 3) What is 13 10 at the end?

        The numbers 13 and 10 refer to carriage return (CR) and line feed (LF) characters, respectively. 
        Together, they signify the end of a line in text files, especially on Windows systems. 
        Here’s a breakdown:

            Carriage Return (CR): 13 (in ASCII) or \r in C#.
            Line Feed (LF): 10 (in ASCII) or \n in C#.
        
        When you see 13 10 in a text file, it's essentially \r\n, marking the end of a line. 
        This line-ending convention is standard for Windows text files, 
        while other systems, like Unix and Linux, use just 10 (LF or \n).

        */

        /* StringReader and StringWriter
         
        The StringReader and StringWriter adapters don’t wrap a stream at all; 
        instead, they use a string or StringBuilder as the underlying data source. 
        
        This means no byte translation is required—in fact, the classes do nothing you couldn’t easily achieve
        with a string or StringBuilder coupled with an index variable. 
        
        Their advantage, though, is that they share a base class with StreamReader/StreamWriter. 
        For instance, suppose that we have a string containing XML and want to parse it with an XmlReader. 
        
        The XmlReader.Create method accepts one of the following:
        • A URI
        • A Stream
        • A TextReader
        
        We can instantiate and pass in a StringReader as follows:
        XmlReader r = XmlReader.Create (new StringReader (myString));

        */

        /* Binary Adapters
         
        Binary adapters in .NET, specifically BinaryReader and BinaryWriter, 
        are used to read and write binary (non-textual) data directly in its native format, 
        which allows you to work with primitive types like integers, floats, and doubles efficiently. 

        --Key Points of BinaryReader and BinaryWriter:

        BinaryReader and BinaryWriter work by storing data in the same format used in memory. 
        For example, an integer (int) always uses exactly 4 bytes, and a double uses 8 bytes. 
        There is no conversion to text or additional delimiters, making this a compact and fast method for data storage.

        Since each data type has a known fixed byte size, binary adapters don’t need delimiters to separate values. 
        This is different from text-based storage, 
        where delimiters (like commas or newlines) are often needed to separate data.

        Example with a Custom Class (Person):

        public class Person
        {
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
            public int Height { get; set; }

            public void SaveData(Stream stream)
            {
                BinaryWriter writer = new BinaryWriter(stream);

                writer.Write(Name);
                writer.Write(Age);
                writer.Write(Height);

                writer.Flush();
            }

            public void LoadData(Stream stream)
            {
                BinaryReader reader = new BinaryReader(stream);

                Name = reader.ReadString() + "test";
                Age = reader.ReadInt32() + 100;
                Height = reader.ReadInt32() + 200;
            }
        }

        Person person = new Person() { Name = "Mahammad", Age = 21, Height = -10 };
        MemoryStream stream = new MemoryStream();

        Console.WriteLine("Before");
        person.SaveData(stream);
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}, Height: {person.Height}");
        Console.WriteLine($"Length of Stream: {stream.Length}");

        stream.Position = 0;

        person.LoadData(stream);
        Console.WriteLine("After");
        Console.WriteLine($"Length of Stream: {stream.Length}");
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}, Height: {person.Height}");

        */

        /* Closing and Disposing Stream Adapters
        
        In .NET, the way we close or dispose of stream adapters is essential for proper resource management and 
        for controlling how underlying streams are handled.
        
        1) Closing the Adapter Only:
        2) Closing Both the Adapter and Stream:

        Options 1 and 2 are semantically identical because closing an adapter automatically closes the underlying stream. 
        Whenever you nest using statements, you’re implicitly taking option 2:
        
        using (FileStream fs = File.Create("test.txt"))
        using (TextWriter writer = new StreamWriter(fs))
        {
            writer.WriteLine("Line");
        }

        Because the nest disposes from the inside out, the adapter is closed first, and then the stream. 
        Furthermore, if an exception is thrown within the adapter’s constructor, the stream still closes. 
        It’s hard to go wrong with nested using statements!

        */

        // todo: pages [ 681 - 685 ] Compression related Subjects

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

        //------------------------------------------------------

        //try
        //{
        //    using TcpClient client = new TcpClient("127.0.0.1", 5672);
        //    using NetworkStream stream = client.GetStream();

        //    stream.WriteTimeout = 2 * 1000;
        //    stream.ReadTimeout = 2 * 1000;

        //    byte[] buffer = new byte[1024];
        //    int bytesRead = stream.Read(buffer, 0, buffer.Length);
        //    Console.WriteLine($"Bytes read: {bytesRead}");
        //}
        //catch (IOException ex)
        //{
        //    Console.WriteLine($"An IO error occurred (likely a timeout): {ex.Message}");
        //}

        //-------------------------------------------------------

        //using (TextWriter writer = File.CreateText("test.txt"))
        //{
        //    writer.WriteLine("Line1");
        //    writer.WriteLine("Line2");
        //}

        //using (TextWriter writer = File.AppendText("test.txt"))
        //{
        //    writer.WriteLine("Line3");
        //}

        //using TextReader reader = File.OpenText("test.txt");

        //string? readline = string.Empty;
        //while ((readline = reader.ReadLine()) != null)
        //{
        //    Console.WriteLine(readline);
        //}

        //while (reader.Peek() > -1)
        //{
        //    Console.WriteLine(reader.ReadLine());
        //}

        //-------------------------------------------

        //using (FileStream fileStream = File.OpenWrite("test.txt"))
        //using (TextWriter writer = new StreamWriter(fileStream))
        //{
        //    writer.WriteLine("Line1");
        //    writer.WriteLine("Line2");
        //}

        //using (FileStream fileStream = File.OpenRead("test.txt"))
        //using (TextReader reader = new StreamReader(fileStream))
        //{
        //    Console.WriteLine(reader.ReadLine());
        //    Console.WriteLine(reader.ReadLine());
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