using DrawBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_SwissChessDraw.TestClasses
{
  internal class TestTournament : ITournamentBase
  {
    public TestTournament(int roundCount)
    {
      CurrentRound = 0;
      CurrentStage = 1;
      RoundCount = roundCount;
      PlayerData = new List<IPlayerData>();
      Rounds = new List<IRoundBase>();
    }

    public int CurrentRound { get; set; }
    public int CurrentStage { get; set; }
    public int DrawnRound { get; set; }

    public bool IsNull { get; }

    public int PlayerCount { get; set; }
    public int RoundCount { get; set; }
    public int StageCount { get; set; }
    public int TurnementMode { get; set; }
    public int UseAsTWZ { get; set; }
    public List<IPlayerData> PlayerData { get; set; }
    public List<IRoundBase> Rounds { get; set; }
  }
}
