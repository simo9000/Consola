using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using webConsole.SelfHost;

namespace Tests
{
    [TestClass]
    public class basicTests : abstractTestClass
    {
        [ClassInitialize]
        public static void initialize(TestContext context)
        {
            startServer();
            startBrowser();
        }

        [ClassCleanup]
        public static void tearDown()
        {
            Host.stop();
            browser.Close();
        }

        [TestMethod]
        public void pageLoadTest()
        {
            ReadOnlyCollection<IWebElement> promptLines = browser.FindElementsByClassName("spaceHolder");
            Assert.IsTrue(promptLines.Count == 1, genMessage("Prompt not found"));
        }

        [TestMethod]
        public void simplePrintTest()
        {
            IWebElement prompt = browser.FindElementByClassName("command");
            prompt.SendKeys("print('test')");
            prompt.SendKeys(Keys.Enter);
            IWebElement outputLine = browser.FindElementByClassName(getLineClass(2));
            Assert.AreEqual("test", outputLine.Text);
        }

        [TestMethod]
        public void simpleMathTest() {
            IWebElement prompt = browser.FindElementByClassName("command");
            prompt.SendKeys("i = 1 + 1");
            prompt.SendKeys(Keys.Enter);
            browser.FindElementByClassName(getLineClass(1));
            prompt = browser.FindElementByClassName("command");
            prompt.SendKeys("print(i)");
            prompt.SendKeys(Keys.Enter);
            IWebElement outputLine = browser.FindElementByClassName(getLineClass(3));
            Assert.AreEqual("2", outputLine.Text);
        }
    }
}
