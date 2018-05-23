using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrongCrawler
{
    public class StrongCrawler:ICrawler
    {
        public StrongCrawler()
        {
            _options = new PhantomJSOptions();
            _options.AddAdditionalCapability("phantomjs.page.settings.userAgent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.119 Safari/537.36");
            

            _service = PhantomJSDriverService.CreateDefaultService(Environment.CurrentDirectory);
            //不加载图片
            _service.AddArgument("--load-images=false");
            
        }
        private PhantomJSOptions _options;
        private PhantomJSDriverService _service;

        public event EventHandler<OnStartEventArgs> OnStrart;

        public event EventHandler<OnCompletedEventArgs> OnCompleted;

        public event EventHandler<OnErrorEventArgs> OnError;
        public PhantomJSDriver Driver
        {
            get;
            set;
        }
        /// <summary>
        /// 高级爬虫
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="script"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public async Task Start(Uri uri, Script script, Operation operation, bool Closed=true)
        {
            await Task.Run(() => { 
                if (OnStrart != null)
                    this.OnStrart(this, new OnStartEventArgs(uri));
                Driver = new PhantomJSDriver(_options);
                try{
                    Driver.Navigate().GoToUrl(uri);
                    var watch = DateTime.Now;
                    if (script != null)
                        Driver.ExecuteScript(script.Code, script.Args);
                    if (operation.Action != null)
                        operation.Action.Invoke(Driver);
                    var driverWait = new WebDriverWait(Driver, TimeSpan.FromMilliseconds(operation.TimeOut));
                    if (operation.Condition != null)
                        driverWait.Until(operation.Condition);
                    var ThreadId = Thread.CurrentThread.ManagedThreadId;
                    var milliseconds = DateTime.Now.Subtract(watch).Milliseconds;
                    var pageSoure = Driver.PageSource;
                    if (this.OnCompleted != null)
                        this.OnCompleted(this, new OnCompletedEventArgs(uri, ThreadId, milliseconds, pageSoure, Driver));
                }
                catch (Exception ex)
                {
                    if (this.OnError != null)
                        this.OnError(this, new OnErrorEventArgs(uri, ex));
                }
                finally
                {
                    if (Closed)
                    {
                        Driver.Close();
                        Driver.Quit();
                    }
                }
            });
        }
    }
}
