using DrawBase;
using SwissChessDraw;

namespace Test_SwissChessDraw.TestClasses
{
  internal class TestRound : IXMLObjekt
  {
    #region Private Fields

    /// <summary>
    /// Internal list of pairings to load them in.
    /// </summary>
    private IList<Pairing> _pairings;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Default Konstruktor. Creates the List to hold the Pairings.
    /// </summary>
    public TestRound()
    {
      _pairings = new List<Pairing>();
      Round = 0;
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// Public Property to access the pairings of this round
    /// </summary>
    public IList<Pairing> Pairings => _pairings;

    /// <summary>
    /// Count of the round.
    /// </summary>
    public int Round { get; set; }

    #endregion Public Properties

    #region Public Methods

    public override bool Equals(object? obj)
    {
      if (obj is IEnumerable<IPairing> pairings)
      {
        int i = 0;
        foreach (IPairing p in pairings)
        {
          if (p.PlayerWhite.PlayerID != Pairings[i].PlayerWhite.PlayerID || p.PlayerBlack.PlayerID != Pairings[i].PlayerBlack.PlayerID)
          {
            return false;
          }
        }
        return true;
      }
      return base.Equals(obj);
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
    /// Mathode to apply the points of this round. Color and Floatings should be set by drawingsystem.
    /// </summary>
    internal void ApplyPoint()
    {
      foreach (Pairing pairing in _pairings)
      {
        pairing.PlayerWhite.Points += pairing.PointsWhite;
        pairing.PlayerBlack.Points += pairing.PointsBlack;
      }
    }

    #endregion Public Methods

    #region Public Classes

    /// <summary>
    /// Internal Class to hold the information about on pairing of the round.
    /// </summary>
    public class Pairing : IXMLObjekt, IPairing
    {
      #region Public Properties

      public bool IsFreePoint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      public IPlayerData PlayerBlack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      public IPlayerData PlayerWhite { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      public float PointsBlack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      public float PointsWhite { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

      #endregion Public Properties

      #region Public Methods

      /// <inheritdoc/>
      public void LoadInformation(XMLReader reader)
      {
        IList<TestPlayer> pairing = new List<TestPlayer>();
        reader.ReadList<TestPlayer>(ref pairing, "Player");

        if (pairing.Count != 2)
        {
          return;
        }

        PlayerWhite = pairing[0];
        PlayerBlack = pairing[1];

        if (reader.ReadAttribute(nameof(PointsWhite)) is string pointsWhiteString)
        {
          PointsWhite = float.Parse(pointsWhiteString);
        }

        if (reader.ReadAttribute(nameof(PointsBlack)) is string pointsBlackString)
        {
          PointsBlack = float.Parse(pointsBlackString);
        }

        if (reader.ReadAttribute(nameof(IsFreePoint)) is string isFreePointString)
        {
          IsFreePoint = bool.Parse(isFreePointString);
        }
      }

      #endregion Public Methods
    }

    #endregion Public Classes
  }
}