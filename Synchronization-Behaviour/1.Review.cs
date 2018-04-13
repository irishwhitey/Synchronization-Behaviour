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

        [Test]
        public void UsingTasks()
        {
            var sut = new HasLongRunningOperations();
            var notUsingTasks = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                sut.ThreeSecondOperation();
                sut.TwoSecondOperation();
            });
            var usingTasks = TimingHelper.TimeToRunInMilliseconds(() =>
            {
                var task = Task.Run(() => sut.ThreeSecondOperation());
                sut.TwoSecondOperation();
                task.Wait();
            });
            Assert.That(usingTasks, Is.LessThan(notUsingTasks));
            Assert.That(usingTasks, Is.LessThan(TimeSpan.FromSeconds(4).TotalMilliseconds));
        }
    }
}
