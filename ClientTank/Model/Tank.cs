using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTank.Model
{
	class Tank : IMachines
	{
		public Player Player { get; set; }
		public ICannon Cannon { get ; set; }
		public PictureBox PictureBox { get ; set; }

		public Tank()
		{

		}
	}
}
