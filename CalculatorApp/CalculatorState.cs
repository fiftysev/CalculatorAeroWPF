namespace CalculatorApp
{
    public struct CalculatorState
    {
        public Operand Input;
        public Operand Buffer;
        public string Operation;
        public string UserInput;
        public string Memory;
        public History History;
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