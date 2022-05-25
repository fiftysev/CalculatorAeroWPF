using System;
using System.Globalization;

namespace CalculatorApp
{
    public class CalculatorController
    {
        private CalculatorState _s;
        public string UiText => _s.UserInput;

        public CalculatorController()
        {
            _s = new CalculatorState();
        }
        /// <summary>
        /// Main method in controller, does matching of operation from button with BL (like FLUX arch)
        /// </summary>
        /// <param name="payload"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Dispatch(in string payload)
        {
            switch (Utils.TypesMap[payload])
            {
                case Utils.CalculatorOperationType.Digit:
                    if (_s.Input.Value is null or "0" || _s.Input.IsOutput || _s.Input.IsModifiedByUnary)
                    {
                        _s.Input.Value = payload;
                        _s.Input.IsOutput = false;
                        _s.Input.IsModifiedByUnary = false;
                    }
                    else _s.Input.Value += payload;
                    _s.UserInput = _s.Input.Value;
                    break;
                case Utils.CalculatorOperationType.FloatingPoint:
                    _s.Input.Value ??= "0";
                    if (!_s.Input.Value.Contains(payload)) _s.Input.Value += payload;
                    _s.UserInput = _s.Input.Value;
                    break;
                case Utils.CalculatorOperationType.Binary:
                    switch ((_s.Buffer.Value is null, _s.Input.Value is null))
                    {
                        case (true, true):
                            break;
                        case (true, false):
                            _s.Buffer.Value = _s.Input.Value;
                            _s.Operation = payload;
                            _s.Input.Value = null;
                            break;
                        case (false, true):
                            _s.Operation = payload;
                            break;
                        case (false, false):
                            if (_s.Operation is not null)
                            {
                                _s.Buffer.Value =
                                    BinaryActionReducer(_s.Buffer.Value, _s.Input.Value, _s.Operation);
                            }

                            _s.Operation = payload;
                            _s.UserInput = _s.Buffer.Value;
                            _s.Input.Value = null;
                            break;
                    }

                    break;
                case Utils.CalculatorOperationType.Unary:
                    if (!string.IsNullOrEmpty(_s.Input.Value)) _s.Input.Value = UnaryActionReducer(_s.Input.Value, payload);
                    _s.UserInput = _s.Input.Value;
                    _s.Input.IsModifiedByUnary = true;
                    break;
                case Utils.CalculatorOperationType.Percent:
                    _s.Input.Value = PercentActionReducer(_s.Buffer.Value, _s.Input.Value);
                    _s.UserInput = _s.Input.Value;
                    _s.Input.IsModifiedByUnary = true;
                    break;
                case Utils.CalculatorOperationType.Output:
                    DispatchOutputAction();
                    break;
                case Utils.CalculatorOperationType.Memory:
                    MemoryActionReducer(payload);
                    break;
                case Utils.CalculatorOperationType.ClearData:
                    DispatchClearInputAction(payload);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        private static string BinaryActionReducer(in string num1, in string num2, in string payload)
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
            return res.ToString(CultureInfo.CurrentCulture);
        }

        private static string UnaryActionReducer(in string num, in string payload)
        {
            double res = 0;
            double.TryParse(num, out var number);
            res = payload switch
            {
                "√" => Math.Sqrt(number),
                "±" => -number,
                "1/x" => 1 / number,
                _ => res
            };
            return res.ToString(CultureInfo.CurrentCulture);
        }

        private static string PercentActionReducer(in string num, in string percents)
        {
            double.TryParse(num, out var number);
            double.TryParse(percents, out var per);
            return ((number / 100) * per).ToString(CultureInfo.CurrentCulture);
        }

        private void MemoryActionReducer(in string action)
        {
            switch (action)
            {
                case "MS":
                    _s.Memory = _s.UserInput;
                    break;
                case "MR":
                    _s.Input.Value = _s.UserInput = _s.Memory;
                    break;
                case "MC":
                    _s.Memory = null;
                    break;
                case "M+":
                    if (_s.Memory is null) return;
                    _s.Memory =
                        (Convert.ToDouble(_s.Memory) + Convert.ToDouble(_s.UserInput)).ToString(CultureInfo
                            .CurrentCulture);
                    break;
                case "M-":
                    if (_s.Memory is null) return;
                    _s.Memory =
                        (Convert.ToDouble(_s.Memory) - Convert.ToDouble(_s.UserInput)).ToString(CultureInfo
                            .CurrentCulture);
                    break;
            }
        }

        private void DispatchClearInputAction(in string action)
        {
            _s.Input.Value = _s.UserInput = "0";
            switch (action)
            {
                case "C":
                    _s.Buffer = new Operand();
                    _s.History = new History();
                    _s.Memory = null;
                    _s.Operation = null;
                    break;
                case "CE":
                    break;
            }
        }

        private void DispatchOutputAction()
        {
            while (true)
            {
                switch ((_s.Buffer.Value is null, _s.Input.Value is null))
                {
                    case (true, true):
                        _s.UserInput = "0";
                        break;
                    case (true, false) when !string.IsNullOrEmpty(_s.History.Operand):
                        _s.Buffer.Value = _s.Input.Value;
                        _s.Input.Value = _s.History.Operand;
                        continue;
                    case (false, true):
                        _s.Input.Value = _s.Buffer.Value;
                        continue;
                    case (false, false) when !string.IsNullOrEmpty(_s.Operation) || !string.IsNullOrEmpty(_s.History.Operation):
                        var operation = _s.Operation ?? _s.History.Operation;
                        if (!_s.Input.IsOutput) _s.History.Operand = _s.Input.Value;
                        _s.Input.Value = BinaryActionReducer(_s.Buffer.Value, _s.Input.Value, operation);
                        _s.History.Operation = operation;
                        _s.UserInput = _s.Input.Value;
                        _s.Input.IsOutput = true;
                        _s.Buffer = new Operand();
                        _s.Operation = null;
                        break;
                }

                break;
            }
        }
    }
}