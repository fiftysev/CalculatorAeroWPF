using CalculatorApp.Controllers;
using NUnit.Framework;

namespace BusinessLogicTest
{
    [TestFixture]
    public class OutputCasesTest
    {
        private CalculatorController _c;

        [SetUp]
        public void SetUp()
        {
            _c = new CalculatorController();
        }

        [Test]
        public void WithoutAnyData()
        {
            _c.Dispatch("=");

            Assert.AreEqual("0", _c.UiText);
        }

        [Test]
        public void WithSingleOperandAndOperation()
        {
            _c.Dispatch("4");
            _c.Dispatch("+");
            _c.Dispatch("=");

            Assert.AreEqual("8", _c.UiText);
        }

        [Test]
        public void ManyTimesWithSingleOperandAndOperation()
        {
            _c.Dispatch("4");
            _c.Dispatch("+");
            _c.Dispatch("=");
            _c.Dispatch("=");
            _c.Dispatch("=");

            Assert.AreEqual("16", _c.UiText);
        }

        [Test]
        public void WithTwoOperands()
        {
            _c.Dispatch("5");
            _c.Dispatch("+");
            _c.Dispatch("6");
            _c.Dispatch("=");

            Assert.AreEqual("11", _c.UiText);
        }


        [Test]
        public void ManyTimesWithTwoOperands()
        {
            _c.Dispatch("5");
            _c.Dispatch("+");
            _c.Dispatch("6");
            _c.Dispatch("=");
            _c.Dispatch("=");
            _c.Dispatch("=");

            Assert.AreEqual("23", _c.UiText);
        }

        [Test]
        public void InputAfterOutput()
        {
            _c.Dispatch("5");
            _c.Dispatch("+");
            _c.Dispatch("6");
            _c.Dispatch("=");
            
            _c.Dispatch("12");
            
            Assert.AreEqual("12", _c.UiText);
        }
    }
}