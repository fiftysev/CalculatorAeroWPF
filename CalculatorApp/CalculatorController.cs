using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;


namespace CalculatorApp
{
    public class CalculatorController
    {
        public void DispatchAction(string action, ref CalculatorState state)
        {
            if (double.TryParse(action, NumberStyles.Float, CultureInfo.InvariantCulture, out var num))
            {
                state.RightOperand += action;
                state.CurrentInput = state.RightOperand;
            }

            else if (action == ".")
            {
                state.RightOperand =
                    state.RightOperand.Contains(".") ? state.RightOperand : state.RightOperand + action;
                state.CurrentInput = state.RightOperand;
            }
            
            else if ("+-/*".Contains(action))
            {
                if (string.IsNullOrEmpty(state.Operation))
                {
                    double.TryParse(state.RightOperand, NumberStyles.Float, CultureInfo.InvariantCulture, out var leftOp);
                    state.LeftOperand = leftOp;
                    state.RightOperand = "";
                    state.Operation = action;
                    state.CurrentInput = action;
                }
                else
                {
                    DispatchBinaryAction(ref state, state.Operation);
                    state.Operation = action;
                    state.RightOperand = "";
                }
            }
            else if ("√±1/x=".Contains(action)) DispatchUnaryAction(ref state, action);
            else if (action.Contains("C")) DispatchInputAction(action);
            else if (action.Contains("M")) DispatchMemoryAction(action);
        }

        private void DispatchBinaryAction(ref CalculatorState s, in string action)
        {
            
            double res = 0;
            double first = s.LeftOperand;
            double second = double.Parse(s.RightOperand, CultureInfo.InvariantCulture);
            switch (action)
            { 
                case "+": 
                    res = first + second;
                    break; 
                case "-":
                    res = first - second; 
                    break;
                case "*":
                    res = first * second;
                    break;
                case "/":
                    res = first / second;
                    break;
                case "%":
                    res = first % second;
                    break;
                default:
                    break;
            }
            s.LeftOperand = res;
            s.CurrentInput = res.ToString(CultureInfo.InvariantCulture);
        }

        private void DispatchUnaryAction(ref CalculatorState s, in string action, in double operand = 0)
        {
            switch (action)
            {
                case "1/x":
                    break;
                case "√":
                    break;
                case "±":
                    break;
                case "=":
                    
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