using DrawBase;

namespace SwissChessDraw
{
  internal class SwissChessPairing : IPairing
  {
    #region Public Constructors

    /// <summary>
    /// Konstruktor to instance a new SwissChess Pairing.
    /// </summary>
    public SwissChessPairing(IPlayerData playerWhite, IPlayerData playerBlack)
    {
      this.PlayerWhite = playerWhite;
      this.PlayerBlack = playerBlack;
    }

    public SwissChessPairing(IPlayerData player)
    {
      this.PlayerWhite = player;
      this.PlayerBlack = null;
      IsFreePoint = true;
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
  }
}