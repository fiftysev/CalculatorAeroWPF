using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;


namespace CalculatorApp
{
    public static class CalculatorController
    {
        public static void DispatchAction(string action, ref CalculatorState s)
        {
            if (int.TryParse(action, out var num))
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
                    if (double.IsNaN(s.LeftOperand))
                        double.TryParse(s.RightOperand,
                            NumberStyles.Number,
                            CultureInfo.InvariantCulture,
                            out s.LeftOperand);
                    s.Operation = action;
                }

                s.RightOperand = "";
            }
            else if ("√±1/x".Contains(action)) DispatchUnaryAction(ref s, action);
            else if ("=".Contains(action)) DispatchUnaryAction(ref s, action);
            else if (action.Contains("C")) DispatchInputAction(ref s, action);
            else if (action.Contains("M")) DispatchMemoryAction(ref s, action);
        }

        public static void Dispatcher(ref CalculatorState s, string payload)
        {
            switch (Utils.TypesMap[payload])
            {
                case Utils.CalculatorOperationType.Digit:
                    if (s.RightOperand is null || s.RightOperand.Equals("0")) s.RightOperand = payload;
                    else s.RightOperand += payload;
                    s.CurrentInput.Value = s.RightOperand;
                    break;
                case Utils.CalculatorOperationType.FloatingPoint:
                    if (s.RightOperand is null) s.RightOperand = "0";
                    s.RightOperand = s.RightOperand.Contains(payload) ? s.RightOperand : s.RightOperand + payload;
                    s.CurrentInput.Value = s.RightOperand;
                    break;
                case Utils.CalculatorOperationType.Binary:
                    if (s.RightOperand is null)
                    {
                        s.Operations.Push(payload);
                        return;
                    } 
                    s.Operands.Push(s.RightOperand);
                    if (s.Operations.Count != 0 && s.Operands.Count >= 2) DispatchBinaryAction(ref s, s.Operations.Pop());
                    s.Operations.Push(payload);
                    s.RightOperand = null;
                    break;
                case Utils.CalculatorOperationType.Unary:
                    break;
                case Utils.CalculatorOperationType.Output:
                    break;
                case Utils.CalculatorOperationType.Memory:
                    break;
                case Utils.CalculatorOperationType.ClearData:
                    break;
            }
        }

        private static void DispatchBinaryAction(ref CalculatorState s, in string payload)
        {
            double res = 0;
            double.TryParse(s.Operands.Pop(), out var second);
            double.TryParse(s.Operands.Pop(), out var first);
            switch (payload)
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

            string stringify = res.ToString(CultureInfo.CurrentCulture);
            s.Operands.Push(stringify);
            s.CurrentInput.Value = stringify;
        }

        private static void DispatchUnaryAction(ref CalculatorState s, in string action)
        {
            double res = 0;
            switch (action)
            {
                case "1/x":
                    res = 1 / s.LeftOperand;
                    break;
                case "√":
                    res = Math.Sqrt(s.LeftOperand);
                    break;
                case "±":
                    res = -s.LeftOperand;
                    break;
                case "=":
                    if (string.IsNullOrEmpty(s.RightOperand))
                        s.RightOperand = s.LeftOperand.ToString(CultureInfo.InvariantCulture);
                    DispatchBinaryAction(ref s, s.Operation);
                    s.RightOperand = "";
                    return;
            }
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
                    s = new CalculatorState { LeftOperand = double.NaN };
                    break;
                case "CE":
                    s.RightOperand = "";
                    s.CurrentInput.Value = s.RightOperand;
                    break;
            }
        }
    }
}