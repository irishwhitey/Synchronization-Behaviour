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
        private int _magicTimeInMilliseconds = 5200;

        [Test]
        public void WaitingOnAsyncStuffIsExpensive()
        {
            var gg = new GoogleGetter();
            var noAwaitTime = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                var length = gg.GetResponseContentType();
                Thread.Sleep(5000);
            });
            Assert.That(noAwaitTime, Is.GreaterThanOrEqualTo(_magicTimeInMilliseconds));

        }

        //[Test]
        //public void NotWaitingOnAsyncStuffIsBetter()
        //{
        //    var gg = new GoogleGetter();
        //    var noAwaitTime = TimingHelper.TimeToRunInMilliseconds(() =>
        //    {
        //        var task = gg.GetResponseContentTypeAsync();
        //        Thread.Sleep(5000);
        //        var p = task.Result;

        //    });
        //    Assert.That(noAwaitTime, Is.LessThanOrEqualTo(_magicTimeInMilliseconds));
        //}

        //[Test]
        //public void CallingResultIsJustSynchronous()
        //{
        //    var gg = new GoogleGetter();
        //    var noAwaitTime = TimingHelper.TimeToRunInMilliseconds(() =>
        //    {
        //        var length = gg.GetResponseContentTypeAsync().Result;
        //        Thread.Sleep(5000);

        //    });
        //    Assert.That(noAwaitTime, Is.GreaterThanOrEqualTo(_magicTimeInMilliseconds));

        //}
    }

    public class GoogleGetter
    {
        public string GetResponseContentType()
        {
            WebResponse resp = null;
            var wr = CreateWebRequest("http://www.google.co.uk/");
            try
            {
                resp = wr.GetResponse();
            }
            catch (Exception e)
            {

            }

            return resp.ContentType;
        }

        private static WebRequest CreateWebRequest(string url)
        {
            var wr = WebRequest.Create(url);
            wr.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Reload);
            return wr;
        }

        public async Task<string> GetResponseContentTypeAsync()
        {
            WebResponse resp = null;
            var wr = CreateWebRequest("http://www.google.com/");
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
