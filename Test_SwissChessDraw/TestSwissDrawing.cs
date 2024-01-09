using DrawBase;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using SwissChessDraw;
using System.Reflection;
using Test_SwissChessDraw.TestClasses;
using SWT_Converter;

namespace Test_SwissChessDraw
{
  [TestFixture]
  internal class TestSwissDrawing
  {
    public static void Main(string[] args)
    {
      Thread thread = new Thread(() =>
      {
        OpenFileDialog openSWTDialog = new OpenFileDialog();
        openSWTDialog.Filter = "SWT-FIle *.SWT| *.SWT;*.swt";
        if (openSWTDialog.ShowDialog() == DialogResult.OK)
        {
          string path = openSWTDialog.FileName;
          try
          {
            SWTTurnament turnament = new SWTTurnament(path, new TestObjectGenerator());
            XMLSaver saver = new XMLSaver();
            saver.CreateNewNode("Test", true);
            foreach (TestPlayer player in turnament.PlayerData)
            {
              player.Save(saver, false);
            }

            foreach (TestRound round in turnament.Rounds)
            {
              round.Save(saver, null);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML-File *.xml | *.xml";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
              saver.Save(saveFileDialog.FileName);
            }
          }
          catch
          { }
        }
      });

      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();
      thread.Join();
    }

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
      IList<TestPlayer> playerDatas = new List<TestPlayer>();
      XMLReader reader = new XMLReader();
      Assert.IsTrue(reader.OpenFile(testFileName[1]));
      reader.ReadList<TestPlayer>(ref playerDatas, false, "Player");
      Dictionary<Guid, IPlayerData> playerDic = playerDatas.ToDictionary(p => p.PlayerID, p => (IPlayerData)p);

      IList<TestRound> rounds = new List<TestRound>();
      reader.ReadList<TestRound>(ref rounds, playerDic, "Round");
      Assert.That(rounds.Any(), Is.True);
      TestTournament testTournament = new TestTournament(rounds.Count);
      IDrawSystem drawSystem = new SwissChessDrawSystem(testTournament);

      for (int i = 0; i < rounds.Count; i++)
      {
        List<IPairing> roundParings = drawSystem.CreateNewRound(playerDic.ToList().Select(p => p.Value).ToList(), testTournament);
        Assert.That(rounds[i].Pairings.Count , Is.EqualTo(roundParings.Count));
        rounds[i].ApplyPoint();
      }
    }
  }
}