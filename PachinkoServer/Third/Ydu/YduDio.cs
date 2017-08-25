using System;
using System.Runtime.InteropServices;

namespace YduCs
{
	public class YduDio
	{
		///////////////////////////////////////////////////////////////////////////////
		//
		// YduDio.cs
		//
		// Copyright (C) 2015 Y2 Corporation
		//
		///////////////////////////////////////////////////////////////////////////////
		public YduDio(){}
		
		//------------------------------------------------------------------------------
		// API
		//------------------------------------------------------------------------------
		[DllImport("Ydu.DLL")]
        private static extern int YduDioOutput(ushort wUnitID, byte[] pbyData, ushort wStart, ushort wCount);
        public static int Output(ushort wUnitID, byte[] pbyData, ushort wStart, ushort wCount)
        {
            int ret = YduDioOutput(wUnitID, pbyData, wStart, wCount);
            return ret;
        }
        [DllImport("Ydu.DLL")]
        private static extern int YduDioOutputStatus(ushort wUnitID, byte[] pbyData, ushort wStart, ushort wCount);
        public static int OutputStatus(ushort wUnitID, byte[] pbyData, ushort wStart, ushort wCount)
        {
            int ret = YduDioOutputStatus(wUnitID, pbyData, wStart, wCount);
            return ret;
        }
        [DllImport("Ydu.DLL")]
        private static extern int YduDioInput(ushort wUnitID, byte[] pbyData, ushort wStart, ushort wCount);
        public static int Input(ushort wUnitID, byte[] pbyData, ushort wStart, ushort wCount)
        {
            int ret = YduDioInput(wUnitID, pbyData, wStart, wCount);
            return ret;
        }
	}
}

