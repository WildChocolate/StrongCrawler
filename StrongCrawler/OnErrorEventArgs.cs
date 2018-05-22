using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrongCrawler
{
    public class OnErrorEventArgs
    {
        public Uri uri { get; set; }
        public Exception ex { get; set; }

        public OnErrorEventArgs(Uri uri, Exception ex)
        {
            // TODO: Complete member initialization
            this.uri = uri;
            this.ex = ex;
        }
    }
}
