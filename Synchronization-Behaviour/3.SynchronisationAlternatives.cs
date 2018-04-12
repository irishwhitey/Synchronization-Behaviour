using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Synchronization_Challenge
{
    [TestFixture]
    class SynchronizationTradeOffs
    {
        [Test]
        public void LocksAreSlowThough()
        {
            var tasks = new List<Task>();
            var theClass = new SingleCountingClass();
            var numberOfWritingLocks = 10;
            var numberOfReadingLocks = 10;
            var usingLockTiming = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                for (int i = 0; i < numberOfWritingLocks; i++)
                {
                    tasks.Add(Task.Run(() => theClass.AddOneWithLock()));
                }
                for (int i = 0; i < numberOfReadingLocks; i++)
                {
                    tasks.Add(Task.Run(() => theClass.GetCounterWithLock()));
                }
                tasks.ForEach(t => t.Wait());
            });
            var minimumTime = (numberOfWritingLocks * theClass.DelayBetweenReadAndWrite) +
                              (numberOfReadingLocks * theClass.DelayBetweenReadAndWrite);
            Assert.That(usingLockTiming, Is.GreaterThanOrEqualTo(minimumTime));
        }

        [Test]
        public void SeparateReadAndWriteIsBetter()
        {
            var tasks = new List<Task>();
            var theClass = new SingleCountingClass();
            var numberOfWritingLocks = 10;
            var numberOfReadingLocks = 10;
            var usingReaderWriterSlimLockTiming = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                for (int i = 0; i < numberOfWritingLocks; i++)
                {
                    tasks.Add(Task.Run(() => theClass.AddOneUsingReaderWriteSlim()));
                }
                for (int i = 0; i < numberOfReadingLocks; i++)
                {
                    tasks.Add(Task.Run(() => theClass.GetCounterUsingReaderWriteSlim()));
                }
                tasks.ForEach(t => t.Wait());
            });
            var minimumTime = (numberOfWritingLocks * theClass.DelayBetweenReadAndWrite) +
                              (numberOfReadingLocks * theClass.DelayBetweenReadAndWrite);
            Assert.That(usingReaderWriterSlimLockTiming, Is.LessThanOrEqualTo(minimumTime));
        }
    }
}
