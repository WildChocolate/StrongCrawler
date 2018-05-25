using OpenQA.Selenium;
using StrongCrawler.Model;
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
        StringBuilder mainBuilder = new StringBuilder();
        StringBuilder commentBuiler = new StringBuilder();
        StrongCrawler hotelCrawler = new StrongCrawler();
        StrongCrawler commentCrawler = new StrongCrawler();
        By previous = By.XPath("//*[@id='divCtripComment']/div[5]/div/a[class='c_up']");
        By next = By.XPath("//*[@id='divCtripComment']/div[5]/div/a[class='c_down']");
        HotelDetail hd = new HotelDetail();
        public Form1()
        {
            InitializeComponent();
            kvs.Add(new KeyValuePair<string, string>("北京华滨国际大酒店", "http://hotels.ctrip.com/hotel/434938.html"));
            hotelCrawler.OnStrart += hotelCrawler_OnStrart;
            hotelCrawler.OnError += hotelCrawler_OnError;
            hotelCrawler.OnCompleted += hotelCrawler_OnCompleted;
        }
        List<Hotel> hotelList = new List<Hotel>();
        List<City> cityList = new List<City>();
        List<KeyValuePair<string, string>> kvs = new List<KeyValuePair<string, string>>();
        
        bool forComment = false;
        private async void button1_Click(object sender, EventArgs e)
        {
            this.searchBtn.Enabled = false;
            this.searchBtn.Text = "请等待...";
            if (hotelCrawler.Driver != null)
            {
                hotelCrawler.Driver.Close();
                hotelCrawler.Driver.Quit();
            }
            
            //var hotelUrl = "http://hotels.ctrip.com/hotel/434938.html";
            Uri hotelUrl =null;
            if (HotelListBox.SelectedValue == null)
            {
                hotelUrl = hotelList[0].Uri;
            }
            else
                hotelUrl = HotelListBox.SelectedValue as Uri;
            
            var operation = new Operation
            {
                Action = (x) =>
                {
                    var s = x.PageSource;
                    
                    x.FindElement(By.XPath(@"//*[@id='commentTab']")).Click();
                },
                Condition = (x) => {
                    var s = x.PageSource;
                    var cmtLisgt = x.FindElement(By.Id("commentList"));
                    return cmtLisgt.Displayed && cmtLisgt.Displayed && !cmtLisgt.Text.Contains("点评载入中");
                },
                TimeOut = 1500
            };
            await hotelCrawler.Start(hotelUrl, null, operation, false);
            this.searchBtn.Enabled=true;
            this.searchBtn.Text = "查看";
            this.skipBtn.Enabled = true;
        }

        void hotelCrawler_OnCompleted(object sender, OnCompletedEventArgs e)
        {
           HotelCrawler(e);
        }
        private void loadHotelInfo()
        {
            refreshMainInfo();
            refreshComments(hd.Comments);
            this.Invoke(new Action(() =>
            {
                DetailTxt.Text = mainBuilder.ToString()+commentBuiler.ToString();
            }));
        }
        private void HotelCrawler(OnCompletedEventArgs e)
        {
            hd = ConvertHelper.ConvertWebeleToHotel(e.driver);
            loadHotelInfo();
            CheckBtnStatus();
        }
        private void refreshMainInfo()
        {
            mainBuilder.Clear();
            mainBuilder.AppendLine("名称：" + hd.HotelName);
            mainBuilder.AppendLine("地址：" + hd.Address);
            mainBuilder.AppendLine("价格:" + hd.Price);
            mainBuilder.AppendLine("数量" + hd.Sumary);
            mainBuilder.AppendLine("============================" + Environment.NewLine);
            mainBuilder.AppendLine(hd.Pager.ToString());
            mainBuilder.AppendLine("============================" + Environment.NewLine);
            mainBuilder.AppendLine("点评内容" + Environment.NewLine);
        }
        private void refreshComments(IList<Comment> Comments)
        {
            commentBuiler.Clear();
            foreach (var comment in Comments)
            {
                commentBuiler.AppendLine(string.Empty);
                commentBuiler.AppendLine("顾客：" + comment.Name + "     评分：" + comment.Score);
                commentBuiler.AppendLine("房间类型：" + comment.Type);
                commentBuiler.AppendLine("入住时间：" + comment.LiveTime + "       " + comment.PublishDate);
                commentBuiler.AppendLine("评价：" + comment.Content);
                commentBuiler.Append(string.Empty);
            }
            
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
                this.cityListBox.DataSource = cityList;
                cityListBox.SelectedIndex = 0;
                cityListBox_DoubleClick(this, null);
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

        private async void button2_Click_1(object sender, EventArgs e)
        {
            this.previousBtn.Enabled = this.nextBtn.Enabled = false;
            var driver = hotelCrawler.Driver;
            driver.FindElement(By.XPath("//*[@id='divCtripComment']/div[@class='c_page_box']/div/a[@class='c_up']")).Click();
            var tab = driver.FindElement(By.Id("commentTab"));
            while (!tab.Displayed)
            {
                await Task.Delay(100);
                tab = driver.FindElement(By.Id("commentTab"));
            }
            tab.Click();
            await Task.Delay(100);
            forComment = !forComment;
            var s = driver.PageSource;
            var comments = ConvertHelper.ConvertWebeleToCmt(driver.FindElement(By.Id("commentList")));
            hd.Comments = comments;
            hd.Pager.Previous--;
            hd.Pager.Next--;
            loadHotelInfo();
            CheckBtnStatus();
        }

        void commentCrawler_OnError(object sender, OnErrorEventArgs e)
        {
            Console.WriteLine("抓取错误："+e.ex);
        }

        void commentCrawler_OnStrart(object sender, OnStartEventArgs e)
        {
            Console.WriteLine("开始抓取评论："+e.Uri);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            this.previousBtn.Enabled=this.nextBtn.Enabled = false;
            var driver = hotelCrawler.Driver;
            driver.FindElement(By.XPath("//*[@id='divCtripComment']/div[@class='c_page_box']/div/a[@class='c_down']")).Click();
            var tab = driver.FindElement(By.Id("commentTab"));
            while (!tab.Displayed)
            {
                await Task.Delay(100);
                tab = driver.FindElement(By.Id("commentTab"));
            }
            tab.Click();
            await Task.Delay(100);
            forComment = !forComment;
            var s = driver.PageSource;
            var comments = ConvertHelper.ConvertWebeleToCmt(driver.FindElement(By.Id("commentList")));
            hd.Comments = comments;
            hd.Pager.Previous++;
            hd.Pager.Next++;
            loadHotelInfo();
            CheckBtnStatus();
        }
        private void CheckBtnStatus()
        {
            var pager = hd.Pager;
            this.Invoke(new Action(() => { 
                if (pager.CurrentPage == pager.TotalPage)
                {
                    nextBtn.Enabled = false;
                }
                else
                {
                    nextBtn.Enabled = true ;
                }
                if (pager.CurrentPage == 1)
                {
                    previousBtn.Enabled = false;
                }
                else
                {
                    previousBtn.Enabled = true;
                }
                if (pager.TotalPage == 0)
                {
                    previousBtn.Enabled = false;
                    nextBtn.Enabled = false;
                }
            }));
            
        }

        private void skipTxtbox_Leave(object sender, EventArgs e)
        {
            var val = skipTxtbox.Text;
            var page=0;
            if (int.TryParse(val, out page))
            {
                if (page < 1)
                {
                    page = 1;
                }
                else if (page > hd.Pager.TotalPage)
                    page = hd.Pager.TotalPage;
            }
            else
                page = hd.Pager.CurrentPage;
            skipTxtbox.Text = page.ToString();
        }

        private async void skipBtn_Click(object sender, EventArgs e)
        {
            this.skipBtn.Enabled = false;
            var page = skipTxtbox.Text;
            var driver = hotelCrawler.Driver;
            var pageBox = driver.FindElementById("cPageNum");
            pageBox.SendKeys(page);
            var confirmBtn = driver.FindElementById("cPageBtn");
            confirmBtn.Click();
            var tab = driver.FindElement(By.Id("commentTab"));
            while (!tab.Displayed)
            {
                await Task.Delay(100);
                tab = driver.FindElement(By.Id("commentTab"));
            }
            tab.Click();
            await Task.Delay(100);
            while (tab.Text.Contains("点评载入中"))
            {
                await Task.Delay(100);
            }
            var s = driver.PageSource;
            var comments = ConvertHelper.ConvertWebeleToCmt(driver.FindElementById("commentTab"));
            hd.Comments = comments;
            hd.Pager.Previous = int.Parse(page)-1;
            hd.Pager.Next= int.Parse(page)+1;
            loadHotelInfo();
            CheckBtnStatus();
            this.skipBtn.Enabled = true;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            var val = citySearchTxt.Text;
            var bs = new BindingSource();
            if (string.IsNullOrWhiteSpace(val))
            {
                bs.DataSource = cityList;
            }
            else
            {
                var city_copy = cityList.FindAll(c =>
                {
                    return c.CityName.Contains(val);
                });
                bs.DataSource = city_copy;
            }
            cityListBox.DataSource = bs;
        }

        

        private async void cityListBox_DoubleClick(object sender, EventArgs e)
        {
            await Task.Delay(300);
            var cityUrl = cityListBox.SelectedValue as Uri;
            var cityName = cityListBox.Text;
            var hotelListCrawler = new StrongCrawler();
            hotelListCrawler.OnStrart += delegate
            {
                Console.WriteLine("开始搜索" + cityName);
            };
            hotelListCrawler.OnError += (s, arg) =>
            {
                Console.WriteLine("搜索发生错误：" + arg.ex);
            };
            hotelListCrawler.OnCompleted += hotelListCrawler_OnCompleted;
            this.cityListBox.Enabled = false;
            await hotelListCrawler.Start(cityUrl, null, new Operation());
            this.cityListBox.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            hotelCrawler.Driver.Close();
            hotelCrawler.Driver.Quit();
        }

        private void HotelListBox_DataSourceChanged(object sender, EventArgs e)
        {
            HotelListBox.Select();
            button1_Click(sender, e);
        }

        private void citySearchTxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == System.Convert.ToChar(13))
            {
                e.Handled = true;
                var idx = this.cityListBox.SelectedIndex;
                if (idx >-1)
                {
                    //MessageBox.Show(cityListBox.SelectedIndex+"");
                    cityListBox_DoubleClick(sender, e);
                }
                else
                    return;
            }
        }
    }
}
