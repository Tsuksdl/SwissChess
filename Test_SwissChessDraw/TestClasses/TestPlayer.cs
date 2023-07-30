using DisplayBase;
using SwissChessDraw;

namespace Test_SwissChessDraw.TestClasses
{
  internal class TestPlayer : IDisplayPlayerData, IXMLObjekt
  {
    #region Public Constructors

    public TestPlayer()
    {
      LastPairings = new List<Guid>();
      LastColor = null;
      LastColorCount = 0;
      LastFloatCount = 0;
      Points = 0;
      usedFreePoint = true;
      PlayerID = Guid.NewGuid();
      ColorDifferenz = 0;
      Club = string.Empty;
      Gender = string.Empty;
      Name = string.Empty;
      PlayerStatus = string.Empty;
      Title = string.Empty;
      zps = string.Empty;
    }

    public TestPlayer(IPlayerData data) : this()
    {
      LastPairings = data.LastPairings;
      LastColor = data.LastColor;
      LastColorCount = data.LastColorCount;
      LastFloatCount = data.LastFloatCount;
      Points = data.Points;
      usedFreePoint = data.usedFreePoint;
      PlayerID = data.PlayerID;
      ColorDifferenz = data.ColorDifferenz;
    }

    #endregion Public Constructors

    #region Properties

    public int ColorDifferenz { get; set; }

    public bool? LastColor { get; set; }

    public int LastColorCount { get; set; }

    public int LastFloatCount { get; set; }

    public List<Guid> LastPairings { get; private set; }

    public Guid PlayerID { get; set; }

    public float Points { get; set; }

    public int StartPosition { get; set; }

    public bool usedFreePoint { get; set; }

    public int Birthyear { get; set; }

    public string Club { get; set; }

    public float ClubIdentificationNumber { get; set; }

    public float FIDE_cco { get; set; }

    public float FIDE_Elo { get; set; }

    public int FideID { get; set; }

    public string Gender { get; set; }

    public string Name { get; set; }

    public float NAT_cco { get; set; }

    public string PlayerStatus { get; set; }

    public float Start_NVZ { get; set; }

    public string Title { get; set; }

    public float TVN { get; set; }

    public string zps { get; set; }

    #endregion Properties

    #region Public Methods

    public void Load(XMLReader reader, object? args)
    {
      if (reader.ReadAttribute(nameof(PlayerID)) is string playerIDString)
      {
        PlayerID = new Guid(playerIDString);
      }

      if (args is bool inputGuidOnly && inputGuidOnly)
      {
        return;
      }

      if (reader.ReadAttribute(nameof(ColorDifferenz)) is string colorDifferenzString)
      {
        ColorDifferenz = int.Parse(colorDifferenzString);
      }

      if (reader.ReadAttribute(nameof(LastColor)) is string lastColorString)
      {
        LastColor = bool.Parse(lastColorString);
      }

      if (reader.ReadAttribute(nameof(LastFloatCount)) is string lastFloatCountString)
      {
        LastFloatCount = int.Parse(lastFloatCountString);
      }

      if (reader.ReadAttribute(nameof(LastColorCount)) is string lastColorCountString)
      {
        LastColorCount = int.Parse(lastColorCountString);
      }

      if (reader.ReadAttribute(nameof(StartPosition)) is string startPositionString)
      {
        StartPosition = int.Parse(startPositionString);
      }


      if (reader.ReadAttribute(nameof(Points)) is string pointsString)
      {
        Points = float.Parse(pointsString);
      }

      if (reader.ReadAttribute(nameof(usedFreePoint)) is string usedFreePointString)
      {
        usedFreePoint = bool.Parse(usedFreePointString);
      }

      LastPairings = reader.ReadGuidList();
    }

    public bool Save(XMLSaver saver, object? args)
    {
      bool result = false;
      if (saver.CreateNewNode("Player", true))
      {
        result = true;
        result &= saver.WriteAtribute(nameof(PlayerID), PlayerID.ToString());
        if (args is bool inputGuidOnly && inputGuidOnly)
        {
          result &= saver.MoveToParent();
          return result;
        }

        result &= saver.WriteAtribute(nameof(ColorDifferenz), ColorDifferenz.ToString());
        result &= saver.WriteAtribute(nameof(LastFloatCount), LastFloatCount.ToString());
        result &= saver.WriteAtribute(nameof(LastColorCount), LastColorCount.ToString());
        result &= saver.WriteAtribute(nameof(StartPosition), StartPosition.ToString());
        result &= saver.WriteAtribute(nameof(Points), Points.ToString());
        result &= saver.WriteAtribute(nameof(usedFreePoint), usedFreePoint.ToString());
        result &= saver.SaveGUIDList(LastPairings);
        result &= saver.MoveToParent();
      }
      return result;
    }

    #endregion Public Methods
  }
}