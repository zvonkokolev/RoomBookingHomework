using RoomBooking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Utils;

namespace RoomBooking.ImportConsole
{
    public static class ImportController
    {
        /// <summary>
        /// Liest die Buchungen mit ihren Räumen und Kunden aus der
        /// csv-Datei ein.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Booking>> ReadBookingsFromCsvAsync()
        {
            List<Customer> customers = (await MyFile.ReadStringMatrixFromCsvAsync("bookings.csv", true))
                .Select(c => new Customer
                {
                    LastName = c[0],
                    FirstName = c[1],
                    Iban = c[2]
                })
                .ToList()
                ;
            List<Room> rooms = (await MyFile.ReadStringMatrixFromCsvAsync("bookings.csv", true))
                .Select(c => new Room
                {
                    RoomNumber = c[3]
                })
                .ToList()
                ;
            List<Booking> bookings = (await MyFile.ReadStringMatrixFromCsvAsync("bookings.csv", true))
                .Select(b => new Booking
                {
                    Customer = customers.Where(c => c.LastName.Equals(b[0]) && c.FirstName.Equals(b[1]) && c.Iban.Equals(b[2])).FirstOrDefault(),
                    Room = rooms.Where(r => r.RoomNumber.Equals(b[3])).FirstOrDefault(),
                    From = b[4],
                    To = b[5]
                })
                .ToList();

            return bookings;
        }
    }
}
