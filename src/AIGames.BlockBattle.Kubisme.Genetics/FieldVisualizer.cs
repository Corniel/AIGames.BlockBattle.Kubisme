using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class FieldVisualizer
	{
		public FieldVisualizer(int blockSize)
		{
			BlockSize = blockSize;
		}
		public int BlockSize { get; set; }

		public void Draw(List<Field> left, List<Field> right, DirectoryInfo dir)
		{
			if (left.Count != right.Count) { throw new ArgumentOutOfRangeException("Different lenghts for left and right."); }

			for (var turn = 0; turn < left.Count; turn++)
			{
				var file = new FileInfo(Path.Combine(dir.FullName, String.Format("{0:000}.png", turn)));

				Draw(left[turn], right[turn], file);
			}
		}

		public void Draw(Field field, DirectoryInfo dir, int turn)
		{
			var file = new FileInfo(Path.Combine(dir.FullName, String.Format("{0:000}.png", turn)));
			Draw(field, file);
		}

		private void Draw(Field field, FileInfo file)
		{
			var xMax = 10;
			var yMax = field.RowCount;

			var image = new Bitmap(xMax * BlockSize, yMax * BlockSize);

			for (var y = 0; y < yMax; y++)
			{
				var row = field[y];

				for (var x = 0; x < xMax; x++)
				{
					if ((Row.Flag[x] & row) != 0)
					{
						DrawPixel(image, x, y, Color.Red);
					}
					else
					{
						DrawPixel(image, x, y, Color.White);
					}
				}
			}
			image.Save(file.FullName, ImageFormat.Png);
		}

		private void Draw(Field left, Field right, FileInfo file)
		{
			var yMax = Math.Max(left.RowCount, right.RowCount);

			if (yMax == 0) { return; }

			var image = new Bitmap(21 * BlockSize, yMax * BlockSize);

			for (var y = 0; y < yMax; y++)
			{
				DrawPixel(image, 10, y, Color.Black);

				if (y < left.RowCount)
				{
					var rLeft = left[y];

					for (var x = 0; x < 10; x++)
					{
						if ((Row.Flag[x] & rLeft) != 0)
						{
							DrawPixel(image, x, y, Color.Red);
						}
						else
						{
							DrawPixel(image, x, y, Color.White);
						}
					}
				}
				if (y < right.RowCount)
				{
					var rRght = right[y];

					for (var x = 0; x < 10; x++)
					{
						if ((Row.Flag[x] & rRght) != 0)
						{
							DrawPixel(image, x + 11, y, Color.Blue);
						}
						else
						{
							DrawPixel(image, x + 11, y, Color.White);
						}
					}
				}
			}
			image.Save(file.FullName, ImageFormat.Png);
		}

		private void DrawPixel(Bitmap image, int x, int y, Color color)
		{
			var xMin = x * BlockSize;
			var yMin = y * BlockSize;

			for (var yy = yMin; yy < yMin + BlockSize; yy++)
			{
				for (var xx = xMin; xx < xMin + BlockSize; xx++)
				{
					image.SetPixel(xx, yy, color);
				}
			}
		}
	}
}
