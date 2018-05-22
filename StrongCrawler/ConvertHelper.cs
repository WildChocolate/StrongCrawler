using OpenQA.Selenium;
using StrongCrawler.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongCrawler
{
    public static class ConvertHelper
    {
        
        public static HotelDetail ConvertWebeleToHotel(IWebDriver HtmlDoc)
        {
            var hotel = new HotelDetail();
            hotel.HotelName=HtmlDoc.FindElement(By.XPath("//*[@id='J_htl_info']/div[2]/h2[1]")).Text;
            //*[@id="J_htl_info"]/div[4]
            var addrs = HtmlDoc.FindElements(By.XPath("//*[@id='J_htl_info']/div[position()<4]"));
            foreach (var addr in addrs)
            {
                hotel.Address += addr.Text+" ";
            }
            //*[@id="div_minprice"]/p[2]/span[2]
            hotel.Price = HtmlDoc.FindElement(By.XPath("//*[@id='div_minprice']/p[@class='staring_price']/span[2]")).Text;
            hotel.Score = HtmlDoc.FindElement(By.XPath("//*[@id='base_bd']/div[4]/div[2]/div[1]/div/a/p[@class='s_row']/span[@class='score']")).Text;
            //*[@id="base_bd"]/div[4]/div[2]/div[1]/div/a/span[2]
            hotel.Sumary = HtmlDoc.FindElement(By.XPath("//*[@id='base_bd']/div[4]/div[2]/div[1]/div/a/span[@class='commnet_num']")).Text;
            var commentList = HtmlDoc.FindElement(By.Id("commentList"));
            hotel.Comments = ConvertWebeleToCmt(commentList);
            hotel.Pager = ConvertWebeleToPager(commentList);
            hotel.Pager.Count = hotel.Comments.Count;
            return hotel;
        }
        public static List<Comment> ConvertWebeleToCmt(IWebElement commentList) {
            var list = new List<Comment>();
            //*[@id="divCtripComment"]/div[3]四姑娘
            //*[@id="divCtripComment"]/div[4]北京
            var cmts = commentList.FindElements(By.XPath("//*[@id='divCtripComment']/div[@class='comment_detail_list']/div"));
            foreach (var item in cmts)
            {
                //var details = item.Text.Split('\n');
                var cmt = new Comment {
                    Name = item.FindElement(CommentXPath.Name).Text,
                    Score = item.FindElement(CommentXPath.Score).Text,
                    Type = item.FindElement(CommentXPath.Type).Text,
                    LiveTime = item.FindElement(CommentXPath.LiveTime).Text,
                    Content = item.FindElement(CommentXPath.Content).Text,
                    PublishDate = item.FindElement(CommentXPath.PublishDate).Text
                };
                list.Add(cmt);
            }
            return list;
        }
        public static Pager ConvertWebeleToPager(IWebElement commentList)
        {
            var pageInfo = commentList.FindElement(By.ClassName("c_page"));
            
            var down = Convert.ToInt32(pageInfo.FindElement(By.XPath("./a[last()]")).GetAttribute("value"));
            var total = Convert.ToInt32( pageInfo.FindElement(By.XPath("./div/a[last()]")).Text);  
            var pager = new Pager {
                Previous = down-2,
                Next = down,
                TotalPage = total
            };
            return pager;
        }
    }
}
