using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy.Hosting.Self;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using webConsole;
using System.Collections.ObjectModel;
using System.Text;

namespace Tests
{
    [TestClass]
    public class abstractTestClass
    {
        private static NancyHost host;
        private static PhantomJSDriver browser;
        private static Uri hostLocation = new Uri("http://localhost:3589");

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

        [ClassInitialize]
        public static void initialize(TestContext context)
        {
            startServer();
            startBrowser();
        }
        
        [TestInitialize]
        public void test_start()
        {
            browser.Navigate().GoToUrl(hostLocation + "Console");
        }

        [TestMethod]
        public void pageLoadTest()
        {
            try
            {
                browser.FindElementByClassName("spaceHolder");
            }
            catch
            {
                var h = header;
                if (jsErrors.Count > 0)
                {
                    StringBuilder messageBuilder = new StringBuilder();
                    foreach (LogEntry entry in jsErrors)
                    {
                        messageBuilder.Append(entry.Message + Environment.NewLine);
                    }
                    Assert.Fail(messageBuilder.ToString());
                }
            }
            
        }

        public static void startServer()
        {
            HostConfiguration hostConfigs = new HostConfiguration();
            hostConfigs.UrlReservations.CreateAutomatically = true;
            host = new NancyHost(hostLocation, new Bootstrapper(), hostConfigs);
            host.Start();
            
        }

        public static void startBrowser()
        {
            browser = new PhantomJSDriver();
            browser.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));
        }
    }
}
