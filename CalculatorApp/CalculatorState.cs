using System.Collections.Generic;

namespace CalculatorApp
{
    public struct CalculatorState
    {
        public double LeftOperand;
        public string RightOperand;
        public string Operation;
        public string CurrentInput;
        public Stack<string> History;
    }
}