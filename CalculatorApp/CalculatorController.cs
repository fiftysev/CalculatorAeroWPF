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
                    s.CurrentInput = s.RightOperand;
                    break;
                case Utils.CalculatorOperationType.FloatingPoint:
                    if (s.RightOperand is null) s.RightOperand = "0";
                    s.RightOperand = s.RightOperand.Contains(payload) ? s.RightOperand : s.RightOperand + payload;
                    s.CurrentInput = s.RightOperand;
                    break;
                case Utils.CalculatorOperationType.Binary:
                    if (s.RightOperand is null)
                    {
                        s.Operations.Push(payload);
                        return;
                    } 
                    s.Operands.Push(s.RightOperand);
                    if (s.Operations.Count != 0 && s.Operands.Count >= 2) DispatchBinaryAction(ref s, s.Operations.Peek());
                    s.Operations.Push(payload);
                    s.RightOperand = null;
                    break;
                case Utils.CalculatorOperationType.Unary:
                    break;
                case Utils.CalculatorOperationType.Output:
                    DispatchOutputAction(ref s);
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

            var stringify = res.ToString(CultureInfo.CurrentCulture);
            s.Operands.Push(stringify);
            s.CurrentInput = stringify;
            s.History.Operand = second.ToString(CultureInfo.CurrentCulture);
            s.History.Operation = payload;
        }

        private static void DispatchUnaryAction(ref CalculatorState s, in string action)
        {
            double res = 0;
            switch (action)
            {
                case "1/x":
                    break;
                case "√":
                    break;
                case "±":
                    break;
            }
        }

        private static void DispatchMemoryAction(ref CalculatorState s, in string action)
        {
            switch (action)
            {
                case "MS":
                    s.Memory = s.CurrentInput;
                    break;
                case "MR":
                    s.RightOperand = s.CurrentInput = s.Memory;
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

        private static void DispatchInputAction(ref CalculatorState s, in string action)
        {
            switch (action)
            {
                case "C":
                    s.RightOperand = s.CurrentInput = "0";
                    break;
                case "CE":
                    s.RightOperand = s.CurrentInput = "0";
                    s.Operands.Clear();
                    s.Operations.Clear();
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
            if (s.Operations.Count != 0 && s.Operands.Count >= 2)
            {
                DispatchBinaryAction(ref s, s.History.Operation ?? s.Operations.Peek());
            }

            s.RightOperand = null;
        }
    }
}