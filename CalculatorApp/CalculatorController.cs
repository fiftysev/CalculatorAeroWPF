using System;
using System.Collections.Generic;
using System.Globalization;

namespace CalculatorApp
{
    public static class CalculatorController
    {
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
                    if (s.RightOperand is null) return;
                    s.Operands.Push(s.RightOperand);
                    if (s.Operations.Count != 0 && s.Operands.Count >= 2) DispatchBinaryAction(ref s, s.Operations.Pop());
                    s.Operations.Push(payload);
                    s.RightOperand = null;
                    break;
                case Utils.CalculatorOperationType.Unary:
                    break;
                case Utils.CalculatorOperationType.Output:
                    DispatchOutputAction(ref s);
                    break;
                case Utils.CalculatorOperationType.Memory:
                    DispatchMemoryAction(ref s, payload);
                    break;
                case Utils.CalculatorOperationType.ClearData:
                    DispatchClearInputAction(ref s, payload);
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

            var stringify = res.ToString(CultureInfo.CurrentCulture);
            s.Operands.Push(stringify);
            s.CurrentInput.Value = stringify;
            s.History.Operand = second.ToString(CultureInfo.CurrentCulture);
            s.History.Operation = payload;
        }

        private static void DispatchMemoryAction(ref CalculatorState s, in string action)
        {
            switch (action)
            {
                case "MS":
                    s.Memory = s.CurrentInput.Value;
                    break;
                case "MR":
                    s.RightOperand = s.CurrentInput.Value = s.Memory;
                    break;
                case "MC":
                    s.Memory = "";
                    break;
                case "M+":
                    s.Memory = (Convert.ToDouble(s.Memory) + Convert.ToDouble(s.CurrentInput)).ToString(CultureInfo.CurrentCulture);
                    break;
                case "M-":
                    s.Memory = (Convert.ToDouble(s.Memory) - Convert.ToDouble(s.CurrentInput)).ToString(CultureInfo.CurrentCulture);
                    break;
            }
        }

        private static void DispatchClearInputAction(ref CalculatorState s, in string action)
        {
            switch (action)
            {
                case "C":
                    s.RightOperand = s.CurrentInput.Value = "0";
                    break;
                case "CE":
                    s.RightOperand = s.CurrentInput.Value = "0";
                    s.Operands.Clear();
                    s.Operations.Clear();
                    s.History = new History();
                    break;
            }
        }

        private static void DispatchOutputAction(ref CalculatorState s)
        {
            if (s.Operands.Count < 2 && s.Operands.Count != 0)
            {
                if (s.RightOperand is null) s.Operands.Push(string.IsNullOrEmpty(s.History.Operand) ? s.Operands.Peek() : s.History.Operand);
                else s.Operands.Push(s.RightOperand);
            }
            if (s.Operations.Count != 0 && s.Operands.Count >= 2) DispatchBinaryAction(ref s, s.History.Operation ?? s.Operations.Pop());
        }
    }
}