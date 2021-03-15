using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ServerTank
{
	public class ClientTank
	{
		Server server;
		public Player player;
	    TcpClient client;
		public NetworkStream networkStream { get; set; }
		public string id;
		
		public ClientTank(Server server,TcpClient tcpClient)
		{
			player = new Player();
		//	this.address = IPAddress.Parse(tcpClient.Client.RemoteEndPoint.ToString());
			player.Id = tcpClient.Client.RemoteEndPoint.ToString();

			this.server = server;

			this.client = tcpClient;

			this.id = tcpClient.Client.RemoteEndPoint.ToString();

			server.AddConnection(this);

			networkStream = client.GetStream();


			player.X = server.random.Next(130)*5;

			player.Y = server.random.Next(130)*5;

			player.Hit = false;

			byte[] data = player.Serialize();

			networkStream.Write(data,0,data.Length);

			server.BroadCast(this);

			server.GetOpponent(this);
		}
		protected internal void Process()
		{
			try
			{	
				
				while (true)
				{
					try
					{

						GetAction();
						if (player.Shot)
						{
							
							server.BroadCast(this);
							player.Shot = false;
						}
						//else if(player.Hit)
						//{
						//	server.BroadCast(this);
						//	player.Hit = true;
						//}
						else
						{
							ActionHandler();
							server.BroadCast(this);
						}

					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						Close();
						break;
					}
					//finally
					//{
					//	client?.Close();
					//}
				}
			}
			catch (Exception ex)
			{

				
				Close();
			}
		}
		/// <summary> 
		/// Размер окна
		/// X 740
		/// Y 720
		/// </summary>
		protected internal void ActionHandler()
		{
			if (player.Move == "Up" && player.Y >=5)
			{
				player.Y -= 5;
				if (server.Collision(this))
				{
					player.Y += 5;
				}

			}	
			else if (player.Move == "Down" && player.Y <=715)
			{
				player.Y += 5;
				if (server.Collision(this))
				{
					player.Y -= 5;
				}
			}
			else if (player.Move == "Left" && player.X >=5)
			{
				player.X -= 5;
				if (server.Collision(this))
				{
					player.X += 5;
				}
				
			}
			else if (player.Move == "Right" && player.X<=735)
			{
				player.X += 5;
				if (server.Collision(this))
				{
					player.X -= 5;
				}
			}
		}
		protected internal void GetAction()
		{
			byte[] data = new byte[1024];
			do
			{
				networkStream.Read(data, 0, data.Length);
				player = player.Desserialize(data);

			} while (networkStream.DataAvailable);

		}
		internal void Close()
		{
			networkStream?.Close();
			client?.Close();
			server.Disconnect(this);
		}

	}
}
