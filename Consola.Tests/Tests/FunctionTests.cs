using System.Text;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.IO;
using System.Net;

namespace Consola.Tests
{
    public partial class Tests
    {
        [TestMethod, TestCategory("Basic Tests")]
        public void ListObjectsTest()
        {
            cout("ListObjects()");
            cout(Keys.Enter);
            
            Assert.AreEqual("proxy.BasicFunctions", getLineText(2));
            Assert.AreEqual("proxy.CreatorFunctions", getLineText(3));
            Assert.AreEqual("proxy.Child", getLineText(4));
        }

        [TestMethod, TestCategory("Basic Functions")]
        public void ShowTest()
        {
            cout("proxy.BasicFunctions.show()");
            cout(Keys.Enter);
            string expectation = @"BasicFunctions:
                                   --------------
                                   Fields:
                                       Int32 id Basic Id field
                                       Byte? Byte Nullable field
                                       Dictionary<Int32, String> Dictionary Generic field
                                   Properties:
                                       String Name Basic Name property
                                       DateTime? Time Nullable property
                                       IEnumerable<DateTime> Dates Generic property
                                   Methods:
                                       Void printMessage() Prints Hello World
                                       Void printList() Prints List
                                       Void printHtml() Prints Html
                                       Void printHtmlList() Prints Html List
                                       Void download(String content) Downloads file
                                       Int32? getNullableInt(Boolean? b) Method with nullables in signature
                                       Void show() Inherited: Displays info about class members";
            var result = getLineText(2);
            showCompare(expectation, result);
        }

        [TestMethod, TestCategory("Basic Functions")]
        public void GetSetNameTest()
        {
            cout("proxy.BasicFunctions.Name = 'steve'");
            cout(Keys.Enter);
            cout("print(proxy.BasicFunctions.Name)");
            cout(Keys.Enter);

            Assert.AreEqual("steve", getLineText(3));
        }

        [TestMethod, TestCategory("Basic Functions")]
        public void PrintMessage()
        {
            cout("proxy.BasicFunctions.printMessage()");
            cout(Keys.Enter);

            Assert.AreEqual("Hello World", getLineText(2));
        }

        [TestMethod, TestCategory("Basic Functions")]
        public void DownloadTextFile()
        {
            String oracle = "Test";
            browser.ExecuteScript("prompt.testDownload = true;");
            cout(String.Format("proxy.BasicFunctions.download(\"{0}\")",oracle));
            cout(Keys.Enter);
            using (WebResponse response = DownloadFile())
            using(StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                String Contents = reader.ReadToEnd();
                Assert.AreEqual(oracle, Contents);
            }
        }

        [TestMethod, TestCategory("Basic Functions")]
        public void PrintTextList()
        {
            cout("proxy.BasicFunctions.printList()");
            cout(Keys.Enter);

            Assert.AreEqual("Line 1", getLineText(2));
            Assert.AreEqual("Line 2", getLineText(3));
        }

        [TestMethod, TestCategory("Basic Functions")]
        public void PrintHtml()
        {
            cout("proxy.BasicFunctions.printHtml()");
            cout(Keys.Enter);

            IWebElement output = getHtmlLine(2).FindElement(By.CssSelector("span"));
            Assert.AreEqual("text", output.Text);
            Assert.AreEqual("rgba(255, 0, 0, 1)", output.GetCssValue("color"));
        }

        [TestMethod, TestCategory("Basic Functions")]
        public void PrintHtmlList()
        {
            cout("proxy.BasicFunctions.printHtmlList()");
            cout(Keys.Enter);

            IWebElement line1 = getHtmlLine(2).FindElement(By.CssSelector("span"));
            Assert.AreEqual("Line 1", line1.Text);
            Assert.AreEqual("rgba(0, 128, 0, 1)", line1.GetCssValue("color"));
            IWebElement line2 = getHtmlLine(3).FindElement(By.CssSelector("a"));
            Assert.AreEqual("Line 2", line2.Text);
            Assert.AreEqual("http://home.simo9000.com/git", line2.GetAttribute("href"));
        }

        private const string expectedProgenyShow = @"Progeny:
                                               -------
                                               Properties:
                                                    String Name Simple Name Field
                                               Methods:
                                                    Void show() Inherited: Displays info about class members";

        [TestMethod, TestCategory("Creator Functions")]
        public void CreateProgeny()
        {
            cout("progeny = proxy.CreatorFunctions.CreateProgeny()");
            cout(Keys.Enter);
            cout("progeny.show()");
            cout(Keys.Enter);
            var result = getLineText(3);
            showCompare(expectedProgenyShow, result);

        }

        [TestMethod, TestCategory("Creator Functions")]
        public void LazyCreateProgeny()
        {
            cout("proxy.CreatorFunctions.LazyProgeny.show()");
            cout(Keys.Enter);
            var result = getLineText(2);
            showCompare(expectedProgenyShow, result);
        }

        [TestMethod, TestCategory("Child Functions")]
        public void ShowTest_c()
        {
            cout("proxy.Child.show()");
            cout(Keys.Enter);

            var result = @"Child:
                           -----
                           Methods:
                                Void printChild() Child Function
                                Void show() Inherited: Displays info about class members
                                Void printParent() Parent Function";
            showCompare(result, getLineText(2));
        }

        [TestMethod, TestCategory("Child Functions")]
        public void Functions_c()
        {
            cout("proxy.Child.printParent()");
            cout(Keys.Enter);

            Assert.AreEqual("Parent", getLineText(2));

            cout("proxy.Child.printChild()");
            cout(Keys.Enter);

            Assert.AreEqual("Child", getLineText(4));
        }
    }
}
