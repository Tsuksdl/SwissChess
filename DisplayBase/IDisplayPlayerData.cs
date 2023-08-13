using DrawBase;

namespace DisplayBase
{
  public interface IDisplayPlayerData : IPlayerData
  {
    #region Properties

    /// <summary>
    /// Year of the birth of the player.
    /// </summary>
    int Birthyear { get; set; }

    /// <summary>
    /// Club for whom the player is playing
    /// </summary>
    string Club { get; set; }

    /// <summary>
    /// Club identification Number
    /// </summary>
    float ClubIdentificationNumber { get; set; }

    float FIDE_cco { get; set; }

    /// <summary>
    /// Offical ELO of the player determind by the FIDE
    /// </summary>
    float FIDE_Elo { get; set; }

    /// <summary>
    /// Identificationnumber of the player in the FIDE Database
    /// </summary>
    int FideID { get; set; }

    /// <summary>
    /// Gender as String of the player
    /// </summary>
    string Gender { get; set; }

    /// <summary>
    /// Name of the player to display
    /// </summary>
    string Name { get; set; }

    float NAT_cco { get; set; }

    /// <summary>
    /// State of the player in the turnement (inaktive, etc.)
    /// </summary>
    string PlayerStatus { get; set; }

    /// <summary>
    /// Startvalue of the Nation value Number
    /// </summary>
    float Start_NVZ { get; set; }

    /// <summary>
    /// Title of the player like (GM, Fm, WG, etc.)
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// Value Number of the player used to determin the starting position. Can differ from ELO and NVZ.
    /// </summary>
    float TVN { get; set; }

    string zps { get; set; }

    #endregion Properties
  }
}