using Rot13.ClipboardUtils;
using System;
using System.Windows;
using System.Windows.Input;

namespace Rot13
{
    /// <summary>
    /// The ViewModel for rot13.exe. Exposes a Text property which may or may not be encoded and
    /// Encode and PasteRot13 commands for interaction with the UI. 
    /// </summary>
    public class Rot13Encoder : NotifiableObject, IDisposable
    {
        // Adapter between view and viewmodel for the Encode button
        public ICommand Encode => new CommandHook(_ => DoEncode(), _ => CanEncode(), this);

        // Adapter between view and viewmodel for the PasteRot13 button
        public ICommand PasteRot13 => new CommandHook(_ => PasteAndEncode(), _ => CanPaste());

        // too much boilerplate!! The Text property, which is as close as this gets
        // to the model in the MVVM pattern
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        // Watches for clipboard changes
        private IClipboardMonitor clippy;

        // Constructor
        public Rot13Encoder()
        {
            // Initialize clipboard monitoring
            clippy = ClipboardMonitorFactory.Create();
            clippy.ClipboardHasData += ClipboardHasData;
            
            // start up by en/decoding the clipboard.
            PasteAndEncode(); 
        }

        // *******************************************************************
        // Do it - all of this code to call a single extension method. Tragic.
        // *******************************************************************
        private void DoEncode()
            => this.Text = this.Text.Rot13();

        // Is there anything to encode?
        private bool CanEncode()
            => !string.IsNullOrWhiteSpace(Text);

        // When the clipboard has new data, update the command status
        private void ClipboardHasData(object sender, EventArgs e)
            => (PasteRot13 as CommandHook).SuggestCanExecuteChanged();

        // If there is usable data in the clipboard, encode it and show it
        private void PasteAndEncode()
        {
            if (Clipboard.ContainsText())
            {
                this.Text = Clipboard.GetText().Rot13();
            }
        }

        // Enable the paste encoded button, if there's anything to paste
        private static bool CanPaste()
            => Clipboard.ContainsText();

        #region IDisposable Support
        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    clippy.Dispose();
                }
                isDisposed = true;
            }
        }

        ~Rot13Encoder()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            // Implemented to keep code analysis happy
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}