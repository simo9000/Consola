using Consola.SelfHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Threading;

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
        }

        [TestMethod]
        public void BasicTests_showTest()
        {
            cout("proxy.BasicFunctions.show()");
            cout(Keys.Enter);

            Assert.AreEqual("BasicFunctions:", getLineText(2));
            Assert.AreEqual("--------------", getLineText(3));
            Assert.AreEqual("Properties:", getLineText(4));
            Assert.AreEqual(" Name (System.String) Basic Name field", getLineText(5));
            Assert.AreEqual("Methods:", getLineText(6));
            Assert.AreEqual(" System.Void printMessage() Prints Hello World", getLineText(7));
            Assert.AreEqual(" System.Void download(System.String content) Downloads file", getLineText(8));
            Assert.AreEqual(" System.Void show() Inherited: Displays info about class members", getLineText(9));
        }

        [TestMethod]
        public void BasicTests_getSetName()
        {
            cout("proxy.BasicFunctions.Name = 'steve'");
            cout(Keys.Enter);
            Thread.Sleep(500); // needed to allow for script env state change
            cout("print(proxy.BasicFunctions.Name)");
            cout(Keys.Enter);

            Assert.AreEqual("steve", getLineText(3));
        }

        [TestMethod]
        public void BasicTests_printMessage()
        {
            cout("proxy.BasicFunctions.printMessage()");
            cout(Keys.Enter);

            Assert.AreEqual("Hello World", getLineText(2));
        }
    }
}
