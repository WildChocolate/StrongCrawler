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
        
        /// <summary>
        /// 高级爬虫
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="script"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public async Task Start(Uri uri, Script script, Operation operation)
        {
            await Task.Run(() => { 
                if (OnStrart != null)
                    this.OnStrart(this, new OnStartEventArgs(uri));
                var driver = new PhantomJSDriver( _options);

                try{
                    driver.Navigate().GoToUrl(uri);
                    var watch = DateTime.Now;
                    if (script != null)
                        driver.ExecuteScript(script.Code, script.Args);
                    if (operation.Action != null)
                        operation.Action.Invoke(driver);
                    var driverWait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(operation.TimeOut));
                    if (operation.Condition != null)
                        driverWait.Until(operation.Condition);
                    var ThreadId = Thread.CurrentThread.ManagedThreadId;
                    var milliseconds = DateTime.Now.Subtract(watch).Milliseconds;
                    var pageSoure = driver.PageSource;
                    if (this.OnCompleted != null)
                        this.OnCompleted(this, new OnCompletedEventArgs(uri, ThreadId, milliseconds, pageSoure, driver));
                }
                catch (Exception ex)
                {
                    if (this.OnError != null)
                        this.OnError(this, new OnErrorEventArgs(uri, ex));
                }
                finally
                {
                    driver.Close();
                    driver.Quit();
                }
            });
        }
    }
}
