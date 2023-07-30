using DrawBase;
using SwissChessDraw;

namespace SWT_Converter
{
  public class SWTTurnament : ITurnamentBase
  {
    IObjectCreator _objectCreator;

    #region Public Constructors

    /// <summary>
    /// Konstruktor to creat a new turnament objekt based on a SWT-File.
    /// </summary>
    /// <param name="path">Path to the file to open and read.</param>
    /// <exception cref="FileNotFoundException"> If the given path is not pointing at the rigth file</exception>
    public SWTTurnament(string path, IObjectCreator objectCreator)
    {
      SWTBaseReader reader = new SWTBaseReader();
      _objectCreator = objectCreator;
      if (!reader.OpenFile(path))
      {
        throw new FileNotFoundException(path);
      }

      RoundCount = reader.ReadInt(1, 2);
      CurrentRound = reader.ReadInt(3, 2);
      DrawnRound = reader.ReadInt(5, 2);
      PlayerCount = reader.ReadInt(7, 2);
      TurnementMode = reader.ReadInt(596, 1);
      UseAsTWZ = reader.ReadInt(582, 1);
      CurrentStage = reader.ReadInt(598, 1);
      StageCount = reader.ReadInt(599, 1);
      IsNull = false;

      int offset;

      if (PlayerCount != 0)
      { //Turnier ist bereits angefangen
        if (TurnementMode == 1)
        { //Vollrundig
          offset = 13384 + PlayerCount * RoundCount * StageCount * 19;
        }
        else
        {
          offset = 13384 + PlayerCount * RoundCount * 19;
        }
      }
      else
      { //Turnier ist noch nicht angefangen
        offset = 13384;
      }

      PlayerData = new List<IPlayerData>();

      for (int i = 0; i < PlayerCount; i++)
      {
        PlayerData.Add(reader.ReadPlayer(this, _objectCreator, ref offset));
      }

      Rounds = reader.ReadPairings(this, _objectCreator, PlayerData);
    }

    #endregion Public Constructors

    #region Properties

    public int CurrentRound { get; set; }

    public int CurrentStage { get; set; }

    public int DrawnRound { get; set; }

    public bool IsNull { get; private set; } = true;

    public int PlayerCount { get; set; }

    public List<IPlayerData> PlayerData { get; set; }

    public int RoundCount { get; set; }

    public List<IRoundBase> Rounds { get; set; }

    public int StageCount { get; set; }

    public int TurnementMode { get; set; }

    public int UseAsTWZ { get; set; }

    #endregion Properties
  }
}