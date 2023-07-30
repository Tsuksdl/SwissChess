using DrawBase;
using SwissChessDraw;

namespace Test_SwissChessDraw.TestClasses
{
  internal class TestRound : IRoundBase, IXMLObjekt
  {
    #region Public Constructors

    /// <summary>
    /// Default Konstruktor. Creates the List to hold the Pairings.
    /// </summary>
    public TestRound()
    {
      Pairings = new List<IPairing>();
      Rounde = 0;
      Name = string.Empty;
      Date = string.Empty;
    }

    public TestRound(IRoundBase round) : this()
    {
      foreach(IPairing paring in round.Pairings)
      {
        Pairings.Add(new Pairing(paring));
      }
      Name = round.Name;
      Date = round.Date;
      RoundFinished = round.RoundFinished;
      Stage = round.Stage;
    }

    #endregion Public Constructors

    #region Properties

    /// <summary>
    /// Count of the round.
    /// </summary>
    public int Rounde { get; set; }
    public int Stage { get; set; }
    public string Name { get; set; }
    public bool RoundFinished { get; set; }
    public int tl_ok { get; set; }
    public string Date { get; set; }

    public List<IPairing> Pairings { get; set; }

    #endregion Properties

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

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    /// <inheritdoc/>
    public void Load(XMLReader reader, object? args)
    {
      if (args is Dictionary<Guid, IPlayerData> playerDic)
      { 
        if (reader.ReadAttribute(nameof(Rounde)) is string roundString)
        {
          Rounde = int.Parse(roundString);
        }

        if (Pairings != null)
        {
          IList<Pairing> tmpList = new List<Pairing>();
          reader.ReadList<Pairing>(ref tmpList, playerDic, "Pairing");
          Pairings = tmpList.Cast<IPairing>().ToList();
        }
      }
    }

    public void OrderPairings()
    {
      Pairings = Pairings.OrderBy(n => n.PairingNumber).ToList();
    }

    public bool Save(XMLSaver saver, object? args)
    {
      bool result = false;
      if (saver.CreateNewNode("Round", true))
      {
        result = true;
        foreach (Pairing p in Pairings)
        {
          result &= p.Save(saver, null);
        }
        result &= saver.WriteAtribute(nameof(Rounde), Rounde.ToString());
        result &= saver.MoveToParent();
      }
      return result;
    }

    #endregion Public Methods

    #region Internal Methods

    /// <summary>
    /// Mathode to apply the points of this round. Color and Floatings should be set by drawingsystem.
    /// </summary>
    internal void ApplyPoint()
    {
      foreach (Pairing pairing in Pairings)
      {
        pairing.PlayerWhite.Points += pairing.PointsWhite;
        pairing.PlayerBlack.Points += pairing.PointsBlack;
      }
    }

    #endregion Internal Methods

    #region Classes

    /// <summary>
    /// Internal Class to hold the information about on pairing of the round.
    /// </summary>
    public class Pairing : IPairing, IXMLObjekt
    {
      #region Public Constructors

      public Pairing()
      {
        PlayerBlack = new TestPlayer();
        PlayerWhite = new TestPlayer();
      }

      public Pairing(IPairing pairing)
      {
        PlayerWhite = new TestPlayer(pairing.PlayerWhite);
        PlayerBlack = new TestPlayer(pairing.PlayerBlack);
        IsFreePoint = pairing.IsFreePoint;
        PairingNumber = pairing.PairingNumber;
        PointsWhite = pairing.PointsWhite;
        PointsBlack = pairing.PointsBlack;
      }

      #endregion Public Constructors

      #region Properties

      public bool IsFreePoint { get; set; }

      public int PairingNumber { get; set; }

      public IPlayerData PlayerBlack { get; set; }

      public IPlayerData PlayerWhite { get; set; }

      public float PointsBlack { get; set; }

      public float PointsWhite { get; set; }

      #endregion Properties

      #region Public Methods

      /// <inheritdoc/>
      public void Load(XMLReader reader, object? args)
      {
        if (args is Dictionary<Guid, IPlayerData> playerDic)
        {
          IList<TestPlayer> pairing = new List<TestPlayer>();
          reader.ReadList<TestPlayer>(ref pairing, true, "Player");

          if (pairing.Count != 2 || !playerDic.ContainsKey(pairing[0].PlayerID) || !playerDic.ContainsKey(pairing[1].PlayerID))
          {
            return;
          }

          PlayerWhite = playerDic[pairing[0].PlayerID];
          PlayerBlack = playerDic[pairing[1].PlayerID];

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
      }

      public bool Save(XMLSaver saver, object? args)
      {
        bool result = false;
        if (saver.CreateNewNode("Pairing", true))
        {
          result = true;
          if (PlayerWhite is IXMLObjekt xmlPlayerWhite)
          {
            result &= xmlPlayerWhite.Save(saver, true);
          }

          if (PlayerBlack is IXMLObjekt xmlPLayerBlack)
          {
            result &= xmlPLayerBlack.Save(saver, true);
          }

          result &= saver.WriteAtribute(nameof(PointsWhite), PointsWhite.ToString());
          result &= saver.WriteAtribute(nameof(PointsBlack), PointsBlack.ToString());
          result &= saver.WriteAtribute(nameof(IsFreePoint), IsFreePoint.ToString());
          result &= saver.MoveToParent();
        }
        return result;
      }

      #endregion Public Methods
    }

    #endregion Classes
  }
}