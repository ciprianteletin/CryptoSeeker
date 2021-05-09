using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace Proiect.NET
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private String Username;
        private Thread TimerThread;
        private CryptoLinqDataContext cryptoLinq;

        public MainMenu()
        {
            InitializeComponent();
            cryptoLinq = new CryptoLinqDataContext(Constants.connString);
            HideFields();
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

        private void UsernameHistory_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameHistory.Text == "")
            {
                UsernameHistory.Text = "Username";
                UsernameHistory.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void UsernameHistory_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameHistory.Text == "Username")
            {
                UsernameHistory.Text = "";
                UsernameHistory.Foreground = new SolidColorBrush(Color.FromRgb(177, 218, 187));
            }
        }

        private void UsernameAccount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameDeleteAccount.Text == "")
            {
                UsernameDeleteAccount.Text = "Username";
                UsernameDeleteAccount.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void UsernameAccount_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameDeleteAccount.Text == "Username")
            {
                UsernameDeleteAccount.Text = "";
                UsernameDeleteAccount.Foreground = new SolidColorBrush(Color.FromRgb(177, 218, 187));
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

        private void UpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            if(!ValidatePassword())
            {
                return;
            }
            User userByUsername = cryptoLinq.Users.FirstOrDefault(u => u.username.Equals(Username));
            string encryptedPassword = Utils.EncryptPassword(Password.Password);
            userByUsername.password = encryptedPassword;
            this.cryptoLinq.SubmitChanges();
            this.ErrorLabel.Content = "Password changed with success!";
        }

        private Boolean ValidatePassword()
        {
            StringBuilder sb = new StringBuilder();
            if (Password.Password.Length < 8)
            {
                sb.Append("Password must have at least 8 characters!\n");
            }

            if (Password.Password != ConfirmPassword.Password)
            {
                sb.Append("Password and Confirm Password must match!");
            }

            if(sb.Length != 0)
            {
                this.ErrorLabel.Content = sb.ToString();
                return false;
            }

            return true;
        }

        private void HideFields()
        {
            this.DisplayUpdatePassword(false);
            this.DisplayDeleteHistory(false);
            this.DisplayDeleteAccount(false);
        }

        private void DisplayUpdatePassword(bool value)
        {
            System.Windows.Visibility prop = GetVisibleProperty(value);

            this.Password.IsEnabled = value;
            this.Password.Visibility = prop;
            this.ConfirmPassword.IsEnabled = value;
            this.ConfirmPassword.Visibility = prop;

            this.PasswordLabel.Visibility = prop;
            this.ConfirmPasswordLabel.Visibility = prop;
            this.ErrorLabel.Visibility = prop;

            this.UpdatePassword.Visibility = prop;
            this.UpdatePassword.IsEnabled = value;
        }

        private void DisplayDeleteHistory(bool value)
        {
            System.Windows.Visibility prop = GetVisibleProperty(value);

            this.DeleteHistoryLabel.Visibility = prop;
            this.DeleteHistoryBtn.IsEnabled = value;
            this.DeleteHistoryBtn.Visibility = prop;
            this.UsernameHistory.Visibility = prop;
        }

        private void DisplayDeleteAccount(bool value)
        {
            System.Windows.Visibility prop = GetVisibleProperty(value);

            this.DeleteAccountLabel.Visibility = prop;
            this.UsernameDeleteAccount.Visibility = prop;
            this.DeleteAccountBtn.Visibility = prop;
            this.DeleteAccountBtn.IsEnabled = value;
        }

        public static System.Windows.Visibility GetVisibleProperty(bool value)
        {
            if (value)
            {
                return System.Windows.Visibility.Visible;
            }

            return System.Windows.Visibility.Hidden;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayUpdatePassword(true);
            this.DisplayDeleteHistory(false);
            this.DisplayDeleteAccount(false);
        }

        private void DeleteHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
           if(!this.Username.Equals(this.UsernameHistory.Text))
            {
                return;
            }
        }

        private void DeleteHistory_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayDeleteHistory(true);
            this.DisplayUpdatePassword(false);
            this.DisplayDeleteAccount(false);
        }

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            this.DisplayDeleteAccount(true);
            this.DisplayDeleteHistory(false);
            this.DisplayUpdatePassword(false);        }

        private void DeleteAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!this.Username.Equals(this.UsernameDeleteAccount.Text))
            {
                return;
            }
            User userByUsername = cryptoLinq.Users.FirstOrDefault(u => u.username.Equals(Username));
            this.cryptoLinq.Users.DeleteOnSubmit(userByUsername);
            this.cryptoLinq.SubmitChanges();
            this.NavigationService.Navigate(new Uri("Login.xaml", UriKind.Relative));
        }
    }
}
