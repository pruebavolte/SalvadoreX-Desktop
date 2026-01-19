using Dapper;
using SalvadoreXDesktop.Models;

namespace SalvadoreXDesktop.Data
{
    public class CustomerRepository
    {
        private readonly string _restaurantId;
        
        public CustomerRepository()
        {
            _restaurantId = DatabaseManager.GetSetting("restaurant_id", "00000000-0000-0000-0000-000000000001");
        }
        
        public List<Customer> GetAll(bool activeOnly = true)
        {
            using var connection = DatabaseManager.GetConnection();
            var sql = "SELECT * FROM customers WHERE restaurant_id = @RestaurantId";
            
            if (activeOnly)
                sql += " AND active = 1";
                
            sql += " ORDER BY name";
            
            return connection.Query<Customer>(sql, new { RestaurantId = _restaurantId }).ToList();
        }
        
        public Customer? GetById(string id)
        {
            using var connection = DatabaseManager.GetConnection();
            return connection.QueryFirstOrDefault<Customer>(
                "SELECT * FROM customers WHERE id = @Id",
                new { Id = id }
            );
        }
        
        public List<Customer> Search(string query)
        {
            using var connection = DatabaseManager.GetConnection();
            var searchTerm = $"%{query}%";
            var sql = @"SELECT * FROM customers 
                        WHERE restaurant_id = @RestaurantId 
                        AND active = 1
                        AND (name LIKE @Search OR email LIKE @Search OR phone LIKE @Search)
                        ORDER BY name
                        LIMIT 50";
            return connection.Query<Customer>(sql, new { RestaurantId = _restaurantId, Search = searchTerm }).ToList();
        }
        
        public void Insert(Customer customer)
        {
            customer.RestaurantId = _restaurantId;
            customer.CreatedAt = DateTime.UtcNow;
            customer.UpdatedAt = DateTime.UtcNow;
            customer.NeedSync = true;
            
            using var connection = DatabaseManager.GetConnection();
            connection.Execute(@"
                INSERT INTO customers (id, name, email, phone, address, rfc, credit_limit, current_credit,
                    loyalty_points, notes, active, restaurant_id, created_at, updated_at, need_sync)
                VALUES (@Id, @Name, @Email, @Phone, @Address, @Rfc, @CreditLimit, @CurrentCredit,
                    @LoyaltyPoints, @Notes, @Active, @RestaurantId, @CreatedAt, @UpdatedAt, 1)",
                new {
                    customer.Id, customer.Name, customer.Email, customer.Phone, customer.Address,
                    customer.Rfc, customer.CreditLimit, customer.CurrentCredit, customer.LoyaltyPoints,
                    customer.Notes, Active = customer.Active ? 1 : 0, customer.RestaurantId,
                    CreatedAt = customer.CreatedAt.ToString("o"), 
                    UpdatedAt = customer.UpdatedAt.ToString("o")
                }
            );
        }
        
        public void Update(Customer customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;
            customer.NeedSync = true;
            
            using var connection = DatabaseManager.GetConnection();
            connection.Execute(@"
                UPDATE customers SET 
                    name = @Name, email = @Email, phone = @Phone, address = @Address,
                    rfc = @Rfc, credit_limit = @CreditLimit, current_credit = @CurrentCredit,
                    loyalty_points = @LoyaltyPoints, notes = @Notes, active = @Active,
                    updated_at = @UpdatedAt, need_sync = 1
                WHERE id = @Id",
                new {
                    customer.Id, customer.Name, customer.Email, customer.Phone, customer.Address,
                    customer.Rfc, customer.CreditLimit, customer.CurrentCredit, customer.LoyaltyPoints,
                    customer.Notes, Active = customer.Active ? 1 : 0,
                    UpdatedAt = customer.UpdatedAt.ToString("o")
                }
            );
        }
        
        public void Delete(string id)
        {
            using var connection = DatabaseManager.GetConnection();
            var now = DateTime.UtcNow.ToString("o");
            connection.Execute(
                "UPDATE customers SET active = 0, updated_at = @UpdatedAt, need_sync = 1 WHERE id = @Id",
                new { Id = id, UpdatedAt = now }
            );
        }
    }
}
