using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;


namespace Consola.Tests
{
    public partial class Tests
    {
        private void History_printStarter()
        {
            Action<int> set = i =>
            {
                cout(String.Format("i = {0}", i));
                cout(Keys.Enter);
            };

            for (int i = 1; i < 4; i++) set(i);
        }


        [TestMethod, TestCategory("History")]
        public void SimplePreviousTest()
        {
            History_printStarter();

            cout(Keys.Up); // i = 3
            cout(Keys.Enter);

            Assert.AreEqual("i = 3", getLineText(4));
        }

        [TestMethod, TestCategory("History")]
        public void UpperBoundryTest()
        {
            History_printStarter();

            cout(Keys.Up); // i = 3
            cout(Keys.Up); // i = 2
            cout(Keys.Up); // i = 1
            cout(Keys.Up); // i = 1
            cout(Keys.End); // move cursor to end of line (temp fix)

            cout(Keys.Enter);

            Assert.AreEqual("i = 1", getLineText(4));
        }

        [TestMethod, TestCategory("History")]
        public void EditLineTest()
        {
            History_printStarter();

            cout(Keys.Up); // i = 3
            cout(Keys.Up); // i = 2
            cout(Keys.Backspace); // i =
            cout(Keys.Down); // i = 3

            cout(Keys.Enter);

            Assert.AreEqual("i = 3", getLineText(4));
        }
    }
}