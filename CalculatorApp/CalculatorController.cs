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
                    if (s.RightOperand is null)
                    {
                        s.Operations.Push(payload);
                        return;
                    } 
                    s.Operands.Push(s.RightOperand);
                    if (s.Operations.Count != 0 && s.Operands.Count >= 2)
                    {
                        DispatchBinaryAction(ref s, s.Operations.Peek());
                        s.Operations.Clear();
                    }
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
            s.CurrentInput.Value = stringify;
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

        private static void DispatchOutputAction(ref CalculatorState s)
        {
            if (s.Operands.Count < 2 && s.RightOperand is null) s.Operands.Push(s.Operands.Peek());
            else s.Operands.Push(s.RightOperand);
            if (s.Operations.Count != 0 && s.Operands.Count >= 2)
            {
                DispatchBinaryAction(ref s, s.Operations.Peek());
                s.Operations.Clear();
            }
            s.RightOperand = null;
        }
    }
}