using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace rot13
{
    /// <summary>
    /// The ViewModel for rot13.exe. Exposes a Text property which may or may not be encoded and
    /// Encode and PasteRot13 commands for interaction with the UI. 
    /// </summary>
    public class Rot13Encoder : NotifiableObject
    {
        private ClipboardMonitor clippy;

        public ICommand Encode { get; }

        public ICommand PasteRot13 { get; }

        // too much boilerplate!!
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        // Constructor
        public Rot13Encoder()
        {
            // Initialize clipboard monitoring
            clippy = new ClipboardMonitor();
            clippy.ClipboardHasData += ClipboardHasData;
            
            // Initialize commands
            Encode = new CommandHook(_ => DoEncode(), _ => CanEncode(), this);
            PasteRot13 = new CommandHook(_ => PasteAndEncode(), _ => CanPaste());
            
            // start up by en/decoding the clipboard.
            PasteAndEncode(); 
        }

        // When the clipboard has new data, update the command status
        private void ClipboardHasData(object sender, EventArgs e)
            => (PasteRot13 as CommandHook).SuggestCanExecuteChanged();

        // Is there anything to encode?
        private bool CanEncode()
            => !string.IsNullOrWhiteSpace(Text);

        // Do it - all of this extra code to call a single extention method. Tragic. 
        private void DoEncode()
            => this.Text = this.Text.Rot13();
        
        // Enable the paste encoded button, if there's anything to paste
        private bool CanPaste()
            => Clipboard.ContainsText();

        // If there is usable data in the clipboard, encode it and show it
        private void PasteAndEncode()
        {
            if (Clipboard.ContainsText())
            {
                this.Text = Clipboard.GetText().Rot13();
            }
        }
    }
}