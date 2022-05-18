using System.Collections.Generic;

namespace CalculatorApp
{
    public struct CalculatorState
    {
        public string RightOperand;
        public Stack<string> Operands;
        public Stack<string> Operations;
        public InputState CurrentInput;
        public History History;
    }

    public struct InputState
    {
        public string Value;
        public bool IsModifiedByUnary;
    }

    public struct History
    {
        public string Operand;
        public string Operation;
    }
}