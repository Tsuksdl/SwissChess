namespace DrawBase
{
  /// <summary>
  /// Base Interface for one Rounde with it pairings
  /// </summary>
  public interface IRoundBase
  {
    /// <summary>
    /// Pairings with result in the round.
    /// </summary>
    List<IPairing> Pairings { get; }

    /// <summary>
    /// Number of the round in the Turnement of the stage.
    /// </summary>
    int Rounde { get; set; }

    /// <summary>
    /// Count of the Stage this Round is played in.
    /// </summary>
    int Stage { get; set; }

    /// <summary>
    /// Name of the round to display 
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Flag to show if the round is played and the rersults are all set.
    /// </summary>
    bool RoundFinished { get; set; }

    /// <summary>
    /// </summary>
    int tl_ok { get; set; }

    /// <summary>
    /// String of the Date for the round. --> later as date avaliable!
    /// </summary>
    string Date { get; set; }

    /// <summary>
    /// Methode to order the pairings by her PairingNumber.
    /// </summary>
    void OrderPairings();
  }
}