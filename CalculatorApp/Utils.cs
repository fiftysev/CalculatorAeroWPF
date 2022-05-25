using System.Collections.Generic;

namespace CalculatorApp
{
   public static class Utils
   {
      public enum CalculatorOperationType
      {
         Binary,
         Unary,
         Percent,
         ClearData,
         Memory,
         Output,
         Digit,
         FloatingPoint
      }

      public static Dictionary<string, CalculatorOperationType> TypesMap =
         new Dictionary<string, CalculatorOperationType>
         {
            {"1", CalculatorOperationType.Digit},
            {"2", CalculatorOperationType.Digit},
            {"3", CalculatorOperationType.Digit},
            {"4", CalculatorOperationType.Digit},
            {"5", CalculatorOperationType.Digit},
            {"6", CalculatorOperationType.Digit},
            {"7", CalculatorOperationType.Digit},
            {"8", CalculatorOperationType.Digit},
            {"9", CalculatorOperationType.Digit},
            {"0", CalculatorOperationType.Digit},
            {",", CalculatorOperationType.FloatingPoint},
            {".", CalculatorOperationType.FloatingPoint},
            {"+", CalculatorOperationType.Binary},
            {"*", CalculatorOperationType.Binary},
            {"/", CalculatorOperationType.Binary},
            {"-", CalculatorOperationType.Binary},
            {"1/x", CalculatorOperationType.Unary},
            {"√", CalculatorOperationType.Unary},
            {"±", CalculatorOperationType.Unary},
            {"%", CalculatorOperationType.Percent},
            {"MC", CalculatorOperationType.Memory},
            {"MR", CalculatorOperationType.Memory},
            {"MS", CalculatorOperationType.Memory},
            {"M+", CalculatorOperationType.Memory},
            {"M-", CalculatorOperationType.Memory},
            {"C", CalculatorOperationType.ClearData},
            {"CE", CalculatorOperationType.ClearData},
            {"=", CalculatorOperationType.Output}
         };
   } 
}