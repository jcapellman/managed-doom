﻿using System;
using System.IO;

namespace ManagedDoom
{
	public sealed class Animation
	{
		public AnimationType type;
		public int period;
		public int nanims;
		public int locX;
		public int locY;
		public int data1;
		public int data2;
		public Patch[] p;
		public int nexttic;
		public int lastdrawn;
		public int ctr;
		public int state;

		private Intermission im;
		private int number;

		public Animation(Intermission intermission, AnimationInfo info, int number, int bcnt)
		{
			im = intermission;
			this.number = number;

			type = info.Type;
			period = info.Period;
			nanims = info.Count;
			locX = info.X;
			locY = info.Y;
			data1 = info.Data;

			// specify the next time to draw it
			if (type == AnimationType.Always)
			{
				nexttic = bcnt + 1 + (im.Random.Next() % period);
			}
			else if (type == AnimationType.Random)
			{
				nexttic = bcnt + 1 + data2 + (im.Random.Next() % data1);
			}
			else if (type == AnimationType.Level)
			{
				nexttic = bcnt + 1;
			}
		}

		public void Update(int bcnt)
		{
			if (bcnt == nexttic)
			{
				switch (type)
				{
					case AnimationType.Always:
						if (++ctr >= nanims)
						{
							ctr = 0;
						}
						nexttic = bcnt + period;
						break;

					case AnimationType.Random:
						ctr++;
						if (ctr == nanims)
						{
							ctr = -1;
							nexttic = bcnt + data2 + (im.Random.Next() % data1);
						}
						else
						{
							nexttic = bcnt + period;
						}
						break;

					case AnimationType.Level:
						// Gawd-awful hack for level anims.
						if (!(im.state == IntermissionState.StatCount && number == 7) && im.Wbs.Next == data1)
						{
							ctr++;
							if (ctr == nanims)
							{
								ctr--;
							}
							nexttic = bcnt + period;
						}
						break;
				}
			}
		}
	}
}