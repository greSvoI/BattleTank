using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTank
{
	interface IMachines
	{
		PictureBox PictureBox { get; set; }
		Player Player { get; set; }
		ICannon Cannon { get; set; }
	}
}
