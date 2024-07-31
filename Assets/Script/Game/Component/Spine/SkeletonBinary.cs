using System;
using System.Collections.Generic;
using System.IO;

namespace Spine
{
	public class SkeletonBinary
	{
		public SkeletonBinary(params Atlas[] atlasArray) : this(new AtlasAttachmentLoader(atlasArray))
		{
		}

		public SkeletonBinary(AttachmentLoader attachmentLoader)
		{
			if (attachmentLoader == null)
			{
				throw new ArgumentNullException("attachmentLoader cannot be null.");
			}
			this.attachmentLoader = attachmentLoader;
			this.Scale = 1f;
		}

		public float Scale { get; set; }

		public SkeletonData ReadSkeletonData(string path)
		{
			SkeletonData result;
			using (BufferedStream bufferedStream = new BufferedStream(new FileStream(path, FileMode.Open)))
			{
				SkeletonData skeletonData = this.ReadSkeletonData(bufferedStream);
				skeletonData.name = Path.GetFileNameWithoutExtension(path);
				result = skeletonData;
			}
			return result;
		}

		public SkeletonData ReadSkeletonData(Stream input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input cannot be null.");
			}
			float scale = this.Scale;
			SkeletonData skeletonData = new SkeletonData();
			skeletonData.hash = this.ReadString(input);
			if (skeletonData.hash.Length == 0)
			{
				skeletonData.hash = null;
			}
			skeletonData.version = this.ReadString(input);
			if (skeletonData.version.Length == 0)
			{
				skeletonData.version = null;
			}
			skeletonData.width = this.ReadFloat(input);
			skeletonData.height = this.ReadFloat(input);
			bool flag = this.ReadBoolean(input);
			if (flag)
			{
				skeletonData.imagesPath = this.ReadString(input);
				if (skeletonData.imagesPath.Length == 0)
				{
					skeletonData.imagesPath = null;
				}
			}
			int i = 0;
			int num = this.ReadInt(input, true);
			while (i < num)
			{
				string name = this.ReadString(input);
				BoneData parent = null;
				int num2 = this.ReadInt(input, true) - 1;
				if (num2 != -1)
				{
					parent = skeletonData.bones[num2];
				}
				BoneData boneData = new BoneData(name, parent);
				boneData.x = this.ReadFloat(input) * scale;
				boneData.y = this.ReadFloat(input) * scale;
				boneData.scaleX = this.ReadFloat(input);
				boneData.scaleY = this.ReadFloat(input);
				boneData.rotation = this.ReadFloat(input);
				boneData.length = this.ReadFloat(input) * scale;
				boneData.flipX = this.ReadBoolean(input);
				boneData.flipY = this.ReadBoolean(input);
				boneData.inheritScale = this.ReadBoolean(input);
				boneData.inheritRotation = this.ReadBoolean(input);
				if (flag)
				{
					this.ReadInt(input);
				}
				skeletonData.bones.Add(boneData);
				i++;
			}
			int j = 0;
			int num3 = this.ReadInt(input, true);
			while (j < num3)
			{
				IkConstraintData ikConstraintData = new IkConstraintData(this.ReadString(input));
				int k = 0;
				int num4 = this.ReadInt(input, true);
				while (k < num4)
				{
					ikConstraintData.bones.Add(skeletonData.bones[this.ReadInt(input, true)]);
					k++;
				}
				ikConstraintData.target = skeletonData.bones[this.ReadInt(input, true)];
				ikConstraintData.mix = this.ReadFloat(input);
				ikConstraintData.bendDirection = (int)this.ReadSByte(input);
				skeletonData.ikConstraints.Add(ikConstraintData);
				j++;
			}
			int l = 0;
			int num5 = this.ReadInt(input, true);
			while (l < num5)
			{
				string name2 = this.ReadString(input);
				BoneData boneData2 = skeletonData.bones[this.ReadInt(input, true)];
				SlotData slotData = new SlotData(name2, boneData2);
				int num6 = this.ReadInt(input);
				slotData.r = (float)(((long)num6 & (long)((ulong)0xFF000000)) >> 24) / 255f;
				slotData.g = (float)((num6 & 16711680) >> 16) / 255f;
				slotData.b = (float)((num6 & 65280) >> 8) / 255f;
				slotData.a = (float)(num6 & 255) / 255f;
				slotData.attachmentName = this.ReadString(input);
				slotData.blendMode = (BlendMode)this.ReadInt(input, true);
				skeletonData.slots.Add(slotData);
				l++;
			}
			Skin skin = this.ReadSkin(input, "default", flag);
			if (skin != null)
			{
				skeletonData.defaultSkin = skin;
				skeletonData.skins.Add(skin);
			}
			int m = 0;
			int num7 = this.ReadInt(input, true);
			while (m < num7)
			{
				skeletonData.skins.Add(this.ReadSkin(input, this.ReadString(input), flag));
				m++;
			}
			int n = 0;
			int num8 = this.ReadInt(input, true);
			while (n < num8)
			{
				EventData eventData = new EventData(this.ReadString(input));
				eventData.Int = this.ReadInt(input, false);
				eventData.Float = this.ReadFloat(input);
				eventData.String = this.ReadString(input);
				skeletonData.events.Add(eventData);
				n++;
			}
			int num9 = 0;
			int num10 = this.ReadInt(input, true);
			while (num9 < num10)
			{
				this.ReadAnimation(this.ReadString(input), input, skeletonData);
				num9++;
			}
			skeletonData.bones.TrimExcess();
			skeletonData.slots.TrimExcess();
			skeletonData.skins.TrimExcess();
			skeletonData.events.TrimExcess();
			skeletonData.animations.TrimExcess();
			skeletonData.ikConstraints.TrimExcess();
			return skeletonData;
		}

		private Skin ReadSkin(Stream input, string skinName, bool nonessential)
		{
			int num = this.ReadInt(input, true);
			if (num == 0)
			{
				return null;
			}
			Skin skin = new Skin(skinName);
			for (int i = 0; i < num; i++)
			{
				int slotIndex = this.ReadInt(input, true);
				int j = 0;
				int num2 = this.ReadInt(input, true);
				while (j < num2)
				{
					string text = this.ReadString(input);
					skin.AddAttachment(slotIndex, text, this.ReadAttachment(input, skin, text, nonessential));
					j++;
				}
			}
			return skin;
		}

		private Attachment ReadAttachment(Stream input, Skin skin, string attachmentName, bool nonessential)
		{
			float scale = this.Scale;
			string text = this.ReadString(input);
			if (text == null)
			{
				text = attachmentName;
			}
			switch (input.ReadByte())
			{
			case 0:
			{
				string text2 = this.ReadString(input);
				if (text2 == null)
				{
					text2 = text;
				}
				RegionAttachment regionAttachment = this.attachmentLoader.NewRegionAttachment(skin, text, text2);
				if (regionAttachment == null)
				{
					return null;
				}
				regionAttachment.Path = text2;
				regionAttachment.x = this.ReadFloat(input) * scale;
				regionAttachment.y = this.ReadFloat(input) * scale;
				regionAttachment.scaleX = this.ReadFloat(input);
				regionAttachment.scaleY = this.ReadFloat(input);
				regionAttachment.rotation = this.ReadFloat(input);
				regionAttachment.width = this.ReadFloat(input) * scale;
				regionAttachment.height = this.ReadFloat(input) * scale;
				int num = this.ReadInt(input);
				regionAttachment.r = (float)(((long)num & (long)((ulong)0xFF000000)) >> 24) / 255f;
				regionAttachment.g = (float)((num & 16711680) >> 16) / 255f;
				regionAttachment.b = (float)((num & 65280) >> 8) / 255f;
				regionAttachment.a = (float)(num & 255) / 255f;
				regionAttachment.UpdateOffset();
				return regionAttachment;
			}
			case 1:
			{
				BoundingBoxAttachment boundingBoxAttachment = this.attachmentLoader.NewBoundingBoxAttachment(skin, text);
				if (boundingBoxAttachment == null)
				{
					return null;
				}
				boundingBoxAttachment.vertices = this.ReadFloatArray(input, scale);
				return boundingBoxAttachment;
			}
			case 2:
			{
				string text3 = this.ReadString(input);
				if (text3 == null)
				{
					text3 = text;
				}
				MeshAttachment meshAttachment = this.attachmentLoader.NewMeshAttachment(skin, text, text3);
				if (meshAttachment == null)
				{
					return null;
				}
				meshAttachment.Path = text3;
				meshAttachment.regionUVs = this.ReadFloatArray(input, 1f);
				meshAttachment.triangles = this.ReadShortArray(input);
				meshAttachment.vertices = this.ReadFloatArray(input, scale);
				meshAttachment.UpdateUVs();
				int num2 = this.ReadInt(input);
				meshAttachment.r = (float)(((long)num2 & (long)((ulong)0xFF000000)) >> 24) / 255f;
				meshAttachment.g = (float)((num2 & 16711680) >> 16) / 255f;
				meshAttachment.b = (float)((num2 & 65280) >> 8) / 255f;
				meshAttachment.a = (float)(num2 & 255) / 255f;
				meshAttachment.HullLength = this.ReadInt(input, true) * 2;
				if (nonessential)
				{
					meshAttachment.Edges = this.ReadIntArray(input);
					meshAttachment.Width = this.ReadFloat(input) * scale;
					meshAttachment.Height = this.ReadFloat(input) * scale;
				}
				return meshAttachment;
			}
			case 3:
			{
				string text4 = this.ReadString(input);
				if (text4 == null)
				{
					text4 = text;
				}
				SkinnedMeshAttachment skinnedMeshAttachment = this.attachmentLoader.NewSkinnedMeshAttachment(skin, text, text4);
				if (skinnedMeshAttachment == null)
				{
					return null;
				}
				skinnedMeshAttachment.Path = text4;
				float[] array = this.ReadFloatArray(input, 1f);
				int[] triangles = this.ReadShortArray(input);
				int num3 = this.ReadInt(input, true);
				List<float> list = new List<float>(array.Length * 3 * 3);
				List<int> list2 = new List<int>(array.Length * 3);
				for (int i = 0; i < num3; i++)
				{
					int num4 = (int)this.ReadFloat(input);
					list2.Add(num4);
					int num5 = i + num4 * 4;
					while (i < num5)
					{
						list2.Add((int)this.ReadFloat(input));
						list.Add(this.ReadFloat(input) * scale);
						list.Add(this.ReadFloat(input) * scale);
						list.Add(this.ReadFloat(input));
						i += 4;
					}
				}
				skinnedMeshAttachment.bones = list2.ToArray();
				skinnedMeshAttachment.weights = list.ToArray();
				skinnedMeshAttachment.triangles = triangles;
				skinnedMeshAttachment.regionUVs = array;
				skinnedMeshAttachment.UpdateUVs();
				int num6 = this.ReadInt(input);
				skinnedMeshAttachment.r = (float)(((long)num6 & (long)((ulong)0xFF000000)) >> 24) / 255f;
				skinnedMeshAttachment.g = (float)((num6 & 16711680) >> 16) / 255f;
				skinnedMeshAttachment.b = (float)((num6 & 65280) >> 8) / 255f;
				skinnedMeshAttachment.a = (float)(num6 & 255) / 255f;
				skinnedMeshAttachment.HullLength = this.ReadInt(input, true) * 2;
				if (nonessential)
				{
					skinnedMeshAttachment.Edges = this.ReadIntArray(input);
					skinnedMeshAttachment.Width = this.ReadFloat(input) * scale;
					skinnedMeshAttachment.Height = this.ReadFloat(input) * scale;
				}
				return skinnedMeshAttachment;
			}
			default:
				return null;
			}
		}

		private float[] ReadFloatArray(Stream input, float scale)
		{
			int num = this.ReadInt(input, true);
			float[] array = new float[num];
			if (scale == 1f)
			{
				for (int i = 0; i < num; i++)
				{
					array[i] = this.ReadFloat(input);
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					array[j] = this.ReadFloat(input) * scale;
				}
			}
			return array;
		}

		private int[] ReadShortArray(Stream input)
		{
			int num = this.ReadInt(input, true);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (input.ReadByte() << 8) + input.ReadByte();
			}
			return array;
		}

		private int[] ReadIntArray(Stream input)
		{
			int num = this.ReadInt(input, true);
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = this.ReadInt(input, true);
			}
			return array;
		}

		private void ReadAnimation(string name, Stream input, SkeletonData skeletonData)
		{
			List<Timeline> list = new List<Timeline>();
			float scale = this.Scale;
			float num = 0f;
			int i = 0;
			int num2 = this.ReadInt(input, true);
			while (i < num2)
			{
				int slotIndex = this.ReadInt(input, true);
				int j = 0;
				int num3 = this.ReadInt(input, true);
				while (j < num3)
				{
					int num4 = input.ReadByte();
					int num5 = this.ReadInt(input, true);
					if (num4 != 4)
					{
						if (num4 == 3)
						{
							AttachmentTimeline attachmentTimeline = new AttachmentTimeline(num5);
							attachmentTimeline.slotIndex = slotIndex;
							for (int k = 0; k < num5; k++)
							{
								attachmentTimeline.SetFrame(k, this.ReadFloat(input), this.ReadString(input));
							}
							list.Add(attachmentTimeline);
							num = Math.Max(num, attachmentTimeline.frames[num5 - 1]);
						}
					}
					else
					{
						ColorTimeline colorTimeline = new ColorTimeline(num5);
						colorTimeline.slotIndex = slotIndex;
						for (int l = 0; l < num5; l++)
						{
							float time = this.ReadFloat(input);
							int num6 = this.ReadInt(input);
							float r = (float)(((long)num6 & (long)((ulong)0xFF000000)) >> 24) / 255f;
							float g = (float)((num6 & 16711680) >> 16) / 255f;
							float b = (float)((num6 & 65280) >> 8) / 255f;
							float a = (float)(num6 & 255) / 255f;
							colorTimeline.SetFrame(l, time, r, g, b, a);
							if (l < num5 - 1)
							{
								this.ReadCurve(input, l, colorTimeline);
							}
						}
						list.Add(colorTimeline);
						num = Math.Max(num, colorTimeline.frames[num5 * 5 - 5]);
					}
					j++;
				}
				i++;
			}
			int m = 0;
			int num7 = this.ReadInt(input, true);
			while (m < num7)
			{
				int boneIndex = this.ReadInt(input, true);
				int n = 0;
				int num8 = this.ReadInt(input, true);
				while (n < num8)
				{
					int num9 = input.ReadByte();
					int num10 = this.ReadInt(input, true);
					switch (num9)
					{
					case 0:
					case 2:
					{
						float num11 = 1f;
						TranslateTimeline translateTimeline;
						if (num9 == 0)
						{
							translateTimeline = new ScaleTimeline(num10);
						}
						else
						{
							translateTimeline = new TranslateTimeline(num10);
							num11 = scale;
						}
						translateTimeline.boneIndex = boneIndex;
						for (int num12 = 0; num12 < num10; num12++)
						{
							translateTimeline.SetFrame(num12, this.ReadFloat(input), this.ReadFloat(input) * num11, this.ReadFloat(input) * num11);
							if (num12 < num10 - 1)
							{
								this.ReadCurve(input, num12, translateTimeline);
							}
						}
						list.Add(translateTimeline);
						num = Math.Max(num, translateTimeline.frames[num10 * 3 - 3]);
						break;
					}
					case 1:
					{
						RotateTimeline rotateTimeline = new RotateTimeline(num10);
						rotateTimeline.boneIndex = boneIndex;
						for (int num13 = 0; num13 < num10; num13++)
						{
							rotateTimeline.SetFrame(num13, this.ReadFloat(input), this.ReadFloat(input));
							if (num13 < num10 - 1)
							{
								this.ReadCurve(input, num13, rotateTimeline);
							}
						}
						list.Add(rotateTimeline);
						num = Math.Max(num, rotateTimeline.frames[num10 * 2 - 2]);
						break;
					}
					case 5:
					case 6:
					{
						FlipXTimeline flipXTimeline = (num9 != 5) ? new FlipYTimeline(num10) : new FlipXTimeline(num10);
						flipXTimeline.boneIndex = boneIndex;
						for (int num14 = 0; num14 < num10; num14++)
						{
							flipXTimeline.SetFrame(num14, this.ReadFloat(input), this.ReadBoolean(input));
						}
						list.Add(flipXTimeline);
						num = Math.Max(num, flipXTimeline.frames[num10 * 2 - 2]);
						break;
					}
					}
					n++;
				}
				m++;
			}
			int num15 = 0;
			int num16 = this.ReadInt(input, true);
			while (num15 < num16)
			{
				IkConstraintData item = skeletonData.ikConstraints[this.ReadInt(input, true)];
				int num17 = this.ReadInt(input, true);
				IkConstraintTimeline ikConstraintTimeline = new IkConstraintTimeline(num17);
				ikConstraintTimeline.ikConstraintIndex = skeletonData.ikConstraints.IndexOf(item);
				for (int num18 = 0; num18 < num17; num18++)
				{
					ikConstraintTimeline.SetFrame(num18, this.ReadFloat(input), this.ReadFloat(input), (int)this.ReadSByte(input));
					if (num18 < num17 - 1)
					{
						this.ReadCurve(input, num18, ikConstraintTimeline);
					}
				}
				list.Add(ikConstraintTimeline);
				num = Math.Max(num, ikConstraintTimeline.frames[num17 * 3 - 3]);
				num15++;
			}
			int num19 = 0;
			int num20 = this.ReadInt(input, true);
			while (num19 < num20)
			{
				Skin skin = skeletonData.skins[this.ReadInt(input, true)];
				int num21 = 0;
				int num22 = this.ReadInt(input, true);
				while (num21 < num22)
				{
					int slotIndex2 = this.ReadInt(input, true);
					int num23 = 0;
					int num24 = this.ReadInt(input, true);
					while (num23 < num24)
					{
						Attachment attachment = skin.GetAttachment(slotIndex2, this.ReadString(input));
						int num25 = this.ReadInt(input, true);
						FFDTimeline ffdtimeline = new FFDTimeline(num25);
						ffdtimeline.slotIndex = slotIndex2;
						ffdtimeline.attachment = attachment;
						for (int num26 = 0; num26 < num25; num26++)
						{
							float time2 = this.ReadFloat(input);
							int num27;
							if (attachment is MeshAttachment)
							{
								num27 = ((MeshAttachment)attachment).vertices.Length;
							}
							else
							{
								num27 = ((SkinnedMeshAttachment)attachment).weights.Length / 3 * 2;
							}
							int num28 = this.ReadInt(input, true);
							float[] array;
							if (num28 == 0)
							{
								if (attachment is MeshAttachment)
								{
									array = ((MeshAttachment)attachment).vertices;
								}
								else
								{
									array = new float[num27];
								}
							}
							else
							{
								array = new float[num27];
								int num29 = this.ReadInt(input, true);
								num28 += num29;
								if (scale == 1f)
								{
									for (int num30 = num29; num30 < num28; num30++)
									{
										array[num30] = this.ReadFloat(input);
									}
								}
								else
								{
									for (int num31 = num29; num31 < num28; num31++)
									{
										array[num31] = this.ReadFloat(input) * scale;
									}
								}
								if (attachment is MeshAttachment)
								{
									float[] vertices = ((MeshAttachment)attachment).vertices;
									int num32 = 0;
									int num33 = array.Length;
									while (num32 < num33)
									{
										array[num32] += vertices[num32];
										num32++;
									}
								}
							}
							ffdtimeline.SetFrame(num26, time2, array);
							if (num26 < num25 - 1)
							{
								this.ReadCurve(input, num26, ffdtimeline);
							}
						}
						list.Add(ffdtimeline);
						num = Math.Max(num, ffdtimeline.frames[num25 - 1]);
						num23++;
					}
					num21++;
				}
				num19++;
			}
			int num34 = this.ReadInt(input, true);
			if (num34 > 0)
			{
				DrawOrderTimeline drawOrderTimeline = new DrawOrderTimeline(num34);
				int count = skeletonData.slots.Count;
				for (int num35 = 0; num35 < num34; num35++)
				{
					int num36 = this.ReadInt(input, true);
					int[] array2 = new int[count];
					for (int num37 = count - 1; num37 >= 0; num37--)
					{
						array2[num37] = -1;
					}
					int[] array3 = new int[count - num36];
					int num38 = 0;
					int num39 = 0;
					for (int num40 = 0; num40 < num36; num40++)
					{
						int num41 = this.ReadInt(input, true);
						while (num38 != num41)
						{
							array3[num39++] = num38++;
						}
						array2[num38 + this.ReadInt(input, true)] = num38++;
					}
					while (num38 < count)
					{
						array3[num39++] = num38++;
					}
					for (int num42 = count - 1; num42 >= 0; num42--)
					{
						if (array2[num42] == -1)
						{
							array2[num42] = array3[--num39];
						}
					}
					drawOrderTimeline.SetFrame(num35, this.ReadFloat(input), array2);
				}
				list.Add(drawOrderTimeline);
				num = Math.Max(num, drawOrderTimeline.frames[num34 - 1]);
			}
			int num43 = this.ReadInt(input, true);
			if (num43 > 0)
			{
				EventTimeline eventTimeline = new EventTimeline(num43);
				for (int num44 = 0; num44 < num43; num44++)
				{
					float time3 = this.ReadFloat(input);
					EventData eventData = skeletonData.events[this.ReadInt(input, true)];
					eventTimeline.SetFrame(num44, time3, new Event(eventData)
					{
						Int = this.ReadInt(input, false),
						Float = this.ReadFloat(input),
						String = ((!this.ReadBoolean(input)) ? eventData.String : this.ReadString(input))
					});
				}
				list.Add(eventTimeline);
				num = Math.Max(num, eventTimeline.frames[num43 - 1]);
			}
			list.TrimExcess();
			skeletonData.animations.Add(new Animation(name, list, num));
		}

		private void ReadCurve(Stream input, int frameIndex, CurveTimeline timeline)
		{
			int num = input.ReadByte();
			if (num != 1)
			{
				if (num == 2)
				{
					timeline.SetCurve(frameIndex, this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input), this.ReadFloat(input));
				}
			}
			else
			{
				timeline.SetStepped(frameIndex);
			}
		}

		private sbyte ReadSByte(Stream input)
		{
			int num = input.ReadByte();
			if (num == -1)
			{
				throw new EndOfStreamException();
			}
			return (sbyte)num;
		}

		private bool ReadBoolean(Stream input)
		{
			return input.ReadByte() != 0;
		}

		private float ReadFloat(Stream input)
		{
			this.buffer[3] = (byte)input.ReadByte();
			this.buffer[2] = (byte)input.ReadByte();
			this.buffer[1] = (byte)input.ReadByte();
			this.buffer[0] = (byte)input.ReadByte();
			return BitConverter.ToSingle(this.buffer, 0);
		}

		private int ReadInt(Stream input)
		{
			return (input.ReadByte() << 24) + (input.ReadByte() << 16) + (input.ReadByte() << 8) + input.ReadByte();
		}

		private int ReadInt(Stream input, bool optimizePositive)
		{
			int num = input.ReadByte();
			int num2 = num & 127;
			if ((num & 128) != 0)
			{
				num = input.ReadByte();
				num2 |= (num & 127) << 7;
				if ((num & 128) != 0)
				{
					num = input.ReadByte();
					num2 |= (num & 127) << 14;
					if ((num & 128) != 0)
					{
						num = input.ReadByte();
						num2 |= (num & 127) << 21;
						if ((num & 128) != 0)
						{
							num = input.ReadByte();
							num2 |= (num & 127) << 28;
						}
					}
				}
			}
			return (!optimizePositive) ? (num2 >> 1 ^ -(num2 & 1)) : num2;
		}

		private string ReadString(Stream input)
		{
			int num = this.ReadInt(input, true);
			if (num == 0)
			{
				return null;
			}
			if (num != 1)
			{
				num--;
				char[] array = this.chars;
				if (array.Length < num)
				{
					array = (this.chars = new char[num]);
				}
				int i = 0;
				int num2 = 0;
				while (i < num)
				{
					num2 = input.ReadByte();
					if (num2 > 127)
					{
						break;
					}
					array[i++] = (char)num2;
				}
				if (i < num)
				{
					this.ReadUtf8_slow(input, num, i, num2);
				}
				return new string(array, 0, num);
			}
			return string.Empty;
		}

		private void ReadUtf8_slow(Stream input, int charCount, int charIndex, int b)
		{
			char[] array = this.chars;
			for (;;)
			{
				switch (b >> 4)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
					array[charIndex] = (char)b;
					break;
				case 12:
				case 13:
					array[charIndex] = (char)((b & 31) << 6 | (input.ReadByte() & 63));
					break;
				case 14:
					array[charIndex] = (char)((b & 15) << 12 | (input.ReadByte() & 63) << 6 | (input.ReadByte() & 63));
					break;
				}
				if (++charIndex >= charCount)
				{
					break;
				}
				b = (input.ReadByte() & 255);
			}
		}

		public const int TIMELINE_SCALE = 0;

		public const int TIMELINE_ROTATE = 1;

		public const int TIMELINE_TRANSLATE = 2;

		public const int TIMELINE_ATTACHMENT = 3;

		public const int TIMELINE_COLOR = 4;

		public const int TIMELINE_FLIPX = 5;

		public const int TIMELINE_FLIPY = 6;

		public const int CURVE_LINEAR = 0;

		public const int CURVE_STEPPED = 1;

		public const int CURVE_BEZIER = 2;

		private AttachmentLoader attachmentLoader;

		private char[] chars = new char[32];

		private byte[] buffer = new byte[4];
	}
}
