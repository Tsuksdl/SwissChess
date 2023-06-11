namespace Test_SwissChessDraw
{
  internal interface IXMLObjekt
  {
    #region Public Methods

    /// <summary>
    /// Methode to load the information from a XML file.
    /// </summary>
    /// <param name="reader">Object with the open XML file</param>
    public void LoadInformation(XMLReader reader);

    #endregion Public Methods
  }
}