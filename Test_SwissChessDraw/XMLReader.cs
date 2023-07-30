using System.Xml;

namespace Test_SwissChessDraw
{
  internal class XMLReader
  {
    #region Fields

    /// <summary>
    /// Readerobject with hte open document.
    /// </summary>
    private XmlDocument? _reader;

    /// <summary>
    /// Referernz to the activeNode of the dokument to read information from.
    /// </summary>
    private XmlNode? _aktiveNode;

    #endregion Fields

    #region Public Methods

    /// <summary>
    /// Metode to read the string of an attribute of the active node.
    /// </summary>
    /// <param name="attributeName">Name of the attribute to read.</param>
    /// <returns>string of the Attribute or null</returns>
    public string? ReadAttribute(string attributeName)
    {
      if (_aktiveNode != null)
      {
        return _aktiveNode.Attributes?.GetNamedItem(attributeName)?.Value;
      }
      return null;
    }

    /// <summary>
    /// Methode to load a list of objects of type T from the interface <see cref="IXMLObjekt"/>.
    /// </summary>
    /// <typeparam name="T">Type of the elements in the list.</typeparam>
    /// <param name="collection">Base list to add the elements to.</param>
    /// <param name="nodeName">
    /// Name of the nodes to use to read the element T from. If empty alle nodes are used.
    /// </param>
    public void ReadList<T>(ref IList<T> collection, object? args, string nodeName = "") where T : IXMLObjekt
    {
      if (_aktiveNode != null)
      {
        XmlNode baseNode = _aktiveNode;
        foreach (XmlNode node in baseNode.ChildNodes)
        {
          _aktiveNode = node;
          if (node.Name.Contains(nodeName))
          {
            var nextElement = Activator.CreateInstance(typeof(T));
            if (nextElement is T nextTElement)
            {
              nextTElement.Load(this, args);
              collection.Add(nextTElement);
            }
          }
        }
        _aktiveNode = baseNode;
      }
    }

    /// <summary>
    /// Open and start reading a new file.
    /// </summary>
    /// <param name="filePath">Path location of the file.</param>
    /// <returns>true if the file could be opened, else flase if not possible.</returns>
    public bool OpenFile (string filePath)
    {
      try
      {
        _reader = new XmlDocument();
        _reader.Load(filePath);
        _aktiveNode = _reader.ChildNodes.Count > 0 ? _reader.ChildNodes[1] : null;
        return true;
      }
      catch
      {
        return false;
      }
    }

    internal List<Guid> ReadGuidList()
    {
      List<Guid> list = new List<Guid>();
      if (_aktiveNode != null)
      {
        foreach (XmlNode child in _aktiveNode.ChildNodes)
        {
          if (child.Name.Equals("GUID"))
          {
            list.Add(new Guid(child.InnerText));
          }
        }
      }
      return list;
    }

    #endregion Public Methods
  }
}