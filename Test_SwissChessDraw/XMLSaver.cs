using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Test_SwissChessDraw
{
  public class XMLSaver
  {
    private XmlDocument? _openDucument;

    private XmlNode? _activeNode;

    public XMLSaver ()
    {
      _openDucument = new XmlDocument ();
      _openDucument.AppendChild(_openDucument.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
    }

    public void Save(string path)
    {
      if (_openDucument == null || _activeNode == null)
      {
        return;
      }

      _openDucument.AppendChild(_activeNode);
      _openDucument.Save(path);
    }

    public bool MoveToParent ()
    {
      if (_activeNode == null || _activeNode.ParentNode == null)
      {
        return false;
      }
      _activeNode = _activeNode.ParentNode;
      return true;
    }

    public bool MoveToChild (string name, int index = 0)
    {
      if (_activeNode == null || _activeNode.SelectNodes(name)?.Count > index)
      {
        return false;
      }

      _activeNode = _activeNode.SelectNodes(name)?.Item(index);
      return true;
    }

    public bool SaveGUIDList(IList<Guid> guidList)
    {
      if (_openDucument == null || _activeNode == null)
      {
        return false;
      }

      foreach (var guid in guidList)
      {
        XmlNode guidNode = _openDucument.CreateNode(XmlNodeType.Element, "GUID", null);
        guidNode.InnerText = guid.ToString();
        _activeNode.AppendChild(guidNode);
      }

      return true;
    }

    public bool CreateNewNode (string name, bool moveToIt = false)
    {
      if (_openDucument == null)
      {
        return false;
      }


      XmlNode node = _openDucument.CreateNode(XmlNodeType.Element, name, null);
      
      if (_activeNode == null)
      {
        _activeNode = node;
      }
      else
      {
        _activeNode.AppendChild(node);
        if (moveToIt)
        {
          _activeNode = node;
        }
      }

      return true;
    }

    public bool WriteAtribute (string name, string value)
    {
      if (_activeNode == null || _openDucument == null || _activeNode.Attributes == null)
      {
        return false;
      }

      XmlAttribute newAttribute = _openDucument.CreateAttribute(name);
      newAttribute.Value = value;
      _activeNode.Attributes.Append(newAttribute);
      return true;
    }
  }
}
