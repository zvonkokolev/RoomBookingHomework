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
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace RoomBooking.Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Booking> _bookings;
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<Customer> _customers;

        private Booking _selectedBooking;
        private Room _selectedRoom;
        private Customer _selectedCustomer;

        private static IEnumerable<Booking> _allBookings;
        private ICommand _cmdEditCustomer;

        public ObservableCollection<Booking> Bookings 
        {
            get { return _bookings; }
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
            }
        }

        public ObservableCollection<Room> Rooms
        {
            get { return _rooms; }
            set
            {
                _rooms = value;
                OnPropertyChanged(nameof(Rooms));
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        public Booking SelectedBooking
        {
            get { return _selectedBooking; }
            set
            {
                _selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBooking));
            }
        }

        public Room SelectedRoom 
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
                OnChangedSelectedRoom_LoadChangedBookingData(_selectedRoom.Id);
            } 
        }

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        public MainViewModel(IWindowController windowController) : base(windowController)
        {
            Validate();
        }

        private async Task LoadDataAsync()
        {
            using IUnitOfWork unitOfWork = new UnitOfWork();
            var bookings = await unitOfWork.Bookings.GetAllBookingsWithRoomsAndCustomersAsync()
                ;
            var rooms = await unitOfWork.Rooms.GetAllAsync()
                ;
            var customers = await unitOfWork.Customers.GetAllWithBookingsAndRoomsAsync()
                ;
            _allBookings = bookings;
            Rooms = new ObservableCollection<Room>(rooms);
            SelectedRoom = Rooms.FirstOrDefault();
            var bookingsForRoom = bookings.Where(r => r.RoomId == SelectedRoom.Id).ToList();
            Bookings = new ObservableCollection<Booking>(bookingsForRoom);
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

        private void OnChangedSelectedRoom_LoadChangedBookingData(int newSelectedRoomId)
        {
            var bookingsForRoom = _allBookings.Where(r => r.RoomId == newSelectedRoomId).ToList();
            Bookings = new ObservableCollection<Booking>(bookingsForRoom);
        }

        // commands
        public ICommand CmdEditCustomer
        {
            get
            {
                if(_cmdEditCustomer == null)
                {
                    _cmdEditCustomer = new RelayCommand(
                        execute: _ => 
                        {
                            var window = new CustomerViewModel(Controller, SelectedBooking.Customer);
                            window.Controller.ShowWindow(window, true);
                            
                        },
                        canExecute: _ => SelectedBooking != null);
                }
                return _cmdEditCustomer;
            }
            set
            {
                _cmdEditCustomer = value;
            }
        }
    }
}
