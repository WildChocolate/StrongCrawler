using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrongCrawler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            kvs.Add(new KeyValuePair<string, string>("北京华滨国际大酒店", "http://hotels.ctrip.com/hotel/434938.html"));
        }
        List<Hotel> hotelList = new List<Hotel>();
        List<City> cityList = new List<City>();
        List<KeyValuePair<string, string>> kvs = new List<KeyValuePair<string, string>>();
        StringBuilder mailBuilder = new StringBuilder();
        StringBuilder commentBuiler = new StringBuilder();
        private async void button1_Click(object sender, EventArgs e)
        {
            var hotelUrl = "http://hotels.ctrip.com/hotel/434938.html";
            hotelUrl = HotelListBox.SelectedValue.ToString();
            hotelUrl = hotelUrl.Split('?')[0];
            var hotelCrawler = new StrongCrawler();
            hotelCrawler.OnStrart += hotelCrawler_OnStrart;
            hotelCrawler.OnError += hotelCrawler_OnError;
            hotelCrawler.OnCompleted += hotelCrawler_OnCompleted;
            var operation = new Operation
            {
                Action = (x) =>
                {
                    var s = x.PageSource;
                    
                    x.FindElement(By.XPath(@"//*[@id='commentTab']")).Click();
                },
                Condition = (x) => {
                    var s = x.PageSource;
                    return x.FindElement(By.XPath("//*[@id='commentList']")).Displayed && x.FindElement(By.XPath("//*[@id='commentList']")).Displayed
                        && !x.FindElement(By.XPath("//*[@id='commentList']")).Text.Contains("点评载入中");
                },
                TimeOut = 1500
            };
            await hotelCrawler.Start(new Uri(hotelUrl), null, operation);
        }

        void hotelCrawler_OnCompleted(object sender, OnCompletedEventArgs e)
        {
            HotelCrawler(e);
        }

        private void HotelCrawler(OnCompletedEventArgs e)
        {
            var hotel = ConvertHelper.ConvertWebeleToHotel(e.driver);
            mailBuilder.Clear();
            commentBuiler.Clear();
            mailBuilder.AppendLine("名称：" + hotel.HotelName);
            mailBuilder.AppendLine("地址：" + hotel.Address);
            mailBuilder.AppendLine("价格:" + hotel.Price);
            mailBuilder.AppendLine("数量" + hotel.Sumary);
            mailBuilder.AppendLine(hotel.Pager.ToString());
            mailBuilder.AppendLine("===========================================" + Environment.NewLine);
            mailBuilder.AppendLine("点评内容" + Environment.NewLine);
            foreach (var comment in hotel.Comments)
            {
                commentBuiler.AppendLine(string.Empty);
                commentBuiler.AppendLine("顾客："+comment.Name+ "     评分：" + comment.Score);
                commentBuiler.AppendLine("房间类型：" + comment.Type );
                commentBuiler.AppendLine("入住时间：" + comment.LiveTime + "       "+comment.PublishDate);
                commentBuiler.AppendLine("评价：" + comment.Content);
                commentBuiler.Append(string.Empty);
            }
            this.Invoke(new Action(() => {
                DetailTxt.Text = mailBuilder.ToString()+commentBuiler.ToString();
            }));
        }

        void hotelCrawler_OnError(object sender, OnErrorEventArgs e)
        {
            Console.WriteLine("爬虫抓取出现错误："+e.uri+",异常消息："+e.ex.ToString());
        }

        void hotelCrawler_OnStrart(object sender, OnStartEventArgs e)
        {
            Console.WriteLine("爬虫开始抓取地址"+e.Uri);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var testresult = string.Empty;
            using (var stream = File.OpenText(@"testResult.html"))
            {
                testresult = stream.ReadToEnd();
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var hotelsCrawler = new StrongCrawler();
            hotelsCrawler.OnStrart += hotelsCrawler_OnStrart;
            hotelsCrawler.OnError += hotelsCrawler_OnError;
            hotelsCrawler.OnCompleted += hotelsCrawler_OnCompleted;
            var cityUrl = "http://hotels.ctrip.com/cityList";
            Operation op = new Operation ();

            await hotelsCrawler.Start(new Uri(cityUrl), null, op);
        }

        void hotelsCrawler_OnCompleted(object sender, OnCompletedEventArgs e)
        {
            var s = e.pageSoure;
            ExtractCitys(e);
        }

        private void ExtractCitys(OnCompletedEventArgs e)
        {
            var matches = Regex.Matches(e.pageSoure, @"<a[^>]+href=""*(?<href>/hotel/[^>\s]+)""\s*[^>]*>(?<text>(?!.*img).*?)</a>");
            cityList.Clear();
            foreach (Match m in matches)
            {
                var city = new City
                {
                    CityName = m.Groups["text"].Value,
                    Uri = new Uri("http://hotels.ctrip.com" + m.Groups["href"].Value)
                };
                if (!cityList.Contains(city))
                    cityList.Add(city);
            }
            
            Console.WriteLine("城市抓取完成！");
            Console.WriteLine("耗时：" + e.milliseconds + "毫秒");
            Console.WriteLine("线程:" + e.ThreadId);
            Console.WriteLine("地址:" + e.uri.ToString());
            this.Invoke(new Action(() => {
                //this.cityListBox.Items.AddRange(items.ToArray());
                this.cityListBox.DataSource = cityList;
            }));
        }

        void hotelsCrawler_OnError(object sender, OnErrorEventArgs e)
        {
            Console.WriteLine("抓取错误"+e.uri);
            Console.WriteLine(e.ex);
        }

        void hotelsCrawler_OnStrart(object sender, OnStartEventArgs e)
        {
            Console.WriteLine("开始抓取酒店");
        }

        private async void cityListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            var cityUrl = cityListBox.SelectedValue as Uri;
            var cityName = cityListBox.Text;
            var hotelListCrawler = new StrongCrawler();
            hotelListCrawler.OnStrart += delegate{
                Console.WriteLine("开始搜索" + cityName);
            };
            hotelListCrawler.OnError += (s, arg)=>
            {
                Console.WriteLine("搜索发生错误："+arg.ex );
            };
            hotelListCrawler.OnCompleted += hotelListCrawler_OnCompleted;
            
            await hotelListCrawler.Start(cityUrl, null, new Operation());
        }

        void hotelListCrawler_OnCompleted(object sender, OnCompletedEventArgs e)
        {
            Console.WriteLine("酒店搜索完成");
            var matches = Regex.Matches(e.pageSoure, "><a[^>]+href=\"*(?<href>/hotel/[^>\\s]+)\"\\s*data-dopost[^>]*><span[^>]+>.*?</span>(?<text>.*?)</a>");
            hotelList.Clear();
            foreach (Match m in matches)
            {
                var hotel = new Hotel
                {
                    HotelName = m.Groups["text"].Value,
                    Uri = new Uri("http://hotels.ctrip.com" + m.Groups["href"].Value)
                };
                if (!hotelList.Contains(hotel))
                    hotelList.Add(hotel);
            }
            
            HotelListBox.Invoke(new Action(() =>
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = hotelList;
                this.HotelListBox.DataSource = bs;
            }));
        }

        private void HotelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HotelListBox.Text.Length==0)
                return;
            var hotelUrl = HotelListBox.SelectedValue as Uri;

        }
    }
}
