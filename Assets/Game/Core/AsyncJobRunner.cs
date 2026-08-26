using System;
using System.Collections.Generic;

namespace Bussigo.Game.Core
{
    public interface IJob
    {
        void Execute();
        bool IsCompleted { get; }
    }

    public class AsyncJobRunner
    {
        private readonly Queue<Action> _mainThreadQueue = new Queue<Action>();
        private readonly object _queueLock = new object();

        public void EnqueueMainThreadAction(Action action)
        {
            if (action == null) return;
            lock (_queueLock)
            {
                _mainThreadQueue.Enqueue(action);
            }
        }

        public void ProcessMainThreadJobs(float maxExecutionTimeMs = 4.0f)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            while (stopwatch.Elapsed.TotalMilliseconds < maxExecutionTimeMs)
            {
                Action nextAction = null;
                lock (_queueLock)
                {
                    if (_mainThreadQueue.Count > 0)
                    {
                        nextAction = _mainThreadQueue.Dequeue();
                    }
                }

                if (nextAction == null) break;

                try
                {
                    nextAction.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AsyncJobRunner] Error executing job: {ex}");
                }
            }
        }
    }
}
