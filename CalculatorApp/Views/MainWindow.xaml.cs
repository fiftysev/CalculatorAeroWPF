using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CalculatorApp.Controllers;

namespace CalculatorApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly string[][] _buttonsLayout =
        {
            new[] { "MC", "MR", "MS", "M+", "M-" },
            new[] { "🠔", "CE", "C", "±", "√" },
            new[] { "7", "8", "9", "/", "%" },
            new[] { "4", "5", "6", "*", "1/x" },
            new[] { "1", "2", "3", "-", "=" },
            new[] { "0", "0", ",", "+", "=" }
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
            try
            {
                _controller.Dispatch(content);
                NumInput.Text = _controller.UiText;
                HistoryLabel.Text = _controller.Log;
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                Console.WriteLine(keyNotFoundException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                MessageBox.Show(invalidOperationException.Message);
                _controller.ResetState();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var operation = e.Key switch
            {
                Key.Back => "🠔",
                Key.OemPlus when e.KeyboardDevice.Modifiers == ModifierKeys.Shift => "+",
                Key.Return or Key.OemPlus => "=",
                Key.Escape => "C",
                Key.Delete => "CE",
                Key.D0 or Key.NumPad0 => "0",
                Key.D1 or Key.NumPad1 => "1",
                Key.D2 or Key.NumPad2 when e.KeyboardDevice.Modifiers == ModifierKeys.Shift => "√",
                Key.D2 or Key.NumPad2 => "2",
                Key.D3 or Key.NumPad3 => "3",
                Key.D4 or Key.NumPad4 => "4",
                Key.D5 or Key.NumPad5 when e.KeyboardDevice.Modifiers == ModifierKeys.Shift => "%",
                Key.D5 or Key.NumPad5 => "5",
                Key.D6 or Key.NumPad6 => "6",
                Key.D7 or Key.NumPad7 => "7",
                Key.D8 or Key.NumPad8 when e.KeyboardDevice.Modifiers == ModifierKeys.Shift => "*",
                Key.D8 or Key.NumPad8 => "8",
                Key.D9 or Key.NumPad9 => "9",
                Key.Multiply => "*",
                Key.Add => "+",
                Key.Subtract or Key.OemMinus => "-",
                Key.Decimal or Key.OemComma or Key.OemPeriod => ",",
                Key.Divide or Key.OemQuestion => "/",
                Key.M when e.KeyboardDevice.Modifiers == ModifierKeys.Control => "MS",
                Key.P when e.KeyboardDevice.Modifiers == ModifierKeys.Control => "M+",
                Key.Q when e.KeyboardDevice.Modifiers == ModifierKeys.Control => "M-",
                Key.R when e.KeyboardDevice.Modifiers == ModifierKeys.Control => "MR",
                Key.L when e.KeyboardDevice.Modifiers == ModifierKeys.Control => "MC",
                Key.R => "1/x",
                Key.F9 => "±",
                _ => null
            };

            try
            {
                _controller.Dispatch(operation);
                NumInput.Text = _controller.UiText;
                HistoryLabel.Text = _controller.Log;
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                Console.WriteLine(keyNotFoundException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                MessageBox.Show(invalidOperationException.Message);
                _controller.ResetState();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void InitUi()
        {
            for (var i = 0; i < _buttonsLayout.Length; i++)
            {
                for (var j = 0; j < _buttonsLayout[i].Length; j++)
                {
                    if (i > 0 && j > 0)
                    {
                        if (_buttonsLayout[i][j - 1] == _buttonsLayout[i][j] ||
                            _buttonsLayout[i - 1][j] == _buttonsLayout[i][j]) continue;
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