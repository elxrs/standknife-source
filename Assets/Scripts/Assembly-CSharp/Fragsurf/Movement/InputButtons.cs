using System;

namespace Fragsurf.Movement
{
	[Flags]
	public enum InputButtons
	{
		None = 0,
		Jump = 2,
		Duck = 4,
		Speed = 8,
		MoveLeft = 0x10,
		MoveRight = 0x20,
		MoveForward = 0x40,
		MoveBack = 0x80
	}
}
