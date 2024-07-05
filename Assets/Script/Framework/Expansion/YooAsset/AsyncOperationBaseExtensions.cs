//using System;
//using YooAsset;
//using static Cysharp.Threading.Tasks.Internal.Error;

//namespace Cysharp.Threading.Tasks
//{
//    public static class AsyncOperationBaseExtensions
//    {
//        public static IEnumerator.Awaiter GetAwaiter(this AsyncOperationBase handle)
//        {
//            return ToIEnumerator(handle).GetAwaiter();
//        }

//        public static IEnumerator ToIEnumerator(this AsyncOperationBase handle,
//                                        IProgress<float>        progress = null,
//                                        PlayerLoopTiming        timing   = PlayerLoopTiming.Update)
//        {
//            ThrowArgumentNullException(handle, nameof(handle));

//            if (handle.IsDone)
//            {
//                return IEnumerator.CompletedTask;
//            }

//            return new IEnumerator(
//                AsyncOperationBaserConfiguredSource.Create(
//                    handle,
//                    timing,
//                    progress,
//                    out var token
//                ),
//                token
//            );
//        }

//        sealed class AsyncOperationBaserConfiguredSource : IIEnumeratorSource,
//                                                           IPlayerLoopItem,
//                                                           ITaskPoolNode<AsyncOperationBaserConfiguredSource>
//        {
//            private static TaskPool<AsyncOperationBaserConfiguredSource> pool;

//            private AsyncOperationBaserConfiguredSource nextNode;

//            public ref AsyncOperationBaserConfiguredSource NextNode => ref nextNode;

//            static AsyncOperationBaserConfiguredSource()
//            {
//                TaskPool.RegisterSizeGetter(typeof(AsyncOperationBaserConfiguredSource), () => pool.Size);
//            }

//            private readonly Action<AsyncOperationBase>             continuationAction;
//            private          AsyncOperationBase                     handle;
//            private          IProgress<float>                       progress;
//            private          bool                                   completed;
//            private          IEnumeratorCompletionSourceCore<AsyncUnit> core;

//            AsyncOperationBaserConfiguredSource() { continuationAction = Continuation; }

//            public static IIEnumeratorSource Create(AsyncOperationBase handle,
//                                                PlayerLoopTiming   timing,
//                                                IProgress<float>   progress,
//                                                out short          token)
//            {
//                if(!pool.TryPop(out var result))
//                {
//                    result = new AsyncOperationBaserConfiguredSource();
//                }

//                result.handle    = handle;
//                result.progress  = progress;
//                result.completed = false;
//                TaskTracker.TrackActiveTask(result, 3);

//                if(progress != null)
//                {
//                    PlayerLoopHelper.AddAction(timing, result);
//                }

//                handle.Completed += result.continuationAction;

//                token = result.core.Version;

//                return result;
//            }

//            private void Continuation(AsyncOperationBase _)
//            {
//                handle.Completed -= continuationAction;

//                if(completed)
//                {
//                    TryReturn();
//                }
//                else
//                {
//                    completed = true;
//                    if(handle.Status == EOperationStatus.Failed)
//                    {
//                        core.TrySetException(new Exception(handle.Error));
//                    }
//                    else
//                    {
//                        core.TrySetResult(AsyncUnit.Default);
//                    }
//                }
//            }

//            bool TryReturn()
//            {
//                TaskTracker.RemoveTracking(this);
//                core.Reset();
//                handle   = default;
//                progress = default;
//                return pool.TryPush(this);
//            }

//            public IEnumeratorStatus GetStatus(short token) => core.GetStatus(token);

//            public void OnCompleted(Action<object> continuation, object state, short token)
//            {
//                core.OnCompleted(continuation, state, token);
//            }

//            public void GetResult(short token) { core.GetResult(token); }

//            public IEnumeratorStatus UnsafeGetStatus() => core.UnsafeGetStatus();

//            public bool MoveNext()
//            {
//                if(completed)
//                {
//                    TryReturn();
//                    return false;
//                }

//                if(!handle.IsDone)
//                {
//                    progress?.Report(handle.Progress);
//                }

//                return true;
//            }
//        }
//    }
//}