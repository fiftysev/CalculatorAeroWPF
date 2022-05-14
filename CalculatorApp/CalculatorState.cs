using System.Collections.Generic;

namespace CalculatorApp
{
    public struct CalculatorState
    {
        public double LeftOperand;
        public string RightOperand;
        public string Operation;
        public InputState CurrentInput;
    }

    public struct InputState
    {
        public string Value;
        public bool IsModifiedByUnary;
    }

    public struct LastOperationHistory
    {
        public string Operand;
        public string Operation;
    }
}