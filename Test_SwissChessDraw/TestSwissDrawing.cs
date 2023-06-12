using NUnit.Framework;
using SwissChessDraw;

namespace Test_SwissChessDraw
{
  [TestFixture]
  internal class TestSwissDrawing
  {
    [Test]
    public void Test_CreatFloater()
    {
      IList<TestPlayer> playerDatas = new List<TestPlayer>();
      XMLReader reader = new XMLReader();
      Assert.IsTrue(reader.OpenFile(@"C:\Users\fritz\SwissChess\Test_SwissChessDraw\TestCases\TestLoadPlayer.xml"));
      reader.ReadList<TestPlayer>(ref playerDatas, "Player");
      Assert.That(playerDatas.Count, Is.EqualTo(1));
    }
  }
}