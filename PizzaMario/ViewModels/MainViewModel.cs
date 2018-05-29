using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using System.Windows.Input;
using PizzaMario.Models;
using PizzaMario.Utils;
using PizzaMario.Views;
using Prism.Commands;

namespace PizzaMario.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public ICommand ClickUpdateClientCommand
        {
            get;
        }
        public ICommand ClickDeleteClientCommand
        {
            get;
        }
        public ICommand ClickAddClientCommand
        {
            get;
        }
        public ICommand ClickUpdateMenuItemCommand
        {
            get;
        }
        public ICommand ClickDeleteMenuItemCommand
        {
            get;
        }
        public ICommand ClickAddMenuItemCommand
        {
            get;
        }
        public ICommand ClickDeleteOrderItemCommand
        {
            get;
        }
        public ICommand ClickAddOrderItemCommand
        {
            get;
        }
        public ICommand ClickUpdateOrderCommand
        {
            get;
        }
        public ICommand ClickDeleteOrderCommand
        {
            get;
        }
        public ICommand ClickAddOrderCommand
        {
            get;
        }

        private ObservableCollection<Client> clients;

        public ObservableCollection<Client> Clients
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PhoneNumberToSearch))
                    return clients;
                else
                    return new ObservableCollection<Client>(clients.Where(x => x.PhoneNumber.Contains(PhoneNumberToSearch)));
            }
            set
            {
                clients = value;
                NotifyPropertyChanged();
            }
        }

        private Client currentClient;

        public Client CurrentClient
        {
            get { return currentClient; }
            set
            {
                if (value == currentClient) return;
                currentClient = value;
                NotifyPropertyChanged("CurrentClient");
                (ClickUpdateClientCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickDeleteClientCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string phoneNumberToSearch;

        public string PhoneNumberToSearch
        {
            get { return phoneNumberToSearch; }
            set
            {
                phoneNumberToSearch = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Clients");
            }
        }

        private ObservableCollection<MenuItem> menuItems;

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                if (string.IsNullOrWhiteSpace(menuItemNameToSearch))
                    return menuItems;
                else
                    return new ObservableCollection<MenuItem>(menuItems.Where(x => x.Name.ToLower().Contains(MenuItemNameToSearch.ToLower())));
            }
            set
            {
                menuItems = value;
                NotifyPropertyChanged();
            }
        }

        private MenuItem currentMenuItem;

        public MenuItem CurrentMenuItem
        {
            get { return currentMenuItem; }
            set
            {
                currentMenuItem = value;
                NotifyPropertyChanged();
                (ClickUpdateMenuItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickDeleteMenuItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickAddOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private Order order;

        public Order Order
        {
            get { return order; }
            set
            {
                order = value;
                NotifyPropertyChanged();
                (ClickAddOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private Order currentOrder;

        public Order CurrentOrder
        {
            get { return currentOrder; }
            set
            {
                currentOrder = value;
                NotifyPropertyChanged();
                (ClickUpdateOrderCommand as DelegateCommand)?.RaiseCanExecuteChanged();
                (ClickDeleteOrderCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Order> orders;

        public ObservableCollection<Order> Orders
        {
            get
            {
                if (string.IsNullOrWhiteSpace(OrderClientNumberToSearch))
                    return orders;
                else
                    return new ObservableCollection<Order>(orders.Where(x => x.Client.PhoneNumber.Contains(OrderClientNumberToSearch)));
            }
            set
            {
                orders = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<OrderItem> orderItems;

        public ObservableCollection<OrderItem> OrderItems
        {
            get
            {
                return orderItems;
            }
            set
            {
                orderItems = value;
                NotifyPropertyChanged();
            }
        }

        private OrderItem currentOrderItem;

        public OrderItem CurrentOrderItem
        {
            get { return currentOrderItem; }
            set
            {
                currentOrderItem = value;
                NotifyPropertyChanged();
                (ClickDeleteOrderItemCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string orderClientNumberToSearch;

        public string OrderClientNumberToSearch
        {
            get { return orderClientNumberToSearch; }
            set
            {
                orderClientNumberToSearch = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Orders");
            }
        }

        private string menuItemNameToSearch;

        public string MenuItemNameToSearch
        {
            get { return menuItemNameToSearch; }
            set
            {
                menuItemNameToSearch = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("MenuItems");
            }
        }

        private int tabIndex = 0;

        public int TabIndex
        {
            get { return tabIndex; }
            set
            {
                tabIndex = value;
                NotifyPropertyChanged();
            }
        }

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

        public void LoadClients()
        {
            using (var context = new PizzaDbContext())
            {
                if (Clients == null)
                    clients = new ObservableCollection<Client>();
                else
                    clients.Clear();
                clients.AddRange(context.Clients);
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
                    menuItems = new ObservableCollection<MenuItem>();
                else
                    menuItems.Clear();
                menuItems.AddRange(context.MenuItems.Include(x => x.Category));
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
                    orders = new ObservableCollection<Order>();
                else
                    orders.Clear();
                orders.AddRange(context.Orders.Include(x => x.Client));
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
        }

        public bool CanUpdateOrder()
        {
            return CurrentOrder != null;
        }

        public void AddOrder()
        {
            CurrentOrder = null;
            Order = new Order();
            using (var context = new PizzaDbContext())
            {
                Order.Id = context.Orders.OrderByDescending(x => x.Id).First().Id + 1;
            }

            TabIndex = 1;
            LoadOrderItems();
        }

        public void SubmitOrder()
        {
            CurrentOrder = null;
            Order = new Order();
            TabIndex = 0;
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
            {
                using (var context = new PizzaDbContext())
                {
                    OrderItems = new ObservableCollection<OrderItem>(context.OrderItems.Include(x => x.MenuItem).Where(x => x.OrderId == Order.Id));
                }
            }
            else
            {
                OrderItems = new ObservableCollection<OrderItem>();
            }
        }

        public void AddOrderItem()
        {
            OrderItems.Add(new OrderItem()
            {
                MenuItem = CurrentMenuItem,
                MenuItemId = CurrentMenuItem.Id,
                Order = Order,
                OrderId = Order.Id
            });
        }

        public bool CanAddOrderItem()
        {
            return Order != null && CurrentMenuItem != null;
        }

        public void DeleteOrderItem()
        {
            OrderItems.Remove(CurrentOrderItem);
        }

        public bool CanDeleteOrderItem()
        {

            return Order != null && CurrentOrderItem != null;
        }
    }
}
