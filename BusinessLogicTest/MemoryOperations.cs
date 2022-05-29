using CalculatorApp.Controllers;
using NUnit.Framework;

namespace BusinessLogicTest
{
    [TestFixture]
    public class MemoryOperations
    {
        private CalculatorController _c;

        [SetUp]
        public void SetUp()
        {
            _c = new CalculatorController();
        }

        [Test]
        public void MemorySaveAndRead()
        {
            _c.Dispatch("5");
            _c.Dispatch("MS");
            _c.Dispatch("CE");
            _c.Dispatch("MR");

            Assert.AreEqual("5", _c.UiText);
        }

        [Test]
        public void MemorySaveAndPlus()
        {
            _c.Dispatch("5");
            _c.Dispatch("MS");
            _c.Dispatch("CE");
            _c.Dispatch("9");
            _c.Dispatch("M+");
            _c.Dispatch("MR");

            Assert.AreEqual("14", _c.UiText);
        }

        [Test]
        public void MemorySaveAndMinus()
        {
            _c.Dispatch("5");
            _c.Dispatch("MS");
            _c.Dispatch("CE");
            _c.Dispatch("9");
            _c.Dispatch("M-");
            _c.Dispatch("MR");

            Assert.AreEqual("-4", _c.UiText);
        }

        [Test]
        public void ClearDataAndReadFromMemory()
        {
            _c.Dispatch("5");
            _c.Dispatch("MS");
            _c.Dispatch("C");
            Assert.AreEqual("0", _c.UiText);
            
            _c.Dispatch("MR"); 
            Assert.AreEqual("5", _c.UiText);
        }
    }
}