using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrongCrawler
{
    public class Operation
    {
        public Action<IWebDriver> Action { get; set; }


        public Func<IWebDriver, bool> Condition;

        public double TimeOut { get; set; }
    }
}
