using DrawBase;
using System.Collections.Immutable;

namespace SwissChessDraw
{
  internal static class PairingsGenerator
  {
    internal static List<IPairing> CreatePairings(Dictionary<float, List<IPlayerData>> playerGroups, Dictionary<float, List<IPlayerData>> floater, ITournamentBase turnamentBase, SwissChessDrawSystem swissChessDrawSystem, int playerCount)
    {
      // Laut definitionso anzuwenden.
      float middelScoreGroupe = turnamentBase.RoundCount / 2;

      List<IPairing> result = new List<IPairing>();

      for (float scoreGroup = turnamentBase.RoundCount; scoreGroup > middelScoreGroupe; scoreGroup += .5f)
      {
        if (playerGroups.ContainsKey(scoreGroup))
        {
        }
      }

      return result;
    }
  }
}