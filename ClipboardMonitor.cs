using System;
using System.Diagnostics;
using System.Windows.Interop;

namespace rot13
{
    public class ClipboardMonitor : IDisposable
    {
        private HwndSource source; // wrapper for a Win32 window/HWnd
        private IntPtr nextViewer; // next registered clipboard viewer (some other process probably)

        // simplest possible flag to signal containing code to do something with the clipboard
        // whatver handles this event will check to see what's in the clipboard so unless there
        // are tons of event observers it would be redundant and long-winded to check here...
        public event EventHandler<EventArgs> ClipboardHasData;

        public ClipboardMonitor()
        {
            // Create a new window. Name is required, otherwise the window not only won't
            // be shown but it won't be removed when the main window is closed, resulting
            // in a zombie process
            source = new HwndSource(
                new HwndSourceParameters("_")
                {
                    HwndSourceHook = WndProc, // window messages are sent here
                    WindowStyle = 0 // don't put any controls in the window's frame
                                    // and in fact don't even show it.
                }
            );
            // Register this hwnd to get notifications about the clipboard.
            // Method returns the next entity in the clipboard notification chain
            nextViewer = NativeMethods.SetClipboardViewer(source.Handle);
        }

        // this is an HwndSourceHook delegate
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...
            Debug.WriteLine($"Got message {msg}");
            // if we care about them
            switch (msg)
            {
                // Clipboard contents have changed
                case NativeMethods.WM_DRAWCLIPBOARD:
                    Debug.WriteLine("Clipboard updated!");
                    OnClipboardHasData(); // Fire event
                    NativeMethods.SendMessage(nextViewer, msg, wParam, lParam);
                    break;
                // Another entity in the clipboard chain has unregistered.
                // wParam is the unregistering entity and lParam is its 
                // next link. In case wParam is also our next link, update 
                // our records.
                case NativeMethods.WM_CHANGECBCHAIN:
                    Debug.WriteLine("Updating clipboard message chain");
                    if (nextViewer == wParam) nextViewer = lParam;
                    break;
            }
            // We're not actually handling any of these messages so leave the handled
            // flag unchanged and return Zero.
            return IntPtr.Zero;
        }

        private void OnClipboardHasData()
        {
            if (ClipboardHasData != null)
                ClipboardHasData(null, new EventArgs());
        }

        /// <summary>
        /// Not to be called from anywhere except Dispose()
        /// </summary>
        private void DisposeHwndSource()
        {
            // unregister from clipboard notification chain during Dispose() process
            NativeMethods.ChangeClipboardChain(source.Handle, nextViewer);
            source.Dispose();
        }

        #region IDisposable Support
        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    DisposeHwndSource();
                }
                // No unmanaged resources to free; HWndSource takes care of that
                isDisposed = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}