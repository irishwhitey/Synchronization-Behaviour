using System.Threading;

namespace Synchronization_Challenge
{
    public class SingleCountingClass
    {
        private int _counter;
        private readonly object thelock = new object();
        public int DelayBetweenReadAndWrite = 500;
        private readonly ReaderWriterLockSlim rwls = new ReaderWriterLockSlim();

        public void AddOne()
        {
            var _counterValue = GetCounterDirectlyWithDelay();
            _counter = _counterValue + 1;
        }

        public void AddOneWithLock()
        {
            lock (thelock)
            {
                AddOne();
            }
        }

        public int GetCounterWithLock()
        {
            lock (thelock)
            {
                return GetCounterDirectlyWithDelay();
            }
        }

        private int GetCounterDirectlyWithDelay()
        {
            var tmp = _counter;
            Thread.Sleep(DelayBetweenReadAndWrite);
            return tmp;
        }

        public void AddOneUsingReaderWriteSlim()
        {
            rwls.EnterUpgradeableReadLock();
            var tmp = GetCounterDirectlyWithDelay();
            rwls.EnterWriteLock();
            _counter = tmp + 1;
            rwls.ExitWriteLock();
            rwls.ExitUpgradeableReadLock();
        }

        public void GetCounterUsingReaderWriteSlim()
        {
            rwls.EnterUpgradeableReadLock();
            var tmp = _counter;
            rwls.ExitUpgradeableReadLock();
        }
    }
}