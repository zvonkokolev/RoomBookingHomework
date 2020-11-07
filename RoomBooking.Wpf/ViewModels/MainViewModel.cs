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
using System.Threading.Tasks;

namespace RoomBooking.Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Booking> _bookings;

        public ObservableCollection<Booking> Bookings 
        {
            get { return _bookings; }
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
                Validate();
            }
        }

        public MainViewModel(IWindowController windowController) : base(windowController)
        {
            Validate();
        }

        private async Task LoadDataAsync()
        {
            using IUnitOfWork unitOfWork = new UnitOfWork();
            var bookings = (await unitOfWork.Bookings
                .GetAllAsync())
                .ToList()
                ;
            Bookings = new ObservableCollection<Booking>(bookings);
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
