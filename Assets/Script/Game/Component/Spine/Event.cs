using System;

namespace Spine
{
	public class Event
	{
		public Event(EventData data)
		{
			this.Data = data;
		}

		public EventData Data { get; private set; }

		public int Int { get; set; }

		public float Float { get; set; }

		public string String { get; set; }

		public override string ToString()
		{
			return this.Data.Name;
		}
	}
}
