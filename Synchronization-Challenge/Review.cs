using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Synchronization_Challenge
{
    [TestFixture]
    class Review
    {
        [Test]
        public void ASeparateThreadShouldBeQuicker()
        {
            var sut = new HasLongRunningOperations();
            var noThreadTime = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                sut.ThreeSecondOperation();
                sut.TwoSecondOperation();
            });

            var withThreadTime = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                var t = new Thread(sut.ThreeSecondOperation);
                t.Start();
                sut.TwoSecondOperation();
                t.Join();
            });
            Assert.That(withThreadTime, Is.LessThan(noThreadTime));
            Assert.That(withThreadTime, Is.LessThan(TimeSpan.FromSeconds(4).TotalMilliseconds));
        }
    }
}
