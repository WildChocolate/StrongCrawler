using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrongCrawler.Model
{
    public class Pager
    {
        public int Previous { get; set; }
        public int Next { get; set; }
        public int CurrentPage
        {
            get { return (Next + Previous)/2; }
        }
        public int TotalPage { get; set; }
        public int Count { get; set; }
        public override string ToString()
        {
            return "当前页(" + this.CurrentPage + ") 下一页(" + this.Next + ") 总页数(" + this.TotalPage + ") 每页(" + this.Count + ")";
        }
    }
}
