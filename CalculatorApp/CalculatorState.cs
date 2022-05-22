using System.Collections.Generic;

namespace CalculatorApp
{
    public struct CalculatorState
    {
        public string RightOperand;
        public CurrentInput CurrentInput;
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

    public struct CurrentInput
    {
       public string Value;
       public bool IsModifiedByUnary;
       public bool IsOutput;
    }
}