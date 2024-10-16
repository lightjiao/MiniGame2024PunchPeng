/*
 * 为UniTask添加锁
 * https://gist.github.com/lightjiao/707ff5cc4697c01319e1d2023620233f
 */

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Cysharp.Threading
{
    public readonly struct UniTaskLock : IDisposable
    {
        private readonly int _lockId;

        private UniTaskLock(int lockId)
        {
            _lockId = lockId;
        }

        public void Dispose()
        {
            Unlock(_lockId);
        }

        // --- STATIC METHOD --- //

        private static readonly Dictionary<int, bool> LockDic = new();
        private static readonly Dictionary<int, Queue<UniTaskCompletionSource<UniTaskLock>>> TcsQueueDic = new();

        public static async UniTask<UniTaskLock> Lock(int lockId)
        {
            if (!TcsQueueDic.TryGetValue(lockId, out var queue))
            {
                queue = new Queue<UniTaskCompletionSource<UniTaskLock>>();
                TcsQueueDic[lockId] = queue;
            }

            if (LockDic.TryGetValue(lockId, out var hasLock) && hasLock)
            {
                var tcs = new UniTaskCompletionSource<UniTaskLock>();
                queue.Enqueue(tcs);
                return await tcs.Task;
            }

            LockDic[lockId] = true;
            var taskLock = new UniTaskLock(lockId);
            return taskLock;
        }

        private static void Unlock(int lockId)
        {
            if (!TcsQueueDic.TryGetValue(lockId, out var queue))
            {
                queue = new Queue<UniTaskCompletionSource<UniTaskLock>>();
                TcsQueueDic[lockId] = queue;
            }

            if (queue.Count == 0)
            {
                LockDic.Remove(lockId);
                return;
            }

            queue.Dequeue().TrySetResult(new UniTaskLock(lockId));
        }
    }
}