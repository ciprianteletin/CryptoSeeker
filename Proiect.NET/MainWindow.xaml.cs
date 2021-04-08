using System.Windows;
using System.Configuration;

namespace Proiect.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Navigate(new Login());
        }
    }
}

