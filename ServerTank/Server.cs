
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerTank
{
	public class Server
	{
		TcpListener tcpListener;
		IPAddress iPAddress = IPAddress.Parse("192.168.0.103");
		List<ClientTank> clients = new List<ClientTank>();
		public Random random = new Random();
		protected internal void AddConnection(ClientTank item)
		{
			clients.Add(item);
			
		}
		protected internal async void GetOpponent(ClientTank client)
		{
			foreach(ClientTank item in clients)
			{
				byte[] data = item.player.Serialize();
				//if(item.id != client.id)
				await client.networkStream.WriteAsync(data, 0, data.Length);
			}
		}
		protected internal void Disconnect(ClientTank client)
		{
			clients.Remove(client);
		}
		protected internal void Disconnect()
		{
			tcpListener.Stop();
			for (int i = 0; i < clients.Count; i++)
			{
				clients[i].Close();
			}
			Environment.Exit(0);
		}
		protected internal void  BroadCast(ClientTank client)
		{
			byte[] data = client.player.Serialize();
			foreach (var item in clients)
			{
					//if(item.id != client.id)  //Самого себя не передаст
					item.networkStream.Write(data, 0, data.Length);
			}
		}
		protected internal bool Collision(ClientTank client)
		{
			foreach(ClientTank item in clients)
			{
				if(item.id != client.player.Id)
				{
					double res = Math.Sqrt(Math.Pow((client.player.X) - item.player.X, 2) + Math.Pow(client.player.Y - item.player.Y, 2));
					if (res < 45)
						return true;
					else 
						return false;
				}

			}
			return false;
		}
		protected internal void ListenTcp()
		{
			try
			{
				this.tcpListener = new TcpListener(IPAddress.Any, 8000);
				tcpListener.Start();
				while (true)
				{
					TcpClient tcpClient = tcpListener.AcceptTcpClient();
					ClientTank client = new ClientTank(this, tcpClient);
					Thread thread = new Thread(new ThreadStart(client.Process));
					thread.Start();
				}

			}
			catch (Exception ex)
			{
				Disconnect();
				Console.WriteLine(ex.Message);
			}
		}
	}
}
