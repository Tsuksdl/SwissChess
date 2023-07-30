namespace Test_SwissChessDraw
{
  /// <summary>
  /// Default interface to make a Objekt loadable as list with the XML-reader 
  /// </summary>
  internal interface IXMLObjekt
  {
    #region Public Methods

    /// <summary>
    /// Methode to load the information from a XML file.
    /// </summary>
    /// <param name="reader">Object with the open XML file</param>
    internal void Load(XMLReader reader, object? args);

    /// <summary>
    /// Methode to write the information of the object in a XML-file.
    /// </summary>
    /// <param name="saver"> Saver with the open XML-Dokument. </param>
    internal bool Save(XMLSaver saver, object? args);

    #endregion Public Methods
  }
}