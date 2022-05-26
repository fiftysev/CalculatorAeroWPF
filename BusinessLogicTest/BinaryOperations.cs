using NUnit.Framework;
using CalculatorApp.Controllers;
using CalculatorApp.Models;

namespace BusinessLogicTest
{
    [TestFixture]
    public class BinaryOperations
    {
        private CalculatorController _c;

        [SetUp]
        public void SetUp()
        {
            _c = new CalculatorController();
        }

        [Test]
        public void WithoutOutput()
        {
            _c.Dispatch("12");
            _c.Dispatch("+");
            _c.Dispatch("4");
            _c.Dispatch("-");

            Assert.AreEqual("16", _c.UiText);
        }

        [Test]
        public void WithoutOutputWithOperationChanging()
        {
            _c.Dispatch("12");
            _c.Dispatch("+");
            _c.Dispatch("*");
            
            _c.Dispatch("4");
            _c.Dispatch("/");

            Assert.AreEqual("48", _c.UiText);
        }

        [Test]
        public void ManyOperationsWithoutOutput()
        {
            _c.Dispatch("12");
            _c.Dispatch("+");
            _c.Dispatch("-");
            _c.Dispatch("/");
            _c.Dispatch("*");
            _c.Dispatch("4");
            _c.Dispatch("-");

            Assert.AreEqual("48", _c.UiText);

            _c.Dispatch("5");
            _c.Dispatch("/");

            Assert.AreEqual("43", _c.UiText);
        }

        [Test]
        public void WithOutput()
        {
            _c.Dispatch("6");
            _c.Dispatch("+");
            _c.Dispatch("3");
            _c.Dispatch("=");

            Assert.AreEqual("9", _c.UiText);
        }

        [Test]
        public void WithOutputWithOperationChanging()
        {
            _c.Dispatch("6");
            _c.Dispatch("+");
            _c.Dispatch("-");
            _c.Dispatch("/");
            _c.Dispatch("3");
            _c.Dispatch("=");

            Assert.AreEqual("2", _c.UiText);
        }

        [Test]
        public void ManyOperationsWithOutput()
        {
            _c.Dispatch("6");
            _c.Dispatch("+");
            _c.Dispatch("3");
            _c.Dispatch("=");
            
            Assert.AreEqual("9", _c.UiText);
            
            _c.Dispatch("/");
            _c.Dispatch("3");
            _c.Dispatch("=");

            Assert.AreEqual("3", _c.UiText);
        }
    }
}