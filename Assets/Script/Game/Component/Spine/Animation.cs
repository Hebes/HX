using System;
using System.Collections.Generic;

namespace Spine
{
	public class Animation
	{
		public Animation(string name, List<Timeline> timelines, float duration)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			if (timelines == null)
			{
				throw new ArgumentNullException("timelines cannot be null.");
			}
			this.name = name;
			this.timelines = timelines;
			this.duration = duration;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public List<Timeline> Timelines
		{
			get
			{
				return this.timelines;
			}
			set
			{
				this.timelines = value;
			}
		}

		public float Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = value;
			}
		}

		public void Apply(Skeleton skeleton, float lastTime, float time, bool loop, List<Event> events)
		{
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton cannot be null.");
			}
			if (loop && this.duration != 0f)
			{
				time %= this.duration;
				lastTime %= this.duration;
			}
			List<Timeline> list = this.timelines;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				list[i].Apply(skeleton, lastTime, time, events, 1f);
				i++;
			}
		}

		public void Mix(Skeleton skeleton, float lastTime, float time, bool loop, List<Event> events, float alpha)
		{
			if (skeleton == null)
			{
				throw new ArgumentNullException("skeleton cannot be null.");
			}
			if (loop && this.duration != 0f)
			{
				time %= this.duration;
				lastTime %= this.duration;
			}
			List<Timeline> list = this.timelines;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				list[i].Apply(skeleton, lastTime, time, events, alpha);
				i++;
			}
		}

		internal static int binarySearch(float[] values, float target, int step)
		{
			int num = 0;
			int num2 = values.Length / step - 2;
			if (num2 == 0)
			{
				return step;
			}
			int num3 = (int)((uint)num2 >> 1);
			for (;;)
			{
				if (values[(num3 + 1) * step] <= target)
				{
					num = num3 + 1;
				}
				else
				{
					num2 = num3;
				}
				if (num == num2)
				{
					break;
				}
				num3 = (int)((uint)(num + num2) >> 1);
			}
			return (num + 1) * step;
		}

		internal static int binarySearch(float[] values, float target)
		{
			int num = 0;
			int num2 = values.Length - 2;
			if (num2 == 0)
			{
				return 1;
			}
			int num3 = (int)((uint)num2 >> 1);
			for (;;)
			{
				if (values[num3 + 1] <= target)
				{
					num = num3 + 1;
				}
				else
				{
					num2 = num3;
				}
				if (num == num2)
				{
					break;
				}
				num3 = (int)((uint)(num + num2) >> 1);
			}
			return num + 1;
		}

		internal static int linearSearch(float[] values, float target, int step)
		{
			int i = 0;
			int num = values.Length - step;
			while (i <= num)
			{
				if (values[i] > target)
				{
					return i;
				}
				i += step;
			}
			return -1;
		}

		internal List<Timeline> timelines;

		internal float duration;

		internal string name;
	}
}
