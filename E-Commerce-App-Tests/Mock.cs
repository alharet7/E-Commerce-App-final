using E_Commerce_App.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_App_Tests
{
    public class Mock : IDisposable
    {
        private readonly SqliteConnection _connection;

        protected readonly StoreDbContext _db;


        public Mock()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _db = new StoreDbContext(
                new DbContextOptionsBuilder<StoreDbContext>()
                .UseSqlite(_connection).Options);

            _db.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _db?.Dispose();
            _connection?.Dispose();
        }
    }
}