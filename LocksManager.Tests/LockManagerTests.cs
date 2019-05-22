using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocksManager.Tests
{
    [TestClass]
    public class LockManagerTests
    {
        static readonly LocksManager LockManager = new LocksManager();

        [TestMethod]
        public async Task Test_Sync_Lock()
        {
            var tasks = new List<Task>();
            var items = new List<int>();

            var n = 10;

            for (var i = 0; i < n; i++)
            {
                var j = i;
                tasks.Add(Task.Run(() =>
                {
                    using (LockManager.Lock("Sync_Lock"))
                    {
                        items.Add(j);
                    }
                }));
            }

            await Task.WhenAll(tasks);

            Assert.AreEqual(items.Count, n);
        }


        [TestMethod]
        public async Task Test_Async_Lock()
        {
            var tasks = new List<Task>();
            var items = new List<int>();

            var n = 10;

            for (var i = 0; i < n; i++)
            {
                var j = i;
                tasks.Add(Task.Run(async () =>
                {
                    using (await LockManager.LockAsync("Async_Lock"))
                    {
                        items.Add(j);
                    }
                }));
            }

            await Task.WhenAll(tasks);

            Assert.AreEqual(items.Count, n);
        }


        [TestMethod]
        public async Task Test_Timeout_Lock()
        {
            var tasks = new List<Task>();
            var items = new List<int>();
            var n = 2;

            for (var i = 0; i < n; i++)
            {
                var j = i;
                tasks.Add(Task.Run(async () =>
                {
                    using (var handler = await LockManager.LockAsync("Timeout_Lock", TimeSpan.FromSeconds(1)))
                    {
                        await Task.Delay(1500);

                        if (handler != null)
                            items.Add(j);
                    }
                }));
            }

            await Task.WhenAll(tasks);
            
            Assert.AreNotEqual(items.Count, n);
        }



    }
}
