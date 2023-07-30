using DisplayBase;
using DrawBase;
using SwissChessDraw;

namespace SWT_Converter
{
  internal class SWTBaseReader
  {
    #region Fields

    /// <summary>
    /// Open stream reader to extract the information from the file.
    /// </summary>
    private StreamReader? reader;

    #endregion Fields

    #region Public Constructors

    public SWTBaseReader()
    {
      reader = null;
    }

    #endregion Public Constructors

    #region Internal Methods

    internal bool OpenFile(string path)
    {
      if (!File.Exists(path))
      {
        return false;
      }

      reader = new StreamReader(path);
      return true;
    }

    internal float ReadFloat(int offset, int length)
    {
      float result = 0f;

      if (reader == null || length < 1)
      {
        return result;
      }

      try
      {
        result = float.Parse(ReadString(offset, length));
      }
      catch
      { }

      return result;
    }

    /// <summary>
    /// Methode to read a set of int and get the as strings!
    /// </summary>
    /// <param name="offset">Position of the Char in the file</param>
    /// <param name="length">Amount of chars to read from the file</param>
    /// <returns> -1 if no read was Possible</returns>
    internal int ReadInt(int offset, int length = 1)
    {
      if (reader == null || length < 1)
      {
        return -1;
      }

      byte[] resultChars = new byte[length];
      try
      {
        reader.BaseStream.Position = offset;
        reader.BaseStream.Read(resultChars, 0, length);
      }
      catch
      {
        return -1;
      }

      int result = 0;
      int i = 0;

      foreach (byte c in resultChars)
      {
        result += (int)(c * Math.Pow(265, i));
        i++;
      }

      return result;
    }

    internal List<IRoundBase> ReadPairings(ITurnamentBase turnamentBase, IObjectCreator objectCreator, List<IPlayerData> playerData)
    {
      List<IRoundBase> result = new List<IRoundBase>();

      int swtRoundCount = 0;
      if (turnamentBase.CurrentRound != 0)
      {
        if (turnamentBase.TurnementMode == 3)
        {
          swtRoundCount = turnamentBase.RoundCount * turnamentBase.StageCount;
        }
        else
        {
          swtRoundCount = turnamentBase.RoundCount;
        }
      }

      // Base offset for the pairings
      int offset = 13384;

      List<List<DataHolderPairing>> Pairings = new List<List<DataHolderPairing>>();

      // Read data
      for (int sp = 0; sp < turnamentBase.PlayerCount; sp++)
      {
        for (int currentRound = 0; currentRound < swtRoundCount; currentRound++, offset += 19)
        {
          if (ReadInt(offset + 13, 1) < 1 && ReadInt(offset + 11, 1) < 1)
          {
            continue;
          }

          if (Pairings.Count <= currentRound)
          {
            Pairings.Add(new List<DataHolderPairing>());
            result.Add(objectCreator.CreateRoundObject());
            result.Last().Rounde = currentRound;
          }

          Pairings[currentRound].Add(new DataHolderPairing(this, sp, offset));
        }
      }

      // Zusammen gruppieren der
      for (int i = 0; i < Pairings.Count; i++)
      {
        List<DataHolderPairing> roundPairings = Pairings[i];
        foreach (DataHolderPairing pairing in roundPairings)
        {
          if (pairing.IsHomeTeam == 1 || pairing.IsHomeTeam == 2)
          {
            if (playerData.Count > pairing.PlayerWhite && playerData.Count > pairing.PlayerBlack)
            {
              int index = roundPairings.FindIndex(p => p.PlayerWhite == pairing.PlayerBlack);
              if (index < 0)
              {
                continue;
              }

              IPlayerData playerWhite = playerData[pairing.PlayerWhite];
              IPlayerData playerBlack = playerData[pairing.PlayerBlack];

              SetPlayerInformation(playerWhite, playerBlack);

              (float PointsWhite, float PointsBlack) gameResult = pairing._calculateCLMErgebnisWhite(roundPairings[index]);

              result[i].Pairings.Add(objectCreator.CreatePairingObject());
              result[i].Pairings.Last().PlayerWhite = playerWhite;
              result[i].Pairings.Last().PlayerBlack = playerBlack;
              result[i].Pairings.Last().PointsWhite = gameResult.PointsWhite;
              result[i].Pairings.Last().PointsBlack = gameResult.PointsBlack;
              result[i].Pairings.Last().IsFreePoint = pairing.Attribute == 2;
              result[i].Pairings.Last().PairingNumber = pairing.BoardNumber;
            }
          }
        }
      }

      result.ForEach(r => r.OrderPairings());
      return result;
    }

    /// <summary>
    /// Static Methode to read all player from an SWT-File. the turnement is not a team-Turnemt at
    /// this point!
    /// </summary>
    /// <param name="reader">Reade of the SWT-File open to extract the player.</param>
    /// <param name="turnamentBase">
    /// Reference of the turnament information to set the right TWZ and player state
    /// </param>
    /// <param name="offset">
    /// Position to start reading the player. The offset is moved to the next player after finishing reading.
    /// </param>
    /// <returns>A new player object with the read data from the file if possible.</returns>
    internal IDisplayPlayerData ReadPlayer(ITurnamentBase turnamentBase, IObjectCreator objectCreator, ref int offset)
    {
      IDisplayPlayerData player = objectCreator.CreateDisplayPlayerObject();

      if (ReadInt(offset + 189, 1) != 102)
      {
        player.Name = ReadString(offset, 33);
        player.Club = ReadString(offset + 34, 33);
        player.Title = ReadString(offset + 66, 3);
        player.FIDE_Elo = ReadFloat(offset + 70, 4);
        player.Start_NVZ = ReadFloat(offset + 75, 4);
        player.FIDE_cco = ReadFloat(offset + 105, 3);
        player.NAT_cco = ReadFloat(offset + 109, 3);
        player.Birthyear = ReadInt(offset + 128, 4);
        player.zps = ReadString(offset + 153, 5);
        player.ClubIdentificationNumber = ReadFloat(offset + 159, 4);
        player.Gender = ReadString(offset + 184, 1);
        player.PlayerStatus = ReadString(offset + 184, 1) == "*" ? "0" : "1";

        if (turnamentBase.TurnementMode == 4 || turnamentBase.TurnementMode == 6)
        {
          player.PlayerStatus = "1";
        }

        player.FideID = ReadInt(offset + 324, 12);

        int s_points = ReadInt(offset + 273, 1);
        int s_sign = ReadInt(offset + 274, 1);
        if (s_sign == 255)
        {
          s_points = (s_points - 256);
        }
        float s_punkte = s_points / 2;
        player.Points = s_punkte;

        //TWZ-Bestimmen
        if (turnamentBase.UseAsTWZ == 0)
        {
          if (player.FIDE_Elo >= player.Start_NVZ)
          {
            player.TVN = player.FIDE_Elo;
          }
          else
          {
            player.TVN = player.Start_NVZ;
          }
        }
        else if (turnamentBase.UseAsTWZ == 1)
        {
          if (player.Start_NVZ > 0)
          {
            player.TVN = player.Start_NVZ;
          }
          else
          {
            player.TVN = player.FIDE_Elo;
          }
        }
        else if (turnamentBase.UseAsTWZ == 2)
        {
          if (player.FIDE_Elo > 0)
          {
            player.TVN = player.FIDE_Elo;
          }
          else
          {
            player.TVN = player.Start_NVZ;
          }
        }

        // Geschlecht korrigieren
        // Keine Angabe = Männlich
        if (player.Gender == string.Empty || player.Gender == " ")
        {
          player.Gender = "M";
        }

        offset += 655;
      }

      return player;
    }

    /// <summary>
    /// Methode to read a set of chars and get the as strings!
    /// </summary>
    /// <param name="offset">Position of the Char in the file</param>
    /// <param name="length">Amount of chars to read from the file</param>
    /// <returns>Empty if no read was possible!</returns>
    internal string ReadString(int offset, int length = 1)
    {
      if (reader == null || length < 1)
      {
        return string.Empty;
      }

      List<char> resultChars = new List<char>();
      try
      {
        // it is not possible to work with stream read funktion, because the set Poisition of the
        // offset is not used rigth in multiple iterations
        reader.BaseStream.Position = offset;
        char[] charArray = new char[length];
        for (int i = 0; i < charArray.Length; i++)
        {
          charArray[i] = Convert.ToChar(reader.BaseStream.ReadByte());
        }
        resultChars.AddRange(charArray);
      }
      catch
      {
        return string.Empty;
      }

      while (resultChars.Remove(new char())) ;

      return new string(resultChars.ToArray());
    }

    #endregion Internal Methods

    #region Private Methods

    /// <summary>
    /// Methode to set the information gaint by creating the pairings of one round.
    /// </summary>
    /// <param name="playerWhite">Player playing white in the current set round. </param>
    /// <param name="playerBlack">Player playing black in the current set round. </param>
    private static void SetPlayerInformation(IPlayerData playerWhite, IPlayerData playerBlack)
    {
      playerWhite.LastPairings.Add(playerBlack.PlayerID);
      playerBlack.LastPairings.Add(playerWhite.PlayerID);
    }

    #endregion Private Methods

    #region Classes

    private class DataHolderPairing
    {
      #region Public Constructors

      public DataHolderPairing(SWTBaseReader reader, int sp, int offset)
      {
        this.PlayerWhite = sp;
        IsHomeTeam = reader.ReadInt(offset + 8, 1);
        BoardNumber = reader.ReadInt(offset + 13, 1);
        PlayerBlack = reader.ReadInt(offset + 9, 1);
        PlayerBlack--;
        Result = reader.ReadInt(offset + 11, 1);
        Attribute = reader.ReadInt(offset + 15, 1);
      }

      #endregion Public Constructors

      #region Properties

      public int Attribute { get; set; }

      public int BoardNumber { get; set; }

      public int IsHomeTeam { get; set; }

      public int PlayerBlack { get; set; }

      public int PlayerWhite { get; set; }

      public int Result { get; set; }

      #endregion Properties

      #region Public Methods

      public (float, float) _calculateCLMErgebnisWhite(DataHolderPairing blackDataHolderPairing)
      {
        if (this.Result == 241)
        {
          this.Result = 1;
        }

        if (this.Result == 242)
        {
          this.Result = 2;
        }

        if (this.Result == 243)
        {
          this.Result = 3;
        }

        if (blackDataHolderPairing.Result == 241)
        {
          blackDataHolderPairing.Result = 1;
        }

        if (blackDataHolderPairing.Result == 242)
        {
          blackDataHolderPairing.Result = 2;
        }

        if (blackDataHolderPairing.Result == 243)
        {
          blackDataHolderPairing.Result = 3;
        }

        if (this.Result == 1 && this.Attribute != 2 && blackDataHolderPairing.Result == 3 && blackDataHolderPairing.Attribute != 2)
        {
          return (0, 1); // 0-1
        }
        else if (this.Result == 3 && this.Attribute != 2 && blackDataHolderPairing.Result == 1 && blackDataHolderPairing.Attribute != 2)
        {
          return (1, 0); // 1-0
        }
        else if (this.Result == 2 && this.Attribute != 2 && blackDataHolderPairing.Result == 2 && blackDataHolderPairing.Attribute != 2)
        {
          return (0.5f, 0.5f); // 0,5-0,5
        }
        else if (this.Result == 1 && this.Attribute != 2 && blackDataHolderPairing.Result == 1 && blackDataHolderPairing.Attribute != 2)
        {
          return (0, 0); // 0-0
        }
        else if (this.Result == 1 && this.Attribute == 2 && blackDataHolderPairing.Result == 3 && blackDataHolderPairing.Attribute == 2)
        {
          return (0, 1); // -/+
        }
        else if (this.Result == 3 && this.Attribute == 2 && blackDataHolderPairing.Result == 1 && blackDataHolderPairing.Attribute == 2)
        {
          return (1, 0); // +/-
        }
        else if (this.Result == 1 && this.Attribute == 2 && blackDataHolderPairing.Result == 1 && blackDataHolderPairing.Attribute == 2)
        {
          return (0, 0); // -/-
        }
        else if (this.Result == 1 && this.Attribute != 2 && blackDataHolderPairing.Result == 2 && blackDataHolderPairing.Attribute != 2)
        {
          return (0.5f, 0); // 0,5-0
        }
        else if (this.Result == 2 && this.Attribute != 2 && blackDataHolderPairing.Result == 1 && blackDataHolderPairing.Attribute != 2)
        {
          return (0, 0.5f); // 0-0,5
        }
        else if (this.Result == 2 && this.Attribute == 2 && blackDataHolderPairing.Result == 2 && blackDataHolderPairing.Attribute == 2)
        {
          return (0.5f, 0); // 0,5---
        }
        else
        {
          return (-1, -1); // noch nicht gespielt
        }
      }

      #endregion Public Methods
    }

    #endregion Classes
  }
}