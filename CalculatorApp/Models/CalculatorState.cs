using System;
using System.Collections.Generic;
using System.Globalization;

namespace CalculatorApp.Models
{
    public struct CalculatorState
    {
        public Operand Input;
        public Operand Buffer;
        public string Operation;
       
        public string UserInput;
        public string Memory;
        public History History;
        
        public Stack<string> Log;

        public bool LastInputInLogIsBinaryOperation()
        {
            return Utils.GetOperationType(Log.Peek()) == Utils.CalculatorOperationType.Binary;
        }
    }

    public struct History
    {
        public string Operand;
        public string Operation;
    }

    public struct Operand
    {
       public string Value;
       public bool IsModifiedByUnary;
       public bool IsOutput;
    }
}