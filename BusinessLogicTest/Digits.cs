using System;
using System.Collections.Generic;
using NUnit.Framework;
using CalculatorApp;

namespace BusinessLogicTest
{
    [TestFixture]
    public class DigitsAdding
    {
        private CalculatorController _c;

        [SetUp]
        public void SetUp()
        {
            _c = new CalculatorController();
        }

        [Test]
        public void SingleDigit()
        {
            _c.Dispatch("1");
            Assert.AreEqual("1", _c.UiText);
        }

        [Test]
        public void ManyDigits()
        {
            _c.Dispatch("2");
            _c.Dispatch("3");
            _c.Dispatch("4");
            Assert.AreEqual("234", _c.UiText);
        }

        [Test]
        public void WithSingleFloatingPoint()
        {
            _c.Dispatch("3");
            _c.Dispatch(",");
            _c.Dispatch("3");
            Assert.AreEqual("3,3", _c.UiText);
        }

        [Test]
        public void SingleFloatingPoint()
        {
            _c.Dispatch(",");
            Assert.AreEqual("0,", _c.UiText);
        }

        [Test]
        public void WithManyFloatingPoints()
        {
            _c.Dispatch("3");
            _c.Dispatch(",");
            _c.Dispatch(",");
            _c.Dispatch("4");
            _c.Dispatch(",");
            Assert.AreEqual("3,4", _c.UiText);
        }

        [Test]
        public void ManyFloatingPoints()
        {
            _c.Dispatch(",");
            _c.Dispatch(",");
            _c.Dispatch(",");
            Assert.AreEqual("0,", _c.UiText);
        }

        [Test]
        public void WithLeadingZeros()
        {
            _c.Dispatch("0");
            _c.Dispatch("0");
            _c.Dispatch("0");
            _c.Dispatch("3");
            Assert.AreEqual("3", _c.UiText);
        }
    }
}