using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_Converter.SWT_Convert
{
  public class SWTBaseReader
  {
    private StreamReader? reader;

    public SWTBaseReader()
    {
      reader = null;
    }

    public bool OpenFile(string path)
    {
      if (!File.Exists(path))
      {
        return false;
      }

      reader = new StreamReader(path);
      return true;
    }

    /// <summary>
    /// Methode to read a set of chars and get the as strings!
    /// </summary>
    /// <param name="offset">Position of the Char in the file</param>
    /// <param name="length">Amount of chars to read from the file</param>
    /// <returns>Empty if no read was possible!</returns>
    public string ReadString(int offset, int length = 1)
    {
      if (reader == null || length < 1)
      {
        return String.Empty;
      }

      byte[] resultChars = new byte[length];
      try
      {
        reader.BaseStream.Position = offset;
        reader.BaseStream.Read(resultChars, 0, length);
      }
      catch
      {
        return String.Empty;
      }

      return BitConverter.ToString(resultChars);
    }

    /// <summary>
    /// Methode to read a set of int and get the as strings!
    /// </summary>
    /// <param name="offset">Position of the Char in the file</param>
    /// <param name="length">Amount of chars to read from the file</param>
    /// <returns> -1 if no read was Possible</returns>
    public int ReadInt(int offset, int length = 1)
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
  }
}