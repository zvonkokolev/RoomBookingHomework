using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Persistence;
using RoomBooking.Wpf.Common;
using RoomBooking.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace RoomBooking.Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Booking> _bookings;
        private ObservableCollection<Room> _rooms;
        private Room _selectedRoom;
        private ObservableCollection<Customer> _customers;
        private Customer _selectedCustomer;
        private string _roomNumberText;
        private string _selectedCustomerName;
        private string _from;
        private string _to;

        public ObservableCollection<Booking> Bookings 
        {
            get { return _bookings; }
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
                //Validate();
            }
        }

        public ObservableCollection<Room> Rooms
        {
            get { return _rooms; }
            set
            {
                _rooms = value;
                OnPropertyChanged(nameof(Rooms));
                //Validate();
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
                //Validate();
            }
        }

        public Room SelectedRoom 
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
                RoomNumberText = _selectedRoom?.RoomNumber;
                OnPropertyChanged(nameof(SelectedRoom));
                //Validate();
            } 
        }

        public string RoomNumberText
        { 
            get { return _roomNumberText; }
            set
            {
                _roomNumberText = value;
                OnPropertyChanged(nameof(RoomNumberText));
                //Validate();
            }
        }

        public string From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged(nameof(From));
                //Validate();
            }
        }

        public string To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged(nameof(To));
                //Validate();
            }
        }

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                SelectedCustomerName = _selectedCustomer.LastName
                    + " " + _selectedCustomer.FirstName;
                OnPropertyChanged(nameof(SelectedCustomer));
                //Validate();
            }
        }

        public string SelectedCustomerName
        {
            get { return _selectedCustomerName; }
            set
            {
                _selectedCustomerName = value;
                OnPropertyChanged(nameof(SelectedCustomerName));
                //Validate();
            }
        }

        public MainViewModel(IWindowController windowController) : base(windowController)
        {
            Validate();
        }

        private async Task LoadDataAsync()
        {
            using IUnitOfWork unitOfWork = new UnitOfWork();
            var bookings = (await unitOfWork.Bookings.GetAllBookingsWithRoomsAndCustomersAsync())
                .ToList()
                ;
            var rooms = (await unitOfWork.Rooms.GetAllAsync())
                .ToList()
                ;
            var customers = (await unitOfWork.Customers.GetAllWithBookingsAndRoomsAsync())
                .ToList()
                ;
            Bookings = new ObservableCollection<Booking>(bookings);
            Rooms = new ObservableCollection<Room>(rooms);
            Customers = new ObservableCollection<Customer>(customers);
        }

        public static async Task<MainViewModel> CreateAsync(IWindowController windowController)
        {
            var viewModel = new MainViewModel(windowController);
            await viewModel.LoadDataAsync();
            return viewModel;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Bookings == null)
            {
                yield return new ValidationResult($"Datenbank ist fehlerhaft", new string[] { nameof(Bookings) });
            }
        }
    }
}
