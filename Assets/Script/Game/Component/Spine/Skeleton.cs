using System;
using System.Collections.Generic;

namespace Spine
{
	public class Skeleton
	{
		public Skeleton(SkeletonData data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data cannot be null.");
			}
			this.data = data;
			this.bones = new List<Bone>(data.bones.Count);
			foreach (BoneData boneData in data.bones)
			{
				Bone bone = (boneData.parent != null) ? this.bones[data.bones.IndexOf(boneData.parent)] : null;
				Bone item = new Bone(boneData, this, bone);
				if (bone != null)
				{
					bone.children.Add(item);
				}
				this.bones.Add(item);
			}
			this.slots = new List<Slot>(data.slots.Count);
			this.drawOrder = new List<Slot>(data.slots.Count);
			foreach (SlotData slotData in data.slots)
			{
				Bone bone2 = this.bones[data.bones.IndexOf(slotData.boneData)];
				Slot item2 = new Slot(slotData, bone2);
				this.slots.Add(item2);
				this.drawOrder.Add(item2);
			}
			this.ikConstraints = new List<IkConstraint>(data.ikConstraints.Count);
			foreach (IkConstraintData ikConstraintData in data.ikConstraints)
			{
				this.ikConstraints.Add(new IkConstraint(ikConstraintData, this));
			}
			this.UpdateCache();
		}

		public SkeletonData Data
		{
			get
			{
				return this.data;
			}
		}

		public List<Bone> Bones
		{
			get
			{
				return this.bones;
			}
		}

		public List<Slot> Slots
		{
			get
			{
				return this.slots;
			}
		}

		public List<Slot> DrawOrder
		{
			get
			{
				return this.drawOrder;
			}
		}

		public List<IkConstraint> IkConstraints
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

		public Skin Skin
		{
			get
			{
				return this.skin;
			}
			set
			{
				this.skin = value;
			}
		}

		public float R
		{
			get
			{
				return this.r;
			}
			set
			{
				this.r = value;
			}
		}

		public float G
		{
			get
			{
				return this.g;
			}
			set
			{
				this.g = value;
			}
		}

		public float B
		{
			get
			{
				return this.b;
			}
			set
			{
				this.b = value;
			}
		}

		public float A
		{
			get
			{
				return this.a;
			}
			set
			{
				this.a = value;
			}
		}

		public float Time
		{
			get
			{
				return this.time;
			}
			set
			{
				this.time = value;
			}
		}

		public float X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		public float Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		public bool FlipX
		{
			get
			{
				return this.flipX;
			}
			set
			{
				this.flipX = value;
			}
		}

		public bool FlipY
		{
			get
			{
				return this.flipY;
			}
			set
			{
				this.flipY = value;
			}
		}

		public Bone RootBone
		{
			get
			{
				return (this.bones.Count != 0) ? this.bones[0] : null;
			}
		}

		public void UpdateCache()
		{
			List<List<Bone>> list = this.boneCache;
			List<IkConstraint> list2 = this.ikConstraints;
			int count = list2.Count;
			int num = count + 1;
			if (list.Count > num)
			{
				list.RemoveRange(num, list.Count - num);
			}
			int i = 0;
			int count2 = list.Count;
			while (i < count2)
			{
				list[i].Clear();
				i++;
			}
			while (list.Count < num)
			{
				list.Add(new List<Bone>());
			}
			List<Bone> list3 = list[0];
			int j = 0;
			int count3 = this.bones.Count;
			while (j < count3)
			{
				Bone bone = this.bones[j];
				Bone bone2 = bone;
				int k;
				for (;;)
				{
					k = 0;
					IL_13A:
					while (k < count)
					{
						IkConstraint ikConstraint = list2[k];
						Bone bone3 = ikConstraint.bones[0];
						Bone bone4 = ikConstraint.bones[ikConstraint.bones.Count - 1];
						while (bone2 != bone4)
						{
							if (bone4 == bone3)
							{
								k++;
								goto IL_13A;
							}
							bone4 = bone4.parent;
						}
						goto Block_4;
					}
					bone2 = bone2.parent;
					if (bone2 == null)
					{
						goto Block_7;
					}
				}
				IL_15B:
				j++;
				continue;
				Block_4:
				list[k].Add(bone);
				list[k + 1].Add(bone);
				goto IL_15B;
				Block_7:
				list3.Add(bone);
				goto IL_15B;
			}
		}

		public void UpdateWorldTransform()
		{
			List<Bone> list = this.bones;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Bone bone = list[i];
				bone.rotationIK = bone.rotation;
				i++;
			}
			List<List<Bone>> list2 = this.boneCache;
			List<IkConstraint> list3 = this.ikConstraints;
			int num = 0;
			int num2 = list2.Count - 1;
			for (;;)
			{
				List<Bone> list4 = list2[num];
				int j = 0;
				int count2 = list4.Count;
				while (j < count2)
				{
					list4[j].UpdateWorldTransform();
					j++;
				}
				if (num == num2)
				{
					break;
				}
				list3[num].apply();
				num++;
			}
		}

		public void SetToSetupPose()
		{
			this.SetBonesToSetupPose();
			this.SetSlotsToSetupPose();
		}

		public void SetBonesToSetupPose()
		{
			List<Bone> list = this.bones;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				list[i].SetToSetupPose();
				i++;
			}
			List<IkConstraint> list2 = this.ikConstraints;
			int j = 0;
			int count2 = list2.Count;
			while (j < count2)
			{
				IkConstraint ikConstraint = list2[j];
				ikConstraint.bendDirection = ikConstraint.data.bendDirection;
				ikConstraint.mix = ikConstraint.data.mix;
				j++;
			}
		}

		public void SetSlotsToSetupPose()
		{
			List<Slot> list = this.slots;
			this.drawOrder.Clear();
			this.drawOrder.AddRange(list);
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				list[i].SetToSetupPose(i);
				i++;
			}
		}

		public Bone FindBone(string boneName)
		{
			if (boneName == null)
			{
				throw new ArgumentNullException("boneName cannot be null.");
			}
			List<Bone> list = this.bones;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Bone bone = list[i];
				if (bone.data.name == boneName)
				{
					return bone;
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
			List<Bone> list = this.bones;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				if (list[i].data.name == boneName)
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		public Slot FindSlot(string slotName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			List<Slot> list = this.slots;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Slot slot = list[i];
				if (slot.data.name == slotName)
				{
					return slot;
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
			List<Slot> list = this.slots;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				if (list[i].data.name.Equals(slotName))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		public void SetSkin(string skinName)
		{
			Skin skin = this.data.FindSkin(skinName);
			if (skin == null)
			{
				throw new ArgumentException("Skin not found: " + skinName);
			}
			this.SetSkin(skin);
		}

		public void SetSkin(Skin newSkin)
		{
			if (newSkin != null)
			{
				if (this.skin != null)
				{
					newSkin.AttachAll(this, this.skin);
				}
				else
				{
					List<Slot> list = this.slots;
					int i = 0;
					int count = list.Count;
					while (i < count)
					{
						Slot slot = list[i];
						string attachmentName = slot.data.attachmentName;
						if (attachmentName != null)
						{
							Attachment attachment = newSkin.GetAttachment(i, attachmentName);
							if (attachment != null)
							{
								slot.Attachment = attachment;
							}
						}
						i++;
					}
				}
			}
			this.skin = newSkin;
		}

		public Attachment GetAttachment(string slotName, string attachmentName)
		{
			return this.GetAttachment(this.data.FindSlotIndex(slotName), attachmentName);
		}

		public Attachment GetAttachment(int slotIndex, string attachmentName)
		{
			if (attachmentName == null)
			{
				throw new ArgumentNullException("attachmentName cannot be null.");
			}
			if (this.skin != null)
			{
				Attachment attachment = this.skin.GetAttachment(slotIndex, attachmentName);
				if (attachment != null)
				{
					return attachment;
				}
			}
			if (this.data.defaultSkin != null)
			{
				return this.data.defaultSkin.GetAttachment(slotIndex, attachmentName);
			}
			return null;
		}

		public void SetAttachment(string slotName, string attachmentName)
		{
			if (slotName == null)
			{
				throw new ArgumentNullException("slotName cannot be null.");
			}
			List<Slot> list = this.slots;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				Slot slot = list[i];
				if (slot.data.name == slotName)
				{
					Attachment attachment = null;
					if (attachmentName != null)
					{
						attachment = this.GetAttachment(i, attachmentName);
						if (attachment == null)
						{
							throw new Exception("Attachment not found: " + attachmentName + ", for slot: " + slotName);
						}
					}
					slot.Attachment = attachment;
					return;
				}
				i++;
			}
			throw new Exception("Slot not found: " + slotName);
		}

		public IkConstraint FindIkConstraint(string ikConstraintName)
		{
			if (ikConstraintName == null)
			{
				throw new ArgumentNullException("ikConstraintName cannot be null.");
			}
			List<IkConstraint> list = this.ikConstraints;
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				IkConstraint ikConstraint = list[i];
				if (ikConstraint.data.name == ikConstraintName)
				{
					return ikConstraint;
				}
				i++;
			}
			return null;
		}

		public void Update(float delta)
		{
			this.time += delta;
		}

		public void UpdateToTime(float _time)
		{
			this.time = _time;
		}

		internal SkeletonData data;

		internal List<Bone> bones;

		internal List<Slot> slots;

		internal List<Slot> drawOrder;

		internal List<IkConstraint> ikConstraints;

		private List<List<Bone>> boneCache = new List<List<Bone>>();

		internal Skin skin;

		internal float r = 1f;

		internal float g = 1f;

		internal float b = 1f;

		internal float a = 1f;

		internal float time;

		internal bool flipX;

		internal bool flipY;

		internal float x;

		internal float y;
	}
}
