using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Still a WIP, there's a few things to fix and a few bugs to squish
    /// </summary>
    public partial class MainWindow : Window
    {
        private decimal a = 0; // current displayed value
        private decimal b = 0; // last value
        private string operation = "";
        private bool isInt = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetText(decimal x)
        {
            tbOut.Text = x.ToString();
        }

        private void ResetText()
        {
            tbOut.Text = "0";
        }

        private void btnEquals_Click(object sender, RoutedEventArgs e)
        {
            switch (operation) 
            {
                case "+":
                    b += a;
                    SetText(b);
                    break;
                case "-":
                    b = b - a;
                    SetText(b);
                    break;
                case "*":
                    b *= a;
                    SetText(b);
                    break;
                case "/":
                    if (a != 0)
                    {
                        b = b / a;
                        SetText(b);
                    }
                    break;
                default:
                    isInt = true;
                    SetText(a);
                    break;
            }
            SetText((decimal)double.Parse(tbOut.Text.ToString())); // removes trailing zeros
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            a = 0;
            b = 0;
            isInt = true;
            ResetText();
        }

        private void btnClearEntry_Click(object sender, RoutedEventArgs e)
        {
            a = 0;
            isInt = true;
            ResetText();
        }

        private void btnPoint_Click(object sender, RoutedEventArgs e)
        {
            if (isInt) 
            {
                isInt = false;
                tbOut.Text = a.ToString() + ".";
            }
        }

        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            if (isInt)
            {
                if (a > 9 || a < -9)
                {
                    a = decimal.Parse(a.ToString().Remove(a.ToString().Length - 1));
                    SetText(a);
                }
                else
                {
                    a = 0;
                    ResetText();
                }
            }
            else
            {
                string txt = a.ToString();
                if (Regex.IsMatch(txt, "[^0-9]$"))
                {
                    txt = txt.Remove(txt.Length - 1);
                }
                if (txt.Length > 1)
                {
                    a = decimal.Parse(txt.Remove(txt.Length - 1));
                }
                else
                {
                    a = 0;
                    isInt = true;
                }
                SetText(a);
            }
        }

        private void btnPlusMinus_Click(object sender, RoutedEventArgs e)
        {
            b = decimal.Parse(tbOut.Text.ToString()) * -1;
            SetText(b);
        }

        private void Operate(string x)
        {
            operation = x;
            b = decimal.Parse(tbOut.Text.ToString());
            a = 0;
            isInt = true;
        }

        private void btnDivide_Click(object sender, RoutedEventArgs e)
        {
            Operate("/");
        }

        private void btnTimes_Click(object sender, RoutedEventArgs e)
        {
            Operate("*");
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            Operate("-");
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Operate("+");
        }
        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (a.ToString().Length < 16)
            {
                Button number = (Button)sender;
                string txt = tbOut.Text.ToString();
                if (Regex.IsMatch(txt, "[^0-9]$"))
                {
                    a = Math.Round(decimal.Parse(a.ToString() + "." + number.Content.ToString()), 15);
                }
                else
                {
                    a = Math.Round(decimal.Parse(a.ToString() + number.Content.ToString()), 15);
                }
                SetText(a);
            }
        }
    }
}
