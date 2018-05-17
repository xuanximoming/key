using System;
using System.Runtime.InteropServices;

namespace DrectSoft
{
	internal struct LastInputInfo
	{
		[MarshalAs(UnmanagedType.U4)]
		public int cbSize;

		[MarshalAs(UnmanagedType.U4)]
		public uint dwTime;
	}
}
