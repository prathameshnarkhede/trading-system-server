using IO.ApiClient.Api;
using IO.ApiClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TradingClient.Models;
using TradingClient.WebManager;

namespace TradingClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// Inject data from here
        /// </summary>
        public MainWindowViewModel()
        {
            LoginCommand = new RelayCommand(Login);

            // Automatic updating of User Interface with History and Market values
            timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// Timer event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer_Tick(object sender, EventArgs e)
        {
            var orders = await TransactionsWebManager.GetMarketOrders();
            OrderBooks = new ObservableCollection<MarketOrderBook>(orders);

            var history = await TransactionsWebManager.GetHistory();
            TradeHistory = new ObservableCollection<Trade>(history);
        }

        /// <summary>
        /// Starts the timer with real time data for logged in user.
        /// </summary>
        /// <param name="obj"></param>
        private void Login(object obj)
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                MessageBox.Show("Invalid User Name", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            timer.Start();
        }

        /// <summary>
        /// UserName or ID
        /// </summary>
        public string UserName
        {
            get => TransactionsWebManager.UserId;
            set
            {
                TransactionsWebManager.UserId = value;
                RaisePropertyChanged(nameof(UserName));
            }
        }


        public IEnumerable<SideEnum> StrategyTypes =>
            Enum.GetValues(typeof(SideEnum)).Cast<SideEnum>();

        private Order order = new Order(string.Empty, "Stock A", 0, 0.0);

        public Order Order
        {
            get => order;
            set
            {
                order = value;
                RaisePropertyChanged(nameof(Order));
            }
        }

        private ObservableCollection<Trade> tradeHistory = new ObservableCollection<Trade>();

        public ObservableCollection<Trade> TradeHistory
        {
            get => tradeHistory;
            set
            {
                tradeHistory = value;
                RaisePropertyChanged(nameof(TradeHistory));
            }
        }

        private ObservableCollection<MarketOrderBook> orderBooks = new ObservableCollection<MarketOrderBook>();

        public ObservableCollection<MarketOrderBook> OrderBooks
        {
            get => orderBooks;
            set
            {
                orderBooks = value;
                RaisePropertyChanged(nameof(OrderBooks));
            }
        }


        #region ICommand
        public override async void Submit(object obj)
        {
            if (Order.Price is 0 || Order.Quantity is 0)
            {
                MessageBox.Show("Invalid Data", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Order.UserId = UserName;
            await TransactionsWebManager.PostOrder(Order);
        }
        #endregion

        ~MainWindowViewModel()
        {
            timer.Stop();
            timer.Tick -= Timer_Tick;
        }
    }
}
