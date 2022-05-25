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
                    if (s.Input.Value is null or "0" || s.Input.IsOutput || s.Input.IsModifiedByUnary)
                        s.Input.Value = payload;
                    else s.Input.Value += payload;
                    s.UserInput = s.Input.Value;
                    break;
                case Utils.CalculatorOperationType.FloatingPoint:
                    s.Input.Value ??= "0";
                    if (!s.Input.Value.Contains(payload)) s.Input.Value += payload;
                    s.UserInput = s.Input.Value;
                    break;
                case Utils.CalculatorOperationType.Binary:
                    switch ((s.Buffer.Value is null, s.Input.Value is null))
                    {
                        case (true, true):
                            break;
                        case (true, false):
                            s.Buffer.Value = s.Input.Value;
                            s.Operation = payload;
                            s.Input.Value = null;
                            break;
                        case (false, true):
                            s.Operation = payload;
                            break;
                        case (false, false):
                            if (s.Operation is not null)
                            {
                                s.Buffer.Value =
                                    DispatchBinaryAction(s.Buffer.Value, s.Input.Value, s.Operation)
                                        .ToString(CultureInfo.CurrentCulture);
                            }

                            s.Operation = payload;
                            s.UserInput = s.Buffer.Value;
                            s.Input.Value = null;
                            break;
                    }

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

        private static double DispatchBinaryAction(in string num1, in string num2, in string payload)
        {
            double res = 0;
            double.TryParse(num1, out var first);
            double.TryParse(num2, out var second);
            res = payload switch
            {
                "+" => first + second,
                "-" => first - second,
                "*" => first * second,
                "/" => first / second,
                _ => res
            };
            return res;
        }

        private static void DispatchMemoryAction(ref CalculatorState s, in string action)
        {
            switch (action)
            {
                case "MS":
                    s.Memory = s.UserInput;
                    break;
                case "MR":
                    s.Input.Value = s.UserInput = s.Memory;
                    break;
                case "MC":
                    s.Memory = "";
                    break;
                case "M+":
                    s.Memory =
                        (Convert.ToDouble(s.Memory) + Convert.ToDouble(s.UserInput)).ToString(CultureInfo
                            .CurrentCulture);
                    break;
                case "M-":
                    s.Memory =
                        (Convert.ToDouble(s.Memory) - Convert.ToDouble(s.UserInput)).ToString(CultureInfo
                            .CurrentCulture);
                    break;
            }
        }

        private static void DispatchClearInputAction(ref CalculatorState s, in string action)
        {
            s.Input.Value = s.UserInput = "0";
            switch (action)
            {
                case "C":
                    s.Buffer = new Operand();
                    s.History = new History();
                    s.Memory = null;
                    s.Operation = null;
                    break;
                case "CE":
                    break;
            }
        }

        private static void DispatchOutputAction(ref CalculatorState s)
        {
        }
    }
}