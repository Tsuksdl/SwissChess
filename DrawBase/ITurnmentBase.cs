using SwissChessDraw;

namespace DrawBase
{
  public interface ITurnamentBase
  {
    int CurrentRound { get; set; }
    int CurrentStage { get; set; }
    int DrawnRound { get; set; }
    bool IsNull { get; }
    int PlayerCount { get; set; }
    int RoundCount { get; set; }
    int StageCount { get; set; }
    int TurnementMode { get; set; }
    int UseAsTWZ { get; set; }
    List<IPlayerData> PlayerData { get; set; }

    List<IRoundBase> Rounds { get; set; }
  }
}