﻿using System.Collections.Generic;

namespace CalculatorApp
{
    public struct CalculatorState
    {
        public double LeftOperand;
        public string RightOperand;
        public string Operation;
        public Stack<string> Operands;
        public Stack<string> Operations;
        public InputState CurrentInput;
    }

    public struct InputState
    {
        public string Value;
        public bool IsModifiedByUnary;
    }

}