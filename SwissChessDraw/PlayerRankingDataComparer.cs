using DrawBase;

namespace SwissChessDraw
{
  internal class PlayerRankingDataComparer : IComparer<IPlayerData>
  {
    public int Compare(IPlayerData? x, IPlayerData? y)
    {
      if (x == null && y == null)
      {
        return 0;
      }

      if (x == null)
      {
        return -1;
      }

      if (y == null)
      {
        return 1;
      }

      if (x.Points >  y.Points)
      {
        return 1;
      }
      else if (x.Points < y.Points)
      {
        return -1;
      }
      else
      {
        if (x.StartPosition < y.StartPosition)
        {
          return 1;
        }
        else
        {
          return -1;
        }
      }
    }
  }
}