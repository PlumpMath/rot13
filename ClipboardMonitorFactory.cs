using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rot13
{
    /// <summary>
    /// Get a valid flavor of monitor from here
    /// </summary>
    public static class ClipboardMonitorFactory
    {
        // Return legacy version for Windows XP or older
        public static IClipboardMonitor Create()
             => Environment.OSVersion.Version.Major <= 5 
                ? new LegacyClipboardMonitor() as IClipboardMonitor
                : new ClipboardMonitor() as IClipboardMonitor;
        // Environment.OSVersion.Version will always return 6.2 as the version,
        // even if the OS is Windows 8.1 or 10, as there is no manifest file in
        // the application resources that explicitly claims support for newer
        // versions of Windows.
    }
}
