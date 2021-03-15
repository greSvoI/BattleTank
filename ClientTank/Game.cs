using ClientTank.Model.Cannon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTank
{
	public partial class Game : Form
	{
		Battle client;
		System.Windows.Forms.Timer timer_reload;
		System.Windows.Forms.Timer timer_move_shells;
		bool hit = true;
		int timer_count = 3;
		public static PictureBox MyPictureBox { get; set; }
		
		public Game()
		{
			InitializeComponent();
			MyPictureBox = new PictureBox()
			{
				//Image = Image.FromFile("background.jpg"),
				SizeMode = PictureBoxSizeMode.StretchImage,
				Size = ClientSize
			};
			Controls.Add(MyPictureBox);
			client = new Battle();
			Thread thread = new Thread(new ThreadStart(client.GetAction));
			thread.Start();


			timer_move_shells = new System.Windows.Forms.Timer();
			timer_move_shells.Interval = 10;
			timer_move_shells.Tick += Timer_move_shells_Tick;
			timer_move_shells.Start();


			timer_reload = new System.Windows.Forms.Timer();
			timer_reload.Interval = 500;
			timer_reload.Tick += Timer_Tick;
		}

		private void Timer_move_shells_Tick(object sender, EventArgs e)
		{
			
			for (int i = 0; i < client.Cannons.Count; i++)
			{
				if (client.Cannons[i].ID != client.player.Id)
				{
					double res = Math.Sqrt(Math.Pow(client.Cannons[i].X - client.player.X, 2) + Math.Pow(client.Cannons[i].Y - client.player.Y, 2));
					if (res < 45)
					{
						client.player.Hit = true;
						client.player.Tank_Fire = false;
						byte[] data = client.player.Serialize();
						client.networkStream.Write(data, 0, data.Length);
						client.player.picture.Image = Image.FromFile("boom.png");
						hit = false;
						timer_reload.Start();
						client.Cannons[i].Dispose();
					}
				}
				if (client.Cannons[i].Move == "Down")
				{
					if (client.Cannons[i].Outside()) client.Cannons[i].Dispose();
					else
					{
						client.Cannons[i].Y += 5;
						client.Cannons[i].Point = new Point(client.Cannons[i].X, client.Cannons[i].Y);
					}
				}
				else if(client.Cannons[i].Move == "Up")
				{
					if (client.Cannons[i].Outside()) client.Cannons[i].Dispose();
					else
					{
						client.Cannons[i].Y -= 5;
						client.Cannons[i].Point = new Point(client.Cannons[i].X, client.Cannons[i].Y);
					}
				}
				else if (client.Cannons[i].Move == "Left")
				{
					if (client.Cannons[i].Outside()) client.Cannons[i].Dispose();
					else
					{
						client.Cannons[i].X -= 5;
						client.Cannons[i].Point = new Point(client.Cannons[i].X, client.Cannons[i].Y);
					}
				}

				else if (client.Cannons[i].Move == "Right")
				{
					if (client.Cannons[i].Outside()) client.Cannons[i].Dispose();
					else
					{
						client.Cannons[i].X += 5;
						client.Cannons[i].Point = new Point(client.Cannons[i].X, client.Cannons[i].Y);
					}
				}
				MyPictureBox.Invalidate();
			}
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			labelReload.Visible = true;
			labelReload.Text = "Reload " + timer_count.ToString();
			if (timer_count <=0)
			{
				timer_reload.Stop();
				timer_count = 3;
				labelReload.Visible = false;
				if(!hit)
				{
					hit = true;
					client.player.Hit = false;
					client.player.Tank_Fire = true;
					byte[] data = client.player.Serialize();
					client.networkStream.Write(data, 0, data.Length);
				}

			}
			--timer_count;
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			this.Text ="X: " + client.player.picture.Location.X +" Y: "+client.player.picture.Location.Y.ToString();


			if (e.KeyData == Keys.Up || e.KeyData == Keys.Down || e.KeyData == Keys.Right || e.KeyData == Keys.Left)
			{
				if (e.KeyData == Keys.Up)
					client.player.Move = "Up";
				if (e.KeyData == Keys.Down)
					client.player.Move = "Down";
				if (e.KeyData == Keys.Left)
					client.player.Move = "Left";
				if (e.KeyData == Keys.Right)
					client.player.Move = "Right";
				
				byte[] data = client.player.Serialize();
				client.networkStream.Write(data, 0, data.Length);
			}
			if (e.KeyData == Keys.Space && !timer_reload.Enabled)
			{
				client.player.Shot = true;
				byte[] data = client.player.Serialize();
				client.networkStream.Write(data, 0, data.Length);
				client.player.Shot = false;
				timer_reload.Start();
			}
		}

		private void Game_FormClosing(object sender, FormClosingEventArgs e)
		{
			client.networkStream?.Close();
			client.tcpClient?.Close();
		}
	}
}
