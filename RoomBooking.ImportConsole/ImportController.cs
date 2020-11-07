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
            return (await MyFile.ReadStringMatrixFromCsvAsync("bookings.csv", true))
                .Select(b => new Booking
                {
                    Customer = new Customer()
                    {
                        LastName = b[0],
                        FirstName = b[1],
                        Iban = b[2]
                    },
                    Room = new Room()
                    {
                        RoomNumber = b[3]
                    },
                    From = b[4],
                    To = b[5]
                })
                .ToList();
        }
    }
}
