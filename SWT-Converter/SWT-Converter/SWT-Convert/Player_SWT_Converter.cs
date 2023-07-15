using DisplayBase;

namespace SWT_Converter.SWT_Convert
{
  public class Player_SWT_Converter : IDisplayPlayerData
  {
    #region Public Constructors

    public Player_SWT_Converter()
    {
      LastPairings = new List<Guid>();
      PlayerID = Guid.NewGuid();
      Club = string.Empty;
      Gender = string.Empty;
      Name = string.Empty;
      PlayerStatus = string.Empty;
      zps = string.Empty;
      Title = string.Empty;
    }

    public Player_SWT_Converter(Guid guid) : this()
    {
      LastPairings = new List<Guid>();
      PlayerID = guid;
    }

    #endregion Public Constructors

    #region Public Properties

    public int Birthyear { get; set; }

    public string Club { get; set; }

    public float ClubIdentificationNumber { get; set; }

    public int ColorDifferenz { get; set; }

    public float FIDE_cco { get; set; }

    public float FIDE_Elo { get; set; }

    public int FideID { get; set; }

    public string Gender { get; set; }

    public bool? LastColor { get; set; }

    public int LastColorCount { get; set; }

    public int LastFloatCount { get; set; }

    public List<Guid> LastPairings { get; private set; }

    public string Name { get; set; }

    public float NAT_cco { get; set; }

    public Guid PlayerID { get; private set; }

    public string PlayerStatus { get; set; }

    public float Points { get; set; }

    public float Start_NVZ { get; set; }

    public int StartPosition { get; set; }

    public string Title { get; set; }

    public float TVN { get; set; }

    public bool usedFreePoint { get; set; }

    public string zps { get; set; }

    #endregion Public Properties
  }
}