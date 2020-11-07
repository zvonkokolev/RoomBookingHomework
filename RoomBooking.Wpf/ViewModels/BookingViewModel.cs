using RoomBooking.Wpf.Common;
using RoomBooking.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace RoomBooking.Wpf.ViewModels
{
    public class BookingViewModel : BaseViewModel
    {
        private string _from;
        private string _to;

        public string From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged(nameof(From));
                Validate();
            }
        }
        public string To 
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged(nameof(To));
                Validate();
            }
        }

        public BookingViewModel(IWindowController controller) : base(controller)
        {

        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
