using System.Diagnostics.CodeAnalysis;

namespace SwissChessDraw
{
  /// <summary>
  /// Copmarer Class to check if the points are equal.
  /// </summary>
  internal class FloatEqualityComparer : IEqualityComparer<float>
  {
    #region Public Methods

    public bool Equals(float x, float y)
    {
      return Math.Abs(x - y) < 0.001;
    }

    public int GetHashCode([DisallowNull] float obj)
    {
      return obj.GetHashCode();
    }

    #endregion Public Methods
  }
}