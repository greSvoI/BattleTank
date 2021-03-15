using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTank
{
	interface IPlayer
	{
		string Id { get; set; }
		int X { get; set; }
		int Y { get; set; }
		bool Up { get; set; }
		bool Down { get; set; }
		bool Left { get; set; }
		bool Right { get; set; }
		bool Shot { get; set; }
		bool Hit { get; set; }
		bool Tank_Fire { get; set; }
		string Move { get; set; }
		
		byte[] Serialize();
		Player Desserialize(byte[] data);
	}
}
