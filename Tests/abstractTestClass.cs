using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy.Hosting.Self;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace Tests
{
    [TestClass]
    public class abstractTestClass
    {
        private static NancyHost host;
        private static PhantomJSDriver browser;
        private static Uri hostLocation = new Uri("http://localhost:3589");

        [ClassInitialize]
        public static void initialize(TestContext context)
        {
            host = new NancyHost(hostLocation);
            host.Start();
            browser = new PhantomJSDriver();
            browser.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));
        }
        
        [TestInitialize]
        public void test_start()
        {
            browser.Navigate().GoToUrl(hostLocation + "/Console");
        }

        [TestMethod]
        public void pageLoadTest()
        {
            IWebElement el = browser.FindElementByClassName("spaceHolder");
            var i = 1;
        }
    }
}
