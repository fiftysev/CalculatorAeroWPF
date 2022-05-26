using NUnit.Framework;
using CalculatorApp.Controllers;
using CalculatorApp.Models;

namespace BusinessLogicTest
{
    [TestFixture]
    public class BinaryOperations
    {
        private CalculatorController _c;
        private CalculatorState _s;

        [SetUp]
        public void SetUp()
        {
            _s = new CalculatorState();
            _c = new CalculatorController(ref _s);
        }

        [Test]
        public void WithoutOutput()
        {
            _c.Dispatch("1");
            _c.Dispatch("2");
            _c.Dispatch("+");
            _c.Dispatch("4");
            _c.Dispatch("-");

            Assert.AreEqual("16", _c.UiText);
            Assert.IsNull(_s.Input.Value);
        }

        [Test]
        public void WithoutOutputWithOperationChanging()
        {
            _c.Dispatch("1");
            _c.Dispatch("2");
            _c.Dispatch("+");
            _c.Dispatch("*");
            
            Assert.AreEqual("*", _s.Operation);
            
            _c.Dispatch("4");
            _c.Dispatch("/");

            Assert.AreEqual("48", _c.UiText);
        }

        [Test]
        public void ManyOperationsWithoutOutput()
        {
            _c.Dispatch("1");
            _c.Dispatch("2");
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
            
            Assert.IsNull(_s.Operation);
            Assert.IsNull(_s.Buffer.Value);
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