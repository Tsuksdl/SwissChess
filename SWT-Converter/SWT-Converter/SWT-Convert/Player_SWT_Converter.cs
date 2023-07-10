using SwissChessDraw;

namespace SWT_Converter.SWT_Convert
{
  public class Player_SWT_Converter : IPlayerData
  {
    #region Public Constructors

    public Player_SWT_Converter()
    {
      LastPairings = new List<Guid>();
      PlayerID = Guid.NewGuid();
    }

    public Player_SWT_Converter(Guid guid)
    {
      LastPairings = new List<Guid>();
      PlayerID = guid;
    }

    #endregion Public Constructors

    #region Properties

    public int ColorDifferenz { get; set; }

    public bool? LastColor { get; set; }

    public int LastColorCount { get; set; }

    public int LastFloatCount { get; set; }

    public List<Guid> LastPairings { get; private set; }

    public Guid PlayerID { get; private set; }

    public float Points { get; set; }

    public int StartPosition { get; set; }

    public bool usedFreePoint { get; set; }

    #endregion Properties
  }
}