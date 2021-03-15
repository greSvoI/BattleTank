using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTank
{
	public class Player: IPlayer
	{
		public string Id { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public bool Up { get; set; } = true;
		public bool Down { get; set; } = false;
		public bool Left { get; set; } = false;
		public bool Right { get; set; } = false;
		public bool Shot { get; set; } = false;
		public bool Hit { get; set; } = false; //Попадание
		public string Move { get; set; } = "Up";
		public bool Tank_Fire { get; set; } = false; //Танк в огне подбит

		public byte[] Serialize()
		{
			using (MemoryStream m = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(m))
				{
					writer.Write(Id);
					writer.Write(X);
					writer.Write(Y);
					writer.Write(Up);
					writer.Write(Down);
					writer.Write(Left);
					writer.Write(Right);
					writer.Write(Shot);
					writer.Write(Hit);
					writer.Write(Move);
					writer.Write(Tank_Fire);
					
				}
				return m.ToArray();
			}
		}

		public Player Desserialize(byte[] data)
		{
			Player result = new Player();
			using (MemoryStream m = new MemoryStream(data))
			{
				using (BinaryReader reader = new BinaryReader(m))
				{
					result.Id = reader.ReadString();
					result.X = reader.ReadInt32();
					result.Y = reader.ReadInt32();
					result.Up = reader.ReadBoolean();
					result.Down = reader.ReadBoolean();
					result.Left = reader.ReadBoolean();
					result.Right = reader.ReadBoolean();
					result.Shot = reader.ReadBoolean();
					result.Hit = reader.ReadBoolean();
					result.Move = reader.ReadString();
					result.Tank_Fire = reader.ReadBoolean();
				}
			}
			return result;
		}
	}
}
