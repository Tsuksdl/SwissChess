namespace Test_SwissChessDraw.TestClasses
{
  internal class TestRound : IXMLObjekt
  {
    /// <summary>
    /// Internal list of pairings to load them in.
    /// </summary>
    private IList<Pairing> _pairings;

    /// <summary>
    /// Public Property to access the pairings of this round
    /// </summary>
    public IList<Pairing> Pairings => _pairings;

    /// <summary>
    /// Count of the round.
    /// </summary>
    public int Round { get; set; }

    /// <summary>
    /// Default Konstruktor. Creates the List to hold the Pairings.
    /// </summary>
    public TestRound ()
    {
      _pairings = new List<Pairing>();
      Round = 0;
    }

    /// <inheritdoc/>
    public void LoadInformation(XMLReader reader)
    {
      if (reader.ReadAttribute(nameof(Round)) is string roundString)
      {
        Round = int.Parse(roundString);
      }

      if (_pairings != null)
      {
        reader.ReadList<Pairing>(ref _pairings, "Pairing");
      }
    }

    /// <summary>
    /// Internal Class to hold the information about on pairing of the round.
    /// </summary>
    public class Pairing : IXMLObjekt
    {
      /// <summary>
      /// Guid referenz to the player or team with white/home.
      /// </summary>
      public Guid PlayerWhite { get; set; }

      /// <summary>
      /// Guid referenz to the player or team with black/vistor
      /// </summary>
      public Guid PlayerBlack { get; set; }

      /// <inheritdoc/>
      public void LoadInformation(XMLReader reader)
      {
        List<Guid> pairing = reader.ReadGuidList();
        if (pairing.Count != 2)
        {
          return;
        }
        
        PlayerWhite = pairing[0];
        PlayerBlack = pairing[1];
      }
    }
  }
}