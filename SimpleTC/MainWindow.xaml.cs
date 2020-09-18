using MinTC.ViewModel;
using System.Windows;

namespace MinTC
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
