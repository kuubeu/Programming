using System;

namespace Pilkarze
{
    internal class Pilkarz
    {

        #region Prop
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public uint Wiek { get; set; }
        public uint Waga { get; set; }
        #endregion

        #region constr
        public Pilkarz(string imie, string nazwisko, uint wiek, uint waga)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Wiek = wiek;
            Waga = waga;
        }

        public Pilkarz(string imie, string nazwisko) : this(imie, nazwisko, 30, 75) { }
        #endregion

        #region methods
        public bool isTheSame(Pilkarz pilkarz)
        {
            if (pilkarz.Nazwisko != Nazwisko) return false;
            if (pilkarz.Imie != Imie) return false;
            if (pilkarz.Wiek != Wiek) return false;
            if (pilkarz.Waga != Waga) return false;
            return true;
        }

        public override string ToString()
        {
            return $"{Imie} {Nazwisko} lat: {Wiek} waga: {Waga} kg";
        }

        public string ToFileFormat()
        {
            return $"{Imie}|{Nazwisko}|{Wiek}|{Waga}";
        }

        public static Pilkarz CreateFromString(string sPilkarz)
        {
            string imie, nazwisko;
            uint wiek, waga;
            var pola = sPilkarz.Split('|');
            if (pola.Length == 4)
            {
                nazwisko = pola[1];
                imie = pola[0];
                wiek = uint.Parse(pola[2]);
                waga = uint.Parse(pola[3]);
                return new Pilkarz(imie, nazwisko, wiek, waga);
            }
            throw new Exception("Błędny format danych z pliku");
        }
        #endregion
    }
}
