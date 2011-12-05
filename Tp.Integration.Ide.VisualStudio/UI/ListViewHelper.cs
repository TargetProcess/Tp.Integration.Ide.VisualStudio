using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Tp.Integration.Ide.VisualStudio.UI {
	/// <summary>
	/// See <a href="http://www.thebitguru.com/articles/16-How+to+Set+ListView+Column+Header+Sort+Icons+in+C%23/210-Details">article</a> for details.
	/// </summary>
	internal static class ListViewHelper {
		public static void SetSortIcons(ListView listView, int previouslySortedColumn, int newSortColumn, SortOrder sorting) {
			IntPtr headerHandle = SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
			IntPtr newColumn = new IntPtr(newSortColumn);
			IntPtr prevColumn = new IntPtr(previouslySortedColumn);
			HDITEM hditem;
			// Only update the previous item if it existed and if it was a different one.
			if (previouslySortedColumn != -1 && previouslySortedColumn != newSortColumn) {
				// Clear icon from the previous column.
				hditem = new HDITEM();
				hditem.mask = HDI_FORMAT;
				ItemSendMessage(headerHandle, HDM_GETITEM, prevColumn, ref hditem);
				hditem.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP;
				ItemSendMessage(headerHandle, HDM_SETITEM, prevColumn, ref hditem);
			}
			// Set icon on the new column.
			hditem = new HDITEM();
			hditem.mask = HDI_FORMAT;
			ItemSendMessage(headerHandle, HDM_GETITEM, newColumn, ref hditem);
			if (sorting == SortOrder.Ascending) {
				hditem.fmt &= ~HDF_SORTDOWN;
				hditem.fmt |= HDF_SORTUP;
			}
			else {
				hditem.fmt &= ~HDF_SORTUP;
				hditem.fmt |= HDF_SORTDOWN;
			}
			ItemSendMessage(headerHandle, HDM_SETITEM, newColumn, ref hditem);
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct HDITEM {
			public Int32 mask;
			public Int32 cxy;

			[MarshalAs(UnmanagedType.LPTStr)]
			public String pszText;

			public IntPtr hbm;
			public Int32 cchTextMax;
			public Int32 fmt;
			public Int32 lParam;
			public Int32 iImage;
			public Int32 iOrder;
		} ;

		// Parameters for ListView-Headers
		public const Int32 HDI_FORMAT = 0x0004;
		public const Int32 HDF_LEFT = 0x0000;
		public const Int32 HDF_STRING = 0x4000;
		public const Int32 HDF_SORTUP = 0x0400;
		public const Int32 HDF_SORTDOWN = 0x0200;
		public const Int32 LVM_GETHEADER = 0x1000 + 31; // LVM_FIRST + 31
		public const Int32 HDM_GETITEM = 0x1200 + 11; // HDM_FIRST + 11
		public const Int32 HDM_SETITEM = 0x1200 + 12; // HDM_FIRST + 12

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		private static extern IntPtr ItemSendMessage(IntPtr Handle, Int32 msg, IntPtr wParam, ref HDITEM lParam);
	}
}