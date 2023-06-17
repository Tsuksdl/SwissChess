using SwissChessDraw;

namespace DrawBase
{
  /// <summary>
  /// Base Interface for one pairing in a round. Holds the information about the two players/teams
  /// and there points.
  /// </summary>
  public interface IPairing
  {
    /// <summary>
    /// The white player or the home team.
    /// </summary>
    public IPlayerData PlayerWhite { get; set; }

    /// <summary>
    /// The black player or the guest team 
    /// </summary>
    public IPlayerData PlayerBlack { get; set; }

    /// <summary>
    /// Poinst of the white player from this pairing. Is default 0.
    /// </summary>
    public float PointsWhite { get; set; }

    /// <summary>
    /// Points of the black player from this pairing. Is default 0.
    /// </summary>
    public float PointsBlack { get; set; }

    /// <summary>
    /// Information about the played game is a Bye (free point)
    /// </summary>
    public bool IsFreePoint { get; set; }
  }
}