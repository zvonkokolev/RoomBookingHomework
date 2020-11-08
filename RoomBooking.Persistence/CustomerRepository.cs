using Microsoft.EntityFrameworkCore;
using RoomBooking.Core.Contracts;
using RoomBooking.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomBooking.Persistence
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Customer customer)
            => await _dbContext.AddAsync(customer);

        public bool CheckIfDuplicateName(string lastName, int id)
            => _dbContext.Customers
                .Where(u => u.LastName == lastName && u.Id != id)
                .FirstOrDefault() != null;

        public object GetAll()
            => _dbContext.Customers
              .OrderBy(customers => customers.LastName)
              .ToList();

        public async Task<IEnumerable<Customer>> GetAllAsync()
           => await _dbContext.Customers
              .OrderBy(customers => customers.LastName)
              .ToListAsync();

        public async Task<IEnumerable<Customer>> GetAllWithBookingsAndRoomsAsync()
          => await _dbContext.Customers
              .Include("Bookings.Room")
              .OrderBy(customers => customers.LastName)
              .ToListAsync();

        public async Task<Customer> GetByIdAsync(int id)
          => await _dbContext.Customers.FindAsync(id);

        public void Update(Customer selectedCustomer)
          => _dbContext.Customers.Update(selectedCustomer);
    }
}
