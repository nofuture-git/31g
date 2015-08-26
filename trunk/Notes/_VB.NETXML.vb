Public Module NotesVbNetXml

    Public Sub xmlExample
        '1. instantiate a XML Document
        Dim x As System.Xml.XmlDocument

        x = New System.Xml.XmlDocument

        '2. set up your needed nodes
        Dim decl As System.Xml.XmlDeclaration 'for the <?xml "1.0" such and such
        Dim doc As System.Xml.XmlDocumentType ' for DOCTYPE statement
        Dim myE As System.Xml.XmlElement 'various elements 
        Dim elem2 As System.Xml.XmlElement
        Dim elem3 As System.Xml.XmlElement
        Dim attrit As System.Xml.XmlAttribute

        '3 setup your xml declaration
        decl = x.CreateXmlDeclaration("1.0", Nothing, "no")
        x.AppendChild(decl)
        doc = x.CreateDocumentType("myDTD", Nothing, "C:\myDTD.DTD", Nothing)
        x.AppendChild(doc)


        '4. instantiate elements with XmlDoc's factory method
        myE = x.CreateElement("Session")
        elem2 = x.CreateElement("iclimporter-session")
        elem3 = x.CreateElement("this-little-piggy")

        '5. instantiate attributes
        attrit = x.CreateAttribute("went")


        '5. to put text into an element or attribute - use this
        attrit.Value = "we-we all the way home"
        elem2.InnerText = "here is a value"
        elem3.InnerText = "huff and puff"

        '6. string all the elements and attributes together into a markup
        elem3.SetAttributeNode(attrit)
        myE.AppendChild(elem2)
        myE.AppendChild(elem3)

        '7. append to the XmlDoc to set the root node
        x.AppendChild(myE)

        ' -- use this if you want to get a handle on the root
        Dim root As Xml.XmlNode
        root = x.DocumentElement

        '8 write to file
        x.Save("C:\myXML.xml")
      End Sub
End Module


