
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
         FloatingPoint,
         ErrorType
      }

      public static CalculatorOperationType GetOperationType(string payload)
      {
         var type = payload switch
         {
            _ when double.TryParse(payload, out _) => CalculatorOperationType.Digit,
            _ when "+-/*".Contains(payload) => CalculatorOperationType.Binary,
            _ when "√±1/x".Contains(payload) => CalculatorOperationType.Unary,
            "%" => CalculatorOperationType.Percent,
            _ when payload.Contains("M") => CalculatorOperationType.Memory,
            _ when payload.Contains("C") => CalculatorOperationType.ClearData,
            _ when ".,".Contains(payload) => CalculatorOperationType.FloatingPoint,
            "=" => CalculatorOperationType.Output,
            _ => CalculatorOperationType.ErrorType
         };

         return type;
      }

      public static string WrapUnaryOperationForLogging(string operand, string operation)
      {
         var res = operation switch
         {
            "1/x" => $"reciproc({operand})",
            "√" => $"sqrt({operand})",
            "±" => $"negate({operand}",
            _ => ""
         };

         return res;
      }
   } 
}