using SwissChessDraw;

namespace Test_SwissChessDraw
{
  public class TestPlayer : IPlayerData
  {
    public int ColorDifferenz { get; set; }
    public bool? LastColor { get; set; }
    public int LastColorCount { get; set; }
    public int LastFloatCount { get; set; }

    public List<Guid> LastPairings { get; }

    public Guid PlayerID { get; set; }

    public float Points { get; set; }
    public bool usedFreePoint { get; set; }

    public TestPlayer ()
    {
      LastPairings = new List<Guid> ();
      LastColor = null;
      LastColorCount = 0;
      LastFloatCount = 0;
      Points = 0;
      usedFreePoint = true;
      PlayerID = Guid.NewGuid();
      ColorDifferenz = 0;
    }
  }
}