using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerTank
{
	class Program
	{
		
		static void Main(string[] args)
		{
			Server server = new Server();
			Thread thread = new Thread(new ThreadStart(server.ListenTcp));
			thread.Start();



		}
	}
}
