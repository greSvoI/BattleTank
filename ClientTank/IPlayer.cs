using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTank
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
		 string Move { get; set; }
		bool Tank_Fire { get; set; }
		 PictureBox picture { get; set; }
		 byte[] Serialize();
		void Desserialize(byte[]data);
		void Orientation();
		
	}
}
