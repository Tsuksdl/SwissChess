namespace DrawBase
{
  /// <summary>
  /// BAe Interface for any methode to draw a round for a tournament.
  /// </summary>
  public interface IDrawSystem
  {
    #region Public Methods

    /// <summary>
    /// Method to draw the pairings for a new round.
    /// </summary>
    /// <param name="player">List of players to pair in this round</param>
    /// <param name="turnamentBase">Baseinformation for the tournament </param>
    /// <returns>List of the pairings resulting form the drawing.</returns>
    public List<IPairing> CreateNewRound(List<IPlayerData> player, ITournamentBase turnamentBase);

    #endregion Public Methods
  }
}