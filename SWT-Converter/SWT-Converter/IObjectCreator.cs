using DisplayBase;
using DrawBase;

namespace SWT_Converter
{
  /// <summary>
  /// Factory to create the Object give by a using application.
  /// </summary>
  public interface IObjectCreator
  {
    #region Public Methods

    /// <summary>
    /// Creator for a new Pairing without any input.
    /// </summary>
    /// <returns> A new Object for a Pairing. </returns>
    IPairing CreatePairingObject();

    /// <summary>
    /// Creator for a new Player without any input.
    /// </summary>
    /// <returns> A new Object for a Player. </returns>
    IPlayerData CreatePlayerObject();

    /// <summary>
    /// Creator for a new Player without any input.
    /// </summary>
    /// <returns> A new Object for a Player. </returns>
    IDisplayPlayerData CreateDisplayPlayerObject();

    /// <summary>
    /// Creator for a new Round without any input.
    /// </summary>
    /// <returns> A new Object for a Round. </returns>
    IRoundBase CreateRoundObject();

    #endregion Public Methods
  }
}