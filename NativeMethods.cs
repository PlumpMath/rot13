using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Rot13
{
    /// <summary>
    /// Interop with Win32 Clipboard APIs
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Register with the clipboard viewer chain.
        /// </summary>
        /// <param name="hWndNewViewer">Handle to our window</param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        /// <summary>
        /// Unregister from clipboard chain. Call during cleanup.
        /// </summary>
        /// <param name="hWndRemove">Handle to remove (i.e. our window)</param>
        /// <param name="hWndNewNext">Handle to replace ours in the chain</param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        /// <summary>
        /// Send a message to another window. Called by clipboard viewer to forward messages to
        /// other entities in the clipboard chain.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        // defined in winuser.h
        internal const int WM_DRAWCLIPBOARD = 0x0308;
        internal const int WM_CHANGECBCHAIN = 0x030D;
    }
}
