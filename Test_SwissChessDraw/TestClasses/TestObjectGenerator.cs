using DisplayBase;
using DrawBase;
using SwissChessDraw;
using SWT_Converter;

namespace Test_SwissChessDraw.TestClasses
{
  internal class TestObjectGenerator : IObjectCreator
  {
    #region Public Methods

    public IDisplayPlayerData CreateDisplayPlayerObject()
    {
      return new TestPlayer();
    }

    public IPairing CreatePairingObject()
    {
      return new TestRound.Pairing();
    }

    public IPlayerData CreatePlayerObject()
    {
      return CreateDisplayPlayerObject();
    }

    public IRoundBase CreateRoundObject()
    {
      return new TestRound();
    }

    #endregion Public Methods
  }
}