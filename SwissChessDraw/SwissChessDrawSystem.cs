using DrawBase;

namespace SwissChessDraw
{
  public class SwissChessDrawSystem : IDrawSystem
  {
    #region Public Methods

    public List<IPairing> CreateNewRound(List<IPlayerData> players, ITournamentBase turnamentBase)
    {
      List<IPairing> pairings = new List<IPairing>();

      // 1. Split players in groups with the same points.
      Dictionary<float, List<IPlayerData>> playerGroups = SortPlayerGroups(players);

      // 2. Check if all players in a group have a valid enemy in his group
      Dictionary<float, List<IPlayerData>> floater = CheckForFloaterPrePairing(playerGroups, turnamentBase);

      // 3. sort floater and player groups by points (key)
      floater.OrderBy(n => n.Key);
      playerGroups.OrderBy(n => n.Key);

      MoveFloaterToNextGroup(ref floater, ref playerGroups, turnamentBase);

      return pairings;
    }

    private void MoveFloaterToNextGroup(ref Dictionary<float, List<IPlayerData>> floater, ref Dictionary<float, List<IPlayerData>> playerGroups, ITournamentBase turnamentBase)
    {
      float middleGroup = turnamentBase.CurrentRound / 2;
    }

    private Dictionary<float, List<IPlayerData>> CheckForFloaterPrePairing(Dictionary<float, List<IPlayerData>> playerGroups, ITournamentBase tournamentBase)
    {
      Dictionary<float, List<IPlayerData>> result = new Dictionary<float, List<IPlayerData>>();
      List<Task<KeyValuePair<float, List<IPlayerData>>>> tasks = new List<Task<KeyValuePair<float, List<IPlayerData>>>>();

      foreach(KeyValuePair<float, List<IPlayerData>> playerGroup in playerGroups)
      {
        tasks.Add(new Task<KeyValuePair<float, List<IPlayerData>>>(() => CheckForFloaterInPlayerGroup(playerGroup.Key, playerGroup.Value, tournamentBase)));
        tasks.Last().Start();
      }

      Task.WaitAll(tasks.ToArray());
      tasks.ForEach(t => result.Add(t.Result.Key, t.Result.Value));

      return result;
    }

    private KeyValuePair<float, List<IPlayerData>> CheckForFloaterInPlayerGroup(float points, List<IPlayerData> players, ITournamentBase tournamentBase)
    {
      KeyValuePair<float, List<IPlayerData>> result = new KeyValuePair<float, List<IPlayerData>>(points, new List<IPlayerData>());
      bool hasPairings = false;
      List<IPlayerData> searchedPlayers = new List<IPlayerData> (players);
      List<IPlayerData> avalabilePlayer = new List<IPlayerData> (players);

      for (int i = searchedPlayers.Count - 1; i > -1; i--)
      {
        int j = avalabilePlayer.Count - 1;
        for (; j > -1; j--)
        {
          if (!searchedPlayers[i].LastPairings.Contains(avalabilePlayer[j].PlayerID))
          {
            if (IsPairingPossible(searchedPlayers[i], avalabilePlayer[j], tournamentBase))
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

    private bool IsPairingPossible (IPlayerData player1, IPlayerData player2, ITournamentBase tournamentBase)
    {
      // Simple case: Both player change color or get the same again.
      if (player1.LastColor != player2.LastColor)
      {
        // 1. Check if swap color is for both player possible
        if (IsPlayerColorChangePossible(player1, tournamentBase) && IsPlayerColorChangePossible(player2, tournamentBase))
        {
          return true;
        }
        else if (IsPlayerColorKeepPossible(player1, tournamentBase) && IsPlayerColorKeepPossible (player2, tournamentBase))
        {
          return true;
        }
      }
      else
      {
        if (IsPlayerColorKeepPossible(player1, tournamentBase) && IsPlayerColorChangePossible(player2, tournamentBase))
        {
          return true;
        }
        else if (IsPlayerColorChangePossible(player1, tournamentBase) && IsPlayerColorKeepPossible(player2, tournamentBase))
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
    /// <param name="tournamentBase">Information about the current state of the tournament.</param>
    /// <returns></returns>
    private bool IsPlayerColorChangePossible (IPlayerData player, ITournamentBase tournamentBase)
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
      int remainingRounds = tournamentBase.RoundCount - tournamentBase.CurrentRound;
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
            if (tournamentBase.RoundCount % 2 == 0)
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
            if (tournamentBase.RoundCount % 2 == 0)
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

    private bool IsPlayerColorKeepPossible(IPlayerData player, ITournamentBase tournamentBase)
    {
      if (player.LastColor == null)
      {
        return true;
      }

      // Count of the remaining rounds including the on that is pair right now.
      int remainingRounds = tournamentBase.RoundCount - tournamentBase.CurrentRound;
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

    private Dictionary<float, List<IPlayerData>> SortPlayerGroups(List<IPlayerData> players)
    {
      Dictionary<float, List<IPlayerData>> result = new Dictionary<float, List<IPlayerData>>();

      foreach(IPlayerData player in players) 
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

    #endregion Public Methods
  }
}