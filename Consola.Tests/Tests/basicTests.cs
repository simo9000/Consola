using Consola.SelfHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Collections.ObjectModel;


namespace Consola.Tests
{
    public partial class Tests
    {
        [TestMethod]
        public void pageLoadTest()
        {
            ReadOnlyCollection<IWebElement> promptLines = browser.FindElementsByClassName("spaceHolder");
            Assert.IsTrue(promptLines.Count == 1, genMessage("Prompt not found"));
        }

        [TestMethod]
        public void simplePrintTest()
        {
            cout("print('test')");
            cout(Keys.Enter);

            Assert.AreEqual("test", getLineText(2));
        }

        [TestMethod]
        public void simpleMathTest() {
            cout("i = 1 + 1");
            cout(Keys.Enter);

            browser.FindElementByClassName(getLineClass(1));

            cout("print(i)");
            cout(Keys.Enter);

            Assert.AreEqual("2", getLineText(3));
        }
    }
}
