using System;
using System.Collections.Generic;

namespace Spine
{
	public class Skin
	{
		public Skin(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name cannot be null.");
			}
			this.name = name;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public void AddAttachment(int slotIndex, string name, Attachment attachment)
		{
			if (attachment == null)
			{
				throw new ArgumentNullException("attachment cannot be null.");
			}
			this.attachments[new KeyValuePair<int, string>(slotIndex, name)] = attachment;
		}

		public Attachment GetAttachment(int slotIndex, string name)
		{
			Attachment result;
			this.attachments.TryGetValue(new KeyValuePair<int, string>(slotIndex, name), out result);
			return result;
		}

		public void FindNamesForSlot(int slotIndex, List<string> names)
		{
			if (names == null)
			{
				throw new ArgumentNullException("names cannot be null.");
			}
			foreach (KeyValuePair<int, string> keyValuePair in this.attachments.Keys)
			{
				if (keyValuePair.Key == slotIndex)
				{
					names.Add(keyValuePair.Value);
				}
			}
		}

		public void FindAttachmentsForSlot(int slotIndex, List<Attachment> attachments)
		{
			if (attachments == null)
			{
				throw new ArgumentNullException("attachments cannot be null.");
			}
			foreach (KeyValuePair<KeyValuePair<int, string>, Attachment> keyValuePair in this.attachments)
			{
				if (keyValuePair.Key.Key == slotIndex)
				{
					attachments.Add(keyValuePair.Value);
				}
			}
		}

		public override string ToString()
		{
			return this.name;
		}

		internal void AttachAll(Skeleton skeleton, Skin oldSkin)
		{
			foreach (KeyValuePair<KeyValuePair<int, string>, Attachment> keyValuePair in oldSkin.attachments)
			{
				int key = keyValuePair.Key.Key;
				Slot slot = skeleton.slots[key];
				if (slot.attachment == keyValuePair.Value)
				{
					Attachment attachment = this.GetAttachment(key, keyValuePair.Key.Value);
					if (attachment != null)
					{
						slot.Attachment = attachment;
					}
				}
			}
		}

		internal string name;

		private Dictionary<KeyValuePair<int, string>, Attachment> attachments = new Dictionary<KeyValuePair<int, string>, Attachment>(Skin.AttachmentComparer.Instance);

		private class AttachmentComparer : IEqualityComparer<KeyValuePair<int, string>>
		{
			bool IEqualityComparer<KeyValuePair<int, string>>.Equals(KeyValuePair<int, string> o1, KeyValuePair<int, string> o2)
			{
				return o1.Key == o2.Key && o1.Value == o2.Value;
			}

			int IEqualityComparer<KeyValuePair<int, string>>.GetHashCode(KeyValuePair<int, string> o)
			{
				return o.Key;
			}

			internal static readonly Skin.AttachmentComparer Instance = new Skin.AttachmentComparer();
		}
	}
}
