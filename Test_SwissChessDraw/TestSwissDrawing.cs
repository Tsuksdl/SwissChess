using DrawBase;
using NUnit.Framework;
using SwissChessDraw;
using System.Reflection;
using Test_SwissChessDraw.TestClasses;

namespace Test_SwissChessDraw
{
  [TestFixture]
  internal class TestSwissDrawing
  {
    /// <summary>
    /// Reads all possible Test from a Folder.
    /// </summary>
    /// <param name="testFileLocation">Loaction of the folder to check for the test</param>
    /// <returns>Testfiles with folder location</returns>
    public static List<string[]> LoadTestCases(string testFileLocation)
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      string? currentAssemblyPath = Path.GetDirectoryName(assembly.Location);
      if (currentAssemblyPath == null)
      {
        return new List<string[]>();
      }

      currentAssemblyPath.Replace(@"file:\", string.Empty);
      DirectoryInfo locationInfo = new DirectoryInfo(currentAssemblyPath + testFileLocation);
      List<string[]> fileNameList = new List<string[]>();

      foreach (FileInfo fileInfo in locationInfo.GetFiles())
      {
        //only xml files as test files --> all other files in folders below this folder
        if (fileInfo.Extension.Equals(".xml"))
        {
          fileNameList.Add(new string[]
            {
              fileInfo.Name,
              fileInfo.FullName,
            });
        }
      }

      return fileNameList;
    }

    [Test]
    [TestCaseSource(nameof(LoadTestCases), new object[] { @"\TestCases\" })]
    public void Test_CreatFloater(string[] testFileName)
    {
      IDrawSystem drawSystem = new SwissChessDrawSystem();

      IList<TestPlayer> playerDatas = new List<TestPlayer>();
      XMLReader reader = new XMLReader();
      Assert.IsTrue(reader.OpenFile(testFileName[1]));
      reader.ReadList<TestPlayer>(ref playerDatas, "Player");
      Dictionary<Guid, TestPlayer> playerDic = playerDatas.ToDictionary(p => p.PlayerID, p => p);
      Assert.That(playerDatas.Count, Is.EqualTo(15));

      IList<TestRound> rounds = new List<TestRound>();
      reader.ReadList<TestRound>(ref rounds, "Round");
      Assert.That(rounds.Any(), Is.True);

      for (int i = 0; i < rounds.Count; i++)
      {
        List<IPairing> roundParings = drawSystem.CreateNewRound(playerDic.ToList().Select(p => p.Value as IPlayerData).ToList());
        Assert.That(rounds[i].Equals(roundParings), Is.True);
        rounds[i].ApplyPoint();
      }
    }
  }
}