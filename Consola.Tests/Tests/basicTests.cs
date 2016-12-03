using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;


namespace Consola.Tests
{
    public partial class Tests
    {
        [TestMethod, TestCategory("Basic Tests")]
        public void pageLoadTest()
        {
            ReadOnlyCollection<IWebElement> promptLines = browser.FindElementsByClassName("consola");
            Assert.IsTrue(promptLines.Count == 1, genMessage("Prompt not found"));
        }

        [TestMethod, TestCategory("Basic Tests")]
        public void simplePrintTest()
        {
            cout("print('test')");
            cout(Keys.Enter);

            Assert.AreEqual("test", getLineText(2));
        }

        [TestMethod, TestCategory("Basic Tests")]
        public void simpleMathTest() {
            cout("i = 1 + 1");
            cout(Keys.Enter);
            cout("print(i)");
            cout(Keys.Enter);

            Assert.AreEqual("2", getLineText(3));
        }
    }
}
