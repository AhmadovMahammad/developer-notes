using System.Xml;
using System.Xml.Linq;

namespace LinqToXML;

internal class Program
{
    private static void Main(string[] args)
    {
        /* What is a DOM?
         
        Consider the following XML file:
        
        <?xml version="1.0" encoding="utf-8"?>
        <customer id="123" status="archived">
            <firstname>Mahammad</firstname>
            <lastname>Ahmadov</lastname>
        </customer>
         
        As with all XML files, 
        we start with a declaration and then a root element, whose name is customer.
        
        The customer element has two attributes, 
        each with a name (id and status) and value ("123" and "archived"). 
        Within customer, there are two child elements, firstname and lastname, 
        each having simple text content ("Mahammad" and "Ahmadov").

        In a DOM, every piece of the XML, whether it's an element, an attribute, or some text, 
        becomes an object that can be represented in code. 
        These objects are connected in a tree-like way: 
        
        The customer element is a parent node, 
        and its children are the firstname and lastname elements.

        This is called a Document Object Model, or DOM.

        */

        /* The LINQ to XML DOM or X-DOM
         
        X-DOM is a system in C# that helps you work with XML data in an efficient, intuitive way. 
        It consists of two primary components:

        1. X-DOM:
        This includes types like XDocument, XElement, and XAttribute which allow you 
        to represent an XML file as a tree of objects. 
        You can load XML from a file, manipulate it by adding or changing elements and attributes, 
        and save it back to an XML file. 

        2. Supplementary LINQ Query Operators:
        LINQ to XML comes with a set of query operators that 
        you can use to filter, sort, or transform XML data efficiently.

        ---How Does It Work?

        XDocument: 
        This represents an entire XML document, 
        including the root element and optional declaration.
        
        XElement: 
        Represents an XML element (like <customer>, <firstname>, etc.) and 
        can contain other elements, attributes, or text.
        
        XAttribute: 
        Represents an XML attribute (like id="123" in <customer id="123">).

        <customer id="123" status="archived">
          <firstname>Mahammad</firstname>
          <lastname>Ahmadov</lastname>
        </customer>

        // Create the XElement for the customer
        XElement customer = new XElement("customer",
            new XAttribute("id", 23),
            new XAttribute("status", "archived"),
            new XElement("firstname", "mahammad"),
            new XElement("lastname", "ahmadov"));

        // Add it to an XDocument
        XDocument doc = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"), // Optional XML declaration
            customer);

        // Print the XML document
        Console.WriteLine(doc);

         // Step 2: Apply LINQ query to extract the first name
        var descendants = doc.Descendants();
        foreach (var descendant in descendants)
        {
            Console.WriteLine($"xelement name: {descendant.Name}");
        }

        var firstName = descendants
            .Where(d => d.Name == "firstname")
            .Select(d => d.Value).FirstOrDefault();

        // you can call descendant element directly within a constructor.
        var firstName2 = doc.Descendants("firstname")
            .Select(d => d.Value).FirstOrDefault();

        Console.WriteLine($"First name from the XML: {firstName}");
        Console.WriteLine("====================================");

        // Step 3: Query attributes
        var customerAttributes = descendants
            .Where(d => d.Name == "customer")
            .Attributes();

        foreach (var attribute in customerAttributes)
        {
            Console.WriteLine($"attribute: {attribute.Name}");
        }

        // you can also call attribute without using where linq query
        var customerID1 = customerAttributes
            .Where(a => a.Name == "id");

        var customerID2 = doc.Descendants("customer").Attributes("id").FirstOrDefault();
        Console.WriteLine($"\nCustomer ID: {customerID2?.Value}");

        NOTE:

        you can add an attribute to the firstname element in the XElement structure and 
        still include a value for it. XElement supports both child elements and attributes.
       
        To add an attribute to firstname, 
        you simply need to pass an XAttribute in addition to the element's value.

        XElement customer = new XElement("customer",
            new XAttribute("id", 23),
            new XAttribute("status", "archived"),
            new XElement("firstname",
                new XAttribute("language", "en"),
                "mahammad"),
            new XElement("lastname", "ahmadov"));

        and XML will be like this:

        <customer id="23" status="archived">
          <firstname language="en">mahammad</firstname>
          <lastname>ahmadov</lastname>
        </customer>

        */

        /* Loading and Parsing in LINQ to XML
         
        LINQ to XML provides easy ways to load and parse XML data, 
        either from a file, a URI, or even from raw strings.
        
        This is done using the XDocument or XElement classes, 
        which form the basis of the X-DOM. Here’s a breakdown of how it works:

        1. Loading XML:
        This method is used to load XML from external sources, such as a file, URI, stream, or reader. 
        It reads the XML and builds the X-DOM tree that can be queried or manipulated.

        // Load XML from a URI (like a web resource)
        XDocument fromWeb = XDocument.Load("http://albahari.com/sample.xml");
        
        // Load XML from a file
        XElement fromFile = XElement.Load(@"e:\media\somefile.xml");

        2. Parsing XML:
        If you have XML content in a string, you can use the Parse method to build an X-DOM tree. 
        This is useful when the XML is stored in a variable or retrieved from a source where it’s not stored in a file.

        XElement xElement = XElement.Parse(@"
        <person name=""Mahammad"" age=""21"">
          <job location=""Baku"" workHours=""8"">Software Developer</job>
          <address>
            <city>Sumgait</city>
            <country>Azerbaijan</country>
          </address>
        </person>");

        Console.WriteLine(xElement.Name); // returns person

        */

        /* The difference between Elements() and Descendants()

        1. Elements():
        
        1.1) Returns the immediate child elements of the current node.
        1.2) Use this when you want to get the direct children of an element, 
             but not any deeper or nested elements.

        For example, calling Elements() on a root element will only return its direct children.

        var immediateChildren = xElement.Elements();
        foreach (var element in immediateChildren) //.Concat(new[] { xElement })
        {
            Console.WriteLine(element.Name);  // Outputs: job, address
        }
        ps: it does not return city, country.

        2. Descendants():

        2.1) Returns all elements that are nested within the current element, 
        including all levels of children, grandchildren, etc., 
        but it does not return the current element itself (only its descendants).
        2.2) Use this when you want to retrieve every nested element under a specific element, 
             regardless of the depth.

        var allDescendants = person.Descendants();
        foreach (var descendant in allDescendants)
        {
            Console.WriteLine(descendant.Name);
        }

        Output: job, address, city, country (all child elements at all levels). 
        
        
        */

        /* Saving and Serializing
         
        Calling ToString on any node converts its content to an XML string—formatted 
        with line breaks and indentation as we just saw. 
        (You can disable the line breaks and indentation by specifying SaveOptions.DisableFormatting)

        XElement xElement = XElement.Parse(@"
        <person name=""Mahammad"" age=""21"">
          <job location=""Baku"" workHours=""8"">Software Developer</job>
          <address>
            <city>Sumgait</city>
            <country>Azerbaijan</country>
          </address>
        </person>");

        string xml = xElement.ToString(SaveOptions.DisableFormatting);
        Console.WriteLine(xml);

        // outputs below xml
        <person name="Mahammad" age="21"><job location="Baku" workHours="8">Software Developer
        </job><address><city>Sumgait</city><country>Azerbaijan</country></address></person>
        */

        /* XNode

        XNode is the base class for all XML-related nodes in an XML document. 
        All objects that represent these XML pieces (like XElement, XComment, and XText) inherit from XNode.

        --- Key XNode Types:

        1. XElement: Represents an XML element
        2. XComment: Represents an XML comment
        3. XText: Represents the text content within an element.

        for example:
            new XElement("firstname",
               new XAttribute("language", "en"),
               "mahammad");
        the "Mahammad" part is stored as XText.

        4. XProcessingInstruction: Represents processing instructions in XML.
        Example: <?xml-stylesheet type="text/xsl" href="style.xsl"?>.

        5. XDocumentType: Represents the Document Type Declaration (DTD) in an XML file.


        --- Key XNode Methods:

        NextNode: Returns the next sibling node of the current XNode.
        PreviousNode: Returns the previous sibling node of the current XNode.
        Remove(): Removes the current XNode from its parent container.
        ToString(): Converts the node to an XML string.

        XNode? firstNode = xElement.FirstNode;
        Console.WriteLine(firstNode);
        //<job location=""Baku"" workHours=""8"">Software Developer</job>

        XNode? nextNode = firstNode?.NextNode;
        Console.WriteLine(nextNode);
        //< address >
        //  < city > Sumgait </ city >
        //  < country > Azerbaijan </ country >
        //</ address >

        */

        /* XContainer
        XContainer is an abstract class that extends XNode and represents nodes that can contain other nodes.
        It is the base class for XDocument and XElement, 
        meaning both of these can have child nodes (elements, text, etc.).

        Summary:
        NextNode looks for sibling nodes on the same level.
        Siblings are nodes that share the same parent, like <job> and <address> within <person>.

        Difference between Descendants and DescendantNodes methods

        //This method returns all the XElement nodes that are descendants of the current element.
        //It does not include attributes, comments, text, or other node types.

        foreach (XElement descendant in xElement.Descendants())
        {
            Console.WriteLine(descendant.Name);
            //job
            //address
            //city
            //country
        }

        //This method returns all XNode descendants,
        //which can include XElement, XComment, XText, and other types of nodes like processing instructions or document types.

        foreach (var descendantNode in xElement.DescendantNodes())
        {
            Console.WriteLine(descendantNode);
            //Comment
            //Element
            //Text
            //Element
            //Text
            //Element
            //Text
            //Element
            //Text
        }
        */

        /* Writing a declaration to a String.
        
        Suppose that we want to serialize an XDocument to a string, including the XML declaration. 
        Because ToString doesn’t write a declaration, we’d need to use an XmlWriter, instead:

         var doc = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("customer",
                new XAttribute("id", 23),
                new XAttribute("status", "archived"),
            new XElement("firstname",
                new XAttribute("language", "en"),
                "mahammad"),
            new XElement("lastname", "ahmadov")));

        MemoryStream stream = new MemoryStream();
        XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = false };

        using (XmlWriter xmlWriter = XmlWriter.Create(stream, settings))
        {
            doc.Save(xmlWriter);
        }

        stream.Position = 0;

        using StreamReader streamReader = new StreamReader(stream);
        Console.WriteLine(streamReader.ReadToEnd());

        */

        /* Indent and OmitXmlDeclaration
         
        1. Indent
        When Indent = true, it tells the XmlWriter to format the XML output with line breaks 
        and indentation, making it more human-readable.

        If Indent = false (the default), the XML is written in a compact, 
        minified form without extra spaces or newlines between elements, 
        making it less readable but more efficient for machine processing.

        2. OmitXmlDeclaration

        When OmitXmlDeclaration = true, it omits the XML declaration 
        (<?xml version="1.0" encoding="utf-8" standalone="yes"?>) from the output. 

        The declaration is a metadata section that provides information about the version, 
        encoding, and standalone status of the document.

        */

        var doc = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("customer",
                new XAttribute("id", 23),
                new XAttribute("status", "archived"),
            new XElement("firstname",
                new XAttribute("language", "en"),
                "mahammad"),
            new XElement("lastname", "ahmadov")));

        MemoryStream stream = new MemoryStream();
        XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = false };

        using (XmlWriter xmlWriter = XmlWriter.Create(stream, settings))
        {
            doc.Save(xmlWriter);
        }

        stream.Position = 0;

        using StreamReader streamReader = new StreamReader(stream);
        Console.WriteLine(streamReader.ReadToEnd());

        //XElement xElement = XElement.Parse(@"
        //<!-- This is a comment -->
        //<person name=""Mahammad"" age=""21"">
        //  <job location=""Baku"" workHours=""8"">Software Developer</job>
        //  <address>
        //    <city>Sumgait</city>
        //    <country>Azerbaijan</country>
        //  </address>
        //</person>"
        //);

        // 1. traverse 
        //XElement? child = xElement.Element("job");

        //while (child != null)
        //{
        //    Console.WriteLine(child.Name);
        //    child = (XElement?)child.NextNode;
        //}

        // 2. accessing the first node
        //XNode? firstNode = xElement.FirstNode;
        //Console.WriteLine(firstNode);
        ////<job location=""Baku"" workHours=""8"">Software Developer</job>

        // 3. next node
        //XNode? nextNode = firstNode?.NextNode;
        //Console.WriteLine(nextNode);

        //< address >
        //  < city > Sumgait </ city >
        //  < country > Azerbaijan </ country >
        //</ address >

        // 4. Elements()
        //var immediateChildren = xElement.Elements();
        //foreach (var element in immediateChildren)
        //{
        //    Console.WriteLine(element.Name);  // Outputs: job, address
        //}

        // 5. Descendants()
        //var allDescendants = xElement.Descendants();
        //foreach (var descendant in allDescendants)
        //{
        //    Console.WriteLine(descendant.Name);  // Outputs: job, address, city, country
        //}

        #region Code Examples

        // Create the XElement for the customer
        //XElement customer = new XElement("customer",
        //    new XAttribute("id", 23),
        //    new XAttribute("status", "archived"),

        //    new XElement("firstname",
        //        new XAttribute("language", "en"),
        //        "mahammad"),

        //    new XElement("lastname", "ahmadov"));

        //// Add it to an XDocument
        //XDocument doc = new XDocument(
        //    new XDeclaration("1.0", "utf-8", "yes"), // Optional XML declaration
        //    customer);

        //// Print the XML document
        //Console.WriteLine(doc);
        //Console.WriteLine("====================================");

        //// Step 2: Apply LINQ query to extract the first name
        //var descendants = doc.Descendants();
        //foreach (var descendant in descendants)
        //{
        //    Console.WriteLine($"xelement name: {descendant.Name}");
        //    if (descendant.Descendants().Any())
        //    {
        //        Console.WriteLine("there are inner descendants also");
        //    }
        //}

        //var firstName = descendants
        //    .Where(d => d.Name == "firstname")
        //    .Select(d => d.Value).FirstOrDefault();

        //// you can call descendant element directly within a constructor.
        //var firstName2 = doc.Descendants("firstname")
        //    .Select(d => d.Value).FirstOrDefault();

        //Console.WriteLine($"First name from the XML: {firstName}");
        //Console.WriteLine("====================================");

        //// Step 3: Query attributes
        //var customerAttributes = descendants
        //    .Where(d => d.Name == "customer")
        //    .Attributes();

        //foreach (var attribute in customerAttributes)
        //{
        //    Console.WriteLine($"attribute: {attribute.Name}");
        //}

        //// you can also call attribute without using where linq query
        //var customerID1 = customerAttributes
        //    .Where(a => a.Name == "id");

        //var customerID2 = doc.Descendants("customer").Attributes("id").FirstOrDefault();
        //Console.WriteLine($"\nCustomer ID: {customerID2?.Value}");

        //XElement xElement = XElement.Parse(doc.ToString());
        #endregion
    }
}