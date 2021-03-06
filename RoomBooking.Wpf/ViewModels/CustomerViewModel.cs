﻿using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Core.Validations;
using RoomBooking.Persistence;
using RoomBooking.Wpf.Common;
using RoomBooking.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RoomBooking.Wpf.ViewModels
{
    internal class CustomerViewModel : BaseViewModel
    {
        private string _lastName;
        private string _firstName;
        private string _iban;
        private Customer _selectedCustomer; // Aktuell ausgewählter Kunden
        private Customer _tempCustomer;
        private ObservableCollection<Customer> _customers;
        private ICommand _cmdEditCostumer;
        private ICommand _cmdUndo;
        private ICommand _cmdSave;

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

        public CustomerViewModel(IWindowController controller, Customer customer) : base(controller)
        {
            _selectedCustomer = customer;
            _tempCustomer = customer;
            LoadData();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IbanChecker.CheckIban(Iban))
            {
                yield return new ValidationResult($"IBAN {Iban} ist nich gültig", new string[] { nameof(Iban) });
            }
        }

        private void LoadData()
        {
            using IUnitOfWork uow = new UnitOfWork();
            var customers = uow.Customers.GetAll();
            Customers = new ObservableCollection<Customer>((List<Customer>)customers);

            LastName = _selectedCustomer.LastName;
            FirstName = _selectedCustomer.FirstName;
            Iban = _selectedCustomer.Iban;
        }

        // commands

        public ICommand CmdUndo
        {
            get
            {
                if (_cmdUndo == null)
                {
                    _cmdUndo = new RelayCommand(
                        execute: _ =>
                        {
                            _lastName = _tempCustomer.LastName;
                            _firstName = _tempCustomer.FirstName;
                            _iban = _tempCustomer.Iban;

                            LoadData();
                        },
                        canExecute: _ => _selectedCustomer != null);
                }
                return _cmdUndo;
            }
            set { _cmdUndo = value; }
        }

        public ICommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                {
                    _cmdSave = new RelayCommand(
                        execute: _ =>
                        {
                            using IUnitOfWork uow = new UnitOfWork();
                            Customer customer = new Customer
                            {
                                LastName = _lastName,
                                FirstName = _firstName,
                                Iban = _iban
                            };
                            uow.Customers.AddAsync(customer);
                            try
                            {
                                uow.SaveAsync();
                            }
                            catch (ValidationException e)
                            {
                                DbError = e.ValidationResult.ToString();
                            }

                            LoadData();
                        },
                        canExecute: _ => _selectedCustomer != null);
                }
                return _cmdSave;
            }
            set { _cmdSave = value; }
        }

        public ICommand CmdEditCostumer
        {
            get
            {
                if (_cmdEditCostumer == null)
                {
                    _cmdEditCostumer = new RelayCommand(
                        execute: _ =>
                        {
                            using IUnitOfWork uow = new UnitOfWork();
                            _selectedCustomer.LastName = _lastName;
                            _selectedCustomer.FirstName = _firstName;
                            _selectedCustomer.Iban = _iban;
                            uow.Customers.Update(_selectedCustomer);
                            try
                            {
                                uow.SaveAsync();
                            }
                            catch (ValidationException e)
                            {
                                DbError = e.ValidationResult.ToString();
                            }

                            LoadData();
                        },
                        canExecute: _ => _selectedCustomer != null);
                }
                return _cmdEditCostumer;
            }
            set { _cmdEditCostumer = value; }
        }
    }
}
