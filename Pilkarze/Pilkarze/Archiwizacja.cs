using System.IO;

namespace Pilkarze
{
    static class Archiwizacja
    {
        public static void ZapisPilkarzyDoPliku(string plik, Pilkarz[] pilkarze)
        {
            using (StreamWriter stream = new StreamWriter(plik))
            {
                foreach (var p in pilkarze)
                    stream.WriteLine(p.ToFileFormat());
                stream.Close();
            }
        }
        public static Pilkarz[] CzytajPilkarzyZPliku(string plik)
        {
            Pilkarz[] pilkarze = null;
            if (File.Exists(plik))
            {
                var sPilkarze = File.ReadAllLines(plik);
                var n = sPilkarze.Length;
                if (n > 0)
                {
                    pilkarze = new Pilkarz[n];
                    for (int i = 0; i < n; i++)
                        pilkarze[i] = Pilkarz.CreateFromString(sPilkarze[i]);
                    return pilkarze;
                }
            }
            return pilkarze;
        }
    }
}
