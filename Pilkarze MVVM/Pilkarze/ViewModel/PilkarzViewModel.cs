using System;
using PilkarzeMVVM.Model;
using PilkarzeMVVM.ViewModel.KlasaBazowa;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;

namespace PilkarzeMVVM.ViewModel
{
    internal class PilkarzViewModel : ViewModelBaza
    {
        private Pilkarze pilkarze;

        private void Clear()
        {
            BiezaceImie = "";
            BiezaceNazwisko = "";
            BiezacyWiek = 25;
            BiezacaWaga = 75;
        }

        private ObservableCollection<Pilkarz> _listaGraczy;
        public ObservableCollection<Pilkarz> ListaGraczy
        {
            get { return _listaGraczy; }
            set
            {
                _listaGraczy = value;
                onPropertyChanged(nameof(ListaGraczy));
            }
        }

        private Pilkarz _wybranyElement;
        public Pilkarz WybranyElement
        {
            get
            {
                if (_wybranyElement != null)
                {
                    BiezaceImie = _wybranyElement.Imie;
                    BiezaceNazwisko = _wybranyElement.Nazwisko;
                    BiezacyWiek = _wybranyElement.Wiek;
                    BiezacaWaga = _wybranyElement.Waga;
                }
                return _wybranyElement;
            }
            set
            {
                _wybranyElement = value;
                onPropertyChanged(nameof(WybranyElement));
            }
        }

        private string _biezaceImie;
        public string BiezaceImie
        {
            get { return _biezaceImie; }
            set
            {
                _biezaceImie = value;
                onPropertyChanged(nameof(BiezaceImie));
            }
        }

        private string _biezaceNazwisko;
        public string BiezaceNazwisko
        {
            get { return _biezaceNazwisko; }
            set
            {
                _biezaceNazwisko = value;
                onPropertyChanged(nameof(BiezaceNazwisko));
            }
        }

        private uint _biezacyWiek;
        public uint BiezacyWiek
        {
            get { return _biezacyWiek; }
            set
            {
                _biezacyWiek = value;
                onPropertyChanged(nameof(BiezacyWiek));
            }
        }

        private uint _biezacaWaga;
        public uint BiezacaWaga
        {
            get { return _biezacaWaga; }
            set
            {
                _biezacaWaga = value;
                onPropertyChanged(nameof(BiezacaWaga));
            }
        }

        public PilkarzViewModel()
        {
            pilkarze = new Pilkarze();
            try
            {
                pilkarze = JsonConvert.DeserializeObject<Pilkarze>(File.ReadAllText(@"archiwum.json"));
                _listaGraczy = pilkarze.PilkarzList;
            }
            catch (IOException)
            {
                Console.WriteLine("Nie znaleziono pliku wejściowego");
            }
            Clear();
        }

        private ICommand _dodajPilkarza = null;
        public ICommand DodajPilkarza
        {
            get
            {
                if (_dodajPilkarza == null)
                {
                    _dodajPilkarza = new Przekaz(
                        arg =>
                        {
                            ListaGraczy = pilkarze.DodajPilkarza(new Pilkarz(BiezaceImie, BiezaceNazwisko, BiezacyWiek, BiezacaWaga));
                            File.WriteAllText("archiwum.json", JsonConvert.SerializeObject(pilkarze));
                            Clear();
                        },
                        arg => !string.IsNullOrEmpty(BiezaceImie) && !string.IsNullOrEmpty(BiezaceNazwisko) && _wybranyElement == null
                     );
                }
                return _dodajPilkarza;
            }
        }

        private ICommand _edytujPilkarza = null;
        public ICommand EdytujPilkarza
        {
            get
            {
                if (_edytujPilkarza == null)
                {
                    _edytujPilkarza = new Przekaz(
                        arg =>
                        {
                            ListaGraczy = pilkarze.EdytujPilkarza(_wybranyElement, new Pilkarz(BiezaceImie, BiezaceNazwisko, BiezacyWiek, BiezacaWaga));
                            File.WriteAllText("archiwum.json", JsonConvert.SerializeObject(pilkarze));
                            Clear();
                        },
                        arg => _wybranyElement != null
                     );
                }
                return _edytujPilkarza;
            }
        }

        private ICommand _usunPilkarza = null;
        public ICommand UsunPilkarza
        {
            get
            {
                if (_usunPilkarza == null)
                {
                    _usunPilkarza = new Przekaz(
                        arg =>
                        {
                            ListaGraczy = pilkarze.UsunPilkarza(_wybranyElement);
                            File.WriteAllText("archiwum.json", JsonConvert.SerializeObject(pilkarze));
                            Clear();
                        },
                        arg => _wybranyElement != null
                     );
                }
                return _usunPilkarza;
            }
        }
    }
}
