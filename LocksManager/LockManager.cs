using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocksManager
{
    public class LocksManager
    {
        private static readonly Dictionary<string, RefCounted<SemaphoreSlim>> SemaphoreSlims = new Dictionary<string, RefCounted<SemaphoreSlim>>();

        private sealed class RefCounted<T>
        {
            public RefCounted(T value)
            {
                RefCount = 1;
                Value = value;
            }

            public int RefCount { get; set; }
            public T Value { get; }
        }

        private SemaphoreSlim GetOrCreate(string key)
        {
            RefCounted<SemaphoreSlim> item;
            lock (SemaphoreSlims)
            {
                if (SemaphoreSlims.TryGetValue(key, out item))
                {
                    ++item.RefCount;
                }
                else
                {
                    item = new RefCounted<SemaphoreSlim>(new SemaphoreSlim(1, 1));
                    SemaphoreSlims[key] = item;
                }
            }
            return item.Value;
        }

        public IDisposable Lock(string key, TimeSpan timeSpan)
        {
            var taken = GetOrCreate(key).Wait(timeSpan);

            if (taken)
                return new Releaser { Key = key };

            return null;
        }

        public async Task<IDisposable> LockAsync(string key, TimeSpan timeSpan)
        {
            var taken = await GetOrCreate(key).WaitAsync(timeSpan).ConfigureAwait(false);

            if (taken)
                return new Releaser { Key = key };

            return null;
        }

        public IDisposable Lock(string key)
        {
            GetOrCreate(key).Wait();
            return new Releaser { Key = key };
        }

        public async Task<IDisposable> LockAsync(string key)
        {
            await GetOrCreate(key).WaitAsync().ConfigureAwait(false);
            return new Releaser { Key = key };
        }

        private sealed class Releaser : IDisposable
        {
            public string Key { private get; set; }

            public void Dispose()
            {
                RefCounted<SemaphoreSlim> item;
                lock (SemaphoreSlims)
                {
                    item = SemaphoreSlims[Key];
                    --item.RefCount;
                    if (item.RefCount == 0)
                        SemaphoreSlims.Remove(Key);
                }
                item.Value.Release();
            }
        }
    }
}