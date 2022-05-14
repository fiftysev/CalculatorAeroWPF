using System;
using System.Collections.Generic;
using System.Globalization;


namespace CalculatorApp
{
    public class CalculatorController
    {
        public void DispatchAction(string action, ref CalculatorState s)
        {
            s.History.Push(action);
            if (double.TryParse(action, NumberStyles.Float, CultureInfo.InvariantCulture, out var num))
            {
                if (s.CurrentInput.IsModifiedByUnary)
                {
                    s.RightOperand = "";
                    s.CurrentInput.IsModifiedByUnary = false;
                }
                s.RightOperand += action;
                s.CurrentInput.Value = s.RightOperand;
            }

            else if (action == ".")
            {
                s.RightOperand =
                    s.RightOperand.Contains(".") ? s.RightOperand : s.RightOperand + action;
                s.CurrentInput.Value = s.RightOperand;
            }

            else if ("+-/*".Contains(action))
            {
                if (string.IsNullOrEmpty(s.Operation))
                {
                    if (!double.IsNaN(s.LeftOperand))
                    {
                        s.RightOperand = "";
                        s.Operation = action;
                        s.CurrentInput.Value = action;
                    }
                    else
                    {
                        double.TryParse(s.RightOperand, NumberStyles.Float, CultureInfo.InvariantCulture,
                        out var leftOp);
                        s.LeftOperand = leftOp;
                        s.RightOperand = "";
                        s.Operation = action;
                        s.CurrentInput.Value = action;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(s.RightOperand))
                    {
                        s.Operation = action;
                        s.CurrentInput.Value = action;
                    }
                    else
                    {
                        DispatchBinaryAction(ref s, s.Operation);
                        s.Operation = action;
                        s.RightOperand = "";
                    }
                }
            }
            else if ("√±1/x=".Contains(action)) DispatchUnaryAction(ref s, action);
            else if (action.Contains("C")) DispatchInputAction(ref s, action);
            else if (action.Contains("M")) DispatchMemoryAction(ref s, action);
        }

        private static void DispatchBinaryAction(ref CalculatorState s, in string action)
        {
            double res = 0;
            var first = s.LeftOperand;
            var second = double.Parse(s.RightOperand, CultureInfo.InvariantCulture);
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
            }

            s.LeftOperand = res;
            s.CurrentInput.Value = res.ToString(CultureInfo.InvariantCulture);
        }

        private static void DispatchUnaryAction(ref CalculatorState s, in string action)
        {
            double res = 0;
            if (!double.TryParse(s.RightOperand, NumberStyles.Float, CultureInfo.InvariantCulture, out var operand))
            {
                operand = 0;
            };
            switch (action)
            {
                case "1/x":
                    res = 1 / operand;
                    break;
                case "√":
                    res = Math.Sqrt(operand);
                    break;
                case "±":
                    res = -operand;
                    break;
                case "=":
                    if (string.IsNullOrEmpty(s.RightOperand))
                        s.RightOperand = s.LeftOperand.ToString(CultureInfo.InvariantCulture);
                    DispatchBinaryAction(ref s, s.Operation);
                    return;
            }

            s.RightOperand = res.ToString(CultureInfo.InvariantCulture);
            s.CurrentInput.Value = s.RightOperand;
            s.CurrentInput.IsModifiedByUnary = true;
        }

        private static void DispatchMemoryAction(ref CalculatorState s, in string action)
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
            }
        }

        private static void DispatchInputAction(ref CalculatorState s, in string action)
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