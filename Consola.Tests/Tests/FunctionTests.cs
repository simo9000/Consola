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
        [TestMethod]
        public void ListObjectsTest()
        {
            cout("ListObjects()");
            cout(Keys.Enter);
            
            Assert.AreEqual("proxy.BasicFunctions", getLineText(2));
            Assert.AreEqual("proxy.CreatorFunctions", getLineText(3));
        }

        [TestMethod]
        public void BasicFunctions_showTest()
        {
            cout("proxy.BasicFunctions.show()");
            cout(Keys.Enter);
            string expectation = @"BasicFunctions:
                                   --------------
                                   Properties:
                                       Name String Basic Name field
                                   Methods:
                                       Void printMessage() Prints Hello World
                                       Void download(String content) Downloads file
                                       Void show() Inherited: Displays info about class members";
            var result = getLineText(2);
            showCompare(expectation, result);
        }

        [TestMethod]
        public void BasicFunctions_getSetName()
        {
            cout("proxy.BasicFunctions.Name = 'steve'");
            cout(Keys.Enter);
            cout("print(proxy.BasicFunctions.Name)");
            cout(Keys.Enter);

            Assert.AreEqual("steve", getLineText(3));
        }

        [TestMethod]
        public void BasicFunctions_printMessage()
        {
            cout("proxy.BasicFunctions.printMessage()");
            cout(Keys.Enter);

            Assert.AreEqual("Hello World", getLineText(2));
        }

        [TestMethod]
        public void BasicFunctions_downloadTextFile()
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

        private string expectedProgenyShow = @"Progeny:
                                               -------
                                               Properties:
                                                    Name String Simple Name Field
                                               Methods:
                                                    Void show() Inherited: Displays info about class members";

        [TestMethod]
        public void CreatorFunctions_CreateProgeny()
        {
            cout("progeny = proxy.CreatorFunctions.CreateProgeny()");
            cout(Keys.Enter);
            cout("progeny.show()");
            cout(Keys.Enter);
            var expected = @"Progeny:
                             -------
                             Properties:
                                 Name String Simple Name Field
                             Methods:
                                 Void show() Inherited: Displays info about class members";
            var result = getLineText(3);
            showCompare(expected, result);

        }

        [TestMethod]
        public void CreatorFunctions_LazyCreateProgeny()
        {
            cout("proxy.CreatorFunctions.LazyProgeny.show()");
            cout(Keys.Enter);
            var result = getLineText(2);
            showCompare(expectedProgenyShow, result);
        }
       
    }
}
