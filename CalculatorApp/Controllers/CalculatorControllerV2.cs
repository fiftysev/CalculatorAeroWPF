using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CalculatorApp.Models;

namespace CalculatorApp.Controllers
{
    public class CalculatorController
    {
        private CalculatorState _s;
        public string UiText => _s.UserInput;
        public string Log => string.Join(" ", _s.Log.Reverse().ToArray());

        public CalculatorController()
        {
            _s = new CalculatorState{Log = new Stack<string>()};
        }

        public CalculatorController(ref CalculatorState state)
        {
            _s = state;
        }

        public void ResetState()
        {
            _s = new CalculatorState();
        }

        /// <summary>
        /// Main method in controller, does matching of operation from button with BL (like FLUX arch)
        /// </summary>
        /// <param name="payload"></param>
        public void Dispatch(in string payload)
        {
            switch (Utils.GetOperationType(payload))
            {
                case Utils.CalculatorOperationType.Digit:
                    if (_s.Input.Value is null or "0" || _s.Input.IsOutput || _s.Input.IsModifiedByUnary)
                    {
                        _s.Input.Value = payload;
                        _s.Log.Clear();
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
                            
                            _s.Log.Push(_s.Input.Value);
                            _s.Log.Push(_s.Operation);
                            
                            _s.Input.Value = null;
                            break;
                        case (false, true):
                            if (_s.LastInputInLogIsBinaryOperation() && _s.Log.Peek() != payload)
                            {
                                _s.Log.Pop();
                                _s.Log.Push(payload);
                            }
                            _s.Operation = payload;
                            break;
                        case (false, false):
                            if (_s.Operation is not null)
                            {
                                _s.Buffer.Value =
                                    BinaryActionReducer(_s.Buffer.Value, _s.Input.Value, _s.Operation);
                            }

                            _s.Operation = payload;
                            _s.Log.Push(_s.Input.Value);
                            _s.Log.Push(payload);

                            _s.UserInput = _s.Buffer.Value;
                            _s.Input.Value = null;
                            break;
                    }

                    break;
                case Utils.CalculatorOperationType.Unary:
                    if (string.IsNullOrEmpty(_s.Input.Value) && !string.IsNullOrEmpty(_s.Buffer.Value))
                        _s.Input.Value = _s.Buffer.Value;
                    if (!string.IsNullOrEmpty(_s.Input.Value))
                    {
                        var logValue = _s.Log.Count > 0 && !_s.LastInputInLogIsBinaryOperation()
                            ? _s.Log.Pop()
                            : _s.Input.Value;
                        var wrapForLog = Utils.WrapUnaryOperationForLogging(logValue, payload);
                        if (!string.IsNullOrEmpty(wrapForLog)) _s.Log.Push(wrapForLog);
                        _s.Input.Value = UnaryActionReducer(_s.Input.Value, payload);
                    }
                        
                    _s.UserInput = _s.Input.Value;
                    _s.Input.IsModifiedByUnary = true;
                    break;
                case Utils.CalculatorOperationType.Percent:
                    if (!string.IsNullOrEmpty(_s.Buffer.Value) && string.IsNullOrEmpty(_s.Input.Value) && !string.IsNullOrEmpty(_s.Operation))
                        _s.Input.Value = _s.Buffer.Value;
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
                case Utils.CalculatorOperationType.ErrorType:
                    break;
            }
        }

        /// <summary>
        /// Method for process binary math operations
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="payload"></param>
        /// <returns>Result of binary operation</returns>
        /// <exception cref="InvalidOperationException">If try divide by zero</exception>
        private static string BinaryActionReducer(in string num1, in string num2, in string payload)
        {
            double res = 0;
            double.TryParse(num1, out var first);
            double.TryParse(num2, out var second);
            if (payload.Equals("/") && second.Equals(0))
                throw new InvalidOperationException("Деление на ноль невозможно");
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

        /// <summary>
        /// Method for process unary math operations
        /// </summary>
        /// <param name="num"></param>
        /// <param name="payload"></param>
        /// <returns>Result of binary operation</returns>
        /// <exception cref="InvalidOperationException">If try divide by zero or sqrt negative num</exception>
        private static string UnaryActionReducer(in string num, in string payload)
        {
            double res = 0;
            double.TryParse(num, out var number);
            switch (payload)
            {
                case "1/x" when number.Equals(0):
                    throw new InvalidOperationException("Деление на ноль невозможно");
                case "√" when number < 0:
                    throw new InvalidOperationException("Недопустимый ввод");
                default:
                    res = payload switch
                    {
                        "√" => Math.Sqrt(number),
                        "±" => -number,
                        "1/x" => Math.Pow(number, -1),
                        _ => res
                    };
                    return res.ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Method only for get percent of number
        /// </summary>
        /// <param name="num"></param>
        /// <param name="percents"></param>
        /// <returns></returns>
        private static string PercentActionReducer(in string num, in string percents)
        {
            double.TryParse(num, out var number);
            double.TryParse(percents, out var per);
            return ((number / 100) * per).ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Method for process memory operations
        /// </summary>
        /// <param name="action"></param>
        private void MemoryActionReducer(in string action)
        {
            switch (action)
            {
                case "MS":
                    _s.Memory = _s.UserInput;
                    _s.Input.Value = null;
                    break;
                case "MR":
                    if(_s.Memory is not null) _s.Input.Value = _s.UserInput = _s.Memory;
                    break;
                case "MC":
                    _s.Memory = null;
                    break;
                case "M+":
                    if (_s.Memory is null) return;
                    _s.Memory =
                        (Convert.ToDouble(_s.Memory) + Convert.ToDouble(_s.UserInput)).ToString(CultureInfo
                            .CurrentCulture);
                    _s.Input.Value = null;
                    break;
                case "M-":
                    if (_s.Memory is null) return;
                    _s.Memory =
                        (Convert.ToDouble(_s.Memory) - Convert.ToDouble(_s.UserInput)).ToString(CultureInfo
                            .CurrentCulture);
                    _s.Input.Value = null;
                    break;
            }
        }

        /// <summary>
        /// Method for clear user input and calculator model data
        /// </summary>
        /// <param name="action"></param>
        private void DispatchClearInputAction(in string action)
        {
            switch (action)
            {
                case "🠔":
                    if (string.IsNullOrEmpty(_s.Input.Value) || _s.Input.Value.Equals("0")) return;
                    _s.Input.Value = _s.Input.Value.Remove(_s.Input.Value.Length - 1);
                    if (string.IsNullOrEmpty(_s.Input.Value)) _s.Input.Value = "0";
                    _s.UserInput = _s.Input.Value;
                    break;
                case "C":
                    _s.Buffer = new Operand();
                    _s.History = new History();
                    _s.Log.Clear();
                    _s.Operation = null;
                    _s.Input.Value = _s.UserInput = "0";
                    break;
                case "CE":
                    _s.Input.Value = _s.UserInput = "0";
                    break;
            }
        }

        /// <summary>
        /// Method for operation = logic
        /// </summary>
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
                    case (false, false) when !string.IsNullOrEmpty(_s.Operation) ||
                                             !string.IsNullOrEmpty(_s.History.Operation):
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
                _s.Log.Clear();
                break;
            }
        }
    }
}