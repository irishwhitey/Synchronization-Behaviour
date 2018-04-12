using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Synchronization_Challenge
{
    [TestFixture]
    class Asynchronization
    {
        [Test]
        public void WaitingOnAsyncStuffIsExpensive()
        {
            var gg = new GoogleGetter();
            var noAwaitTime = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                var length = gg.GetResponseContentType();
                Thread.Sleep(5000);
            });
            Assert.That(noAwaitTime, Is.GreaterThanOrEqualTo(6000));
        }

        [Test]
        public void NotWaitingOnAsyncStuffIsBetter()
        {
            var gg = new GoogleGetter();
            var noAwaitTime = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                var task = gg.GetResponseContentTypeAsync();
                Thread.Sleep(5000);
                var p = task.Result;

            });
            Assert.That(noAwaitTime, Is.LessThanOrEqualTo(6000));
        }
    }

    public class GoogleGetter
    {
        public string GetResponseContentType()
        {
            WebResponse resp = null;
            var wr = CreateWebRequest();
            try
            {
                resp = wr.GetResponse();
            }
            catch (Exception e)
            {
                
            }
            
            return resp.ContentType;
        }

        private static WebRequest CreateWebRequest()
        {
            var wr = WebRequest.Create("http://www.google.com/");
            wr.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Reload);
            return wr;
        }

        public async Task<string> GetResponseContentTypeAsync()
        {
            WebResponse resp = null;
            var wr = CreateWebRequest();
            try
            {
                resp = await wr.GetResponseAsync();
            }
            catch (Exception e)
            {
                
            }
            return resp.ContentType;
        }
    }
}
