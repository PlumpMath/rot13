using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace rot13
{
    class Rot13Encoder :NotifiableObject
    {
        public ICommand Encode { get; }
        public ICommand PasteRot13 { get;}

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
         
        public Rot13Encoder()
        {
            Encode = new CommandHook(_ => DoEncode(), _ => CanEncode(), this);
            PasteRot13 = new CommandHook(_ => PasteAndEncode());
            PasteAndEncode(); // start up by en/decoding the clipboard
        }

        private bool CanEncode()
            => !string.IsNullOrWhiteSpace(Text);

        private void DoEncode()
            => this.Text = this.Text.Rot13();

        private void PasteAndEncode()
        {
            if (Clipboard.ContainsText())
            {
                this.Text = Clipboard.GetText().Rot13();
            }
        }

    }
}
