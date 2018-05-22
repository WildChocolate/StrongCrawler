using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongCrawler.Model
{
    public class HotelDetail
    {
        public string HotelName
        {
            get;
            set;
        }
        public string Sumary { get; set; }
        public string Address { get; set; }
        public string Price { get; set; }
        public string Score { get; set; }
        public IList<Comment> Comments { get; set; }
        public Pager Pager { get; set; }
    }
}
