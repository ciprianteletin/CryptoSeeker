using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proiect.NET
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private String Username;
        private Thread TimerThread;

        public MainMenu()
        {
            InitializeComponent();
            this.Timer.Content = "00:00:00";
            InitThread();
        }

        public MainMenu(String username): this()
        {
            this.Username = username;
            this.UsernameLabel.Content = username;
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "Password")
            {
                Password.Password = "";
                Password.Foreground = new SolidColorBrush(Color.FromRgb(177, 218, 187));
            }
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Password.Password == "")
            {
                Password.Password = "Password";
                Password.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void ConfirmPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ConfirmPassword.Password == "")
            {
                ConfirmPassword.Password = "Password";
                ConfirmPassword.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void ConfirmPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ConfirmPassword.Password == "Password")
            {
                ConfirmPassword.Password = "";
                ConfirmPassword.Foreground = new SolidColorBrush(Color.FromRgb(177, 218, 187));
            }
        }

        private void TimerStatus()
        {
            int seconds = 0, minutes = 0, hours = 0;
            while(true)
            {
                ++seconds;
                if(seconds == 60)
                {
                    seconds = 0;
                    ++minutes;
                }

                if(minutes == 60)
                {
                    minutes = 0;
                    hours++;
                }
                this.Dispatcher.Invoke((Action)(() =>
                {
                    this.Timer.Content = Utils.formatTimer(hours) + ":" + Utils.formatTimer(minutes) + ":" + Utils.formatTimer(seconds);
                }));
                Thread.Sleep(1000);
            }
        }

        private void InitThread()
        {
            TimerThread = new Thread(TimerStatus);
            TimerThread.Start();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            CloseThread();
            this.NavigationService.Navigate(new Uri("Login.xaml", UriKind.Relative));
        }

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CloseThread();
            System.Windows.Application.Current.Shutdown();
        }

        private void CloseThread()
        {
            try
            {
                TimerThread.Abort();
            }
            catch (ThreadAbortException E)
            {
                Console.WriteLine(E.ExceptionState);
            }
        }
    }
}
