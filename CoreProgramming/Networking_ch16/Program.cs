using System.Net;
using System.Net.Sockets;

internal class Program
{
    private static void Main(string[] args)
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

    }
}