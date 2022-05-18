using System;
using System.Collections.Generic;
using NUnit.Framework;
using CalculatorApp;

namespace BusinessLogicTest
{
    [TestFixture]
    public class DigitsAdding
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
        public void SingleDigit()
        {
            CalculatorController.Dispatcher(ref s, "1");
            Assert.AreEqual("1", s.RightOperand);
        }

        [Test]
        public void ManyDigits()
        {
            CalculatorController.Dispatcher(ref s, "2");
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, "4");
            Assert.AreEqual("234", s.RightOperand);
        }

        [Test]
        public void WithSingleFloatingPoint()
        {
            CalculatorController.Dispatcher(ref s, "3");
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, "4");
            Assert.AreEqual("3,4", s.RightOperand);
        }

        [Test]
        public void SingleFloatingPoint()
        {
            CalculatorController.Dispatcher(ref s, ",");
            Assert.AreEqual("0,", s.RightOperand);
        }


        [Test]
        public void WithManyFloatingPoints()
        {
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
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, ",");
            CalculatorController.Dispatcher(ref s, ",");
            Assert.AreEqual("0,", s.RightOperand);
        }

        [Test]
        public void WithLeadingZeros()
        {
            CalculatorController.Dispatcher(ref s, "0");
            CalculatorController.Dispatcher(ref s, "0");
            CalculatorController.Dispatcher(ref s, "0");
            CalculatorController.Dispatcher(ref s, "3");
            Assert.AreEqual("3", s.RightOperand);
        }
    }
}