using System;
using NUnit.Framework;
using CalculatorApp;

namespace BusinessLogicTest
{
    [TestFixture]
    public class DigitsAdding
    {
        [Test]
        public void SingleDigit()
        {
            var s = new CalculatorState();
            CalculatorController.Dispatcher(ref s, "1");
            Assert.AreEqual("1", s.RightOperand);
        }

        [Test]
        public void ManyDigits()
        {
            var s = new CalculatorState();
            CalculatorController.Dispatcher(ref s, "2");
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, "4");
            Assert.AreEqual("234", s.RightOperand);
        }

        [Test]
        public void WithSingleFloatingPoint()
        {
            var s = new CalculatorState();
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, "4");
            Assert.AreEqual("3,4", s.RightOperand);
        }

        [Test]
        public void SingleFloatingPoint()
        {
            var s = new CalculatorState();
            CalculatorController.Dispatcher(ref s, ",");
            Assert.AreEqual("0,", s.RightOperand);
        }
        

        [Test]
        public void WithManyFloatingPoints()
        {
            var s = new CalculatorState();
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, "4");
            CalculatorController.Dispatcher(ref s, ",");
            Assert.AreEqual("3,4", s.RightOperand);
        }

        [Test]
        public void ManyFloatingPoints()
        {
            var s = new CalculatorState();
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, ",");
            Assert.AreEqual("0,", s.RightOperand);
        }

        [Test]
        public void WithLeadingZeros()
        {
            var s = new CalculatorState();
            CalculatorController.Dispatcher(ref s, "0");
            CalculatorController.Dispatcher(ref s, "0");
            CalculatorController.Dispatcher(ref s, "0");
            CalculatorController.Dispatcher(ref s, "3");
            Assert.AreEqual("3", s.RightOperand);
        }
    }
}