using SwissChessDraw;

namespace Test_SwissChessDraw.TestClasses
{
  public class TestPlayer : IPlayerData, IXMLObjekt
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

    #endregion Properties

    #region Public Methods

    void IXMLObjekt.LoadInformation(XMLReader reader)
    {
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

      if (reader.ReadAttribute(nameof(PlayerID)) is string playerIDString)
      {
        PlayerID = new Guid(playerIDString);
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

    #endregion Public Methods
  }
}