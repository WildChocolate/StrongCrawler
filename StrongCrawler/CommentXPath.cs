using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongCrawler
{
    public static class CommentXPath
    {
        static CommentXPath()
        {
            //*[@id='divCtripComment']/div[4]/div
            //*[@id="divCtripComment"]/div[3]/div[1]/div[1]/p[2]/span
            Name = By.XPath("div[1]/p[@class='name']/span");
            Score = By.XPath("div[2]/p/span[2]/span");
            Type=By.XPath("div[2]/p/a");
            LiveTime=By.XPath("div[2]/p/span[3]");
            Content = By.XPath("div[2]/div[@class='comment_txt']/div[@class='J_commentDetail']");
            //*[@id="divCtripComment"]/div[3]/div[15]/div[2]/div[2]/div[3]/p/span
            PublishDate = By.XPath("div[2]/div[@class='comment_txt']/div[@class='comment_bar']/p/span");
        }
        public static By Name
        {
            get;
            private set;
        }
        public static By Score
        {
            get;
            private set;
        }
        public static By Type
        {
            get;
            private set;
        }
        public static By LiveTime
        {
            get;
            private set;
        }
        public static By Content
        {
            get;
            private set;
        }
        public static By PublishDate
        {
            get;
            private set;
        }
    }
}
