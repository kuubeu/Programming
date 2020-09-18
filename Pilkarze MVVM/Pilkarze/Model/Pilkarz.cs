namespace PilkarzeMVVM.Model
{
    class Pilkarz
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public uint Wiek { get; set; }
        public uint Waga { get; set; }

        public Pilkarz(string imie, string nazwisko, uint wiek, uint waga)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Wiek = wiek;
            Waga = waga;
        }

        public Pilkarz()
        {
            Imie = "Jacek";
            Nazwisko = "Placek";
            Wiek = 30;
            Waga = 75;
        }

        public override string ToString()
        {
            return $"{Imie} {Nazwisko} lat: {Wiek} waga: {Waga} kg";
        }
    }
}
