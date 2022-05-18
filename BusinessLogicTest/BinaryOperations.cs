using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using CalculatorApp;

namespace BusinessLogicTest
{
    [TestFixture]
    public class BinaryOperations
    {
        private CalculatorState s;

        [SetUp]
        public void SetUp()
        {
            s = new CalculatorState
            {
                Operands = new Stack<string>(),
                Operations = new Stack<string>(),
                RightOperand = "0"
            };
        }

        [Test]
        public void WithoutOutput()
        {
            CalculatorController.Dispatcher(ref s, "1");
            CalculatorController.Dispatcher(ref s, "2");
            CalculatorController.Dispatcher(ref s, "+");

            Assert.AreEqual("12", s.Operands.Peek());
            Assert.AreEqual("+", s.Operations.Peek());
            Assert.AreEqual("12", s.CurrentInput.Value);

            CalculatorController.Dispatcher(ref s, "1");
            CalculatorController.Dispatcher(ref s, "1");

            Assert.AreEqual("11", s.RightOperand);
        }

        [Test]
        public void WithoutOutputWithOperationChanging()
        {
            CalculatorController.Dispatcher(ref s, "1");
            CalculatorController.Dispatcher(ref s, "2");
            CalculatorController.Dispatcher(ref s, "+");
            CalculatorController.Dispatcher(ref s, "-");
            CalculatorController.Dispatcher(ref s, "/");

            Assert.AreEqual("12", s.Operands.Peek());
            Assert.AreEqual("/", s.Operations.Peek());
            Assert.AreEqual("12", s.CurrentInput.Value);

            CalculatorController.Dispatcher(ref s, "1");
            CalculatorController.Dispatcher(ref s, "1");

            Assert.AreEqual("11", s.RightOperand);
        }

        [Test]
        public void TwoOperationsWithoutOutput()
        {
            CalculatorController.Dispatcher(ref s, "1");
            CalculatorController.Dispatcher(ref s, "+");

            Assert.AreEqual("1", s.Operands.Peek());
            Assert.AreEqual("+", s.Operations.Peek());
            Assert.IsNull(s.RightOperand);

            CalculatorController.Dispatcher(ref s, "1");
            CalculatorController.Dispatcher(ref s, "2");
            CalculatorController.Dispatcher(ref s, "-");

            Assert.AreEqual("13", s.Operands.Peek());
            Assert.AreEqual("-", s.Operations.Peek());
        }

        [Test]
        public void ManyOperationsWithoutOutput()
        {
            CalculatorController.Dispatcher(ref s, "1");
            CalculatorController.Dispatcher(ref s, "+");

            Assert.AreEqual("1", s.Operands.Peek());
            Assert.AreEqual("+", s.Operations.Peek());
            Assert.IsNull(s.RightOperand);

            CalculatorController.Dispatcher(ref s, "1");
            CalculatorController.Dispatcher(ref s, "2");
            CalculatorController.Dispatcher(ref s, "-");

            Assert.AreEqual("13", s.Operands.Peek());
            Assert.AreEqual("-", s.Operations.Peek());

            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, "4");

            Assert.AreEqual("3,4", s.RightOperand);

            CalculatorController.Dispatcher(ref s, "+");

            Assert.AreEqual("9,6", s.Operands.Peek());
        }

        [Test]
        public void WithoutOutputTwoOperationsWithOperationChanging()
        {
            CalculatorController.Dispatcher(ref s, "1");
            CalculatorController.Dispatcher(ref s, "+");

            Assert.AreEqual("1", s.Operands.Peek());
            Assert.AreEqual("+", s.Operations.Peek());
            Assert.IsNull(s.RightOperand);

            CalculatorController.Dispatcher(ref s, "1");
            CalculatorController.Dispatcher(ref s, "2");
            CalculatorController.Dispatcher(ref s, "-");
            CalculatorController.Dispatcher(ref s, "+");

            Assert.AreEqual("13", s.Operands.Peek());
            Assert.AreEqual("+", s.Operations.Peek());

            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, "4");

            Assert.AreEqual("3,4", s.RightOperand);

            CalculatorController.Dispatcher(ref s, "+");

            Assert.AreEqual("16,4", s.Operands.Peek());
        }

        [Test]
        public void WithOutput()
        {
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, "+");
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, "=");

            Assert.AreEqual("6", s.Operands.Peek());
            Assert.IsEmpty(s.Operations);
            Assert.AreEqual("6", s.CurrentInput.Value.ToString());
            Assert.IsNull(s.RightOperand);
        }

        [Test]
        public void WithOutputWithOperationChanging()
        {
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, "+");
            CalculatorController.Dispatcher(ref s, "/");
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, "=");

            Assert.AreEqual("1", s.Operands.Peek());
            Assert.IsEmpty(s.Operations);
            Assert.AreEqual("1", s.CurrentInput.Value.ToString());
            Assert.IsNull(s.RightOperand);
        }

        [Test]
        public void ManyOperationsWithOutput()
        {
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, "+");
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, "=");

            Assert.AreEqual("6", s.Operands.Peek());
            Assert.IsEmpty(s.Operations);
            Assert.AreEqual("6", s.CurrentInput.Value.ToString());
            Assert.IsNull(s.RightOperand);
            
            CalculatorController.Dispatcher(ref s, "*");
            CalculatorController.Dispatcher(ref s, "2");
            CalculatorController.Dispatcher(ref s, "=");
            
            Assert.AreEqual("12", s.Operands.Peek());
            Assert.IsEmpty(s.Operations);
        }

        [Test]
        public void ManyOperationsWithOutputWithoutSecondOperand()
        {
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, "+");
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, "=");
            
            Assert.AreEqual("6", s.Operands.Peek());
            
            CalculatorController.Dispatcher(ref s, "=");
            
            Assert.AreEqual("9", s.Operands.Peek());
            
            CalculatorController.Dispatcher(ref s, "=");
            
            Assert.AreEqual("12", s.Operands.Peek());
        }
    }
}