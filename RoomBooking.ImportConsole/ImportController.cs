using RoomBooking.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string[][] matrix = await MyFile.ReadStringMatrixFromCsvAsync("bookings.csv", true);
            List<Customer> customers = matrix
                .Select(c => new Customer
                {
                    LastName    =   c[0],
                    FirstName   =   c[1],
                    Iban        =   c[2]
                })
                .ToList();
            List<Room> rooms = matrix
                .Select(r => new Room
                {
                    RoomNumber = r[3]
                })
                .ToList();
            List<Booking> bookings = matrix
                .Select(b => new Booking
                {
                    Customer = customers.Single(),
                    Room = rooms.Single(),
                    From = b[4],
                    To = b[5]
                })
                .ToList();
            return bookings;
        }

    }
}
