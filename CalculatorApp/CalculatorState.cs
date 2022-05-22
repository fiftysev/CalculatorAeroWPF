using System.Collections.Generic;

namespace CalculatorApp
{
    public struct CalculatorState
    {
        public string RightOperand;
        public string CurrentInput;
        public string Memory;
        public Stack<string> Operands;
        public Stack<string> Operations;
        public History History;
    }

    public struct History
    {
        public string Operand;
        public string Operation;
    }
}