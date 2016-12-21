using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy.Hosting.Self;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Consola.SelfHost;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;

namespace Consola.Tests
{
    [TestClass]
    public partial class Tests
    {
        private static PhantomJSDriver browser;
        private static Uri hostLocation = new Uri("http://localhost:80");

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
            browser.Dispose();
        }

        [TestInitialize]
        public void test_start()
        {
            browser.Navigate().GoToUrl(hostLocation + "Console");
        }

        [TestCleanup]
        public void test_end()
        {
            Assert.AreEqual(1, browser.FindElementsByClassName("consolaCommand").Count);
        }

        private ReadOnlyCollection<LogEntry> jsErrors
        {
            get { return getErrors(LogType.Browser); }
        }

        private ReadOnlyCollection<LogEntry> header
        {
            get { return getErrors("har"); }
        }

        private ReadOnlyCollection<LogEntry> getErrors(string type)
        {
            ILogs logs = browser.Manage().Logs;
            return logs.GetLog(type);
        }

        private string genMessage(string message)
        {
            return String.Format("{0}. {1}", message, getJSErrors());
        }

        protected string getJSErrors()
        {
            if (jsErrors.Count > 0)
            {
                StringBuilder messageBuilder = new StringBuilder("JS Log: ");
                foreach (LogEntry entry in jsErrors)
                {
                    messageBuilder.Append(entry.Message + Environment.NewLine);
                }
                return messageBuilder.ToString();
            }
            return String.Empty;
        }

        private string getLineClass(int lineNumber)
        {
            return String.Format("LN{0}", lineNumber);
        }

        private static void startServer()
        {
            Host.start(true);           
        }

        private static void startBrowser()
        {
            browser = new PhantomJSDriver();
            browser.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 5));
        }

        private void cout(string text)
        {
            browser.FindElementByClassName("consolaCommand").SendKeys(text);
        }

        private string getLineText(int lineNumber)
        {
            return browser.FindElementByClassName(getLineClass(lineNumber)).Text;
        }

        private IWebElement getHtmlLine(int lineNumber)
        {
            return browser.FindElementByClassName(getLineClass(lineNumber));
        }

        private void showCompare(string expected, string actual)
        {
            string newlineRegex = "\r\n|\r|\n";
            string[] expectedLines = Regex.Split(expected, newlineRegex );
            string[] actualLines = Regex.Split(actual, newlineRegex );
            Assert.AreEqual(expectedLines.Length, actualLines.Length);
            int lineCount = expectedLines.Length;
            for(int i = 0; i < lineCount; i++)
            {
                Assert.AreEqual(expectedLines[i].Trim(), actualLines[i].Trim());
            }
        }

        private WebResponse DownloadFile()
        {
            string url = browser.FindElementByTagName("iframe").GetAttribute("src");
            WebRequest request = HttpWebRequest.Create(url);
            return request.GetResponse();
        }
    }
}
