using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrongCrawler
{
    public class OnCompletedEventArgs
    {
        public Uri uri { get; set; }
        public int ThreadId { get; set; }
        public int milliseconds { get; set; }
        public string pageSoure { get; set; }
        public OpenQA.Selenium.IWebDriver driver { get; set; }

        public OnCompletedEventArgs(Uri uri, int ThreadId, int milliseconds, string pageSoure, OpenQA.Selenium.IWebDriver driver)
        {
            // TODO: Complete member initialization
            this.uri = uri;
            this.ThreadId = ThreadId;
            this.milliseconds = milliseconds;
            this.pageSoure = pageSoure;
            this.driver = driver;
        }
    }
}
