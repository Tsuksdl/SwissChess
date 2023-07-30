namespace DrawBase
{
  /// <summary>
  /// Base interface to hold the needed information for the drawing of one round.
  /// </summary>
  public interface IPlayerData
  {
    #region Properties

    /// <summary>
    /// Differenz between the played games with white and black.
    /// (the games as home team and visiting game)
    /// </summary>
    public int ColorDifferenz { get; set; }

    /// <summary>
    /// Flag to show the last played color of the player. It is null if no game is played.
    /// </summary>
    public bool? LastColor { get; set; }

    /// <summary>
    /// Count of the games in a row with the same color.
    /// </summary>
    public int LastColorCount { get; set; }

    /// <summary>
    /// Count of the games, against a player with more or less games.
    /// </summary>
    public int LastFloatCount { get; set; }

    /// <summary>
    /// List of the players played in the previos games.
    /// </summary>
    public List<Guid> LastPairings { get; }

    /// <summary>
    /// Guid of the player to linke to other players
    /// </summary>
    public Guid PlayerID { get; }

    /// <summary>
    /// Recived points from the rounds. Can not be smaler than 0.
    /// </summary>
    public float Points { get; set; }

    /// <summary>
    /// Position of this player in the start sorting.
    /// </summary>
    public int StartPosition { get; set; }

    /// <summary>
    /// Flag to hold the information about the free points from a free game.
    /// </summary>
    public bool usedFreePoint { get; set; }

    #endregion Properties
  }
}