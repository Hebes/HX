using System;
using System.Collections.Generic;

namespace Spine
{
	public class SkeletonData
	{
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public List<BoneData> Bones
		{
			get
			{
				return this.bones;
			}
		}

		public List<SlotData> Slots
		{
			get
			{
				return this.slots;
			}
		}

		public List<Skin> Skins
		{
			get
			{
				return this.skins;
			}
			set
			{
				this.skins = value;
			}
		}

		public Skin DefaultSkin
		{
			get
			{
				return this.defaultSkin;
			}
			set
			{
				this.defaultSkin = value;
			}
		}

		public List<EventData> Events
		{
			get
			{
				return this.events;
			}
			set
			{
				this.events = value;
			}
		}

		public List<Animation> Animations
		{
			get
			{
				return this.animations;
			}
			set
			{
				this.animations = value;
			}
		}

		public List<IkConstraintData> IkConstraints
		{
			get
			{
				return this.ikConstraints;
			}
			set
			{
				this.ikConstraints = value;
			}
		}

		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		public string Hash
		{
			get
			{
				return this.hash;
			}
			set
			{
				this.hash = value;
			}
		}

		public BoneData FindBone(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName cannot be null.");
			}
			List<BoneData> list = this.bones;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				BoneData boneData = list[i];
				if (boneData.name == boneName)
				{
					return boneData;
				}
				i++;
			}
			return null;
		}

		public int FindBoneIndex(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName cannot be null.");
			}
			List<BoneData> list = this.bones;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				if (list[i].name == boneName)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		public SlotData FindSlot(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			List<SlotData> list = this.slots;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				SlotData slotData = list[i];
				if (slotData.name == slotName)
				{
					return slotData;
				}
				i++;
			}
			return null;
		}

		public int FindSlotIndex(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			List<SlotData> list = this.slots;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				if (list[i].name == slotName)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		public Skin FindSkin(string skinName)
		{
			if (skinName == null)
			{
				throw new ArgumentNullException("skinName cannot be null.");
			}
			foreach (Skin skin in this.skins)
			{
				if (skin.name == skinName)
				{
					return skin;
				}
			}
			return null;
		}

		public EventData FindEvent(string eventDataName)
		{
			if (eventDataName == null)
			{
				throw new ArgumentNullException("eventDataName cannot be null.");
			}
			foreach (EventData eventData in this.events)
			{
				if (eventData.name == eventDataName)
				{
					return eventData;
				}
			}
			return null;
		}

		public Animation FindAnimation(string animationName)
		{
			if (animationName == null)
			{
				throw new ArgumentNullException("animationName cannot be null.");
			}
			List<Animation> list = this.animations;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Animation animation = list[i];
				if (animation.name == animationName)
				{
					return animation;
				}
				i++;
			}
			return null;
		}

		public IkConstraintData FindIkConstraint(string ikConstraintName)
		{
			if (ikConstraintName == null)
			{
				throw new ArgumentNullException("ikConstraintName cannot be null.");
			}
			List<IkConstraintData> list = this.ikConstraints;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				IkConstraintData ikConstraintData = list[i];
				if (ikConstraintData.name == ikConstraintName)
				{
					return ikConstraintData;
				}
				i++;
			}
			return null;
		}

		public override string ToString()
		{
			return this.name ?? base.ToString();
		}

		internal string name;

		internal List<BoneData> bones = new List<BoneData>();

		internal List<SlotData> slots = new List<SlotData>();

		internal List<Skin> skins = new List<Skin>();

		internal Skin defaultSkin;

		internal List<EventData> events = new List<EventData>();

		internal List<Animation> animations = new List<Animation>();

		internal List<IkConstraintData> ikConstraints = new List<IkConstraintData>();

		internal float width;

		internal float height;

		internal string version;

		internal string hash;

		internal string imagesPath;
	}
}
