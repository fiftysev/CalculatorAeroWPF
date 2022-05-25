using System.Windows;
using System.Windows.Controls;

namespace CalculatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly string[][] _buttonsLayout =
        {
            new[] {"MC", "MR", "MS", "M+", "M-"},
            new[] {"🠔", "CE", "C", "±", "√"},
            new[] {"7", "8", "9", "/", "%"},
            new[] {"4", "5", "6", "*", "1/x"},
            new[] {"1", "2", "3", "-", "="},
            new[] {"0", "0", ",", "+", "="}
        };

        private readonly CalculatorController _controller;

        public MainWindow()
        {
            _controller = new CalculatorController();
            InitializeComponent();
            InitUi();
        } 

        private void CalcButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            var content = b?.Content.ToString();
            _controller.Dispatch(content);
            NumInput.Text = _controller.UiText;
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
                        Margin = new Thickness(2.5),
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