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

        [TestMethod, TestCategory("List Test")]
        public void listTest()
        {
            cout("list = List[int]([1,2])");
            cout(Keys.Enter);
            cout("list.Add(3)");
            cout(Keys.Enter);
            cout(@"for item in list:
                    print(item)");
            cout(Keys.Enter);

            for (int i = 1; i < 4; i++)
                Assert.AreEqual(i.ToString(), getLineText(i + 4));
        }
    }
}
