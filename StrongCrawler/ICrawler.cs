using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongCrawler
{
    interface ICrawler
    {
        event EventHandler<OnStartEventArgs> OnStrart;
        event EventHandler<OnCompletedEventArgs> OnCompleted;
        event EventHandler<OnErrorEventArgs> OnError;

        Task Start(Uri uri, Script script, Operation opration);
    }
}
