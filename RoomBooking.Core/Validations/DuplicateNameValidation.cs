using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RoomBooking.Core.Validations
{
    public class DuplicateNameValidation : ValidationAttribute
    {
        private readonly IUnitOfWork _unitOfWork;

        public DuplicateNameValidation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Customer customerToValidate = (Customer)validationContext.ObjectInstance;
            if (_unitOfWork.Customers.CheckIfDuplicateName(customerToValidate.LastName, customerToValidate.Id))
            {
                return new ValidationResult("Name ist schon vorhanden");
            }
            return ValidationResult.Success;
        }
    }
}
