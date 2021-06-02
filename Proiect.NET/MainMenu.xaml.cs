using System;
using System.Collections.Generic;
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
        private String SelectedCoin; //used for pdf
        private decimal currentPrice; //used for history
        private Thread TimerThread;
        private CryptoLinqDataContext CryptoLinq;
        private CoinMarket Api;

        public MainMenu()
        {
            InitializeComponent();
            CryptoLinq = new CryptoLinqDataContext(Constants.connString);
            HideFields();
            HideGrids();
            this.CryptoGrid.Visibility = GetVisibleProperty(true);
            var plt = CryptoChart.plt;
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            plt.PlotScatter(dataX, dataY);
            this.Timer.Content = "00:00:00";
            InitThread();
        }

        public MainMenu(String username): this()
        {
            this.Username = username;
            this.UsernameLabel.Content = username;
            InitComboBox();
            InitHistory();
            this.Api = new CoinMarket();
            UpdateChart();
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
            Application.Current.Shutdown();
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
            User userByUsername = CryptoLinq.Users.FirstOrDefault(u => u.username.Equals(Username));
            string encryptedPassword = Utils.EncryptPassword(Password.Password);
            userByUsername.password = encryptedPassword;
            this.CryptoLinq.SubmitChanges();
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

        private void HideGrids()
        {
            Visibility prop = Visibility.Hidden;
            this.SettingsGrid.Visibility = prop;
            this.HistoryGrid.Visibility = prop;
        }

        private void DisplayUpdatePassword(bool value)
        {
            Visibility prop = GetVisibleProperty(value);

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
            Visibility prop = GetVisibleProperty(value);

            this.DeleteHistoryLabel.Visibility = prop;
            this.DeleteHistoryBtn.IsEnabled = value;
            this.DeleteHistoryBtn.Visibility = prop;
            this.UsernameHistory.Visibility = prop;
        }

        private void DisplayDeleteAccount(bool value)
        {
            Visibility prop = GetVisibleProperty(value);

            this.DeleteAccountLabel.Visibility = prop;
            this.UsernameDeleteAccount.Visibility = prop;
            this.DeleteAccountBtn.Visibility = prop;
            this.DeleteAccountBtn.IsEnabled = value;
        }

        public static Visibility GetVisibleProperty(bool value)
        {
            if (value)
            {
                return Visibility.Visible;
            }

            return Visibility.Hidden;
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
            User userByUsername = CryptoLinq.Users.First(u => u.username.Equals(Username));
            CryptoLinq.ExecuteCommand("DELETE FROM dbo.history where user_Id = {0}", userByUsername.Id);
            CryptoLinq.SubmitChanges();
            DeleteHistoryLabel.Content = "History deleted with success!";
            this.HistoryGrid.Items.Clear();
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
            this.DisplayUpdatePassword(false);        
        }

        private void DeleteAccountBtn_Click(object sender, RoutedEventArgs e)
        {
            if(!this.Username.Equals(this.UsernameDeleteAccount.Text))
            {
                return;
            }
            User userByUsername = CryptoLinq.Users.First(u => u.username.Equals(Username));
            this.CryptoLinq.Users.DeleteOnSubmit(userByUsername);
            this.CryptoLinq.SubmitChanges();
            this.TimerThread.Abort();
            this.NavigationService.Navigate(new Uri("Login.xaml", UriKind.Relative));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.HideFields();
            this.SettingsGrid.Visibility = GetVisibleProperty(true);
            this.CryptoGrid.Visibility = GetVisibleProperty(false);
            this.HistoryGrid.Visibility = GetVisibleProperty(false);
        }

        private void InitComboBox()
        {
            foreach(var combo in CoinMarket.coins)
            {
                this.ComboCrypto.Items.Add(combo);
            }
        }

        private void UpdateChart()
        {
            var plt = CryptoChart.plt;
            plt.Clear();
            Api.UpdateData();
            dynamic[] coins = Api.GetMyCoins();
            SelectedCoin = this.ComboCrypto.Text;

            dynamic coin = coins[0]["name"];
            for (int j = 0; j < coins.Length; j++)
            {
                string name = coins[j]["name"];

                if (String.Equals(name, SelectedCoin))
                {
                    coin = coins[j];
                }
            }
            string[] labels = { "90 days", "30 days", "7 days", "24 h", "1h", "current" };

            double currentPrice = coin["quote"]["USD"]["price"];
            this.currentPrice = (decimal)currentPrice;
            double price1H = coin["quote"]["USD"]["percent_change_1h"];
            price1H = Math.Abs((currentPrice * (100 - price1H)) / 100);
            double price24H = coin["quote"]["USD"]["percent_change_24h"];
            price24H = Math.Abs((currentPrice * (100 - price24H)) / 100);
            double price7D = coin["quote"]["USD"]["percent_change_7d"];
            price7D = Math.Abs((currentPrice * (100 - price7D)) / 100);
            double price30D = coin["quote"]["USD"]["percent_change_30d"];
            price30D = Math.Abs((currentPrice * (100 - price30D)) / 100);
            double price90D = coin["quote"]["USD"]["percent_change_90d"];
            price90D = Math.Abs((currentPrice * (100 - price90D)) / 100);

            double[] dataX = new double[] { 0, 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { price90D, price30D, price7D, price24H, price1H, currentPrice };
            plt.PlotScatter(dataX, dataY);
            plt.AxisAuto();
            plt.XTicks(labels);
            plt.Style(ScottPlot.Style.Blue2);
            CryptoChart.Render();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateChart();
            AddHistory();
        }

        private void AddHistory()
        {
            history crtHistory = new history();
            DateTime crtDate = DateTime.Now;
            crtHistory.currency_name = this.SelectedCoin;
            crtHistory.price = this.currentPrice;
            crtHistory.search_date = crtDate;
            User userByUsername = CryptoLinq.Users.First(u => u.username.Equals(Username));
            crtHistory.user_id = userByUsername.Id;

            CryptoLinq.histories.InsertOnSubmit(crtHistory);
            CryptoLinq.SubmitChanges();

            this.HistoryGrid.Items.Add(crtHistory);
        }

        private void CryptoBtn_Click(object sender, RoutedEventArgs e)
        {
            this.SettingsGrid.Visibility = GetVisibleProperty(false);
            this.HistoryGrid.Visibility = GetVisibleProperty(false);
            this.CryptoGrid.Visibility = GetVisibleProperty(true);
        }

        private void PdfBtn_Click(object sender, RoutedEventArgs e)
        {
            var plt = CryptoChart.plt;
            plt.SaveFig("Crypto_Fig.png"); // we have the figure for the pdf
            PDFGenerator.CreateDocument(SelectedCoin, Api.GetMyCoins());
        }

        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            this.SettingsGrid.Visibility = GetVisibleProperty(false);
            this.CryptoGrid.Visibility = GetVisibleProperty(false);
            this.HistoryGrid.Visibility = GetVisibleProperty(true);
        }

        private void InitHistory()
        {
            User userByUsername = CryptoLinq.Users.First(u => u.username.Equals(Username));
            this.HistoryGrid.HeadersVisibility = DataGridHeadersVisibility.Column;
            IEnumerable<history> histories = from hst in CryptoLinq.histories
                           where hst.user_id == userByUsername.Id
                           select hst;
            foreach(history hst in histories)
            {
                this.HistoryGrid.Items.Add(hst);
            }
        }
    }
}
