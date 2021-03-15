using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTank.Model.Cannon
{
	class CannonLV1 : ICannon
	{
		public Point point;
		private Brush brush;
		public int X { get; set; }
		public int Y { get; set; }
		public Point Point { get => point; set { point = value; } }
		public Brush Brush { get => brush; set { brush = value; } }
		public string ID { get ; set ; }
		public string Move { get ; set ; }

		public CannonLV1(Player player,Brush brush)
		{
			this.ID = player.Id;
			this.Move = player.Move;
			this.Brush = brush;
			this.X = player.X + 15;
			this.Y = player.Y + 5;
			Point = new Point(X,Y);
			Game.MyPictureBox.Paint += MyPictureBox_Paint;
		}
		private void MyPictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			e.Graphics.FillEllipse(Brush, Point.X, Point.Y, 10, 10);
		}
		public void Dispose()
		{
			Game.MyPictureBox.Paint -= MyPictureBox_Paint;
		}

		public bool Outside()
		{
			if (Point.X > 800 && Point.X < 0&&Point.Y<0 && Point.Y>800)
				return true;
			
			return false;
		}
	}
}
