using System.Collections.ObjectModel;
using System.Data.Entity;
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

        private int _tabIndex;

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
        }

        public ICommand ClickUpdateClientCommand { get; }

        public ICommand ClickDeleteClientCommand { get; }

        public ICommand ClickAddClientCommand { get; }

        public ICommand ClickUpdateMenuItemCommand { get; }

        public ICommand ClickDeleteMenuItemCommand { get; }

        public ICommand ClickAddMenuItemCommand { get; }

        public ICommand ClickDeleteOrderItemCommand { get; }

        public ICommand ClickAddOrderItemCommand { get; }

        public ICommand ClickUpdateOrderCommand { get; }

        public ICommand ClickDeleteOrderCommand { get; }

        public ICommand ClickAddOrderCommand { get; }

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
            CurrentClient = Clients[Clients.IndexOf(Clients.First(x => x.Id == CurrentOrder.Id))];
            // TODO
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
            // TODO
        }

        public void SubmitOrder()
        {
            CurrentOrder = null;
            Order = new Order();
            TabIndex = 0;
            // TODO
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
            OrderItems.Add(new OrderItem
            {
                MenuItem = CurrentMenuItem,
                MenuItemId = CurrentMenuItem.Id,
                Order = Order,
                OrderId = Order.Id
            });
            NotifyPropertyChanged("IfMenuItemsChoosed");
        }

        public bool CanAddOrderItem()
        {
            return Order != null && CurrentMenuItem != null;
        }

        public void DeleteOrderItem()
        {
            OrderItems.Remove(CurrentOrderItem);
            NotifyPropertyChanged("IfMenuItemsChoosed");
        }

        public bool CanDeleteOrderItem()
        {
            return Order != null && CurrentOrderItem != null;
        }
    }
}