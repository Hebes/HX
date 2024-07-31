using System;
using System.Collections.Generic;

namespace Spine
{
	public class DrawOrderTimeline : Timeline
	{
		public DrawOrderTimeline(int frameCount)
		{
			this.frames = new float[frameCount];
			this.drawOrders = new int[frameCount][];
		}

		public float[] Frames
		{
			get
			{
				return this.frames;
			}
			set
			{
				this.frames = value;
			}
		}

		public int[][] DrawOrders
		{
			get
			{
				return this.drawOrders;
			}
			set
			{
				this.drawOrders = value;
			}
		}

		public int FrameCount
		{
			get
			{
				return this.frames.Length;
			}
		}

		public void SetFrame(int frameIndex, float time, int[] drawOrder)
		{
			this.frames[frameIndex] = time;
			this.drawOrders[frameIndex] = drawOrder;
		}

		public void Apply(Skeleton skeleton, float lastTime, float time, List<Event> firedEvents, float alpha)
		{
			float[] array = this.frames;
			if (time < array[0])
			{
				return;
			}
			int num;
			if (time >= array[array.Length - 1])
			{
				num = array.Length - 1;
			}
			else
			{
				num = Animation.binarySearch(array, time) - 1;
			}
			List<Slot> drawOrder = skeleton.drawOrder;
			List<Slot> slots = skeleton.slots;
			int[] array2 = this.drawOrders[num];
			if (array2 == null)
			{
				drawOrder.Clear();
				drawOrder.AddRange(slots);
			}
			else
			{
				int i = 0;
				int num2 = array2.Length;
				while (i < num2)
				{
					drawOrder[i] = slots[array2[i]];
					i++;
				}
			}
		}

		internal float[] frames;

		private int[][] drawOrders;
	}
}
