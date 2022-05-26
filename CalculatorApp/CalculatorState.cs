using System;
using System.Collections.Generic;
using System.Globalization;

namespace CalculatorApp
{
    public struct CalculatorState
    {
        public Operand Input;
        public Operand Buffer;
        public string Operation;
        
        public string UserInput
        {
            get => _userInput;
            set => _userInput = Convert.ToDouble(value).ToString("#,#", CultureInfo.CurrentCulture);
        }
        public string Memory;
        public History History;
        private string _userInput;
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