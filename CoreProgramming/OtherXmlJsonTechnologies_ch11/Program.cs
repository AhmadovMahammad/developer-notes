internal class Program
{
    private static void Main(string[] args)
    {
        /*  Introduction

        In this chapter, we explore the low-level XmlReader / XmlWriter classes and 
        the types for working with JavaScript Object Notation(JSON), 
        which has become a popular alternative to XML.
        */

        /* XmlReader
        
        XmlReader is a high-performance, low-level class used to read an XML stream 
        in a forward-only manner. 
        It reads XML data sequentially, meaning it can't go backward or revisit previous nodes. 
        This makes it efficient for large XML files or streams but 
        more difficult to navigate compared to higher-level tools like XDocument or XElement.

        Consider the following XML file, customer.xml:
        <?xml version="1.0" encoding="utf-8" standalone="yes"?>
        <customer id="123" status="archived">
            <firstname>Jim</firstname>
            <lastname>Bo</lastname>
        </customer> 

        To instantiate an XmlReader, you use the XmlReader.Create() method. 
        You can pass 1. a file, 2. stream, or 3. URI as the data source. 
        Here's an example of reading from a file:

        */

        /* Basic Usage of XmlReader

       string xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "customer.xml");
       using XmlReader reader = XmlReader.Create(xmlPath);

       while (reader.Read())
       {
           Console.WriteLine($"Node type: {reader.NodeType}\nName: {reader.Name}\nValue: {reader.Value}");
       }

       1. reader.Read() moves the cursor forward through the XML file.
       2. For each node, you can access its NodeType, Name, and Value.


       Attributes in XML are part of a node, 
       but XmlReader doesn't treat them as separate "nodes" when reading through elements. 
       It focuses on Element, Text, and other node types like Comment, 
       but attributes are accessed differently.

       string xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "customer.xml");
       using XmlReader reader = XmlReader.Create(xmlPath);

       while (reader.Read())
       {
           // all the details about xml elements
           Console.WriteLine($"node type: {reader.NodeType} | name: {reader.Name} | value: {reader.Value}");

           if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
           {

               // Loop through all attributes
               while (reader.MoveToNextAttribute())
               {
                   Console.WriteLine($"attribute: {reader.Name}: {reader.Value}");
               }

               // Move the reader back to the element after reading all the attributes.
               reader.MoveToElement();
           }
       }

       Output:

       node type: XmlDeclaration | name: xml | value: version="1.0" encoding="utf-8" standalone="yes"
       node type: Whitespace | name:  | value:

       node type: Element | name: customer | value:
       attribute: id: 123
       attribute: status: archived
       node type: Whitespace | name:  | value:

       node type: Element | name: firstname | value:
       node type: Text | name:  | value: Jim
       node type: EndElement | name: firstname | value:
       node type: Whitespace | name:  | value:

       node type: Element | name: lastname | value:
       node type: Text | name:  | value: Bo
       node type: EndElement | name: lastname | value:
       node type: Whitespace | name:  | value:

       node type: EndElement | name: customer | value:
       node type: Whitespace | name:  | value:


       */

        /* Using XmlReaderSettings to Control Parsing

        The XmlReaderSettings object allows you to configure options when reading XML, 
        like whether to skip comments, processing instructions, or whitespace.

        1. Ignoring Whitespace
        Whitespace in XML can make confusion through the reading process. 
        Use XmlReaderSettings.IgnoreWhitespace to ignore whitespace nodes:

        This will skip over any pure whitespace nodes, 
        making it easier to focus on the meaningful content of the XML.

        string xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "customer.xml");

        XmlReaderSettings settings = new XmlReaderSettings() { IgnoreWhitespace = true };
        using XmlReader reader = XmlReader.Create(xmlPath, settings);

        while (reader.Read())
        {
            // all the details about xml elements
            Console.WriteLine($"node type: {reader.NodeType} | name: {reader.Name} | value: {reader.Value}");

            if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
            {
                // Loop through all attributes
                while (reader.MoveToNextAttribute())
                {
                    Console.WriteLine($"attribute: {reader.Name}: {reader.Value}");
                }

                // Move the reader back to the element after reading all the attributes.
                reader.MoveToElement();
            }
        }

        Similarly, you can skip comments and processing instructions with these properties:

        Pure content:

        node type: XmlDeclaration | name: xml | value: version="1.0" encoding="utf-8" standalone="yes"
        node type: Element | name: customer | value:
        attribute: id: 123
        attribute: status: archived
        node type: Element | name: firstname | value:
        node type: Text | name:  | value: Jim
        node type: Element | name: lastname | value:
        node type: Text | name:  | value: Bo


        */

        /* Conformance Level:

        By default, XmlReader expects to read a complete and well-formed XML document, 
        which includes a root element and a valid structure. This is known as ConformanceLevel.Document.
        
        However, if you're only interested in reading a fragment of XML 
        that doesn't have a root element (i.e., a "fragment"), 
        you must set ConformanceLevel to Fragment.

        if we set conformance level to Document and try to read this xml:

        string xmlFragment = @"
                                <firstname>Jim</firstname>
                                <lastname>Bo</lastname>";

        we will get this error:
        System.Xml.XmlException: 'There are multiple root elements. Line 3, position 34.'

        XmlReaderSettings settings = new XmlReaderSettings()
        {
            IgnoreProcessingInstructions = true,
            IgnoreComments = true,
            IgnoreWhitespace = true,

            ConformanceLevel = ConformanceLevel.Fragment,
        };

        using XmlReader reader = XmlReader.Create(new StringReader(xmlFragment), settings);
        while (reader.Read())
        {
            Console.WriteLine($"Node Type: {reader.NodeType}, Name: {reader.Name}, Value: {reader.Value}");
        }

        With ConformanceLevel.Fragment, 
        this code can read multiple nodes without requiring a root element.


        */

        /* Note:
         
        XmlReader throws an XmlException if any validation fails.
        XmlException has LineNumber and LinePosition properties indicating 
        where the error occurred. Logging this information is essential if the XML file is large! 

        r.MoveToContent(); // Skip over the XML declaration
        */

        /* ReadElementContentAsString
         
        ReadElementContentAsString method simplifies reading element content when you know 
        you're dealing with simple, un-nested text nodes. 
        This method saves you from having to manually advance the XmlReader 
        and handle those steps individually.

        using XmlReader reader = XmlReader.Create(new StringReader(@"
        <customer id=""123"" status=""archived"">
          <firstname>Jim</firstname>
          <lastname>Bo</lastname>
        </customer>"));
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "firstname")
            {
                string firstName = reader.ReadElementContentAsString("firstname", "");
                Console.WriteLine($"First Name: {firstName}");

            }

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "lastname")
            {
                string lastName = reader.ReadElementContentAsString();
                Console.WriteLine($"Last Name: {lastName}");
            }
        }

        */

        /* Empty Elements
         
        The way XmlReader handles empty elements can be tricky because...
        it treats self-closing tags (like <customerList />) differently 
        from explicit open-close tags (like <customerList></customerList>).

        <customerList></customerList>  <!-- Regular open and close tags -->
        <customerList/>                <!-- Self-closing tag -->

        But XmlReader handles these differently.
        
        1. For <customerList></customerList>:
        
        XmlReader will encounter both a start element and an end element, 
        so the code reader.ReadStartElement("customerList"); and reader.ReadEndElement(); 
        works without issues.

        2. For <customerList/>:
        XmlReader treats this as an empty element, 
        meaning it will read the start element but not expect a separate end element. 
        So calling ReadEndElement() after reading the start element will throw an exception.

        using XmlReader reader = XmlReader.Create(new StringReader("<customerList/>"));
        //using XmlReader reader = XmlReader.Create(new StringReader("<customerList></customerList>"));
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "customerList")
            {
                reader.ReadStartElement("customerList");

                if (!reader.IsEmptyElement)
                {
                    reader.ReadEndElement();
                }
            }
        }
        */

        /* How do ReadXXX methods work?
         
        he XmlReader's "move to" methods are responsible for advancing the cursor 
        (or the reading position) to a specific node or attribute within an XML document. 
        XmlReader works in a forward-only, read-once manner, 
        so you can think of it as a cursor that progresses through the XML document. 
        Once the cursor is moved to a particular node, you can read its value or attributes.
         
        1. Read():
        This method advances the cursor to the next node in the XML document.
        It moves the cursor forward in the document to the next XML element or node, 
        and you use this in a loop to iterate over the entire document.

        2. MoveToAttribute(string name):
        Moves the cursor to the specified attribute on the current element.

        3. MoveToElement():
        Moves the cursor back to the element node after you've moved to one of its attributes.
        This is useful after reading an attribute if you want to return to the element itself 
        and continue reading.

        */


        #region code example 6 (reading attributes differentyl)

        //string xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "customer.xml");

        //XmlReaderSettings settings = new XmlReaderSettings()
        //{
        //    IgnoreWhitespace = true,
        //    IgnoreComments = true,
        //    IgnoreProcessingInstructions = true,
        //};
        //using XmlReader reader = XmlReader.Create(xmlPath, settings);

        //while (reader.Read())
        //{
        //    if (reader.NodeType == XmlNodeType.EndElement)
        //    {
        //        continue;
        //    }

        //    // all the details about xml elements
        //    Console.WriteLine($"node type: {reader.NodeType} | name: {reader.Name} | value: {reader.Value}");

        //    if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
        //    {
        //        // Loop through all attributes

        //        // example 2. to read attributes
        //        if (reader.MoveToFirstAttribute())
        //        {
        //            do
        //            {
        //                Console.WriteLine($"{reader.Name}: {reader.Value}");
        //            } while (reader.MoveToNextAttribute());
        //        }

        //        // example 1. to read attributes
        //        //while (reader.MoveToNextAttribute())
        //        //{
        //        //    Console.WriteLine($"attribute: {reader.Name}: {reader.Value}");
        //        //}

        //        // Move the reader back to the element after reading all the attributes.
        //        reader.MoveToElement();
        //    }
        //}

        #endregion

        #region code example 5

        //using XmlReader reader = XmlReader.Create(new StringReader("<customerList/>"));
        //using XmlReader reader = XmlReader.Create(new StringReader("<customerList></customerList>"));
        //while (reader.Read())
        //{
        //    if (reader.NodeType == XmlNodeType.Element && reader.Name == "customerList")
        //    {
        //        reader.ReadStartElement("customerList");

        //        if (!reader.IsEmptyElement)
        //        {
        //            reader.ReadEndElement();
        //        }
        //    }
        //}

        #endregion

        #region code example 4
        //using XmlReader reader = XmlReader.Create(new StringReader(@"
        //<customer id=""123"" status=""archived"">
        //  <firstname>Jim</firstname>
        //  <lastname>Bo</lastname>
        //</customer>"));
        //while (reader.Read())
        //{
        //    if (reader.NodeType == XmlNodeType.Element && reader.Name == "firstname")
        //    {
        //        string firstName = reader.ReadElementContentAsString("firstname", "");
        //        Console.WriteLine($"First Name: {firstName}");

        //    }

        //    if (reader.NodeType == XmlNodeType.Element && reader.Name == "lastname")
        //    {
        //        string lastName = reader.ReadElementContentAsString();
        //        Console.WriteLine($"Last Name: {lastName}");
        //    }
        //} 
        #endregion

        #region code example 3
        //string xmlFragment = @"
        //                        <firstname>Jim</firstname>
        //                        <lastname>Bo</lastname>";

        //XmlReaderSettings settings = new XmlReaderSettings()
        //{
        //    IgnoreProcessingInstructions = true,
        //    IgnoreComments = true,
        //    IgnoreWhitespace = true,

        //    ConformanceLevel = ConformanceLevel.Fragment,
        //};

        //using XmlReader reader = XmlReader.Create(new StringReader(xmlFragment), settings);
        //while (reader.Read())
        //{
        //    Console.WriteLine($"Node Type: {reader.NodeType}, Name: {reader.Name}, Value: {reader.Value}");
        //}
        #endregion

        #region code example 2
        //string xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "customer.xml");

        //XmlReaderSettings settings = new XmlReaderSettings()
        //{
        //    IgnoreWhitespace = true,
        //    IgnoreComments = true,
        //    IgnoreProcessingInstructions = true,
        //};
        //using XmlReader reader = XmlReader.Create(xmlPath, settings);

        //while (reader.Read())
        //{
        //    if (reader.NodeType == XmlNodeType.EndElement)
        //    {
        //        continue;
        //    }

        //    // all the details about xml elements
        //    Console.WriteLine($"node type: {reader.NodeType} | name: {reader.Name} | value: {reader.Value}");

        //    if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
        //    {
        //        // Loop through all attributes
        //        while (reader.MoveToNextAttribute())
        //        {
        //            Console.WriteLine($"attribute: {reader.Name}: {reader.Value}");
        //        }

        //        // Move the reader back to the element after reading all the attributes.
        //        reader.MoveToElement();
        //    }
        //}
        #endregion

        #region code example 1
        //string xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "customer.xml");
        //using XmlReader reader = XmlReader.Create(xmlPath);

        //while (reader.Read())
        //{
        //    // all the details about xml elements
        //    Console.WriteLine($"node type: {reader.NodeType} | name: {reader.Name} | value: {reader.Value}");

        //    if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
        //    {
        //        // Loop through all attributes
        //        while (reader.MoveToNextAttribute())
        //        {
        //            Console.WriteLine($"attribute: {reader.Name}: {reader.Value}");
        //        }

        //        // Move the reader back to the element after reading all the attributes.
        //        reader.MoveToElement();
        //    }
        //}
        #endregion

        //============================================================

        /* XmlWriter

        The XmlWriter class is a forward-only, 
        write-once approach to constructing an XML document. 
        It has a symmetrical design to XmlReader—meaning that while XmlReader helps you 
        consume an XML stream, XmlWriter helps you produce one in a structured way.

        */

        /* Creating an XmlWriter
        
        You create an XmlWriter object using XmlWriter.Create(), 
        optionally passing in a Stream, 1. TextWriter, or a 2. file path, 
        along with an XmlWriterSettings object to control the output.
         
        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = true,
            OmitXmlDeclaration = true,
        };

        StringWriter stringWriter = new StringWriter();
        using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
        {
            xmlWriter.WriteStartElement("customer");
            xmlWriter.WriteElementString("firstname", "mahammad");
            xmlWriter.WriteElementString("lastname", "ahmadov");
            xmlWriter.WriteEndElement();
        }

        // This creates the following XML:
        <?xml version="1.0" encoding="utf-16"?>
        <customer>
          <firstname>mahammad</firstname>
          <lastname>ahmadov</lastname>
        </customer>

        NOTE: 
        
        By default, XmlWriter automatically writes the XML declaration.
        And setting OmitXmlDeclaration = true skips this part.

        -------You can read Xml like this:

        XmlReaderSettings settings = new XmlReaderSettings()
        {
            IgnoreWhitespace = true,
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
        };
        using XmlReader reader = XmlReader.Create(new StringReader(stringWriter.ToString()), settings);

        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.EndElement)
            {
                continue;
            }

            // all the details about xml elements
            Console.WriteLine($"node type: {reader.NodeType} | name: {reader.Name} | value: {reader.Value}");

            if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
            {
                // example 2. to read attributes
                if (reader.MoveToFirstAttribute())
                {
                    do
                    {
                        Console.WriteLine($"{reader.Name}: {reader.Value}");
                    } while (reader.MoveToNextAttribute());
                }

                // Move the reader back to the element after reading all the attributes.
                reader.MoveToElement();
            }
        }

        */

        /* Writing Values

        To write text or non-text data, you can use WriteValue: 
        It Writes a single text node. 
        You can pass non-string types like DateTime, bool, and numeric types. 
        WriteValue will automatically convert these to XML-compliant strings using XmlConvert.

        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = true,
            OmitXmlDeclaration = true,
            ConformanceLevel = ConformanceLevel.Fragment,
        };

        StringWriter stringWriter = new StringWriter();
        using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
        {
            //xmlWriter.WriteElementString("birthdate", DateTime.UtcNow.ToString());

            //xmlWriter.WriteStartElement("birthdate");
            //xmlWriter.WriteValue(DateTime.Now);
            //xmlWriter.WriteEndElement();
        }

        Output: <birthdate>10/19/2024 12:38:08 PM</birthdate>
        */

        /* Writing Attributes
         
        You can write attributes immediately after writing a start element:

        XmlWriterSettings settings = new XmlWriterSettings()
        {
            Indent = true,
            OmitXmlDeclaration = true,
        };

        StringWriter writer = new StringWriter();
        using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
        {
            xmlWriter.WriteStartElement("customer");
            xmlWriter.WriteAttributeString("work", "r.i.s.k.");
            xmlWriter.WriteElementString("firstname", "mahammad");
            xmlWriter.WriteElementString("lastname", "ahmadov");
            xmlWriter.WriteEndElement();
        }

        Console.WriteLine(writer.ToString());

        <customer work="r.i.s.k.">
          <firstname>mahammad</firstname>
          <lastname>ahmadov</lastname>
        </customer>
        */


        #region code example 3

        //XmlWriterSettings settings = new XmlWriterSettings()
        //{
        //    Indent = true,
        //    OmitXmlDeclaration = true,
        //};

        //StringWriter writer = new StringWriter();
        //using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
        //{
        //    xmlWriter.WriteStartElement("customer");
        //    xmlWriter.WriteAttributeString("work", "r.i.s.k.");
        //    xmlWriter.WriteElementString("firstname", "mahammad");
        //    xmlWriter.WriteElementString("lastname", "ahmadov");
        //    xmlWriter.WriteEndElement();
        //}

        //Console.WriteLine(writer.ToString());

        #endregion

        #region code example 2

        //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        //{
        //    Indent = true,
        //    OmitXmlDeclaration = true,
        //    ConformanceLevel = ConformanceLevel.Fragment,
        //};

        //StringWriter stringWriter = new StringWriter();
        //using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
        //{
        //    xmlWriter.WriteElementString("birthdate", DateTime.UtcNow.ToString());

        //    //xmlWriter.WriteStartElement("birthdate");
        //    //xmlWriter.WriteValue(DateTime.Now);
        //    //xmlWriter.WriteEndElement();
        //}

        //Console.WriteLine(stringWriter.ToString());

        #endregion

        #region code example 1

        //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        //{
        //    Indent = true,
        //    OmitXmlDeclaration = true,
        //};

        //StringWriter stringWriter = new StringWriter();
        //using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
        //{
        //    xmlWriter.WriteStartElement("customer");
        //    xmlWriter.WriteElementString("firstname", "mahammad");
        //    xmlWriter.WriteElementString("lastname", "ahmadov");
        //    xmlWriter.WriteEndElement();
        //}

        //Console.WriteLine(stringWriter.ToString());

        //XmlReaderSettings settings = new XmlReaderSettings()
        //{
        //    IgnoreWhitespace = true,
        //    IgnoreComments = true,
        //    IgnoreProcessingInstructions = true,
        //};
        //using XmlReader reader = XmlReader.Create(new StringReader(stringWriter.ToString()), settings);

        //while (reader.Read())
        //{
        //    if (reader.NodeType == XmlNodeType.EndElement)
        //    {
        //        continue;
        //    }

        //    // all the details about xml elements
        //    Console.WriteLine($"node type: {reader.NodeType} | name: {reader.Name} | value: {reader.Value}");

        //    if (reader.NodeType == XmlNodeType.Element && reader.HasAttributes)
        //    {
        //        // example 2. to read attributes
        //        if (reader.MoveToFirstAttribute())
        //        {
        //            do
        //            {
        //                Console.WriteLine($"{reader.Name}: {reader.Value}");
        //            } while (reader.MoveToNextAttribute());
        //        }

        //        // Move the reader back to the element after reading all the attributes.
        //        reader.MoveToElement();
        //    }
        //}


        #endregion
    }
}