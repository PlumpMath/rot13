using System.Windows;

namespace Rot13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var v = System.Environment.OSVersion.Version;
        }
    }
}
