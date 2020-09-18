using MinTC.Model;
using MinTC.ViewModel.Base;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MinTC.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        #region variable declarations
        private PanelTCViewModel _leftPanelTCViewModel;
        private PanelTCViewModel _RightPanelTCViewModel;
        #endregion

        #region constructor
        public MainViewModel()
        {
            LeftPanelTCViewModel = new PanelTCViewModel();
            RightPanelTCViewModel = new PanelTCViewModel();
        }
        #endregion

        #region properties
        public PanelTCViewModel LeftPanelTCViewModel
        {
            get { return _leftPanelTCViewModel; }
            set
            {
                if (value == _leftPanelTCViewModel)
                    return;

                _leftPanelTCViewModel = value;
                onPropertyChanged("LeftPanelTCViewModel");
            }
        }

        public PanelTCViewModel RightPanelTCViewModel
        {
            get { return _RightPanelTCViewModel; }
            set
            {
                if (value == _RightPanelTCViewModel)
                    return;

                _RightPanelTCViewModel = value;
                onPropertyChanged("RightPanelTCViewModel");
            }
        }

        public bool RightToleftCopy { set; get; }
        public String CopyText { get; } = Properties.Resources.Copy;
        public String DeleteText { get; } = Properties.Resources.Delete;
        #endregion

        #region commands
        private ICommand _copy = null;
        public ICommand Copy
        {
            get
            {
                if (_copy == null)
                {
                    _copy = new RelayCommand(
                        arg => {
                            try
                            {
                                string itemToCopy = "";
                                string pathToCopy = "";

                                if (RightToleftCopy == true)
                                {
                                    if (RightPanelTCViewModel.SelectedItem.Contains("<" + RightPanelTCViewModel.CurrentPath[0] + ">"))
                                    {
                                        pathToCopy = LeftPanelTCViewModel.CurrentPath + RightPanelTCViewModel.SelectedItem.Replace("<" + RightPanelTCViewModel.CurrentDrive.Name[0] + ">", "\\");
                                        itemToCopy = RightPanelTCViewModel.CurrentPath + RightPanelTCViewModel.SelectedItem.Replace("<" + RightPanelTCViewModel.CurrentDrive.Name[0] + ">", "");
                                        CopyDir(itemToCopy, pathToCopy);
                                    }
                                    else if (RightPanelTCViewModel.SelectedItem != "...")
                                    {
                                        if (RightPanelTCViewModel.SelectedItem[0] == '\\' || LeftPanelTCViewModel.CurrentPath == LeftPanelTCViewModel.CurrentDrive.Name)
                                            pathToCopy = LeftPanelTCViewModel.CurrentPath + RightPanelTCViewModel.SelectedItem;
                                        else
                                            pathToCopy = LeftPanelTCViewModel.CurrentPath + '\\' + RightPanelTCViewModel.SelectedItem;
                                        itemToCopy = RightPanelTCViewModel.CurrentPath + RightPanelTCViewModel.SelectedItem;

                                        File.Copy(itemToCopy, pathToCopy);
                                    }
                                }
                                else if (RightToleftCopy == false)
                                {
                                    if (LeftPanelTCViewModel.SelectedItem.Contains("<" + LeftPanelTCViewModel.CurrentPath[0] + ">"))
                                    {
                                        pathToCopy = RightPanelTCViewModel.CurrentPath + LeftPanelTCViewModel.SelectedItem.Replace("<" + LeftPanelTCViewModel.CurrentDrive.Name[0] + ">", "\\");
                                        itemToCopy = LeftPanelTCViewModel.CurrentPath + LeftPanelTCViewModel.SelectedItem.Replace("<" + LeftPanelTCViewModel.CurrentDrive.Name[0] + ">", "");
                                        CopyDir(itemToCopy, pathToCopy);
                                    }
                                    else if (LeftPanelTCViewModel.SelectedItem != "...")
                                    {
                                        if (LeftPanelTCViewModel.SelectedItem[0] == '\\' || RightPanelTCViewModel.CurrentPath == RightPanelTCViewModel.CurrentDrive.Name)
                                            pathToCopy = RightPanelTCViewModel.CurrentPath + LeftPanelTCViewModel.SelectedItem;
                                        else
                                            pathToCopy = RightPanelTCViewModel.CurrentPath + '\\' + LeftPanelTCViewModel.SelectedItem;
                                        itemToCopy = LeftPanelTCViewModel.CurrentPath + LeftPanelTCViewModel.SelectedItem;
                                        File.Copy(itemToCopy, pathToCopy);
                                    }
                                }
                                UpdateViewPanels(LeftPanelTCViewModel, RightPanelTCViewModel);
                            }
                            catch (Exception error) { MessageBox.Show(error.Message); }
                        },
                        arg => (!string.IsNullOrEmpty(LeftPanelTCViewModel.SelectedItem) || !string.IsNullOrEmpty(RightPanelTCViewModel.SelectedItem)) && (!string.IsNullOrEmpty(LeftPanelTCViewModel.CurrentPath) && !string.IsNullOrEmpty(RightPanelTCViewModel.CurrentPath)) 
                     );
                }
                return _copy;
            }
        }

        private ICommand _leftFocus = null;
        public ICommand LeftFocus
        {
            get
            {
                if (_leftFocus == null)
                {
                    _leftFocus = new RelayCommand(
                        arg => {
                            RightToleftCopy = false;
                        },
                        arg => true
                     );
                }
                return _leftFocus;
            }
        }

        private ICommand _rightFocus = null;
        public ICommand RightFocus
        {
            get
            {
                if (_rightFocus == null)
                {
                    _rightFocus = new RelayCommand(
                        arg => {
                            RightToleftCopy = true;
                        },
                        arg => true
                     );
                }
                return _rightFocus;
            }
        }
        #endregion

        #region methods
        static public void CopyDir(string sourceDir, string destDir)
        {
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            string[] files = Directory.GetFiles(sourceDir);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destDir, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceDir);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destDir, name);
                CopyDir(folder, dest);
            }
        }

        static public void UpdateViewPanels(PanelTCViewModel LeftPanelTCViewModel, PanelTCViewModel RightPanelTCViewModel) 
        {
            if (LeftPanelTCViewModel.CurrentFile != null)
            {
                if (LeftPanelTCViewModel.CurrentFile.Items[0] == "...")
                {
                    FileModel tmp = new FileModel(LeftPanelTCViewModel.CurrentPath, LeftPanelTCViewModel.CurrentDrive.Name);
                    tmp.Items.Insert(0, "...");
                    LeftPanelTCViewModel.CurrentFile = tmp;
                }
                else
                    LeftPanelTCViewModel.CurrentFile = new FileModel(LeftPanelTCViewModel.CurrentPath, LeftPanelTCViewModel.CurrentDrive.Name);
            }
            if (RightPanelTCViewModel.CurrentFile != null)
            {
                if (RightPanelTCViewModel.CurrentFile.Items[0] == "...")
                {
                    FileModel tmp = new FileModel(RightPanelTCViewModel.CurrentPath, RightPanelTCViewModel.CurrentDrive.Name);
                    tmp.Items.Insert(0, "...");
                    RightPanelTCViewModel.CurrentFile = tmp;
                }
                else
                    RightPanelTCViewModel.CurrentFile = new FileModel(RightPanelTCViewModel.CurrentPath, RightPanelTCViewModel.CurrentDrive.Name);
            }
        }
        #endregion
    }
}
