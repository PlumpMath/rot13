using System;
using System.Diagnostics;

namespace Rot13.ClipboardUtils
{
    /// <summary>
    /// Clipboard Monitoring class - uses newer and simpler APIs than the legacy version,
    /// that are less likely to foul up other clipboard users.
    /// </summary>
    public class ClipboardMonitor : ClipboardMonitorBase
    {
        internal ClipboardMonitor()
        {
            NativeMethods.AddClipboardFormatListener(source.Handle);
        }

        /// <summary>
        /// Handle relevant window messages, if any
        /// </summary>
        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...
            Debug.WriteLine($"Got message {msg}");

            // This message means that the clipboard contents have been updated
            if (msg == NativeMethods.WM_CLIPBOARDUPDATE)
                OnClipboardHasData();

            // We're not actually changing any of these messages so leave the handled
            // flag unchanged and return Zero.
            return IntPtr.Zero;
        }

        // Called during disposal
        private void UnregisterClipboardListener()
        {
            // unregister from clipboard notifications during Dispose() process
            NativeMethods.RemoveClipboardFormatListener(source.Handle);
        }

        #region IDisposable Support
        // Base class also has an isDisposed field, but that controls the base's Dispose()
        // mechanism
        private bool isDisposed = false; // To detect redundant calls

        // Called by base.Dispose(); need to make sure we call base.Dispose(bool) so that
        // the base class's resources are also cleaned up
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    UnregisterClipboardListener();
                }
                isDisposed = true;
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}