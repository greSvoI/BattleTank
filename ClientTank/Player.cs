using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTank
{
	class Player :IPlayer
	{
		public string Id { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public bool Up { get; set; } = true;
		public bool Down { get; set; } = false;
		public bool Left { get; set; } = false;
		public bool Right { get; set; } = false;
		public bool Shot { get; set; } = false;
		public bool Hit { get; set; } = false;
		public string Move { get; set; } = "Up";
		public bool Tank_Fire { get; set; } = false;

		public PictureBox picture { get; set; }
		public ICannon Cannon { get; set; }
	

		public Player()
		{
			
		}
		public Player(Player pl)
		{
			this.Id = pl.Id;
			this.X = pl.X;
			this.Y = pl.Y;
			this.Down = pl.Down;
			this.Up = pl.Up;
			this.Left = pl.Left;
			this.Right = pl.Right;
			this.Shot = pl.Shot;
			this.Hit = pl.Hit;
			this.Move = pl.Move;
			this.Hit = pl.Hit;
			this.Tank_Fire = pl.Tank_Fire;
			picture = new PictureBox();
			picture.Image = Image.FromFile("red.png");
			picture.SizeMode = PictureBoxSizeMode.StretchImage;
			picture.Size = new Size(40, 40);
			picture.Location = new Point(X, Y);
			Game.MyPictureBox.Invoke(new Action(() =>
			{
				Game.MyPictureBox.Controls.Add(picture);
			}));

		}
		public byte[] Serialize()
		{
			using (MemoryStream m = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(m))
				{
					writer.Write(Id);
					writer.Write(X);
					writer.Write(Y);
					writer.Write(Up);
					writer.Write(Down);
					writer.Write(Left);
					writer.Write(Right);
					writer.Write(Shot);
					writer.Write(Hit);
					writer.Write(Move);
					writer.Write(Tank_Fire);
				}
				return m.ToArray();
			}
		}
		public void Desserialize(byte[] data)
		{
			
			using (MemoryStream m = new MemoryStream(data))
			{
				using (BinaryReader reader = new BinaryReader(m))
				{
					Id = reader.ReadString();
					X = reader.ReadInt32();
					Y = reader.ReadInt32();
					Up = reader.ReadBoolean();
					Down = reader.ReadBoolean();
					Left = reader.ReadBoolean();
				    Right = reader.ReadBoolean();
					Shot = reader.ReadBoolean();
					Hit = reader.ReadBoolean();
					Move = reader.ReadString();
					Tank_Fire = reader.ReadBoolean();
				}
			}
		}
		public  void Orientation()
		{
			Bitmap bitmap = new Bitmap(picture.Image);
			if (Move == "Up")
			{
				Up = true;
				if (Left)
				{
					Left = false;
					bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
					picture.Image = bitmap;
				}
				if (Down)
				{
					Down = false;
					bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
					picture.Image = bitmap;
				}
				if (Right)
				{
					Right = false;
					bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
					picture.Image = bitmap;
				}
			}
			else if (Move == "Right")
			{
				Right = true;
				if (Up)
				{
					Up = false;
					bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
					picture.Image = bitmap;
				}
				if (Down)
				{
					Down = false;
					bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
					picture.Image = bitmap;
				}
				if (Left)
				{
					Left = false;
					bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
					picture.Image = bitmap;
				}
			}
			else if (Move == "Down")
			{
				Down = true;

				if (Left)
				{
					Left = false;
					bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
					picture.Image = bitmap;

				}
				if (Right)
				{
					Right = false;
					bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
					picture.Image = bitmap;
				}
				if (Up)
				{
					Up = false;
					bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
					picture.Image = bitmap;
				}
			}
			else if (Move == "Left")
			{

				Left = true;
				if (Up)
				{
					Up = false;
					bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
					picture.Image = bitmap;
				}
				if (Right)
				{
					Right = false;
					bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
					picture.Image = bitmap;
				}
				if (Down)
				{
					Down = false;
					bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
					picture.Image = bitmap;
				}

			}

		}



		public static Player operator +(Player player, Player player1)
		{
			player.Id = player1.Id;
			player.X = player1.X;
			player.Y = player1.Y;
			player.Down = player1.Down;
			player.Up = player1.Up;
			player.Left = player1.Left;
			player.Right = player1.Right;
			player.Hit = player1.Hit;
			player.Move = player1.Move;
			player.Tank_Fire = player1.Tank_Fire;
			return player;
		}
	}

}
