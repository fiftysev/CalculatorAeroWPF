using System;
using System.Collections.Generic;
using System.Windows;
using TFunc =
    System.Func<System.Collections.Generic.IDictionary<string, object>,
        System.Collections.Generic.IDictionary<string, object>>;


namespace CalculatorApp
{
    public class CalculatorController
    {
        private string _inputBuffer;
        private Stack<double> _operandStack = new Stack<double>(5);
        private Stack<string> _actionStack = new Stack<string>(5);
        public void DispatchAction(string action)
        {
            if (double.TryParse(action, out var num))
            {
                _inputBuffer += action;
            }
            
            else if ("+-/*".Contains(action))
            {
               
            }
            else if ("√±1/x".Contains(action)) DispatchUnaryAction(action);
            else if (action.Contains("C")) DispatchInputAction(action);
            else if (action.Contains("M")) DispatchMemoryAction(action);
            return;
        }

        private void DispatchBinaryAction(in string action, in double first = 0, in double second = 0)
        {
            switch (action)
            {
                case "+":
                    break;
                case "-":
                    break;
                case "*":
                    break;
                case "/":
                    break;
                case "%":
                    break;
                default:
                    break;
            }
        }

        private void DispatchUnaryAction(in string action, in double operand = 0)
        {
            switch (action)
            {
                case "1/x":
                    break;
                case "√":
                    break;
                case "±":
                    break;
                default:
                    break;
            }
        }

        private void DispatchMemoryAction(in string action)
        {
            switch (action)
            {
                case "MS":
                    break;
                case "MR":
                    break;
                case "MC":
                    break;
                case "M+":
                    break;
                case "M-":
                    break;
                default:
                    break;
            }
        }

        private void DispatchInputAction(in string action)
        {
            switch (action)
            {
                case "C":
                    break;
                case "CE":
                    break;
            }
        }
    }
}