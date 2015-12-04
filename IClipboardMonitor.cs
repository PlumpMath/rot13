using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rot13
{
    public interface IClipboardMonitor : IDisposable
    {
        // simplest possible flag to signal containing code to do something with the clipboard
        // whatver handles this event will check to see what's in the clipboard so unless there
        // are tons of event observers it would be redundant and long-winded to check here...
        event EventHandler<EventArgs> ClipboardHasData;
    }
}
