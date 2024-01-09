using DrawBase;
using System.Runtime.CompilerServices;

namespace SwissChessDraw
{
  public class SwissChessDrawSystem : IDrawSystem
  {
    #region Public Constructors

    public SwissChessDrawSystem(ITournamentBase turnamentBase)
    {
      CurrentTurnamentBase = turnamentBase;
    }

    #endregion Public Constructors

    #region Properties

    /// <summary>
    /// Instanze des 
    /// </summary>
    private ITournamentBase CurrentTurnamentBase { get; set; }

    #endregion Properties

    #region Public Methods

    /// <summary>
    /// Start methode for the setting of a new Round. To get more information about the system to
    /// set a roudn pleas look at: https://escacs.cat/images/comite/arbitres/C041-SwissFIDESystem.pdf
    /// </summary>
    /// <param name="players">Player that are going to be paired that round.</param>
    /// <param name="turnamentBase">Infromation about the tournement to pair for.</param>
    /// <returns></returns>
    public List<IPairing> CreateNewRound(List<IPlayerData> players, ITournamentBase turnamentBase)
    {
      CurrentTurnamentBase = turnamentBase;
      List<IPairing> pairings = new List<IPairing>();

      // 1. Split players in groups with the same points.
      Dictionary<float, List<IPlayerData>> playerGroups = SortPlayerGroups(players);

      // 2. Check if all players in a group have a valid enemy in his group
      Dictionary<float, List<IPlayerData>> floater = CheckForFloaterPrePairing(playerGroups);

      // 3. sort floater and player groups by points (key)
      floater.OrderBy(n => n.Key);
      playerGroups.OrderBy(n => n.Key);

      MoveFloaterToNextGroup(ref floater, ref playerGroups);
      pairings = CreatePairings(playerGroups, floater, players.Count);

      return pairings;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Methode to check if a group of players to pair, contains any floaters to assign to a diffrent group.
    /// </summary>
    /// <param name="key">Key == to Points fo player, to indentify the group of players to check</param>
    /// <param name="players">List of the players in the point group</param>
    /// <returns>The set of floaters in the point group</returns>
    private KeyValuePair<float, List<IPlayerData>> CheckForFloaterInPlayerGroup(float key, List<IPlayerData> players)
    {
      KeyValuePair<float, List<IPlayerData>> result = new KeyValuePair<float, List<IPlayerData>>(key, new List<IPlayerData>());
      bool hasPairings = false;
      List<IPlayerData> searchedPlayers = new List<IPlayerData>(players);
      List<IPlayerData> avalabilePlayer = new List<IPlayerData>(players);

      for (int i = searchedPlayers.Count - 1; i > -1; i--)
      {
        int j = avalabilePlayer.Count - 1;
        for (; j > -1; j--)
        {
          if (!searchedPlayers[i].LastPairings.Contains(avalabilePlayer[j].PlayerID))
          {
            if (IsPairingPossible(searchedPlayers[i], avalabilePlayer[j]))
            {
              hasPairings = true;
              break;
            }
          }
        }

        if (hasPairings)
        {
          searchedPlayers.RemoveAt(i);
          if (searchedPlayers.Contains(avalabilePlayer[j]))
          {
            searchedPlayers.Remove(avalabilePlayer[j]);
            i--;
          }
        }
        else
        {
          result.Value.Add(searchedPlayers[i]);
          avalabilePlayer.Remove(searchedPlayers[i]);
        }
      }

      return result;
    }

    /// <summary>
    /// Methode to check if there is a set of player has any floaters that should be determined befor any pairing is happening.
    /// </summary>
    /// <param name="playerGroups">List of the player groups to pair in this round.</param>
    /// <param name="tournamentBase">Base information about the turnament.</param>
    /// <returns>Dictonary of the floaters, with all floaters to be found before the </returns>
    private Dictionary<float, List<IPlayerData>> CheckForFloaterPrePairing(Dictionary<float, List<IPlayerData>> playerGroups)
    {
      Dictionary<float, List<IPlayerData>> result = new Dictionary<float, List<IPlayerData>>();
      List<Task<KeyValuePair<float, List<IPlayerData>>>> tasks = new List<Task<KeyValuePair<float, List<IPlayerData>>>>();

      foreach (KeyValuePair<float, List<IPlayerData>> playerGroup in playerGroups)
      {
        tasks.Add(new Task<KeyValuePair<float, List<IPlayerData>>>(() => CheckForFloaterInPlayerGroup(playerGroup.Key, playerGroup.Value)));
        tasks.Last().Start();
      }

      Task.WaitAll(tasks.ToArray());
      tasks.ForEach(t => result.Add(t.Result.Key, t.Result.Value));

      return result;
    }

    /// <summary>
    /// check if pairing between players is possible.
    /// </summary>
    private bool IsPairingPossible(IPlayerData player1, IPlayerData player2)
    {
      // Simple case: Both player change color or get the same again.
      if (player1.LastColor != player2.LastColor)
      {
        // 1. Check if swap color is for both player possible
        if (IsPlayerColorChangePossible(player1) && IsPlayerColorChangePossible(player2))
        {
          return true;
        }
        else if (IsPlayerColorKeepPossible(player1) && IsPlayerColorKeepPossible(player2))

        {
          return true;
        }
      }
      else
      {
        if (IsPlayerColorKeepPossible(player1) && IsPlayerColorChangePossible(player2))
        {
          return true;
        }
        else if (IsPlayerColorChangePossible(player1) && IsPlayerColorKeepPossible(player2))
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Methode to check if a player can change the color for the next game.
    /// </summary>
    /// <param name="player">Player to check</param>
    /// <returns></returns>
    private bool IsPlayerColorChangePossible(IPlayerData player)
    {
      if (player.LastColor == null)
      {
        return true;
      }

      if (player.ColorDifferenz == 0)
      {
        return true;
      }

      // Count of the remaining rounds including the on that is pair right now.
      int remainingRounds = CurrentTurnamentBase.RoundCount - CurrentTurnamentBase.CurrentRound;

      // Now pair round removed.
      remainingRounds--;

      // Player had more white than black!
      if (player.ColorDifferenz > 0)
      {
        //Player had white in last game --> Color change is good!
        if (player.LastColor == true)
        {
          return true;
        }

        // Player ha black and would get another white, check if he can reach a good colorDiffrenz
        // under the rest of the rules
        else
        {
          if (remainingRounds > 2)
          {
            return player.ColorDifferenz + 1 < 3;
          }
          else
          {
            if (CurrentTurnamentBase.RoundCount % 2 == 0)
            {
              return player.ColorDifferenz + 1 < 3 && player.ColorDifferenz + 1 - remainingRounds < 1;
            }
            else
            {
              return player.ColorDifferenz + 1 < 3 && player.ColorDifferenz + 1 - remainingRounds < 2;
            }
          }
        }
      }
      else
      {
        //Player had white in last game --> Color change is good!
        if (player.LastColor == false)
        {
          return true;
        }

        // Player ha black and would get another white, check if he can reach a good colorDiffrenz
        // under the rest of the rules
        else
        {
          if (remainingRounds > 2)
          {
            return player.ColorDifferenz - 1 > -3;
          }
          else
          {
            if (CurrentTurnamentBase.RoundCount % 2 == 0)
            {
              return player.ColorDifferenz - 1 > -3 && player.ColorDifferenz - 1 + remainingRounds > -1;
            }
            else
            {
              return player.ColorDifferenz - 1 > -3 && player.ColorDifferenz - 1 + remainingRounds > -2;
            }
          }
        }
      }
    }

    private bool IsPlayerColorKeepPossible(IPlayerData player)
    {
      if (player.LastColor == null)
      {
        return true;
      }

      // Count of the remaining rounds including the on that is pair right now.
      int remainingRounds = CurrentTurnamentBase.RoundCount - CurrentTurnamentBase.CurrentRound;

      // Now pair round removed.
      remainingRounds--;

      if (player.LastColor == true)
      {
        if (player.ColorDifferenz < 1)
        {
          return true;
        }
        else if (player.ColorDifferenz < 2 && remainingRounds > 0)
        {
          return true;
        }
      }
      else
      {
        if (player.ColorDifferenz > -1)
        {
          return true;
        }
        else if (player.ColorDifferenz > -2 && remainingRounds > 0)
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Methode to move players as floater to the next group of player they could be matched up against.
    /// </summary>
    /// <param name="floater">
    /// Directory to contain the information about all floaters in the pairing of this round.
    /// </param>
    /// <param name="playerGroups">Directory of the diffrent pairinggroups in this round.</param>
    private void MoveFloaterToNextGroup(ref Dictionary<float, List<IPlayerData>> floater, ref Dictionary<float, List<IPlayerData>> playerGroups)
    {
      float middleGroup = this.CurrentTurnamentBase.CurrentRound / 2;
      for (float i = 0; i <= this.CurrentTurnamentBase.CurrentRound; i += 0.5f)
      {
        if (i > middleGroup)
        {
        }
        else
        {
          if ( MoveFloaterUpNextGroup(ref floater, ref playerGroups, i))
          {

          }
        }
      }
    }

    private bool MoveFloaterUpNextGroup(ref Dictionary<float, List<IPlayerData>> floater, ref Dictionary<float, List<IPlayerData>> playerGroups, float startGroup)
    {
      if (floater.ContainsKey(startGroup))
      {
        return true;
      }

      float nextGroupe = startGroup + 0.5f;
      while (nextGroupe <= this.CurrentTurnamentBase.CurrentRound)
      {
        if (playerGroups.ContainsKey(nextGroupe))
        {
          foreach (IPlayerData player in floater[startGroup])
          {
            playerGroups[startGroup].Remove(player);
            playerGroups[nextGroupe].Add(player);
          }
          return true;
        }
      }
      return false;
    }

    private Dictionary<float, List<IPlayerData>> SortPlayerGroups(List<IPlayerData> players)
    {
      Dictionary<float, List<IPlayerData>> result = new Dictionary<float, List<IPlayerData>>();

      foreach (IPlayerData player in players)
      {
        if (!result.ContainsKey(player.Points))
        {
          result.Add(player.Points, new List<IPlayerData> { player });
        }
        else
        {
          result[player.Points].Add(player);
        }
      }

      return result;
    }

    /// <summary>
    /// Methode to set the pairing for group.
    /// </summary>
    /// <param name="playerGroups"></param>
    /// <param name="floater"></param>
    /// <param name="turnamentBase"></param>
    /// <param name="swissChessDrawSystem"></param>
    /// <param name="playerCount"></param>
    /// <returns></returns>
    private List<IPairing> CreatePairings(Dictionary<float, List<IPlayerData>> playerGroups, Dictionary<float, List<IPlayerData>> floater, int playerCount)
    {
      // Laut definitionso anzuwenden.
      float middelScoreGroupe = this.CurrentTurnamentBase.RoundCount / 2;

      List<IPairing> result = new List<IPairing>();

      //TODO: Check if middlegroup is equal to %2 == 0, else extend group by given rules

      for (float scoreGroup = this.CurrentTurnamentBase.RoundCount; scoreGroup > middelScoreGroupe; scoreGroup -= .5f)
      {
        if (playerGroups.ContainsKey(scoreGroup))
        {
          playerGroups[scoreGroup].Sort(new PlayerRankingDataComparer());
          int middle = (int)Math.Round(playerGroups[scoreGroup].Count / 2d, MidpointRounding.ToZero);
          for (int playerIndex1 = 0, playerIndex2 = middle; playerIndex1 < middle; playerIndex1++, playerIndex2++)
          {
            result.Add(new SwissChessPairing(playerGroups[scoreGroup][playerIndex1], playerGroups[scoreGroup][playerIndex2]));
          }
        }
      }

      for (float scoreGroup = 0; scoreGroup < middelScoreGroupe; scoreGroup += .5f)
      {
        if (playerGroups.ContainsKey(scoreGroup))
        {
          playerGroups[scoreGroup].Sort(new PlayerRankingDataComparer());
          int middle = (int)Math.Round(playerGroups[scoreGroup].Count / 2d, MidpointRounding.ToZero);
          for (int playerIndex1 = 0, playerIndex2 = middle; playerIndex1 < middle; playerIndex1++, playerIndex2++)
          {
            result.Add(new SwissChessPairing(playerGroups[scoreGroup][playerIndex1], playerGroups[scoreGroup][playerIndex2]));
          }
        }
      }

      return result;
    }

    #endregion Private Methods
  }
}