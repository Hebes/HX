using UnityEngine.Profiling;

namespace Framework.Core
{
    public class ExpansionProfiler
    {
        /// <summary>
        /// 需要成对出现ProfilerEndSample
        /// </summary>
        /// <param name="str"></param>
        public static void ProfilerBeginSample(string str)
        {
            Profiler.BeginSample(str);
        }

        public static void ProfilerEndSample()
        {
            Profiler.EndSample();
        }
    }
}
