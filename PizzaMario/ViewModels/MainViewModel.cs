using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Input;
using PizzaMario.Models;
using PizzaMario.Views;
using Prism.Commands;

namespace PizzaMario.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Client> _clients;

        private Client _currentClient;

        private MenuItem _currentMenuItem;

        private Order _currentOrder;

        private OrderItem _currentOrderItem;

        private string _menuItemNameToSearch;

        private ObservableCollection<MenuItem> _menuItems;

        private Order _order;

        private string _orderClientNumberToSearch;

        private ObservableCollection<OrderItem> _orderItems;

        private ObservableCollection<Order> _orders;

        private string _phoneNumberToSearch;

        private double _resultSum;

        private string _summaryText;

        private int _tabIndex;

        private bool _wasCalculated;

        public MainViewModel()
        {
            LoadOrders();
            LoadMenuItems();
            LoadClients();
            ClickUpdateClientCommand = new DelegateCommand(UpdateClient, CanUpdateClient);
            ClickDeleteClientCommand = new DelegateCommand(DeleteClient, CanUpdateClient);
            ClickAddClientCommand = new DelegateCommand(AddClient);
            ClickUpdateMenuItemCommand = new DelegateCommand(UpdateMenuItem, CanUpdateMenuItem);
            ClickDeleteMenuItemCommand = new DelegateCommand(DeleteMenuItem, CanUpdateMenuItem);
            ClickAddMenuItemCommand = new DelegateCommand(AddMenuItem);
            ClickUpdateOrderCommand = new DelegateCommand(UpdateOrder, CanUpdateOrder);
            ClickDeleteOrderCommand = new DelegateCommand(DeleteOrder, CanUpdateOrder);
            ClickAddOrderCommand = new DelegateCommand(AddOrder);
            ClickDeleteOrderItemCommand = new DelegateCommand(DeleteOrderItem, CanDeleteOrderItem);
            ClickAddOrderItemCommand = new DelegateCommand(AddOrderItem, CanAddOrderItem);
            ClickCalculateOrderButton = new DelegateCommand(CalculateOrder, CanCalculateOrder);
            ClickSubmitOrderButton = new DelegateCommand(SubmitOrder, CanSubmitOrder);
            ClickDeleteCountOrderItemCommand = new DelegateCommand(DeleteCountOrderItem, CanDeleteCountOrderItem);
            ClickAddCountOrderItemCommand = new DelegateCommand(AddCountOrderItem, CanAddCountOrderItem);
        }

        public ICommand ClickUpdateClientCommand { get; }

        public ICommand ClickDeleteClientCommand { get; }

        public ICommand ClickAddClientCommand { get; }

        public ICommand ClickUpdateMenuItemCommand { get; }

        public ICommand ClickDeleteMenuItemCommand { get; }

        public ICommand ClickAddMenuItemCommand { get; }

        public ICommand ClickDeleteOrderItemCommand { get; }

        public ICommand ClickAddOrderItemCommand { get; }

        public ICommand ClickDeleteCountOrderItemCommand { get; }

        public ICommand ClickAddCountOrderItemCommand { get; }

        public ICommand ClickUpdateOrderCommand { get; }

        public ICommand ClickDeleteOrderCommand { get; }

        public ICommand ClickAddOrderCommand { get; }

        public ICommand ClickCalculateOrderButton { get; }

        public ICommand ClickSubmitOrderButton { get; }

        public ObservableCollection<Client> Clients
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PhoneNumberToSearch))
                    return _clients;
                return new ObservableCollection<Client>(_clients.Where(x =>
                    x.PhoneNumber.Contains(PhoneNumberToSearch)));
            }
            set
            {
                _clients = value;
                NotifyPropertyChanged();
            }
        }

        public Client CurrentClient
        {
            get => _currentClient;
            set
            {
                _currentClient = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("IfClientChoosed");
                (ClickUpdateClientCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickDeleteClientCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                WasCalculated = false;
            }
        }

        public string PhoneNumberToSearch
        {
            get => _phoneNumberToSearch;
            set
            {
                _phoneNumberToSearch = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Clients");
            }
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_menuItemNameToSearch))
                    return _menuItems;
                return new ObservableCollection<MenuItem>(_menuItems.Where(x =>
                    x.Name.ToLower().Contains(MenuItemNameToSearch.ToLower())));
            }
            set
            {
                _menuItems = value;
                NotifyPropertyChanged();
            }
        }

        public MenuItem CurrentMenuItem
        {
            get => _currentMenuItem;
            set
            {
                _currentMenuItem = value;
                NotifyPropertyChanged();
                (ClickUpdateMenuItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickDeleteMenuItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickAddOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public Order Order
        {
            get => _order;
            set
            {
                _order = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("IfOrderChoosed");
                (ClickAddOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                WasCalculated = false;
            }
        }

        public Order CurrentOrder
        {
            get => _currentOrder;
            set
            {
                _currentOrder = value;
                NotifyPropertyChanged();
                (ClickUpdateOrderCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickDeleteOrderCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Order> Orders
        {
            get
            {
                if (string.IsNullOrWhiteSpace(OrderClientNumberToSearch))
                    return _orders;
                return new ObservableCollection<Order>(_orders.Where(x =>
                    x.Client.PhoneNumber.Contains(OrderClientNumberToSearch)));
            }
            set
            {
                _orders = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<OrderItem> OrderItems
        {
            get => _orderItems;
            set
            {
                _orderItems = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("IfMenuItemsChoosed");
                (ClickCalculateOrderButton as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public OrderItem CurrentOrderItem
        {
            get => _currentOrderItem;
            set
            {
                _currentOrderItem = value;
                NotifyPropertyChanged();
                (ClickDeleteOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickDeleteCountOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickAddCountOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public string OrderClientNumberToSearch
        {
            get => _orderClientNumberToSearch;
            set
            {
                _orderClientNumberToSearch = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Orders");
            }
        }

        public string MenuItemNameToSearch
        {
            get => _menuItemNameToSearch;
            set
            {
                _menuItemNameToSearch = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("MenuItems");
            }
        }

        public int TabIndex
        {
            get => _tabIndex;
            set
            {
                _tabIndex = value;
                NotifyPropertyChanged();
            }
        }

        public bool IfOrderChoosed => Order != null;

        public bool IfMenuItemsChoosed => OrderItems != null && OrderItems.Count > 0;

        public bool IfClientChoosed => CurrentClient != null;

        public bool WasCalculated
        {
            get => _wasCalculated;
            set
            {
                _wasCalculated = value;
                NotifyPropertyChanged();
                (ClickSubmitOrderButton as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickCalculateOrderButton as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        public double ResultSum
        {
            get => _resultSum;
            set
            {
                _resultSum = value;
                NotifyPropertyChanged();
            }
        }

        public string SummaryText
        {
            get => _summaryText;
            set
            {
                _summaryText = value;
                NotifyPropertyChanged();
            }
        }

        public void LoadClients()
        {
            using (var context = new PizzaDbContext())
            {
                if (Clients == null)
                    _clients = new ObservableCollection<Client>();
                else
                    _clients.Clear();
                _clients.AddRange(context.Clients);
                NotifyPropertyChanged("Clients");
            }
        }

        public void UpdateClient()
        {
            var app = new ClientEdit();
            var viewModel = new ClientEditViewModel(CurrentClient);
            viewModel.CloseWindowEvent += (s, e) => app.Close();
            app.DataContext = viewModel;
            app.ShowDialog();
            LoadClients();
            WasCalculated = false;
            (ClickCalculateOrderButton as DelegateCommand)?.RaiseCanExecuteChanged();
        }

        public bool CanUpdateClient()
        {
            return CurrentClient != null;
        }

        public void AddClient()
        {
            var app = new ClientEdit();
            var viewModel = new ClientEditViewModel(null);
            viewModel.CloseWindowEvent += (s, e) => app.Close();
            app.DataContext = viewModel;
            app.ShowDialog();
            LoadClients();
            (ClickCalculateOrderButton as DelegateCommand)?.RaiseCanExecuteChanged();
        }

        public void DeleteClient()
        {
            using (var context = new PizzaDbContext())
            {
                var client = context.Clients.FirstOrDefault(x => x.Id == CurrentClient.Id);
                if (client == null) return;
                context.Clients.Remove(client);

                context.SaveChanges();
            }

            LoadClients();
        }

        public void LoadMenuItems()
        {
            using (var context = new PizzaDbContext())
            {
                if (MenuItems == null)
                    _menuItems = new ObservableCollection<MenuItem>();
                else
                    _menuItems.Clear();
                _menuItems.AddRange(context.MenuItems.Include(x => x.Category));
                NotifyPropertyChanged("MenuItems");
            }
        }

        public void UpdateMenuItem()
        {
            var app = new MenuItemEdit();
            var viewModel = new MenuItemEditViewModel(CurrentMenuItem);
            viewModel.CloseWindowEvent += (s, e) => app.Close();
            app.DataContext = viewModel;
            app.ShowDialog();
            LoadMenuItems();
            LoadOrderItems();
            WasCalculated = false;
        }

        public bool CanUpdateMenuItem()
        {
            return CurrentMenuItem != null;
        }

        public void AddMenuItem()
        {
            var app = new MenuItemEdit();
            var viewModel = new MenuItemEditViewModel(null);
            viewModel.CloseWindowEvent += (s, e) => app.Close();
            app.DataContext = viewModel;
            app.ShowDialog();
            LoadMenuItems();
            WasCalculated = false;
        }

        public void DeleteMenuItem()
        {
            using (var context = new PizzaDbContext())
            {
                var menuItem = context.MenuItems.FirstOrDefault(x => x.Id == CurrentMenuItem.Id);
                if (menuItem == null) return;
                context.MenuItems.Remove(menuItem);

                context.SaveChanges();
            }

            LoadMenuItems();
            LoadOrderItems();
            WasCalculated = false;
        }

        public void LoadOrders()
        {
            using (var context = new PizzaDbContext())
            {
                if (Orders == null)
                    _orders = new ObservableCollection<Order>();
                else
                    _orders.Clear();
                _orders.AddRange(context.Orders.Include(x => x.Client));
                NotifyPropertyChanged("Orders");
            }
        }

        public void UpdateOrder()
        {
            using (var context = new PizzaDbContext())
            {
                Order = context.Orders.Include(x => x.Client).FirstOrDefault(x => x.Id == CurrentOrder.Id);
            }

            TabIndex = 1;
            LoadOrderItems();
            CurrentClient = Clients[Clients.IndexOf(Clients.First(x => x.Id == CurrentOrder.ClientId))];
            WasCalculated = false;
        }

        public bool CanUpdateOrder()
        {
            return CurrentOrder != null;
        }

        public void AddOrder()
        {
            CurrentOrder = null;
            Order = new Order();
            CurrentClient = null;
            using (var context = new PizzaDbContext())
            {
                Order.Id = context.Orders.OrderByDescending(x => x.Id).First().Id + 1;
            }

            TabIndex = 1;
            LoadOrderItems();
            WasCalculated = false;
        }

        public void DeleteOrder()
        {
            using (var context = new PizzaDbContext())
            {
                var order = context.Orders.FirstOrDefault(x => x.Id == CurrentOrder.Id);
                if (order == null) return;
                context.Orders.Remove(order);

                context.SaveChanges();
            }

            LoadOrders();
        }

        public void LoadOrderItems()
        {
            if (Order.Id > 0)
                using (var context = new PizzaDbContext())
                {
                    OrderItems = new ObservableCollection<OrderItem>(context.OrderItems.Include(x => x.MenuItem)
                        .Where(x => x.OrderId == Order.Id));
                }
            else
                OrderItems = new ObservableCollection<OrderItem>();
        }

        public void AddOrderItem()
        {
            var orderItem = OrderItems.FirstOrDefault(x => x.MenuItemId == CurrentMenuItem.Id && x.OrderId == Order.Id);
            if (orderItem == null)
            {
                OrderItems.Add(new OrderItem
                {
                    MenuItem = CurrentMenuItem,
                    MenuItemId = CurrentMenuItem.Id,
                    Order = Order,
                    OrderId = Order.Id
                });
                NotifyPropertyChanged("IfMenuItemsChoosed");
                WasCalculated = false;
            }
        }

        public void AddCountOrderItem()
        {
            CurrentOrderItem.Quantity++;
            (ClickDeleteCountOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (ClickAddCountOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            OrderItems = new ObservableCollection<OrderItem>(OrderItems);
        }

        public void DeleteCountOrderItem()
        {
            CurrentOrderItem.Quantity--;
            (ClickDeleteCountOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (ClickAddCountOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            OrderItems = new ObservableCollection<OrderItem>(OrderItems);
        }

        public bool CanAddCountOrderItem()
        {
            return CurrentOrderItem != null;
        }

        public bool CanDeleteCountOrderItem()
        {
            return CurrentOrderItem != null && CurrentOrderItem.Quantity > 1;
        }

        public bool CanAddOrderItem()
        {
            return Order != null && CurrentMenuItem != null;
        }

        public void DeleteOrderItem()
        {
            OrderItems.Remove(CurrentOrderItem);
            NotifyPropertyChanged("IfMenuItemsChoosed");
            WasCalculated = false;
        }

        public bool CanDeleteOrderItem()
        {
            return Order != null && CurrentOrderItem != null;
        }

        public bool CanCalculateOrder()
        {
            return Order != null && OrderItems != null && OrderItems.Count > 0 && CurrentClient != null;
        }

        public bool CanSubmitOrder()
        {
            return WasCalculated;
        }

        public void CalculateOrder()
        {
            ResultSum = 0;
            var wasDiscounts = false;
            SummaryText = string.Empty;

            SummaryText += "Client Info:" + Environment.NewLine;
            SummaryText += new string('=', 30) + Environment.NewLine;
            SummaryText += $"| Name: {CurrentClient.FirstName} {CurrentClient.SecondName}" + Environment.NewLine;
            SummaryText += $"| Phone Number: {CurrentClient.PhoneNumber}" + Environment.NewLine;
            SummaryText += new string('=', 30) + Environment.NewLine;
            SummaryText += Environment.NewLine;
            SummaryText += "Order Info:" + Environment.NewLine;
            SummaryText += new string('=', 30) + Environment.NewLine;
            foreach (var orderItem in OrderItems)
            {
                var productPrice = orderItem.Quantity * orderItem.MenuItem.Price;
                if (orderItem.Quantity >= 10)
                {
                    SummaryText +=
                        $"| {orderItem.MenuItem.Name} x{orderItem.Quantity} = {productPrice * 0.95:0.00} ({productPrice} - 5% discount *)";
                    ResultSum += productPrice * 0.95;
                    wasDiscounts = true;
                }
                else
                {
                    SummaryText += $"| {orderItem.MenuItem.Name} x{orderItem.Quantity} = {productPrice}";
                    ResultSum += productPrice;
                }

                SummaryText += Environment.NewLine;
            }

            SummaryText += new string('=', 30) + Environment.NewLine;
            SummaryText += $"Total sum: {ResultSum}" + Environment.NewLine;
            SummaryText += Environment.NewLine;
            if (ResultSum >= 1000)
            {
                SummaryText += $"RESULT SUM: {ResultSum * 0.95:0.00} ({ResultSum:0.00} - 5% discount **)";
                ResultSum *= 0.95;
                wasDiscounts = true;
            }
            else
            {
                SummaryText += $"RESULT SUM: {ResultSum:0.00}";
            }

            ResultSum = Math.Round(ResultSum, 2, MidpointRounding.AwayFromZero);
            if (wasDiscounts)
            {
                SummaryText += Environment.NewLine;
                SummaryText += new string('-', 30) + Environment.NewLine;
                SummaryText += "*  - if order item count is 10 or more" + Environment.NewLine;
                SummaryText += "     then we reduce product price by 5%" + Environment.NewLine;
                SummaryText += "** - if total sum is more than 1000" + Environment.NewLine;
                SummaryText += "     then we reduce total sum by 5%" + Environment.NewLine;
            }

            WasCalculated = true;
            (ClickSubmitOrderButton as DelegateCommand)?.RaiseCanExecuteChanged();
        }

        public void SubmitOrder()
        {
            if (CurrentOrder == null)
                using (var context = new PizzaDbContext())
                {
                    Order.TotalPrice = ResultSum;
                    Order.Client = CurrentClient;
                    Order.ClientId = CurrentClient.Id;
                    context.Orders.Add(Order);
                    context.OrderItems.AddRange(OrderItems);
                    context.SaveChanges();
                }
            else
                using (var context = new PizzaDbContext())
                {
                    Order.TotalPrice = ResultSum;
                    var orderId = Order.Id;
                    if (context.Orders.Any(e => e.Id == orderId))
                    {
                        context.Orders.Attach(Order);
                        context.Entry(Order).State = EntityState.Modified;
                    }
                    else
                    {
                        context.Orders.Add(Order);
                    }

                    var dbOrderItems = context.OrderItems.Where(x => x.OrderId == Order.Id).ToList();
                    foreach (var orderItem in OrderItems) context.OrderItems.AddOrUpdate(orderItem);

                    foreach (var dbOrderItem in dbOrderItems)
                        if (OrderItems.All(e => e.Id != dbOrderItem.Id))
                            context.OrderItems.Remove(dbOrderItem);

                    context.SaveChanges();
                }

            CurrentOrder = null;
            Order = new Order();
            TabIndex = 0;
            LoadOrders();
        }
    }
}