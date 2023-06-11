using SwissChessDraw;

namespace DrawBase
{
  /// <summary>
  /// BAe Interface for any methode to draw a round for a tournement.
  /// </summary>
  public interface IDrawSystem
  {
    #region Public Methods

    /// <summary>
    /// Method to draw the pairings for a new round.
    /// </summary>
    /// <param name="player">List of players to pair in this round</param>
    /// <returns>List of the pairings resulting form the drawing.</returns>
    public List<IParing> CreateNewRound(List<IPlayerData> player);

    #endregion Public Methods
  }
}