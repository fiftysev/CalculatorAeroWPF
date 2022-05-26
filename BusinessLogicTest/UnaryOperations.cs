using CalculatorApp.Controllers;
using CalculatorApp.Models;
using NUnit.Framework;

namespace BusinessLogicTest
{
    [TestFixture]
    public class UnaryOperations
    {
        private CalculatorController _c;

        [SetUp]
        public void SetUp()
        {
            _c = new CalculatorController();
        }

        [Test]
        [TestCase("1", "1")]
        [TestCase("5", "0,2")]
        public void Reciprocal(string num, string expected)
        {
            _c.Dispatch(num);
            _c.Dispatch("1/x");

            Assert.AreEqual(expected, _c.UiText);
        }

        [Test]
        [TestCase("9", "3")]
        [TestCase("4", "2")]
        [TestCase("5", "2,23606797749979")]
        public void Sqrt(string num, string expected)
        {
            _c.Dispatch(num);
            _c.Dispatch("√");
            
            Assert.AreEqual(expected, _c.UiText);
        }

        [Test]
        [TestCase("5", "-5")]
        [TestCase("-5", "5")]
        public void Negate(string num, string expected)
        {
           _c.Dispatch(num);
           _c.Dispatch("±");
           
           Assert.AreEqual(expected, _c.UiText);
        }

        [Test]
        public void GetPercentWithoutLeftOperand()
        {
            _c.Dispatch("5");
            _c.Dispatch("%");
            
            Assert.AreEqual("0", _c.UiText);
        }

        [Test]
        public void GetPercentWithRightOperand()
        {
            _c.Dispatch("1");
            _c.Dispatch("0");
            _c.Dispatch("0");
            _c.Dispatch("+");
            _c.Dispatch("1");
            _c.Dispatch("0");
            _c.Dispatch("%");
            
            Assert.AreEqual("10", _c.UiText);
        }

        [Test]
        public void InputAfterOperation()
        {
            _c.Dispatch("4");
            _c.Dispatch("√");
            _c.Dispatch("12");
            
            Assert.AreEqual("12", _c.UiText);
        }
    }
}