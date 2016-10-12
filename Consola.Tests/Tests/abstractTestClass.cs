using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy.Hosting.Self;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Consola.SelfHost;
using System.Collections.ObjectModel;
using System.Text;

namespace Tests
{
    [TestClass]
    public abstract class abstractTestClass
    {
        protected static PhantomJSDriver browser;
        protected static Uri hostLocation = new Uri("http://localhost:80");

        

        [TestInitialize]
        public void test_start()
        {
            browser.Navigate().GoToUrl(hostLocation + "Console");
        }

        protected static ReadOnlyCollection<LogEntry> jsErrors
        {
            get { return getErrors(LogType.Browser); }
        }

        protected static ReadOnlyCollection<LogEntry> header
        {
            get { return getErrors("har"); }
        }

        private static ReadOnlyCollection<LogEntry> getErrors(string type)
        {
            ILogs logs = browser.Manage().Logs;
            return logs.GetLog(type);
        }

        protected string genMessage(string message)
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

        protected string getLineClass(int lineNumber)
        {
            return String.Format("LN{0}", lineNumber);
        }

        protected static void startServer()
        {
            Host.start();           
        }

        protected static void startBrowser()
        {
            browser = new PhantomJSDriver();
            browser.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));
        }
    }
}
