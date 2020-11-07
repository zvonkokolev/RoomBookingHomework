using RoomBooking.Core.Entities;
using RoomBooking.Core.Validations;
using RoomBooking.Wpf.Common;
using RoomBooking.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RoomBooking.Wpf.ViewModels
{
    internal class CustomerViewModel : BaseViewModel
    {
        private string _lastName;
        private string _firstName;
        private string _iban;
        private Customer _selectedCustomer; // Aktuell ausgewählter Kunden
        private ObservableCollection<Customer> _customers;

        [Required]
        [MinLength(2)]
        public string LastName 
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                Validate();
            } 
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                Validate();
            }
        }
        public string Name 
        {
            get { return LastName + FirstName; }
        }
        public string Iban
        {
            get { return _iban; }
            set
            {
                _iban = value;
                OnPropertyChanged(nameof(Iban));
                Validate();
            }
        }
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
                Validate();
            }
        }
        public ObservableCollection<Customer> Customers
        {
            get { return _customers; }
            set
            {
                _customers = value;
                OnPropertyChanged(nameof(Customers));
                Validate();
            }
        }

        public CustomerViewModel(IWindowController controller) : base(controller)
        {

        }
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IbanChecker.CheckIban(Iban))
            {
                yield return new ValidationResult($"IBAN {Iban} ist nich gültig", new string[] { nameof(Iban) });
            }
        }
    }
}
