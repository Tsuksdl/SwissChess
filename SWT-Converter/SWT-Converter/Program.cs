// See https://aka.ms/new-console-template for more information
using DisplayBase;
using SWT_Converter.SWT_Convert;

string path = @"C:\Users\fritz\OneDrive\Documents\Chess\Super_Cup\2023_Duatlon\Duathlon.SWT";

Console.WriteLine($"Open File: {path}");

SWTBaseReader reader = new SWTBaseReader();
reader.OpenFile(path);

Console.WriteLine($"Read Turnementtype: {reader.ReadInt(606, 1)}");

int aktuelle_runde = reader.ReadInt(3, 2);
int modus = reader.ReadInt(596, 1);
int offset;
int aktueller_durchgang = reader.ReadInt(598, 1);
int anz_teilnehmer = reader.ReadInt(7, 2);
int anz_runden = reader.ReadInt(1, 2);
int anz_durchgaenge = reader.ReadInt(599, 1);

if (aktuelle_runde != 0)
{ //Turnier ist bereits angefangen
  if (modus == 2)
  { //Vollrundig
    offset = 13384 + anz_teilnehmer * anz_runden * anz_durchgaenge * 19;
  }
  else
  {
    offset = 13384 + anz_teilnehmer * anz_runden * 19;
  }
}
else
{ //Turnier ist noch nicht angefangen
  offset = 13384;
}

Console.WriteLine($"Offset: {offset} ");

for (int i = 0; i < anz_teilnehmer; i++)
{
  IDisplayPlayerData teilnehmer = new Player_SWT_Converter();

  if (reader.ReadInt(offset + 189, 1) == 102)
  {
    teilnehmer = new Player_SWT_Converter(Guid.Empty);
  }
  else
  {
    teilnehmer.Name = reader.ReadString(offset, 32);
    teilnehmer.Club = reader.ReadString(offset + 33, 32);
    teilnehmer.Title = reader.ReadString(offset + 66, 3);
    teilnehmer.FIDE_Elo = float.Parse(reader.ReadString(offset + 70, 4));
    teilnehmer.Start_NVZ = float.Parse(reader.ReadString(offset + 75, 4));
    teilnehmer.FIDE_cco = float.Parse(reader.ReadString(offset + 105, 3));
    teilnehmer.NAT_cco = float.Parse(reader.ReadString(offset + 109, 3));
    teilnehmer.Birthyear = reader.ReadInt(offset + 128, 4);
    teilnehmer.zps = reader.ReadString(offset + 153, 5);
    teilnehmer.ClubIdentificationNumber = float.Parse(reader.ReadString(offset + 159, 4));
    teilnehmer.Gender = reader.ReadString(offset + 184, 1);
    teilnehmer.PlayerStatus = reader.ReadString(offset + 184, 1) == "*" ? "0" : "1";

    if (modus == 3 || modus == 5)
    {
      teilnehmer.PlayerStatus = "1";
    }

    teilnehmer.FideID = reader.ReadInt(offset + 324, 12);

    int s_points = reader.ReadInt(offset + 273, 1);
    int s_sign = reader.ReadInt(offset + 274, 1);
    if (s_sign == 255)
    {
      s_points = (s_points - 256);
    }
    float s_punkte = s_points / 2;
    teilnehmer.Points = s_punkte;

    //TWZ-Bestimmen
    if (useAsTWZ == 0)
    {
      if (teilnehmer.FIDE_Elo >= teilnehmer.Start_NVZ)
      {
        teilnehmer.TVN = teilnehmer.FIDE_Elo;
      }
      else
      {
        teilnehmer.TVN = teilnehmer.Start_NVZ;
      }
    }
    else if (useAsTWZ == 1)
    {
      if (teilnehmer.Start_NVZ > 0)
      {
        teilnehmer.TVN = teilnehmer.Start_NVZ;
      }
      else
      {
        teilnehmer.TVN = teilnehmer.FIDE_Elo;
      }
    }
    else if (useAsTWZ == 2)
    {
      if (teilnehmer.FIDE_Elo > 0)
      {
        teilnehmer.TVN = teilnehmer.FIDE_Elo;
      }
      else
      {
        teilnehmer.TVN = teilnehmer.Start_NVZ;
      }
    }

    // Geschlecht korrigieren
    // Keine Angabe = Männlich
    if (teilnehmer.Gender == string.Empty || teilnehmer.Gender == " ")
    {
      teilnehmer.Gender = "M";
    }
  }
}