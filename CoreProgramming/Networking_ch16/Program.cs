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

        */
    }
}