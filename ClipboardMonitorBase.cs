using System;
using System.Windows.Interop;

namespace Rot13
{
    public abstract class ClipboardMonitorBase : IClipboardMonitor
    {
        protected HwndSource source; // wrapper for a Win32 window/HWnd
                                     
        public event EventHandler<EventArgs> ClipboardHasData;

        public ClipboardMonitorBase()
        {
            // Create a new window. Name is required, otherwise the window not only won't
            // be shown but it won't be removed when the main window is closed, resulting
            // in a zombie process!
            source = new HwndSource(
                new HwndSourceParameters("_")
                {
                    HwndSourceHook = WndProc, // window messages are sent here
                    WindowStyle = 0 // don't put any controls in the window's frame
                                    // and in fact don't even show it.
                }
            );
        }

        protected void OnClipboardHasData()
        {
            if (ClipboardHasData != null)
                ClipboardHasData(null, new EventArgs());
        }

        /// <summary>
        /// HwndSourceHook delegate. In a derived class, handle window messages.
        /// </summary>
        protected abstract IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);

        #region IDisposable Support
        private bool isDisposed = false; // To detect redundant calls

        // Virtual, to be overridden by derived class
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    source.Dispose();
                }
                // No unmanaged resources to free; HWndSource takes care of that
                isDisposed = true;
            }
        }
        
        // This code added to correctly implement the disposable pattern.
        // note this is *not* virtual; 
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Called only in event of an instance not being disposed explicitly
        ~ClipboardMonitorBase()
        {
            Dispose(true);
        }
        #endregion
    }
}
