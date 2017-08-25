using System;
using System.Runtime.InteropServices;

namespace YduCs
{
	public class Ydu
	{
		///////////////////////////////////////////////////////////////////////////////
		//
		// Ydu.cs
		//
		// Copyright (C) 2015 Y2 Corporation
		//
		///////////////////////////////////////////////////////////////////////////////
		public Ydu(){}

        //------------------------------------------------------------------------------
        // 定義
        //------------------------------------------------------------------------------

        // Open
        public const ushort YDU_OPEN_NORMAL = 0;          // 通常オープン
        public const ushort YDU_OPEN_OUT_NOT_INIT = 0x01; // 出力初期化しない

        //------------------------------------------------------------------------------
		// API
		//------------------------------------------------------------------------------
		[DllImport("Ydu.DLL")]
        private static extern int YduOpen(ushort wUnitID, string lpszModelName, ushort wMode);
        public static int Open(ushort wUnitID, string lpszModelName, ushort wMode)
        {
            int ret = YduOpen(wUnitID, lpszModelName, wMode);
            return ret;
        }
        public static int Open(ushort wUnitID, string lpszModelName)
        {
            int ret = YduOpen(wUnitID, lpszModelName, YDU_OPEN_NORMAL);
            return ret;
        }
        [DllImport("Ydu.DLL")]
        private static extern bool YduClose(ushort wUnitID);
        public static bool Close(ushort wUnitID)
        {
            bool ret = YduClose(wUnitID);
            return ret;
        }

		//------------------------------------------------------------------------------
		// ERROR CODE
		//------------------------------------------------------------------------------
		public const int YDU_RESULT_SUCCESS			= 0;			                // 正常終了
		public const int YDU_RESULT_ERROR			= unchecked((int)0xCE000001);	// エラー
		public const int YDU_RESULT_NOT_OPEN		= unchecked((int)0xCE000002);	// オープンされていない
		public const int YDU_RESULT_ALREADY_OPEN	= unchecked((int)0xCE000003);	// オープン済み
		public const int YDU_RESULT_INVALID_UNIT_ID	= unchecked((int)0xCE000004);	// ユニットIDが不正
		public const int YDU_RESULT_INVALID_EPNO	= unchecked((int)0xCE000005);	// EP指定が不正(内部エラー)
		public const int YDU_RESULT_CANNOT_OPEN		= unchecked((int)0xCE000006);	// デバイスがオープンできなかった
		public const int YDU_RESULT_EXT_BOARD_OVER	= unchecked((int)0xCE000007);	// 拡張ボード数上限オーバー
		public const int YDU_RESULT_PARAMETER_ERROR	= unchecked((int)0xCE000008);	// 引数不正
		public const int YDU_RESULT_MEM_ALLOC_ERROR	= unchecked((int)0xCE000009);	// メモリ確保エラー
		public const int YDU_RESULT_MODELNAME_ERROR	= unchecked((int)0xCE00000A);	// 型名指定が不正
		public const int YDU_RESULT_HARDWARE_ERROR	= unchecked((int)0xCE00000B);	// ハードウェアエラー
		public const int YDU_RESULT_NOT_SUPPORTED	= unchecked((int)0xCE00000C);	// サポートされていない

		public const int YDU_RESULT_FATAL_ERROR		= unchecked((int)0xCEFFFFFF);	// 致命的エラー
	}
}

