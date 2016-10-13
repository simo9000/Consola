using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Consola.SelfHost;

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
            cout("print('test')");
            cout(Keys.Enter);
            IWebElement outputLine = browser.FindElementByClassName(getLineClass(2));
            Assert.AreEqual("test", outputLine.Text);
        }

        [TestMethod]
        public void simpleMathTest() {
            cout("i = 1 + 1");
            cout(Keys.Enter);
            browser.FindElementByClassName(getLineClass(1));
            cout("print(i)");
            cout(Keys.Enter);
            IWebElement outputLine = browser.FindElementByClassName(getLineClass(3));
            Assert.AreEqual("2", outputLine.Text);
        }
    }
}
