using NUnit.Framework;

namespace Test_SwissChessDraw
{
  [TestFixture]
  internal class TestSwissDrawing
  {
    [TestCase]
    public void Test_CreatFloater()
    {
      IList<TestPlayer> playerDatas = new List<TestPlayer>();
      XMLReader reader = new XMLReader();
      Assert.IsTrue(reader.OpenFile(@"D:\SwissChess\Test_SwissChessDraw\TestCases\TestLoadPlayer.xml"));
      reader.ReadList<TestPlayer>(ref playerDatas, "Player");
      Assert.That(playerDatas.Count, Is.EqualTo(1));
    }
  }
}