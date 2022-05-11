using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace CalculatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly string[][] _buttonsLayout =
        {
            new string[] {"MC", "MR", "MS", "M+", "M-"},
            new string[] {"🠔", "CE", "C", "±", "√"},
            new string[] {"7", "8", "9", "/", "%"},
            new string[] {"4", "5", "6", "*", "1/x"},
            new string[] {"1", "2", "3", "-", "="},
            new string[] {"0", "0", ".", "+", "="}
        };

        public MainWindow()
        {
            InitializeComponent();
            InitUi();
        }

        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            NumInput.Text += b.Content.ToString();
        }
        
        private void InitUi()
        {
            for (var i = 0; i < _buttonsLayout.Length; i++)
            {
                for (var j = 0; j < _buttonsLayout[i].Length; j++)
                {
                    if (i > 0 && j > 0) 
                    {
                        if  (_buttonsLayout[i][j - 1] == _buttonsLayout[i][j] || _buttonsLayout[i - 1][j] == _buttonsLayout[i][j]) continue;
                    }
                    var b = new Button
                    {
                        Content = _buttonsLayout[i][j],
                        Margin = new Thickness(2.5)
                    };
                    
                    Grid.SetRow(b, i);
                    Grid.SetColumn(b, j);
                    
                    if (j + 1 != _buttonsLayout[i].Length && _buttonsLayout[i][j + 1] == _buttonsLayout[i][j])
                    {
                        var prevColSpan = Grid.GetRowSpan(b);
                        Grid.SetColumnSpan(b, ++prevColSpan);
                    }

                    if (i + 1 != _buttonsLayout.Length && _buttonsLayout[i + 1][j] == _buttonsLayout[i][j])
                    {
                        var prevRowSpan = Grid.GetColumnSpan(b);
                        Grid.SetRowSpan(b, ++prevRowSpan);
                    }

                    b.Click += CalcButton_Click;
                    RootLayout.Children.Add(b);
                }
            }
        }
    }
}