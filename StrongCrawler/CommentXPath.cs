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

            Name = By.XPath("div[1]/p[@class='name']/span");
            //评论有可能是个砖家点评，就没有分数
            //*[@id="divCtripComment"]/div[3]/div[1]/div[2]/p/span[2]/span
            Score = By.XPath("div[@class='comment_main']/p[@class='comment_title']/span[@class='score']/span");
            Type = By.XPath("div[2]/p/a");
            LiveTime = By.XPath("div[2]/p/span[3]");

            //想不通为什么 在http://hotels.ctrip.com/hotel/1553259.html会匹配错误，但在页面里搜索是可以正确定位到元素的
            Content = By.XPath("div[@class='comment_main']/div[@class='comment_txt']/[@class='J_commentDetail']");

            PublishDate = By.XPath("div[@class='comment_main']/div[@class='comment_txt']/div[@class='comment_bar']/p/span[@class='time']");
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
