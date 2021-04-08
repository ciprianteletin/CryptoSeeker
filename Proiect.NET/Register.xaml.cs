using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace Proiect.NET
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {

        private CryptoLinqDataContext cryptoLinq;

        public Register()
        {
            InitializeComponent();
            cryptoLinq = new CryptoLinqDataContext(Constants.connString);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidateInput()
        {
            StringBuilder sb = new StringBuilder();
            if (Username.Text.Length < 5)
            {
                sb.Append("Username must be at least 5 characters!\n");
            }

            if (Password.Password.Length < 8)
            {
                sb.Append("Password must have at least 8 characters!");
            }

            if (Password.Password != ConfirmPassword.Password)
            {
                sb.Append("Password and Confirm Password must match!");
            }

            if (!IsValidEmail(Email.Text))
            {
                sb.Append("\tInvalid email address!");
            }
            // last if, I promise
            if (sb.Length != 0)
            {
                ValidationLabel.Text = sb.ToString();
                return false;
            }

            return true;
        }

        private bool ValidateNewUser()
        {
            User userByUsername = cryptoLinq.Users.FirstOrDefault(u => u.username.Equals(Username.Text));
            if(userByUsername != null)
            {
                ValidationLabel.Text = "Username already used!";
                return false;
            }

            User userByEmail = cryptoLinq.Users.FirstOrDefault(u => u.email.Equals(Email.Text));
            if (userByEmail != null)
            {
                ValidationLabel.Text = "Email already used!";
                return false;
            }

            return true;
        }

        private void Close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void LoginRedirect_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Login.xaml", UriKind.Relative));
        }

        private void Username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "")
            {
                Username.Text = "Username";
                Username.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
        }

        private void Username_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Username.Text == "Username")
            {
                Username.Text = "";
                Username.Foreground = new SolidColorBrush(Color.FromRgb(177, 218, 187));
            }
        }

        private void Email_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Email.Text == "Email")
            {
                Email.Text = "";
                Email.Foreground = new SolidColorBrush(Color.FromRgb(177, 218, 187));
            }
        }

        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Email.Text == "")
            {
                Email.Text = "Email";
                Email.Foreground = new SolidColorBrush(Colors.DarkGray);
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

        private void SaveUser()
        {
            User newUser = new User();
            newUser.username = Username.Text;
            newUser.email = Email.Text;
            newUser.password = Utils.EncryptPassword(Password.Password);
            cryptoLinq.Users.InsertOnSubmit(newUser);

            cryptoLinq.SubmitChanges();
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput() || !ValidateNewUser())
            {
                return;
            }
            SaveUser();

        }
    }
}
