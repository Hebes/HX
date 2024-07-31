using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Spine
{
	public class AnimationState
	{
		public AnimationState(AnimationStateData data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data cannot be null.");
			}
			this.data = data;
		}

		public AnimationStateData Data
		{
			get
			{
				return this.data;
			}
		}

		public float TimeScale
		{
			get
			{
				return this.timeScale;
			}
			set
			{
				this.timeScale = value;
			}
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.StartEndDelegate Start;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.StartEndDelegate End;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.EventDelegate Event;

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AnimationState.CompleteDelegate Complete;

		public void Update(float delta)
		{
			delta *= this.timeScale;
			for (int i = 0; i < this.tracks.Count; i++)
			{
				TrackEntry trackEntry = this.tracks[i];
				if (trackEntry != null)
				{
					float num = delta * trackEntry.timeScale;
					float num2 = trackEntry.time + num;
					float endTime = trackEntry.endTime;
					trackEntry.time = num2;
					if (trackEntry.previous != null)
					{
						trackEntry.previous.time += num;
						trackEntry.mixTime += num;
					}
					if ((!trackEntry.loop) ? (trackEntry.lastTime < endTime && num2 >= endTime) : (trackEntry.lastTime % endTime > num2 % endTime))
					{
						int loopCount = (int)(num2 / endTime);
						trackEntry.OnComplete(this, i, loopCount);
						if (this.Complete != null)
						{
							this.Complete(this, i, loopCount);
						}
					}
					TrackEntry next = trackEntry.next;
					if (next != null)
					{
						next.time = trackEntry.lastTime - next.delay;
						if (next.time >= 0f)
						{
							this.SetCurrent(i, next);
						}
					}
					else if (!trackEntry.loop && trackEntry.lastTime >= trackEntry.endTime)
					{
						this.ClearTrack(i);
					}
				}
			}
		}

		public void UpdateToTime(float _time)
		{
			for (int i = 0; i < this.tracks.Count; i++)
			{
				TrackEntry trackEntry = this.tracks[i];
				if (trackEntry != null)
				{
					float num = _time - trackEntry.time;
					float num2 = num * trackEntry.timeScale;
					float num3 = trackEntry.time + num2;
					float endTime = trackEntry.endTime;
					trackEntry.time = num3;
					if (trackEntry.previous != null)
					{
						trackEntry.previous.time += num2;
						trackEntry.mixTime += num2;
					}
					if ((!trackEntry.loop) ? (trackEntry.lastTime < endTime && num3 >= endTime) : (trackEntry.lastTime % endTime > num3 % endTime))
					{
						int loopCount = (int)(num3 / endTime);
						trackEntry.OnComplete(this, i, loopCount);
						if (this.Complete != null)
						{
							this.Complete(this, i, loopCount);
						}
					}
					TrackEntry next = trackEntry.next;
					if (next != null)
					{
						next.time = trackEntry.lastTime - next.delay;
						if (next.time >= 0f)
						{
							this.SetCurrent(i, next);
						}
					}
					else if (!trackEntry.loop && trackEntry.lastTime >= trackEntry.endTime)
					{
						this.ClearTrack(i);
					}
				}
			}
		}

		public void Apply(Skeleton skeleton)
		{
			List<Event> list = this.events;
			for (int i = 0; i < this.tracks.Count; i++)
			{
				TrackEntry trackEntry = this.tracks[i];
				if (trackEntry != null)
				{
					list.Clear();
					float num = trackEntry.time;
					bool loop = trackEntry.loop;
					if (!loop && num > trackEntry.endTime)
					{
						num = trackEntry.endTime;
					}
					TrackEntry previous = trackEntry.previous;
					if (previous == null)
					{
						if (trackEntry.mix == 1f)
						{
							trackEntry.animation.Apply(skeleton, trackEntry.lastTime, num, loop, list);
						}
						else
						{
							trackEntry.animation.Mix(skeleton, trackEntry.lastTime, num, loop, list, trackEntry.mix);
						}
					}
					else
					{
						float num2 = previous.time;
						if (!previous.loop && num2 > previous.endTime)
						{
							num2 = previous.endTime;
						}
						previous.animation.Apply(skeleton, num2, num2, previous.loop, null);
						float num3 = trackEntry.mixTime / trackEntry.mixDuration * trackEntry.mix;
						if (num3 >= 1f)
						{
							num3 = 1f;
							trackEntry.previous = null;
						}
						trackEntry.animation.Mix(skeleton, trackEntry.lastTime, num, loop, list, num3);
					}
					int j = 0;
					int count = list.Count;
					while (j < count)
					{
						Event e = list[j];
						trackEntry.OnEvent(this, i, e);
						if (this.Event != null)
						{
							this.Event(this, i, e);
						}
						j++;
					}
					trackEntry.lastTime = trackEntry.time;
				}
			}
		}

		public void ClearTracks()
		{
			int i = 0;
			int count = this.tracks.Count;
			while (i < count)
			{
				this.ClearTrack(i);
				i++;
			}
			this.tracks.Clear();
		}

		public void ClearTrack(int trackIndex)
		{
			if (trackIndex >= this.tracks.Count)
			{
				return;
			}
			TrackEntry trackEntry = this.tracks[trackIndex];
			if (trackEntry == null)
			{
				return;
			}
			trackEntry.OnEnd(this, trackIndex);
			if (this.End != null)
			{
				this.End(this, trackIndex);
			}
			this.tracks[trackIndex] = null;
		}

		private TrackEntry ExpandToIndex(int index)
		{
			if (index < this.tracks.Count)
			{
				return this.tracks[index];
			}
			while (index >= this.tracks.Count)
			{
				this.tracks.Add(null);
			}
			return null;
		}

		private void SetCurrent(int index, TrackEntry entry)
		{
			TrackEntry trackEntry = this.ExpandToIndex(index);
			if (trackEntry != null)
			{
				TrackEntry previous = trackEntry.previous;
				trackEntry.previous = null;
				trackEntry.OnEnd(this, index);
				if (this.End != null)
				{
					this.End(this, index);
				}
				entry.mixDuration = this.data.GetMix(trackEntry.animation, entry.animation);
				if (entry.mixDuration > 0f)
				{
					entry.mixTime = 0f;
					if (previous != null && trackEntry.mixTime / trackEntry.mixDuration < 0.5f)
					{
						entry.previous = previous;
					}
					else
					{
						entry.previous = trackEntry;
					}
				}
			}
			this.tracks[index] = entry;
			entry.OnStart(this, index);
			if (this.Start != null)
			{
				this.Start(this, index);
			}
		}

		public TrackEntry SetAnimation(int trackIndex, string animationName, bool loop)
		{
			Animation animation = this.data.skeletonData.FindAnimation(animationName);
			if (animation == null)
			{
				throw new ArgumentException("Animation not found: " + animationName);
			}
			return this.SetAnimation(trackIndex, animation, loop);
		}

		public TrackEntry SetAnimation(int trackIndex, Animation animation, bool loop)
		{
			if (animation == null)
			{
				throw new ArgumentException("animation cannot be null.");
			}
			TrackEntry trackEntry = new TrackEntry();
			trackEntry.animation = animation;
			trackEntry.loop = loop;
			trackEntry.time = 0f;
			trackEntry.endTime = animation.Duration;
			this.SetCurrent(trackIndex, trackEntry);
			return trackEntry;
		}

		public TrackEntry AddAnimation(int trackIndex, string animationName, bool loop, float delay)
		{
			Animation animation = this.data.skeletonData.FindAnimation(animationName);
			if (animation == null)
			{
				throw new ArgumentException("Animation not found: " + animationName);
			}
			return this.AddAnimation(trackIndex, animation, loop, delay);
		}

		public TrackEntry AddAnimation(int trackIndex, Animation animation, bool loop, float delay)
		{
			if (animation == null)
			{
				throw new ArgumentException("animation cannot be null.");
			}
			TrackEntry trackEntry = new TrackEntry();
			trackEntry.animation = animation;
			trackEntry.loop = loop;
			trackEntry.time = 0f;
			trackEntry.endTime = animation.Duration;
			TrackEntry trackEntry2 = this.ExpandToIndex(trackIndex);
			if (trackEntry2 != null)
			{
				while (trackEntry2.next != null)
				{
					trackEntry2 = trackEntry2.next;
				}
				trackEntry2.next = trackEntry;
			}
			else
			{
				this.tracks[trackIndex] = trackEntry;
			}
			if (delay <= 0f)
			{
				if (trackEntry2 != null)
				{
					delay += trackEntry2.endTime - this.data.GetMix(trackEntry2.animation, animation);
				}
				else
				{
					delay = 0f;
				}
			}
			trackEntry.delay = delay;
			return trackEntry;
		}

		public TrackEntry GetCurrent(int trackIndex)
		{
			if (trackIndex >= this.tracks.Count)
			{
				return null;
			}
			return this.tracks[trackIndex];
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			int count = this.tracks.Count;
			while (i < count)
			{
				TrackEntry trackEntry = this.tracks[i];
				if (trackEntry != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(trackEntry.ToString());
				}
				i++;
			}
			if (stringBuilder.Length == 0)
			{
				return "<none>";
			}
			return stringBuilder.ToString();
		}

		private AnimationStateData data;

		private List<TrackEntry> tracks = new List<TrackEntry>();

		private List<Event> events = new List<Event>();

		private float timeScale = 1f;

		public delegate void StartEndDelegate(AnimationState state, int trackIndex);

		public delegate void EventDelegate(AnimationState state, int trackIndex, Event e);

		public delegate void CompleteDelegate(AnimationState state, int trackIndex, int loopCount);
	}
}
