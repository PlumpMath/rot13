using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Rot13.ClipboardUtils
{
    /// <summary>
    /// Clipboard monitor for windows 2000/xp
    /// </summary>
    public class LegacyClipboardMonitor : ClipboardMonitorBase
    {
        // The Extended Correctness rule set will complain about this not being a safehandle.
        [SuppressMessage(
            "Microsoft.Reliability", "CA2006:UseSafeHandleToEncapsulateNativeResources",
            Justification= "Subclassing SafeHandle to encapsulate an HWND causes excessive complexity")]
        private IntPtr nextViewer; // next registered clipboard viewer (some other process probably)

        internal LegacyClipboardMonitor()
        {
            // Register this hwnd to get notifications about the clipboard.
            // Method returns the next entity in the clipboard notification chain
            nextViewer = NativeMethods.SetClipboardViewer(source.Handle);
        }

        // this is an HwndSourceHook delegate
        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
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
            // We're not actually changing any of these messages so leave the handled
            // flag unchanged and return Zero.
            return IntPtr.Zero;
        }

        // Called during disposal
        private void UnregisterClipboardChain()
        {
            // unregister from clipboard notification chain during Dispose() process
            NativeMethods.ChangeClipboardChain(source.Handle, nextViewer);
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
                    UnregisterClipboardChain();
                }
                isDisposed = true;
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}