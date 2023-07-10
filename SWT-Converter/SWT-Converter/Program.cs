// See https://aka.ms/new-console-template for more information
using SwissChessDraw;
using SWT_Converter.SWT_Convert;

string path = @"C:\Users\fritz\OneDrive\Documents\Chess\Super_Cup\2023_Duatlon\Duathlon.SWT";

Console.WriteLine($"Open File: {path}");

SWTBaseReader reader = new SWTBaseReader();
reader.OpenFile(path);

Console.WriteLine($"Read Turnementtype: {reader.ReadInt(606, 1)}");

int aktuelle_runde = reader.ReadInt(3, 2);
int modus = reader.ReadInt(596, 1);
int aktueller_durchgang = reader.ReadInt(598, 1);
int anz_teilnehmer = reader.ReadInt(7, 2);
int offset = 0;
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
  IPlayerData teilnehmer = new Player_SWT_Converter();

  if (reader.ReadInt(offset + 189, 1) == 102)
  {
    teilnehmer = new Player_SWT_Converter(Guid.Empty);
  }
  else
  {
				$teilnehmer->set('name', CLMSWT::readName($swt,$offset, 32));
				$teilnehmer->set('verein', CLMSWT::readName($swt,$offset + 33, 32));
				$teilnehmer->set('title', CLMSWT::readName($swt,$offset + 66, 3));
				$teilnehmer->set('FIDEelo', CLMSWT::readName($swt,$offset + 70, 4));
				$teilnehmer->set('start_dwz', CLMSWT::readName($swt,$offset + 75, 4));
				$teilnehmer->set('FIDEcco', CLMSWT::readName($swt,$offset + 105, 3));
				$teilnehmer->set('NATcco', CLMSWT::readName($swt,$offset + 109, 3));
				$teilnehmer->set('birthYear', CLMSWT::readName($swt,$offset + 128, 4));
				$teilnehmer->set('zps', CLMSWT::readName($swt,$offset + 153, 5));
				$teilnehmer->set('mgl_nr', CLMSWT::readName($swt,$offset + 159, 4));
				$teilnehmer->set('geschlecht', CLMSWT::readName($swt,$offset + 184, 1));
				$teilnehmer->set('tlnrStatus', (CLMSWT::readName($swt,$offset + 184, 1) == "*" ? "0" : "1"));
    if ($modus == 3 OR $modus == 5) $teilnehmer->set('tlnrStatus', 1);
				$teilnehmer->set('FIDEid', CLMSWT::readName($swt,$offset + 324, 12));

				$s_points = CLMSWT::readInt($swt,$offset + 273, 1);
				$s_sign = CLMSWT::readInt($swt,$offset + 274, 1);
if ($s_sign == 255) $s_points = ($s_points - 256);
				$s_punkte = strval($s_points / 2);
				$teilnehmer->set('s_punkte', $s_punkte);

//TWZ-Bestimmen
if ($useAsTWZ == 0) {
  if ($teilnehmer->FIDEelo >= $teilnehmer->start_dwz) { $teilnehmer->set('twz', $teilnehmer->FIDEelo); }
          else { $teilnehmer->set('twz', $teilnehmer->start_dwz); }
}
elseif($useAsTWZ == 1) {
  if ($teilnehmer->start_dwz > 0) { $teilnehmer->set('twz', $teilnehmer->start_dwz); }
          else { $teilnehmer->set('twz', $teilnehmer->FIDEelo); }
}
elseif($useAsTWZ == 2) {
  if ($teilnehmer->FIDEelo > 0) { $teilnehmer->set('twz', $teilnehmer->FIDEelo); }
          else { $teilnehmer->set('twz', $teilnehmer->start_dwz); }
}

// Geschlecht korrigieren
// Keine Angabe = Männlich
if ($teilnehmer->geschlecht == " ") {
					$teilnehmer->set('geschlecht', "M", 1);
}
  }