using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace Proiect.NET
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private CryptoLinqDataContext cryptoLinq;

        public Login()
        {
            InitializeComponent();
            cryptoLinq = new CryptoLinqDataContext(Constants.connString);
        }

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void LoginRedirect_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Register.xaml", UriKind.Relative));
        }

        private void Username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "")
            {
                Username.Text = "Username/Email";
                Username.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void Username_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "Username/Email")
            {
                Username.Text = "";
                Username.Foreground = new SolidColorBrush(Color.FromRgb(177, 218, 187));
            }
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

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;

            User userByUsernameOrEmail = cryptoLinq.Users.FirstOrDefault(u => u.username.Equals(username) || u.email.Equals(username));
            if(userByUsernameOrEmail == null)
            {
                ValidationLabel.Content = "Authentication failed!";
                return;
            }
            string encryptedPassword = Utils.EncryptPassword(password);
            if(!encryptedPassword.Equals(userByUsernameOrEmail.password)) {

                ValidationLabel.Content = "Authentication failed!";
                return;
            }
            MainMenu mainMenu = new MainMenu(username);
            this.NavigationService.Navigate(mainMenu);
        }
    }
}
