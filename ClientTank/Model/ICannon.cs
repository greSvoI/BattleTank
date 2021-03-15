using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTank
{
	interface ICannon : IDisposable
	{
		int X { get; set; }
		int Y { get; set; }
		string ID { get; set; }//имя игрока, кто выстрели
		string Move { get; set; }//направление выстрела
		Point Point { get; set; }//позиция
		Brush Brush { get; set; }//цвет
		bool Outside();

	}
}
