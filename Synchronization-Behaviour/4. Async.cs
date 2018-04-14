using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Synchronization_Challenge
{
    [TestFixture]
    class Asynchronization
    {
        private int codeExecutingTimeInMilliseconds = 100;
        private int _sleepInTest = 5000;

        [Test]
        public void WaitingOnAsyncStuffIsExpensive()
        {
            var fakeTaskFactory = new FakeLongRunningTaskCreator();
            var noAwaitTime = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                var length = fakeTaskFactory.UsingSyncMethodWhichTakesThreeSecondsToComeBack();
                Thread.Sleep(_sleepInTest);
            });
            Assert.That(noAwaitTime, Is.GreaterThanOrEqualTo(_sleepInTest + TimeSpan.FromSeconds(3).TotalMilliseconds));
        }

        [Test]
        public void NotWaitingOnAsyncStuffIsBetter()
        {
            var fakeTaskFactory = new FakeLongRunningTaskCreator();
            var noAwaitTime = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                var task = fakeTaskFactory.GetTaskWhichTakesThreeSecondsToRun();
                Thread.Sleep(_sleepInTest);
                var p = task.Result;

            });
            Assert.That(noAwaitTime, Is.LessThanOrEqualTo(5000 + codeExecutingTimeInMilliseconds));
        }

        [Test]
        public void CallingResultIsJustSynchronous()
        {
            var fakeTaskFactory = new FakeLongRunningTaskCreator();
            var noAwaitTime = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                var length = fakeTaskFactory.GetTaskWhichTakesThreeSecondsToRun().Result;
                Thread.Sleep(_sleepInTest);

            });
            Assert.That(noAwaitTime, Is.GreaterThanOrEqualTo(_sleepInTest + TimeSpan.FromSeconds(3).TotalMilliseconds));
        }

        [Test]
        public async Task OtherStuffHappensWhenAsyncCalled()
        {
            var fakeTaskFactory = new FakeLongRunningTaskCreator();
            var task = fakeTaskFactory.GetTaskWhichTakesThreeSecondsToRun();
            Assert.IsFalse(task.IsCompleted);
            var p = await task;
            Assert.IsTrue(task.IsCompleted);
        }
    }

    public class FakeLongRunningTaskCreator
    {
        public string UsingSyncMethodWhichTakesThreeSecondsToComeBack()
        {
            return CreateTask().Result;
        }

        private static Task<string> CreateTask()
        {
            var task = new Task<string>(() =>
            {
                Thread.Sleep(3000);
                return "fred";
            });
            task.Start();
            return task;
        }

        public Task<string> GetTaskWhichTakesThreeSecondsToRun()
        {
            return CreateTask();
        }
    }
}
