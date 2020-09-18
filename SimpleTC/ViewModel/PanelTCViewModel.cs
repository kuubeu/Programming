using MinTC.Model;
using MinTC.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MinTC.ViewModel
{
    class PanelTCViewModel : BaseViewModel
    {
        #region variable declarations
        private ObservableCollection<Drive> _drives;
        private Drive _currentDrive;
        private FileModel _currentFile;
        private String _currentPaht;
        #endregion

        #region constructor
        public PanelTCViewModel() 
        {
            init();
        }

        private void init() 
        {
            //Zainiciowanie Drives
            _drives = new ObservableCollection<Drive>();
        }
        #endregion

        #region properties
        //Pobranie tekstów z zasobów
        public String PathText { get; } = Properties.Resources.Path;
        public String DriveText { get; } = Properties.Resources.Drive;
        //Zwracanie dostępnych dysków oraz pobieranie danych mianowicie Nazwe oraz label dysku
        public ObservableCollection<Drive> Drives 
        {
            get { return _drives; }
            set 
            {
                if (_drives.Count != 0) { _drives.Clear(); }
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady == true)
                    {
                        _drives.Add(new Drive()
                        {
                            Name = d.Name,
                            Label = d.VolumeLabel
                        });
                    }
                }
            }
        }
        //obecnie używany dysk
        public Drive CurrentDrive 
        {
            get { return _currentDrive; }
            set 
            {
                if (value != null)
                {
                    Files.Clear();
                    _currentDrive = value;
                    CurrentPath = _currentDrive.Name;
                    onPropertyChanged(nameof(CurrentDrive));
                }
            }
        }
        //Obecna ścieżka
        public String CurrentPath 
        {
            get { return _currentPaht; }
            set 
            {
                try
                {
                    Directory.GetDirectories(value);
                    _currentPaht = value;
                    if (Files.Count == 0)
                        Files.Add(new FileModel(CurrentPath, CurrentDrive.Name));
                    CurrentFile = Files[Files.Count - 1];
                    onPropertyChanged(nameof(CurrentPath));
                }
                catch { MessageBox.Show("Nie masz dostępu do tego folderu!", CurrentPath); }
            }
        }
        //Kolekcja objektów FilsModel
        public ObservableCollection<FileModel> Files { get; set; } = new ObservableCollection<FileModel>();
        //Aktualny FileModel
        public FileModel CurrentFile 
        {
            get { return _currentFile; }
            set 
            { 
                _currentFile = value;
                onPropertyChanged(nameof(CurrentFile));
            }
        }
        //Wybrany folder lub plik z listbox'a
        public String SelectedItem { get; set; }
        #endregion

        #region commands
        // Komenda odpowiedzialna za aktualizowanie dysków w ComboBox
        private ICommand _updateDrivers = null;
        public ICommand UpdateDrivers
        {
            get
            {
                if (_updateDrivers == null)
                {
                    _updateDrivers = new RelayCommand(
                        arg => {
                            Drives = new ObservableCollection<Drive>();
                        },
                        arg => true
                     );
                }
                return _updateDrivers;
            }
        }
        // Komenda odpowiedzialna za aktualizowanie naszego listbox'a z folderami i plikami
        private ICommand _updateFiles = null;
        public ICommand UpdateFiles
        {
            get
            {
                if (_updateFiles == null)
                {
                    _updateFiles = new RelayCommand(
                        arg => {
                            // Sprawdzamy czy folder
                            if (SelectedItem.Contains("<"+CurrentDrive.Name[0]+">"))
                            {
                                CurrentPath = SelectedItem.Replace("<"+ CurrentDrive.Name[0] + ">", CurrentPath);
                                FileModel tmp = new FileModel(CurrentPath, CurrentDrive.Name);
                                // Sprawdzamy czy znajdujemy się po za ścieżką np. "C:\"
                                if(CurrentPath != CurrentDrive.Name)
                                    tmp.Items.Insert(0, "...");
                                CurrentFile = tmp;
                            }
                            // Sprawdzamy czy mamy przejść wyżej
                            if (SelectedItem == "...")
                            {
                                // Usuwamy ze ścieżki folder w którym się znajdujemy "C:\Robert\Jas" -> "C:\Robert"
                                CurrentPath = CurrentPath.Remove(CurrentPath.LastIndexOf('\\'));
                                //Sprawdzamy czy przypadkiem nie było tak "C:\" -> "C:" i dodajemy '\' jeżeli tak było
                                if (CurrentPath == CurrentDrive.Name.Remove(2))
                                    CurrentPath += "\\";
                                // Sprawdzamy czy znajdujemy się po za ścieżką np. "C:\"
                                if (CurrentPath != CurrentDrive.Name)
                                {
                                    FileModel tmp = new FileModel(CurrentPath, CurrentDrive.Name);
                                    tmp.Items.Insert(0, "...");
                                    CurrentFile = tmp;
                                }
                                else
                                    CurrentFile = new FileModel(CurrentPath, CurrentDrive.Name);
                            }
                            
                        },
                        arg => (CurrentDrive != null && SelectedItem != null)
                     );
                }
                return _updateFiles;
            }
        }
        #endregion
    }
}
