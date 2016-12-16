using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;


namespace Consola.Tests
{
    public partial class Tests
    {
        private const string BASIC_TESTS = "Basic Tests";

        [TestMethod, TestCategory(BASIC_TESTS)]
        public void pageLoadTest()
        {
            ReadOnlyCollection<IWebElement> promptLines = browser.FindElementsByClassName("consola");
            Assert.IsTrue(promptLines.Count == 1, genMessage("Prompt not found"));
        }

        [TestMethod, TestCategory(BASIC_TESTS)]
        public void simplePrintTest()
        {
            cout("print('test')");
            cout(Keys.Enter);

            Assert.AreEqual("test", getLineText(2));
        }

        [TestMethod, TestCategory(BASIC_TESTS)]
        public void simpleMathTest() {
            cout("i = 1 + 1");
            cout(Keys.Enter);
            cout("print(i)");
            cout(Keys.Enter);

            Assert.AreEqual("2", getLineText(3));
        }

        [TestMethod, TestCategory(BASIC_TESTS)]
        public void listTest()
        {
            cout("list = List[int]([1,2])");
            cout(Keys.Enter);
            cout("list.Add(3)");
            cout(Keys.Enter);
            cout(@"print(list[2])");
            cout(Keys.Enter);

            Assert.AreEqual("3", getLineText(4));
        }

        [TestMethod, TestCategory(BASIC_TESTS)]
        public void longCommandTest()
        {
            string printString = new string('a', 2048);
            cout(String.Format("print('{0}')", printString));
            cout(Keys.Enter);
            string line2 = getLineText(2);
            Assert.AreEqual(printString, line2);
        }
    }
}
