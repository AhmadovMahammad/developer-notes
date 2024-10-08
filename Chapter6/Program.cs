using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;

namespace Chapter6
{
    internal class Program
    {
        static Random sharedRandom = new Random(); // Single instance for the application
        static unsafe void Main(string[] args)
        {
            /* String 
            A C# string (== System.String) is an immutable (unchangeable) sequence of characters.

            ---Constructing strings
            1. The simplest way to construct a string is to assign a literal

            string s1 = "Hello";
            string s2 = "First Line\r\nSecond Line";
            string s3 = @"\\server\fileshare\helloworld.cs";

            2. To create a repeating sequence of characters, you can use string’s constructor:
            Console.Write (new string ('*', 10)); // **********

            3. You can also construct a string from a char array. 
            The ToCharArray method does the reverse:
            
            char[] ca = "hello".ToArray();
            string s = new string(ca); // s = hello

            4. string’s constructor is also overloaded to accept various (unsafe) pointer types, 
            in order to create strings from types such as char*.

            char c = 'a';
            string s = new string(&c);

            // string format...
            var anonymousObj = new[]
            {
                new { Name= "Mahammad", Salary= 20000 },
                new { Name= "Alex", Salary= 15000 },
                new { Name= "Johnathan", Salary= 18000 },
                new { Name= "Jennifer", Salary= 22000 }
            };

            var maxPadRight = anonymousObj.Max(m => m.Name.Length);
            string composite = $"Name: {{0, -{maxPadRight + 5}}} Monthly Salary: {{1, -15:C}}";

            foreach (var m in anonymousObj)
            {
                Console.WriteLine(string.Format(composite, m.Name, m.Salary));
            }

            ---Encoding to byte arrays

            byte[] utf8Bytes = Encoding.UTF8.GetBytes("0123456789");
            byte[] utf16Bytes = Encoding.Unicode.GetBytes("0123456789");
            byte[] utf32Bytes = Encoding.UTF32.GetBytes("0123456789");

            Console.WriteLine(utf8Bytes.Length); // 10
            Console.WriteLine(utf16Bytes.Length); // 20
            Console.WriteLine(utf32Bytes.Length); // 40

            string original1 = Encoding.UTF8.GetString(utf8Bytes);
            string original2 = Encoding.Unicode.GetString(utf16Bytes);
            string original3 = Encoding.UTF32.GetString(utf32Bytes);

            Console.WriteLine(original1); // 0123456789
            Console.WriteLine(original2); // 0123456789
            Console.WriteLine(original3); // 0123456789
            */

            /* Date and Times
             
            Three immutable structs in the System namespace do the job of representing dates and times: 
            1. DateTime, 2. DateTimeOffset, and 3. TimeSpan. 
            C# doesn’t define any special keywords that map to these types.

            ---TimeSpan

            There are three ways to construct a TimeSpan:
            • Through one of the constructors
            • By calling one of the static From… methods
            • By subtracting one DateTime from another

            1. Constructors

            public TimeSpan (int hours, int minutes, int seconds);
            public TimeSpan (int days, int hours, int minutes, int seconds);
            public TimeSpan (int days, int hours, int minutes, int seconds, int milliseconds);

            Console.WriteLine (new TimeSpan (2, 30, 0)); // 02:30:00

            2. The static From… methods are more convenient 
            when you want to specify an interval in just a single unit, such as minutes, hours, and so on:

            public static TimeSpan FromDays (double value);
            public static TimeSpan FromHours (double value);
            public static TimeSpan FromMinutes (double value);
            public static TimeSpan FromSeconds (double value);
            public static TimeSpan FromMilliseconds (double value);

            Console.WriteLine (TimeSpan.FromHours (2.5)); // 02:30:00
            Console.WriteLine (TimeSpan.FromHours (-2.5)); // -02:30:00

            ---Console.WriteLine(new DateTime());      // 1 / 1 / 0001 12:00:00 AM

            NOTE: TimeSpan overloads the < and > operators as well as the + and - operators. 
            The following expression evaluates to a TimeSpan of 2.5 hours:

            TimeSpan ts1 = TimeSpan.FromHours(2) + TimeSpan.FromMinutes(30);
            TimeSpan ts2 = TimeSpan.FromHours(2.5);

            Console.WriteLine($"ts1: {ts1}, ts2: {ts2}\nts1 = ts2: {ts1 == ts2}");

            Using this expression, we can illustrate the -integer- properties 
            [ Days, Hours, Minutes, Seconds, and Milliseconds] :

            TimeSpan nearlyTenDays = TimeSpan.FromDays(10) - TimeSpan.FromSeconds(1);

            Console.WriteLine(nearlyTenDays.Days); // 9
            Console.WriteLine(nearlyTenDays.Hours); // 23
            Console.WriteLine(nearlyTenDays.Minutes); // 59
            Console.WriteLine(nearlyTenDays.Seconds); // 59
            Console.WriteLine(nearlyTenDays.Milliseconds); // 0

            In contrast, the Total... properties return values of type -double- describing the entire time span:

            TimeSpan nearlyTenDays = TimeSpan.FromDays(10) - TimeSpan.FromSeconds(1);

            Console.WriteLine(nearlyTenDays.TotalDays); // 9.99998842592593
            Console.WriteLine(nearlyTenDays.TotalHours); // 239.999722222222
            Console.WriteLine(nearlyTenDays.TotalMinutes); // 14399.9833333333
            Console.WriteLine(nearlyTenDays.TotalSeconds); // 863999
            Console.WriteLine(nearlyTenDays.TotalMilliseconds); // 863999000

            NOTE: The default value for a TimeSpan is TimeSpan.Zero
            Console.WriteLine(new TimeSpan());      // 00:00:00
             
            */

            /* Chosing between DateTime and DateTimeOffset
             DateTime and DateTimeOffset differ in how they handle time zones.

            1. DateTime
            Stores date and time information, but how it handles time zones depends on a three-state flag:
            
            1. Local Time: It represents the time on the computer where the code is running.
            2. UTC: Represents the universal time.
            3. Unspecified: It doesn’t know which time zone it refers to (i.e., no time zone information).

            DateTime localTime = DateTime.Now;                    // Local machine time
            DateTime utcTime = DateTime.UtcNow;                   // UTC time
            DateTime unspecifiedTime = new DateTime(2024, 10, 6); // No time zone

            When comparing two DateTime values, 
            it ignores the time zone information and compares only the date and time fields (year, month, day, etc.).

            DateTime Comparison Example:

            DateTime time1 = new DateTime(2024, 10, 6, 12, 0, 0, DateTimeKind.Utc);  // UTC time
            DateTime time2 = new DateTime(2024, 10, 6, 12, 0, 0, DateTimeKind.Local); // Local time

            Console.WriteLine(time1 == time2);  // True, because DateTime compares components only

            2. DateTimeOffset
            Stores date and time along with an offset from UTC (as a TimeSpan).
            When comparing DateTimeOffset values, the comparison accounts for the absolute time, not just the date and time components.

           var dt1 = new DateTimeOffset (2010, 1, 1, 1, 1, 1, TimeSpan.FromHours(8));
           var dt2 = new DateTimeOffset (2010, 1, 1, 2, 1, 1, TimeSpan.FromHours(9));
           Console.WriteLine (dt1 == dt2); // True

            Console.WriteLine(timeWithOffset1 == timeWithOffset2); // True, as they refer to the same UTC point

            ---Breakdown of the Offsets

            _dt1 Construction:
            
            var dt1 = new DateTimeOffset(2010, 1, 1, 1, 1, 1, TimeSpan.FromHours(8));
            Local Time: January 1, 2010, 1:01:01 AM (UTC+8)
            Offset: +8 hours means that this time is 8 hours ahead of UTC.

            UTC Calculation:
            To convert this to UTC, you subtract the offset:
            1:01:01 AM - 8 hours = December 31, 2009, 5:01:01 PM (UTC)

            _dt2 Construction:

            Local Time: January 1, 2010, 2:01:01 AM (UTC+9)
            Offset: +9 hours means that this time is 9 hours ahead of UTC.
            
            UTC Calculation:
            To convert this to UTC, you also subtract the offset:
            2:01:01 AM - 9 hours = December 31, 2009, 5:01:01 PM (UTC)

            Both dt1 and dt2 resolve to the same UTC time: December 31, 2009, 5:01:01 PM (UTC)

            1. Stores the UTC offset as part of its value.
            2. Comparisons take the time zone offset into account. 
            Even if two DateTimeOffset values have different time components, they can still be equal if they refer to the same point in time.

            */

            /* UTC related discussion
             
            UTC (Coordinated Universal Time) is the primary time standard by which the world regulates clocks and time. 
            It is essentially the modern version of Greenwich Mean Time (GMT) 
            and serves as the global time reference.

            When you use DateTime.UtcNow in C#, it gives you the current time based on UTC,
            which is the same everywhere in the world. 
            It does not account for local time zones or daylight saving time.

            Problem: Inconsistent Timestamps Across Time Zones

            When you're building a logger system or any system that deals with time, 
            users from different countries and time zones will generate events at different times. 
            If you use DateTime.Now, it will reflect the local time of the machine running the code. 

            This can create issues when:
            1. Multiple users in different time zones access the same system.
            2. You need to compare or analyze logs from different regions and time zones.

            For example, if you log an event from Latvia, it might show 7:24 PM, 
            but if someone runs it from Azerbaijan, it will show 8:24 PM. 
            This can cause confusion because these two times refer to the same point in time but 
            appear different depending on the local time zones.

            Solution: Use UTC for Logging, Convert to Local Time for Display
            1. Log Everything in UTC: Use DateTime.UtcNow when logging events. 
            This ensures that no matter where the event occurs, the log always reflects the same universal time.

            2. Convert UTC to Local Time When Needed: When displaying the logs or data to a user, 
            you can convert the UTC time back to the user's local time using their time zone.

            Example: How to Implement This

            Step 1: Log in UTC

            DateTime utcTime = DateTime.UtcNow;
            Console.WriteLine("Event logged at: " + utcTime.ToString("u"));  // Universal time format

            Step 2: Convert UTC to Local Time for Display

            // Convert UTC to local time zone (e.g., Latvia)
            TimeZoneInfo latviaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standart Time");
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, localTimeZone);

            Console.WriteLine("Local time in Latvia: " + localTime);  // Outputs converted time

            */

            /* Working with dates and times

            DateTime dateTime = DateTime.Now;

            Console.WriteLine(dateTime.Year);           // 2024
            Console.WriteLine(dateTime.Month);          // 10
            Console.WriteLine(dateTime.Day);            // 6
            Console.WriteLine(dateTime.DayOfWeek);      // Sunday
            Console.WriteLine(dateTime.DayOfYear);      // 280

            Console.WriteLine(dateTime.Hour); // 10
            Console.WriteLine(dateTime.Minute); // 20
            Console.WriteLine(dateTime.Second); // 30
            Console.WriteLine(dateTime.Millisecond); // 0
            Console.WriteLine(dateTime.Ticks); // 630851700300000000
            Console.WriteLine(dateTime.TimeOfDay); // 10:20:30 (returns a TimeSpan)
            */

            /* Random
             
            1. Standard Random Class

            The Random class is used for non-cryptographic random number generation. 
            By default, it uses the system clock as a seed, but you can specify a seed for reproducibility. 
            If two Random instances are created with the same seed, they will produce the same sequence of random numbers.

            Random r1 = new Random(42);
            Random r2 = new Random(42);

            Console.WriteLine($"r1: {r1.Next(100)}, {r1.Next(100)}"); // Outputs the same sequence
            Console.WriteLine($"r2: {r2.Next(100)}, {r2.Next(100)}"); // Same as r1

            In this case, both r1 and r2 will output the same numbers because they share the same seed (42), making the random sequence predictable. 
            This can be useful for debugging or simulations where you need reproducible behavior.

            Example: Without a seed for non-reproducibility

            Random random = new Random(); // Uses system time as a seed
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Random number {i + 1}: {random.Next(1, 101)}"); // Between 1 and 100
            }

            If you create multiple Random objects quickly (within 10 ms), 
            they may produce identical sequences due to the system clock's limited granularity:

            Random r1 = new Random();
            Random r2 = new Random();
            
            Console.WriteLine(r1.Next()); // May output the same value as r2
            Console.WriteLine(r2.Next());

            Solution: Reuse a single Random object

            static Random sharedRandom = new Random(); // Single instance for the application


            2. Cryptographic Random Number Generation

            For security-critical applications like encryption, the Random class is not suitable because it’s not truly random. 
            Instead, you should use a cryptographic random number generator.

            byte[] secureBytes = new byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(secureBytes);  // Fills the byte array with secure random values

            // Convert the first 4 bytes to an integer
            int secureInt = BitConverter.ToInt32(secureBytes, 0);
            Console.WriteLine($"secure integer: {secureInt}");

            Here, we use the cryptographic random generator to fill a byte array with random values. 
            Since the only way to obtain random data is via byte arrays, 
            BitConverter is used to convert the byte array into an integer or other types.

            NOTE: The reason the Random class in .NET (and in many programming languages) is not considered "truly random" 
            is that it generates numbers using a deterministic algorithm:

            1. Pseudorandomness vs. True Randomness
            
            Pseudorandom Numbers: The Random class produces pseudorandom numbers, meaning the output is determined by an initial value known as the seed. 
            Given the same seed, the sequence of numbers generated will always be the same. 
            This predictability makes it unsuitable for applications like cryptography where unpredictability is essential.

            True Random Numbers: True randomness is derived from inherently unpredictable sources, such as atmospheric noise, radioactive decay, or 
            other quantum phenomena. These sources are not deterministic and produce unique results each time.

            2. Seed and Time-Based Initialization
            
            When you instantiate the Random class without a specific seed, it uses the current system time as the seed. 
            Specifically, it typically takes the number of milliseconds (not seconds) since a fixed date (like January 1, 1970, in Unix time) as the seed value.


            0x7FFFFFFF: This is a hexadecimal representation of a 32-bit integer. In binary, 0x7FFFFFFF is represented as:
            01111111 11111111 11111111 11111111

            This value is significant because it has the highest bit (the sign bit) set to 0, 
            which means the result will be a positive integer when performing the AND operation with another integer.
            */

            /* Process
             
            The Process class in System.Diagnostics allows you to launch a new process.
            For security reasons, the Process class is not available to Windows Store apps, and you cannot start arbitrary processes.

            1. 
            
            The static Process.Start method has several overloads; the simplest accepts a simple filename with optional arguments:
            Process.Start("notepad.exe", "file.txt");
            
            2. 
            The most flexible overload accepts a ProcessStartInfo instance.
            With this, you can capture and redirect the launched process’s input, output, and error output 
            (if you leave UseShellExecute as false).


            // example:
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = "ipconfig all",
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };

            using Process? process = Process.Start(psi);

            if (process is not null)
            {
                string result = process.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
            }

            ---UseShellExecute Property

            1. When UseShellExecute is true:
            The process is started using the operating system shell. You cannot redirect standard input, output, or error streams. 
            This means that if you try to set RedirectStandardOutput, RedirectStandardInput, or RedirectStandardError to true, 
            an InvalidOperationException will be thrown.

            The operating system shell is a user interface that allows users to interact with the operating system (OS) through commands. 
            It serves as a bridge between the user and the underlying system functions, 
            enabling users to execute commands, run programs, manage files, and perform various tasks.

            2. When UseShellExecute is false:
            The process is started directly from the executable file without involving the shell.
            You can redirect standard input, output, and error streams, allowing you to capture the output of the process programmatically.
            */

            /* Redirecting output and error streams
             
            With UseShellExecute false (the default in .NET), you can capture the standard input, output, and error streams and 
            then write/read these streams via the StandardInput, StandardOutput, and StandardError properties.
             
            The following method runs an executable while capturing both the output and error streams:

            static (string output, string errors) RunCommand(string exePath = "", string args = "")
            {
                using Process? process = Process.Start(new ProcessStartInfo(exePath, args)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                });

                StringBuilder errors = new StringBuilder();

                if (process is not null)
                {
                    process.ErrorDataReceived += (sender, errorArgs) =>
                    {
                        if (errorArgs.Data is not null)
                        {
                            errors.AppendLine(errorArgs.Data);
                        }
                    };
                    process.BeginErrorReadLine();

                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    return (output, string.Join('\n', errors));
                }

                return (string.Empty, string.Empty);
            }

             var response = RunCommand("powershell.exe", "ipconfig all");
             if (string.IsNullOrEmpty(response.errors))
             {
                 Console.WriteLine(response.output);
             }

            */

            /* UseShellExecute
             
            The UseShellExecute flag changes how the CLR starts the process. With UseShell Execute true, you can do the following:
             
            */

            var response = RunCommand("powershell.exe", "Get-Process");

            switch (string.IsNullOrEmpty(response.errors))
            {
                case true:
                    Console.WriteLine(response.output);
                    break;
                case false:
                    Console.WriteLine(response.errors);
                    break;
            }
        }

        static (string output, string errors) RunCommand(string exePath = "", string args = "")
        {
            using Process? process = Process.Start(new ProcessStartInfo(exePath, args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            });

            StringBuilder errors = new StringBuilder();

            if (process is not null)
            {
                process.ErrorDataReceived += (sender, errorArgs) =>
                {
                    if (errorArgs.Data is not null)
                    {
                        errors.AppendLine(errorArgs.Data);
                    }
                };
                process.BeginErrorReadLine();

                Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                bool exited = process.WaitForExit(3 * 1000); //  wait for up to 3 seconds.

                if (!exited)
                {
                    Console.WriteLine("Process did not exit within the timeout period.");
                    process.Kill();
                }

                return (outputTask.Result, string.Join('\n', errors));
            }

            return (string.Empty, string.Empty);
        }
    }
}