using UnityEngine.Profiling;

namespace Core
{
    public class ExpansionProfiler
    {
        /// <summary>
        /// 需要成对出现
        /// </summary>
        /// <param name="str"></param>
        public void ProfilerBeginSample(string str)
        {
            Profiler.BeginSample(str);
        }

        public void ProfilerEndSample()
        {
            Profiler.EndSample();
        }
    }
}
