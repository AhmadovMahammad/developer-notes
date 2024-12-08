using System.Net;
using System.Net.Sockets;

internal class Program
{
    private static async Task Main(string[] args)
    {
        /* Network Architecture
         
        To understand network architecture in .NET,
        let’s start by covering the underlying structure that organizes how information travels over a network.

        The OSI model is structured to describe the functionality of networking or telecommunication systems through seven hierarchical layers. 
        Each layer has a specific role and interacts with the layers directly above and below it. 
        
        The OSI model includes these layers, from top to bottom:
            1_ Application, 
            2_ Presentation,
            3_ Session, 
            4_ Transport, 
            5_ Network, 
            6_ Data Link, and 
            7_ Physical.

        .NET’s networking tools in the System.Net namespace largely operate within these layers, 
        especially the Application and Transport layers.

        --- Layer 1: Physical Layer

        The Physical layer is the foundation. 
        It includes the actual physical hardware, like cables, Wi-Fi signals, and network cards. 
        Here, data is transmitted as raw bits (0s and 1s) over a physical medium. 
        This layer doesn’t care about what the data means; it only handles the physical transmission.

        * Analogy: Think of it as the road in a delivery system—it’s the path data uses to move, 
        whether it’s a highway (fiber optic cable) or a residential street (Wi-Fi).

        --- Layer 2: Data Link Layer

        The Data Link layer works directly above the Physical layer to manage how data is framed and moved across the physical medium.
        This layer packages raw bits from the Physical layer into frames (more organized data packets) and 
        ensures error checking to detect any errors during transmission.

        * Analogy: It’s like a delivery vehicle moving along the road (Physical layer). 
        The Data Link layer wraps and tracks each data packet, 
        similar to how packages are loaded onto a delivery truck and labeled.

        --- Layer 3: Network Layer

        The Network layer handles routing, 
        which means determining the correct path data will take to reach its destination across multiple networks. 

        This is where IP addresses come into play, which uniquely identify devices on a network, 
        allowing data to be sent to the correct location.

        * Analogy: Imagine a GPS system for delivery trucks. 
        It decides the route the truck will take to get to the correct city or town. 
        The Network layer uses IP addresses just like an address helps locate a house in a city.

        --- Layer 4: Transport Layer

        The Transport layer manages end-to-end data transmission. 
        It controls how much data is sent, ensures reliability, and checks for errors. 

        end-to-end transmission involves breaking down the data into smaller packets 
        that are transmitted through a network of intermediate nodes, 
        where each node is responsible for forwarding the packets to the next node until they reach the destination. 

        Two key protocols operate here:

        1) TCP (Transmission Control Protocol): TCP ensures reliable communication. 
           It breaks data into packets, tracks them, and reassembles them in order. 
           If packets are lost, TCP requests them again. This reliability is useful for applications like file transfers.

        2) UDP (User Datagram Protocol): UDP is faster than TCP but doesn’t guarantee delivery, ordering, or error checking. 
           It simply sends data without tracking it. 
           It’s suitable for applications like live streaming, where speed is more important than complete reliability.

        * Analogy: Think of TCP like a postal service with package tracking and 
        UDP like dropping postcards in the mail without tracking.

        --- Layer 5: Session Layer

        The Session layer manages the start, control, and end of communication sessions. 
        It keeps data from different applications separate so they don’t interfere with one another. 
        
        For instance, if you’re using a video call app and a web browser simultaneously, 
        the Session layer ensures each session is separate.

        * Analogy: It is like an event organizer who ensures that each speaker at a conference
        has their own time slot.

        --- Layer 6: Presentation Layer

        The Presentation layer acts as a translator, 
        formatting data so that the application on the receiving end can understand it. 
        It handles encryption and decryption (making data secure) and data compression (reducing data size for faster transmission).
        
        * Analogy: It’s like an interpreter at an international conference, 
        translating messages so that everyone can understand.

        --- Layer 7: Application Layer

        The Application layer is the top layer that interacts with end-user applications directly. 
        When you use a web browser, email client, or FTP program, you’re operating in the Application layer. 
        This is where protocols like HTTP (for web pages), FTP (for file transfers), and SMTP (for email) come in. 
        They each define how applications should behave when interacting over a network.
        
        * Analogy: It’s like the user-friendly interface on your phone, 
        like the email app that lets you easily send and receive emails without worrying about what’s happening behind the scenes.

        ---------------------------------------------------------

        --- How .NET Works Within This Model?

        Now, let’s connect this to .NET’s networking tools in the System.Net namespace. 
        .NET provides classes and protocols that work mainly in the Application and Transport layers, 
        meaning .NET allows you to focus on what your application is doing with the network, rather than the mechanics of data movement.

        1_ Application Layer Tools: 
        HttpClient, WebClient, SmtpClient, and other classes handle high-level protocols such as HTTP, FTP, and SMTP. 
        These classes manage the communication specifics, 
        allowing you to upload or download files, make API calls, and send emails without writing low-level code.

        2_ Transport Layer Tools: 
        Classes like TcpClient, UdpClient, and TcpListener operate at a lower level, 
        enabling you to create custom, direct communication channels between devices over TCP or UDP. 
        
        For instance, in a chat application, you might use TcpClient for reliable messaging or 
        UdpClient if you’re creating a fast-paced game where real-time speed is essential.

        */

        /* Addresses
         
        For any device to participate in data transmission on the internet or a local network, 
        it needs a unique identifier, this is where IP addresses come in.
        
        An IP address works like an "address label," helping each device know where to send or receive data. 
        Without IP addresses, there would be no way to direct data to specific devices.
        The internet primarily uses two types of IP addressing systems, IPv4 and IPv6.

        1) IPv4 (Internet Protocol version 4) is the traditional addressing system. 
        IPv4 addresses are 32 bits in length,
        typically formatted as four decimal numbers separated by dots (e.g., 192.168.1.1). 
        Each decimal represents 8 bits of the address, providing about 4.3 billion unique IP addresses, which was initially sufficient. 
        
        However, with the rapid growth of internet-connected devices, 
        IPv4 address space has become increasingly scarce, leading to the development of IPv6.

        2) IPv6 addresses are much larger—128 bits long—and are written in hexadecimal, 
        separated by colons (e.g., 3EA0:FFFF:198A:E4A3:4FF2:54FA:41BC:8D31). 
        
        IPv6 was designed to accommodate a vastly larger number of devices and 
        ensure that new devices could always be uniquely identified on the internet.

        In .NET, the IPAddress class in the System.Net namespace handles both IPv4 and IPv6 addresses. 
        It can be initialized with a byte array (IPv4 only) or parsed from a formatted string, 
        which is convenient when working with either IP format. 

        When parsed, the IPAddress object can return details like the AddressFamily property, 
        which indicates whether it’s an InterNetwork (IPv4) or InterNetworkV6 (IPv6) address.

        IPAddress ipv4Address = new IPAddress(new byte[] { 192, 168, 1, 1 });
        IPAddress ipv4Address_v2 = IPAddress.Parse("192.168.1.1");
        Console.WriteLine(ipv4Address);
        Console.WriteLine(ipv4Address.AddressFamily); // InterNetwork

        IPAddress ipv6Address = IPAddress.Parse("[3EA0:FFFF:198A:E4A3:4FF2:54FA:41BC:8D31]");
        Console.WriteLine(ipv6Address);
        Console.WriteLine(ipv6Address.AddressFamily); // InterNetworkV6

        */

        /* Ports and Protocols
     
        An IP address by itself only identifies a device but 
        does not specify which application or service on that device should handle incoming data. 
        
        This is where ports come in. Ports are essentially "channels" on an IP address, 
        allowing multiple applications to communicate over the same network interface without interfering with each other. 
        
        For instance, on a single IP address, port 80 is traditionally used for HTTP traffic, 
        while port 25 is used for SMTP (email), enabling a computer to handle both web and email traffic simultaneously.

        In .NET, the IPEndPoint class combines an IP address with a specific port. 
        This combination allows for complete identification of a network endpoint for a particular service. 
        The IPEndPoint class is useful for working with protocols like TCP and UDP that operate at the transport layer, 
        facilitating direct data exchange between applications over a network.


        IPAddress iPAddress = IPAddress.Parse("192.168.1.1");
        int port = 8080;
        IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);

        Console.WriteLine(iPEndPoint); // 192.168.1.1:8080

        */

        /* Firewalls and Port Restrictions
         
        In many network environments, especially corporate ones, 
        firewalls restrict access to certain ports to prevent unauthorized access and protect the network. 
        
        Typically, only a few ports, like 80 (HTTP) and 443 (HTTPS), remain open for general traffic. 
        This limitation can impact applications that use non-standard or high-numbered ports (e.g., 49152 to 65535), 
        which are typically reserved for private or dynamic allocation. 
        
        These ports are often suitable for testing and custom applications, 
        but they may be blocked by firewalls in restrictive environments, which is an essential consideration for network application developers.

        */

        /* URIs
         
        A URI (Uniform Resource Identifier) is a standardized way to identify resources on the internet or 
        within a local network, such as web pages, files, or email addresses. 

        http://www.ietf.org
        ftp://myisp/doc.txt
        mailto:joe@bloggs.com

        --- Basic Components of a URI
        A URI is typically broken down into key elements:

        Scheme: Defines the protocol or method used to access the resource, such as http, https, ftp, file, or mailto.
        Authority:  Identifies the host, which could be a domain name like www.example.com or an IP address.
        Path: Specifies the exact location of the resource on the host, such as /images/photo.jpg.

        For instance, in the URI http://www.example.com/images/photo.jpg, 
        1) http is the scheme, 2) www.example.com is the authority, and 3) /images/photo.jpg is the path.

        --- Working with URIs in .NET
        The .NET Uri class (part of the System namespace) provides tools to handle and manipulate URIs, making it easy to:

        1_ Validate URI formatting.
        2_ Separate different components (like scheme, authority, and path).
        3_ Distinguish between absolute and relative URIs.

        This is especially useful when validating, modifying, or analyzing URIs in applications that use network resources.

        Uri uri = new Uri("http://www.example.com:80/page.html");
        Console.WriteLine(uri.Scheme);       // Outputs: http
        Console.WriteLine(uri.Authority);    // Outputs: www.example.com
        Console.WriteLine(uri.Port);         // Outputs: 80
        Console.WriteLine(uri.AbsolutePath); // Outputs: /page.html

        --- URI Types: Absolute vs. Relative URIs

        1) Absolute URIs
        An absolute URI contains all the necessary information to access a resource independently. 
        This includes the scheme, authority, and path, which allow it to stand alone without additional context.

        Examples: 
        http://www.example.com/index.html
        ftp://ftp.example.com/files/readme.txt

        In .NET, if you provide a complete URI string, the Uri class interprets it as an absolute URI by default.

        Uri absoluteUri = new Uri("http://www.example.com/page.html");
        Console.WriteLine(absoluteUri.IsAbsoluteUri); // True

        2) Relative URIs
        A relative URI contains only a partial path to a resource, typically just the path portion, 
        and relies on a base URI to form a complete address. 
        Relative URIs are common in web applications, where paths link to resources on the same host.

        Examples:
        images/photo.jpg
        page.html

        In .NET, you can create a relative URI by specifying UriKind.Relative when constructing a new URI.

        Uri relativeUri = new Uri("page.html", UriKind.Relative);
        Console.WriteLine(relativeUri.IsAbsoluteUri); // Outputs: False

        Since relativeUri lacks full address information, it needs a base URI to form a complete path.

        --- Combining Relative URIs with Base URIs
        
        Combining a base URI with a relative URI generates an absolute URI. 
        This is useful for applications where resources share a common host or directory.

        Uri baseUri = new Uri("http://www.example.com");
        Uri relativeUri = new Uri("page.html", UriKind.Relative);
        Uri absoluteUri = new Uri(baseUri, relativeUri);

        Console.WriteLine(absoluteUri);

        --- Practical Usage of URI Properties and Methods
        IsLoopback: Returns true if the URI references the local host (127.0.0.1).
        IsFile: Indicates if the URI points to a local or UNC file path.
        LocalPath: Converts AbsolutePath to the format native to the OS (slashes for Unix, backslashes for Windows).

        Uri localFile = new Uri(@"file:///C:/myfiles/data.xlsx");
        Console.WriteLine(localFile.IsFile);      // True
        Console.WriteLine(localFile.LocalPath);   // C:\myfiles\data.xlsx

        --- To compare or derive paths, Uri provides methods like MakeRelativeUri():

        Uri baseUri = new Uri("http://www.domain.com:80/info/");
        Uri pageUri = new Uri("http://www.domain.com/info/page.html");

        if (pageUri.IsBaseOf(baseUri))
        {
            Uri relativeUri = baseUri.MakeRelativeUri(pageUri);
            Console.WriteLine(relativeUri.ToString());
        }

        */

        /* HttpClient
         
        HttpClient is a powerful and flexible class provided by .NET to perform HTTP requests and handle responses. 
        It is commonly used to interact with RESTful APIs and handle more complex HTTP tasks

        -- Key Advantages of HttpClient Over WebClient

        1) Concurrency:
        A single instance of HttpClient can handle multiple concurrent requests, 
        enabling efficient resource usage and reducing overhead. 
        In contrast, WebClient requires a new instance for each concurrent request.

        2) Custom Message Handlers:
        HttpClient allows developers to plug in custom message handlers. 
        These handlers can intercept requests and responses to add custom logic, such as logging, compression, or encryption.

        Custom message handlers are also helpful in unit testing because 
        they enable easy mocking of HTTP responses, making HttpClient more test-friendly than WebClient.

        3) Extensible Header and Content System:
        HttpClient has a rich type system for handling headers and content, 
        allowing you to work with complex header configurations and different content types easily.

        --- Using HttpClient for Basic Requests

        The simplest way to use HttpClient is to create an instance and use one of its Get* methods 
        (such as GetStringAsync, GetByteArrayAsync, or GetStreamAsync) to perform a request:

        string html = await new HttpClient().GetStringAsync("https://bra-invest.com/");
        Console.WriteLine(html);

        All HttpClient methods that perform network I/O are asynchronous, 
        which means you should always use await or .Result (with caution) when calling them.
        To avoid DNS resolution overhead and keep sockets from being held open unnecessarily, 
        you should reuse the same HttpClient instance across multiple requests.


        --- Making Concurrent Requests with HttpClient

        HttpClient supports concurrent requests, which makes it very efficient for applications that 
        need to perform multiple network calls at once. Here’s how you can fetch multiple web pages in parallel:

        var client = new HttpClient();

        var task1 = client.GetStringAsync("http://www.linqpad.net");
        var task2 = client.GetStringAsync("http://www.albahari.com");

        string[] results = await Task.WhenAll(task1, task2);
        Console.WriteLine(string.Join("-", results));

        */

        /* Configuration Options in HttpClient

        HttpClient comes with several configurable properties to fine-tune request behavior:

        1) Timeout:
        The Timeout property sets the maximum time to wait for a request to complete. 
        If the timeout is reached, an exception is thrown. 
        By default, this timeout is set to 100 seconds, but you can modify it based on your needs.

        HttpClient client = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };


        2) BaseAddress:
        The BaseAddress property sets a base URL for all requests made by that HttpClient instance. 
        This is particularly helpful when making multiple requests to the same base domain.

        HttpClient client = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(10),
            BaseAddress = new Uri("https://bra-invest.com/")
        };

        string response = await client.GetStringAsync("about"); // Requests https://bra-invest.com/about
         
        
        --- Advanced Configuration with HttpClientHandler

        To manage more advanced configurations, such as cookies, proxies, and authentication, 
        HttpClient uses the HttpClientHandler class. 
        You can create an instance of HttpClientHandler and pass it to the HttpClient constructor to control behavior at a lower level.

        HttpClientHandler handler = new HttpClientHandler()
        {
            UseProxy = false,
            UseCookies = true
        };

        HttpClient httpClient = new HttpClient(handler);

        */

        /* Making a Request with HttpClient
         
        To demonstrate, we'll use the free JSONPlaceholder API (https://jsonplaceholder.typicode.com) to fetch 
        fake data like posts, comments, and users.

        Example: Basic GET Request

        HttpClientHandler handler = new HttpClientHandler() { UseCookies = true };
        HttpClient httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"),
            Timeout = TimeSpan.FromSeconds(5),
        };

        try
        {
            // Send a GET request
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("posts");
            httpResponseMessage.EnsureSuccessStatusCode(); // Ensure the response was successful

            string content = await httpResponseMessage.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {content}");
        }
        catch (HttpRequestException exception)
        {
            Console.WriteLine(exception.Message);
        }

        --- Understanding Headers in HttpClient
        HTTP headers provide additional information about the request or response.
        They are key-value pairs sent in the header section of HTTP messages.

        -Common HTTP Headers
        1) User-Agent:
        Identifies the client application making the request.
        Example: Mozilla/5.0 for browsers or a custom string for APIs.

        httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientExampleApp");

        2) Authorization
        Contains credentials to authenticate the client.
        Example: Bearer <token> for token-based authentication.

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");

        3) Content-Type:
        Specifies the format of the request body.
        Example: application/json for JSON payloads.

        4) Accept:
        Indicates the formats the client can process in the response.
        Example: application/json for JSON responses.

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        ---> Example:

        HttpClientHandler handler = new HttpClientHandler() { UseCookies = true };
        HttpClient httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"),
            Timeout = TimeSpan.FromSeconds(5),
        };

        httpClient.DefaultRequestHeaders.Add("User-Agent", "HttClientExampleApp");

        try
        {
            // Send a GET request
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("posts/1");
            httpResponseMessage.EnsureSuccessStatusCode(); // Ensure the response was successful

            string content = await httpResponseMessage.Content.ReadAsStringAsync();
            using JsonDocument jsonDocument = JsonDocument.Parse(content);
            JsonElement root = jsonDocument.RootElement;

            Console.WriteLine($"id: {root.GetProperty("id")}");
            Console.WriteLine($"title: {root.GetProperty("title")}");
            Console.WriteLine($"body: {root.GetProperty("body")}");
        }
        catch (HttpRequestException exception)
        {
            Console.WriteLine(exception.Message);
        }

        :Summary
        HttpClient is a versatile and powerful tool for making HTTP requests in .NET. 
        It simplifies the process of interacting with web APIs and supports advanced use cases through headers, concurrency, custom handlers. 

        */



        /* HTTP Status Codes and Exception Handling
         
        When you send an HTTP request using HttpClient, the server responds with an HTTP status code.
        This status code indicates the outcome of the request:

        2xx: Success (e.g., 200 OK, 201 Created).
        3xx: Redirection (e.g., 301 Moved Permanently).
        4xx: Client Errors (e.g., 404 Not Found, 400 Bad Request).
        5xx: Server Errors (e.g., 500 Internal Server Error).

        -- Handling HTTP Status Codes in HttpClient

        By default, HttpClient doesn’t throw exceptions for non-successful status codes (e.g., 404). 
        However, network errors or DNS resolution failures will throw exceptions.

        You can enforce status code validation by calling httpResponseMessage.EnsureSuccessStatusCode().
        This method throws an exception the the status code is not in the 2xx range.

        EXAMPLE handling exceptions and status codes

        HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"),
            Timeout = TimeSpan.FromSeconds(5),
        };

        try
        {
            // Send a GET request to a valid endpoint
            HttpResponseMessage response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts/1");

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Request successful!");
                Console.WriteLine($"Status Code: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
            else
            {
                Console.WriteLine($"Request failed. Status Code: {(int)response.StatusCode} {response.ReasonPhrase}");
            }

            // Enforce successful response
            response.EnsureSuccessStatusCode(); // Throws an exception if not successful

            // Read and print the content
            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response Content:");
            Console.WriteLine(content);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }

        The HttpContent.CopyToAsync() method lets you write response content directly to another stream. 
        This is useful for downloading large files without loading them entirely into memory.

        response.EnsureSuccessStatusCode(); // Throws an exception if not successful

        using FileStream fileStream = File.Create("response.json");
        await response.Content.CopyToAsync(fileStream);
        Console.WriteLine("Response content saved to 'response.json'");

        JSON file: 
        {
          "userId": 1,
          "id": 1,
          "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
          "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
        }

        */


        /* HTTP Verbs in HttpClient
         
        HttpClient supports all four primary HTTP verbs through the following methods:

        _ GetAsync: Fetch data.
        _ PostAsync: Upload data to create a resource.
        _ PutAsync: Update a resource.
        _ DeleteAsync: Remove a resource.

        These methods are asynchronous and return an HttpResponseMessage.

        HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"),
            Timeout = TimeSpan.FromSeconds(5),
        };

        PostModel postModel = new PostModel()
        {
            Id = 123456789,
            Title = "foo",
            Body = "bar",
        };

        string json = JsonSerializer.Serialize(postModel);
        HttpContent httpContent = new ByteArrayContent(Encoding.UTF8.GetBytes(json));

        try
        {
            // Send a GET request to a valid endpoint
            HttpResponseMessage response = await httpClient.PostAsync("https://jsonplaceholder.typicode.com/posts", httpContent);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response:");
            Console.WriteLine(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }


        NOTE:
        The four methods just described are all shortcuts for calling SendAsync, 
        the single low-level method into which everything else feeds. 
        To use this, you first construct an HttpRequestMessage:

        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "posts/1");
        is equal to
        httpClient.GetAsync("posts/1");

        HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
        is equal to
        HttpResponseMessage response = await httpClient.PostAsync("https://jsonplaceholder.typicode.com/posts", httpContent);

        Instantiating a HttpRequestMessage object means that you can customize properties of the request, 
        such as the headers and the content itself, allowing you to upload data.
        
        string json = JsonSerializer.Serialize(postModel);
        HttpContent httpContent = new ByteArrayContent(Encoding.UTF8.GetBytes(json));

        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "posts")
        {
            Content = httpContent
        };
        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpRequestMessage.Headers.Add("Custom-Header", "HttpClientExample");

        try
        {
            // Send a GET request to a valid endpoint
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response:");
            Console.WriteLine(responseBody);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
        }

        */

        /* Proxy

        A proxy server acts as an intermediary between a client (your application) and the internet. 
        It routes HTTP or FTP requests and responses, often for security, monitoring, or performance optimization. 
        Proxy servers are widely used in corporate environments, schools, and some ISPs to manage internet access and enforce policies. 
        They can also be used for caching, filtering content, or adding an extra layer of anonymity.

        In .NET, proxies can be configured for your HTTP requests using the HttpClient class through its HttpClientHandler. 
        This approach allows for flexible and efficient network communication while adhering to corporate or network policies.
        Companies route traffic through a proxy to monitor, filter, and block malicious content.

        --- Setting Up a Proxy with HttpClient
        To configure a proxy for your HTTP requests in .NET, you need to:
        
            1_ Define the proxy's address and port.
            2_ Set up credentials if the proxy requires authentication.
            3_ Pass the proxy configuration to the HttpClientHandler, which is then passed to the HttpClient.

        var proxyAddress = "192.178.10.49";
        var proxyPort = 808;

        var proxyCredentials = new NetworkCredential("username", "password", "domain");
        var proxy = new WebProxy(proxyAddress, proxyPort)
        {
            Credentials = proxyCredentials
        };

        var handler = new HttpClientHandler
        {
            Proxy = proxy,
            UseProxy = true
        };

        HttpClient httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:7268"),
            Timeout = TimeSpan.FromSeconds(5),
        };

        */

        /* Using TCP
        
        TCP (Transmission Cont. Prot.) is one of the core protocols of the Internet Protocol Suite, operating at the transport layer. 
        It is connection-oriented, meaning it ensures reliable communication through mechanisms like 
        error-checking, retransmission, and flow control.

        TCP provides a means for applications to exchange data across a network as a stream of bytes. 
        In .NET, TCP communication can be implemented using classes such as TcpClient and TcpListener, or the lower-level Socket class.

        When using TCP, there are distinct roles for the client and server. 
        A client initiates the connection, while a server listens for incoming connections.

        */

        /* Basic TCP Communication
         
        The simplest implementation of TCP involves creating a server that listens for connections and 
        a client that connects to the server. Here is an example:

        The server uses the TcpListener class. This class requires an IP address and port to bind to. 
        The server waits for incoming connections using the AcceptTcpClient method, which blocks until a client connects.

        static void Server()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 51111);
            server.Start();
            Console.WriteLine("Server started, waiting for client...");

            using (TcpClient client = server.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            {
                Console.WriteLine("Client connected.");
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);

                string? message = reader.ReadLine();
                Console.WriteLine($"Received: {message}");

                writer.WriteLine($"{message} right back!");
                writer.Flush();
            }

            server.Stop();
        }

        1) TcpListener listens for connections on a specified IP and port. 
        IPAddress.Any ensures the server listens on all available network interfaces.
        2) The AcceptTcpClient method blocks until a client connects.

        static void Client()
        {
            using (TcpClient client = new TcpClient("localhost", 51111))
            using (NetworkStream stream = client.GetStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                StreamReader reader = new StreamReader(stream);

                writer.WriteLine("Hello");
                writer.Flush();

                string? response = reader.ReadLine();
                Console.WriteLine($"Server responded: {response}");
            }
        }

        1) The TcpClient connects to the server at the specified address (localhost) and port.

        ----- Key Concepts in TCP Communication

        1_ Blocking Nature of Synchronous Methods

        Both AcceptTcpClient and Read/Write operations block until they complete. 
        This means that the server cannot handle multiple clients simultaneously unless 
        additional threads are created for each client. While this is fine for simple applications, it is not scalable.

        2_ NetworkStream

        The NetworkStream provides a two-way communication channel over TCP. 
        It can read and write raw bytes, making it suitable for sending any type of data. 
        
        However, developers need to define their own protocol to ensure both the client and server understand 
        the structure of the transmitted data.

        3_ Reliability

        TCP ensures that data is delivered reliably. 
        It handles retransmission of lost packets, maintaining the order of transmitted bytes. 
        Developers do not need to manage these complexities manually.

        */

        /* Asynchronous TCP Communication
         
        For scalability, asynchronous methods should be used. 
        Instead of blocking threads, asynchronous methods return Task objects that can be awaited. 
        This allows the application to handle thousands of concurrent connections without consuming an excessive number of threads.

        --- Asynchronous Server
        An asynchronous server listens for connections and processes each client in a separate Task.

        static async Task Server()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 51111);
            listener.Start();
            Console.WriteLine("Server is listening...");

            int n = 0;

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected.");
                n++;

                _ = ProcessClient(client, n);

                await Task.Delay(2 * 1000);
            }
        }

        static async Task ProcessClient(TcpClient client, int n = 0)
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            {
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

                string? message = await reader.ReadLineAsync();
                Console.WriteLine($"Received_ {n}: {message}");
                await writer.WriteLineAsync($"{message} right back from {n}!");
            }
        }

        1_ AcceptTcpClientAsync is the asynchronous equivalent of AcceptTcpClient,
        allowing the server to continue listening for new clients while processing current ones.
        2_ Each client is handled in a separate Task, enabling concurrent processing.


        static async Task Client()
        {
            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync("localhost", 51111);
                Console.WriteLine("Connected to a server.");

                using (NetworkStream stream = client.GetStream())
                {
                    StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
                    StreamReader reader = new StreamReader(stream);

                    await writer.WriteLineAsync("Hello");
                    string? response = await reader.ReadLineAsync();
                    Console.WriteLine($"Server responded: {response}");
                }
            }
        }


        --- Protocol Design in TCP
        TCP provides reliable byte-stream communication, but it does not define how to interpret the bytes. 
        Developers must create their own application-level protocol. For instance:

        1_ Prefixing messages with their length helps in determining when a message ends.
        2_ Using JSON or XML can standardize data exchange but may introduce additional overhead.

        The example server and client exchange simple textual messages. 
        For more complex data, you might serialize objects using formats like JSON or 
        Protocol Buffers before sending them over the stream.

        */

        _ = Server();
        for (int i = 0; i < 10; i++)
        {
            _ = Client();
        }

        Console.ReadLine();
    }

    static async Task Server()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 51111);
        listener.Start();
        Console.WriteLine("Server is listening...");

        int n = 0;

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            Console.WriteLine("Client connected.");
            n++;

            _ = ProcessClient(client, n);

            await Task.Delay(2 * 1000);
        }
    }

    static async Task ProcessClient(TcpClient client, int n = 0)
    {
        using (client)
        using (NetworkStream stream = client.GetStream())
        {
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            string? message = await reader.ReadLineAsync();
            Console.WriteLine($"Received_ {n}: {message}");
            await writer.WriteLineAsync($"{message} right back from {n}!");
        }
    }

    static async Task Client()
    {
        using TcpClient client = new TcpClient();
        await client.ConnectAsync("localhost", 51111);
        Console.WriteLine("Connected to a server.");

        using NetworkStream stream = client.GetStream();
        StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
        StreamReader reader = new StreamReader(stream);

        await writer.WriteLineAsync("Hello");
        string? response = await reader.ReadLineAsync();
        Console.WriteLine($"Server responded: {response}");
    }
}