using ClientTank.Model;
using ClientTank.Model.Cannon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTank
{
	class Battle
	{
	    static int port = 8000;
		public TcpClient tcpClient;
		public Player player = new Player();
		public Tank tank = new Tank();
		public NetworkStream networkStream;
		IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.0.103"),port);
		public List<Player> players = new List<Player>();
		public List<ICannon> Cannons = new List<ICannon>();
		public Battle()
		{
			tcpClient = new TcpClient();
			tcpClient.Connect(IPAddress.Parse("192.168.0.103"), port);
			networkStream = tcpClient.GetStream();

			player.picture = new PictureBox();

			GetPlayer();
			player.picture.Image = Image.FromFile("green.png");
			player.picture.SizeMode = PictureBoxSizeMode.StretchImage;
			player.picture.Size = new Size(40, 40);
			player.picture.Location = new Point(player.X, player.Y);
			Game.MyPictureBox.Controls.Add(player.picture);
			players.Add(player);
		}
		private void GetPlayer()
		{
			byte[] data = new byte[1024];
			networkStream.Read(data, 0, data.Length);
			player.Desserialize(data);
		}

		private void Process(byte[]data)
		{
			Player temp = new Player();
			temp.Desserialize(data);
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i].Id == temp.Id)
				{
					if (temp.Hit && !temp.Tank_Fire)
					{
						Game.MyPictureBox.Invoke(new Action(() =>
						{
							Bitmap bitmap = new Bitmap(Image.FromFile("boom.png"));
							players[i].picture.Image = bitmap;
						}));
					}
					else if (!temp.Hit && temp.Tank_Fire)
					{
						Game.MyPictureBox.Invoke(new Action(() =>
						{
							Bitmap bitmap;
							if (players[i].Id == player.Id)
							{
								bitmap = new Bitmap(Image.FromFile("green.png"));
								players[i].picture.Image = bitmap;
								players[i].Up = true;
								players[i].Down = false;
								players[i].Right = false;
								players[i].Left = false;
								players[i].Move = "Up";
								players[i].Tank_Fire = false;
							}
							else
							{
								bitmap = new Bitmap(Image.FromFile("red.png"));
								players[i].picture.Image = bitmap;
								players[i].picture.Image = bitmap;
								players[i].Up = true;
								players[i].Down = false;
								players[i].Right = false;
								players[i].Left = false;
								players[i].Move = "Up";
								players[i].Tank_Fire = false;
							}


						}));
					}
					else if (temp.Shot)
					{
						Game.MyPictureBox.Invoke(new Action(() =>
						{
							if (players[i].Id == player.Id)
								Cannons.Add(new CannonLV1(players[i], Brushes.Green));
							else
								Cannons.Add(new CannonLV1(players[i], Brushes.Red));
						}));
					}
					else
					{
						Game.MyPictureBox.Invoke(new Action(() =>
						{
							players[i] += temp;
							players[i].Orientation();
							players[i].picture.Location = new System.Drawing.Point(temp.X, temp.Y);
						}));
					}

				}
			
				//if (true)
				//{
				//	Game.MyPictureBox.Invoke(new Action(() =>
				//	{
				//		Bitmap bitmap;
				//		if (players[i].Id == player.Id)
				//		{
				//			bitmap = new Bitmap(Image.FromFile("green.png"));
				//			players[i].picture.Image = bitmap;
				//			players[i].Up = true;
				//			players[i].Down = false;
				//			players[i].Right = false;
				//			players[i].Left = false;
				//			players[i].Move = "Up";

				//		}
				//		else
				//		{
				//			bitmap = new Bitmap(Image.FromFile("red.png"));
				//			players[i].picture.Image = bitmap;
				//			players[i].picture.Image = bitmap;
				//			players[i].Up = true;
				//			players[i].Down = false;
				//			players[i].Right = false;
				//			players[i].Left = false;
				//			players[i].Move = "Up";
				//		}


				//	}));

				

			}
		
			if (this.player.Id != temp.Id)
			{
				var flag = players.Find(x => x.Id == temp.Id);
				if (flag == null)
				{
					Player player = new Player(temp);
					Game.MyPictureBox.Invoke(new Action(() => { player.Orientation(); }));
					players.Add(player);
				}
			}
		}
		public async void GetAction()
		{	
			try
			{
					while (true)
					{
						byte[] data = new byte[1024];
						do
						{
							networkStream.Read(data, 0, data.Length);

						} while (networkStream.DataAvailable);

					await Task.Run(()=> { Process(data); }); 

					}
			}
			catch (Exception ex)
			{
					//MessageBox.Show(ex.Message + "GetAction");
			}
			
		}
	}
}
