﻿using Microsoft.EntityFrameworkCore;
using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using RoomBooking.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _dbContext;
        private DuplicateNameValidation _duplicateNameValidation;
        /// <summary>
        /// ConnectionString kommt aus den appsettings.json
        /// </summary>
        public UnitOfWork()
        {
            _dbContext = new ApplicationDbContext();
            Rooms = new RoomRepository(_dbContext);
            Customers = new CustomerRepository(_dbContext);
            Bookings = new BookingRepository(_dbContext);
            _duplicateNameValidation = new DuplicateNameValidation(this);
        }

        public ICustomerRepository Customers { get; }
        public IRoomRepository Rooms { get; }
        public IBookingRepository Bookings { get; }

        public async Task DeleteDatabaseAsync() => await _dbContext.Database.EnsureDeletedAsync();

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task MigrateDatabaseAsync() => await _dbContext.Database.MigrateAsync();

        /// <summary>
        /// Prüfung, ob für den zu buchenden Zeitraum des Raums bereits eine andere Buchung vorliegt.
        /// </summary>
        /// <param name="entity"></param>
        private async Task ValidateEntityAsync(object entity)
        {
            Validator.ValidateObject(entity, new ValidationContext(entity), true);
            ValidationResult result = null;
            if (entity is Booking booking)
            {
                var bookingsForRoom = await _dbContext.Bookings.Include(b => b.Customer).Where(b => b.RoomId == booking.RoomId).ToArrayAsync();
                var bookingFrom = DateTime.Parse(booking.From);
                var bookingTo = DateTime.Parse(booking.To);
                foreach (var bookingForRoom in bookingsForRoom)
                {
                    var bookingForRoomFrom = DateTime.Parse(bookingForRoom.From);
                    var bookingForRoomTo = DateTime.Parse(bookingForRoom.To);
                    if (bookingFrom >= bookingForRoomFrom && bookingFrom <= bookingForRoomTo)
                    {
                        throw new ValidationException($"Es gibt schon eine Buchung von {bookingForRoom.Customer.LastName} von {bookingForRoom.From} bis {bookingForRoom.To} ", null, new List<string> { "From" });
                    }
                    if (bookingTo >= bookingForRoomFrom && bookingTo <= bookingForRoomTo)
                    {
                        throw new ValidationException($"Es gibt schon eine Buchung von {bookingForRoom.Customer.LastName} von {bookingForRoom.From} bis {bookingForRoom.To} ", null, new List<string> { "To" });
                    }
                }
            }
            if (entity is Customer customer)  // WPF-Anwendung
            {
                if(!IbanChecker.CheckIban(customer.Iban))
                {
                    throw new ValidationException($"Der IBAN {customer.Iban} von {customer.LastName} {customer.FirstName} ", null, new List<string> { "IBAN" });
                }
                result = _duplicateNameValidation.GetValidationResult(customer, new ValidationContext(customer));
                
                if (result != null && result != ValidationResult.Success)
                {
                    throw new ValidationException(result, _duplicateNameValidation, entity);
                }
            }
        }

        /// <summary>
        /// Geänderte Entities extra validieren
        /// </summary>
        public async Task SaveAsync()
        {
            var entities = _dbContext.ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added
                                 || entity.State == EntityState.Modified)
                .Select(e => e.Entity);
            foreach (var entity in entities)
            {
                await ValidateEntityAsync(entity);
            }
            await _dbContext.SaveChangesAsync();
        }

    }
}
