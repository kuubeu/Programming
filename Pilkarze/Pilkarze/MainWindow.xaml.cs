using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pilkarze
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string plikArchiwizacji = "archiwum.txt";

        public MainWindow()
        {
            TextBoxWithErrorProvider.BrushForAll = Brushes.Red;
            InitializeComponent();
            textBoxWEPImie.SetFocus();
        }

        private bool IsNotEmpty(TextBoxWithErrorProvider tb)
        {
            if (tb.Text.Trim() == "")
            {
                tb.SetError("Pole nie może być puste!");
                return false;
            }
            tb.SetError("");
            return true;
        }

        private void Clear()
        {
            textBoxWEPImie.Text = "";
            textBoxWEPNazwisko.Text = "";
            sliderWaga.Value = 75;
            sliderWiek.Value = 25;
            buttonEdytuj.IsEnabled = false;
            buttonUsun.IsEnabled = false;
            listBoxPilkarze.SelectedIndex = -1;
            textBoxWEPImie.SetFocus();
        }

        private void LoadPlayer(Pilkarz pilkarz)
        {
            textBoxWEPImie.Text = pilkarz.Imie;
            textBoxWEPNazwisko.Text = pilkarz.Nazwisko;
            sliderWaga.Value = pilkarz.Waga;
            sliderWiek.Value = pilkarz.Wiek;
            buttonEdytuj.IsEnabled = true;
            buttonUsun.IsEnabled = true;
            textBoxWEPImie.SetFocus();
        }

        private void buttonEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (IsNotEmpty(textBoxWEPImie) & IsNotEmpty(textBoxWEPNazwisko))
            {
                var nowyPilkarz = new Pilkarz(textBoxWEPImie.Text.Trim(), textBoxWEPNazwisko.Text.Trim(), (uint)sliderWiek.Value, (uint)sliderWaga.Value);
                bool czyJuzJestNaLiscie = false;
                foreach (var p in listBoxPilkarze.Items)
                {
                    var pilkarz = p as Pilkarz;
                    if (pilkarz.isTheSame(nowyPilkarz))
                    {
                        czyJuzJestNaLiscie = true;
                        break;
                    }
                }

                if (czyJuzJestNaLiscie)
                {
                    var dialogResult = MessageBox.Show($"{nowyPilkarz} już jest na liście. {Environment.NewLine}Czy wyczyścić formularz?", "Uwaga", MessageBoxButton.YesNo);
                    if (dialogResult == MessageBoxResult.Yes)
                        Clear();
                }
                else
                {
                    var biezacyPilkarz = (Pilkarz)listBoxPilkarze.SelectedItem;
                    biezacyPilkarz.Imie = nowyPilkarz.Imie;
                    biezacyPilkarz.Nazwisko = nowyPilkarz.Nazwisko;
                    biezacyPilkarz.Waga = nowyPilkarz.Waga;
                    biezacyPilkarz.Wiek = nowyPilkarz.Wiek;
                    listBoxPilkarze.Items.Refresh();
                }
            }
        }

        private void buttonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (IsNotEmpty(textBoxWEPImie) & IsNotEmpty(textBoxWEPNazwisko))
            {
                var biezacyPilkarz = new Pilkarz(textBoxWEPImie.Text.Trim(), textBoxWEPNazwisko.Text.Trim(), (uint)sliderWiek.Value, (uint)sliderWaga.Value);
                var czyJuzJestNaLiscie = false;
                foreach (var p in listBoxPilkarze.Items)
                {
                    var pilkarz = p as Pilkarz;
                    if (pilkarz.isTheSame(biezacyPilkarz))
                    {
                        czyJuzJestNaLiscie = true;
                        break;
                    }
                }
                if (!czyJuzJestNaLiscie)
                {
                    listBoxPilkarze.Items.Add(biezacyPilkarz);
                    Clear();
                }
                else
                {
                    var dialog = MessageBox.Show($"{biezacyPilkarz.ToString()} już jest na liście {Environment.NewLine}Czy wyczyścić formularz?", "Uwaga", MessageBoxButton.YesNo);
                    if (dialog == MessageBoxResult.Yes)
                    {
                        Clear();
                    }
                }
            }
        }

        private void buttonUsun_Click(object sender, RoutedEventArgs e)
        {
            var wybranaOsoba = (Pilkarz)listBoxPilkarze.SelectedItem;
            var dialog = MessageBox.Show($"You're about to delete {wybranaOsoba.ToString()}\nAre you sure?", "Warning", MessageBoxButton.OKCancel);
            if (dialog == MessageBoxResult.OK)
            {
                listBoxPilkarze.Items.Remove(wybranaOsoba);
                Clear();
            }
        }

        private void listBoxPilkarze_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxPilkarze.SelectedIndex > -1)
            {
                LoadPlayer((Pilkarz)listBoxPilkarze.SelectedItem);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int n = listBoxPilkarze.Items.Count;
            Pilkarz[] pilkarze = null;
            if (n > 0)
            {
                pilkarze = new Pilkarz[n];
                int index = 0;
                foreach (var item in listBoxPilkarze.Items)
                {
                    pilkarze[index++] = item as Pilkarz;
                }
                Archiwizacja.ZapisPilkarzyDoPliku(plikArchiwizacji, pilkarze);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var pilkarze = Archiwizacja.CzytajPilkarzyZPliku(plikArchiwizacji);
            if (pilkarze != null)
                foreach (var p in pilkarze)
                    listBoxPilkarze.Items.Add(p);
        }
    }
}
