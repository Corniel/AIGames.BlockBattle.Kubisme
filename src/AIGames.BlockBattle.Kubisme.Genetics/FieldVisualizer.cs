using AIGames.BlockBattle.Kubisme.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIGames.BlockBattle.Kubisme.Genetics
{
	public class FieldVisualizer
	{
		public FieldVisualizer(int blockSize)
		{
			BlockSize = blockSize;
		}
		public int BlockSize { get; set; }

		public void Draw(Field field, DirectoryInfo dir, int turn)
		{
			var file = new FileInfo(Path.Combine(dir.FullName, String.Format("{0:0000}.png", turn)));
			Draw(field, file);
		}

		private void Draw(Field field, FileInfo file)
		{
			var xMax = 10;
			var yMax = field.RowCount;

			var image = new Bitmap(xMax * BlockSize, yMax * BlockSize);

			for (var y = 0; y < yMax; y++)
			{
				for (var x = 0; x < xMax; x++)
				{
					var row = field[y].row;
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
