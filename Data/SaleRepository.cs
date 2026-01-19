using Dapper;
using SalvadoreXDesktop.Models;

namespace SalvadoreXDesktop.Data
{
    public class SaleRepository
    {
        private readonly string _restaurantId;
        private readonly ProductRepository _productRepo;
        
        public SaleRepository()
        {
            _restaurantId = DatabaseManager.GetSetting("restaurant_id", "00000000-0000-0000-0000-000000000001");
            _productRepo = new ProductRepository();
        }
        
        public List<Sale> GetAll(DateTime? from = null, DateTime? to = null)
        {
            using var connection = DatabaseManager.GetConnection();
            var sql = "SELECT * FROM sales WHERE restaurant_id = @RestaurantId";
            
            if (from.HasValue)
                sql += " AND created_at >= @From";
            if (to.HasValue)
                sql += " AND created_at <= @To";
                
            sql += " ORDER BY created_at DESC";
            
            return connection.Query<Sale>(sql, new { 
                RestaurantId = _restaurantId, 
                From = from?.ToString("o"), 
                To = to?.ToString("o") 
            }).ToList();
        }
        
        public List<Sale> GetToday()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            return GetAll(today, tomorrow);
        }
        
        public Sale? GetById(string id)
        {
            using var connection = DatabaseManager.GetConnection();
            var sale = connection.QueryFirstOrDefault<Sale>(
                "SELECT * FROM sales WHERE id = @Id",
                new { Id = id }
            );
            
            if (sale != null)
            {
                sale.Items = connection.Query<SaleItem>(
                    "SELECT * FROM sale_items WHERE sale_id = @SaleId",
                    new { SaleId = id }
                ).ToList();
            }
            
            return sale;
        }
        
        public string GenerateReceiptNumber()
        {
            using var connection = DatabaseManager.GetConnection();
            var today = DateTime.Today.ToString("yyyyMMdd");
            var count = connection.ExecuteScalar<int>(
                @"SELECT COUNT(*) + 1 FROM sales 
                  WHERE receipt_number LIKE @Pattern",
                new { Pattern = $"{today}%" }
            );
            return $"{today}-{count:D4}";
        }
        
        public void Insert(Sale sale, List<SaleItem> items)
        {
            sale.RestaurantId = _restaurantId;
            sale.CreatedAt = DateTime.UtcNow;
            sale.UpdatedAt = DateTime.UtcNow;
            sale.NeedSync = true;
            sale.ReceiptNumber = GenerateReceiptNumber();
            
            using var connection = DatabaseManager.GetConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            
            try
            {
                // Insertar venta
                connection.Execute(@"
                    INSERT INTO sales (id, customer_id, customer_name, subtotal, tax, discount, total,
                        payment_method, status, notes, receipt_number, restaurant_id, user_id,
                        created_at, updated_at, need_sync)
                    VALUES (@Id, @CustomerId, @CustomerName, @Subtotal, @Tax, @Discount, @Total,
                        @PaymentMethod, @Status, @Notes, @ReceiptNumber, @RestaurantId, @UserId,
                        @CreatedAt, @UpdatedAt, 1)",
                    new {
                        sale.Id, sale.CustomerId, sale.CustomerName, sale.Subtotal, sale.Tax,
                        sale.Discount, sale.Total, sale.PaymentMethod, sale.Status, sale.Notes,
                        sale.ReceiptNumber, sale.RestaurantId, sale.UserId,
                        CreatedAt = sale.CreatedAt.ToString("o"),
                        UpdatedAt = sale.UpdatedAt.ToString("o")
                    }, transaction
                );
                
                // Insertar items y actualizar stock
                foreach (var item in items)
                {
                    item.SaleId = sale.Id;
                    item.CreatedAt = DateTime.UtcNow;
                    item.NeedSync = true;
                    
                    connection.Execute(@"
                        INSERT INTO sale_items (id, sale_id, product_id, product_name, quantity,
                            unit_price, discount, total, notes, created_at, need_sync)
                        VALUES (@Id, @SaleId, @ProductId, @ProductName, @Quantity,
                            @UnitPrice, @Discount, @Total, @Notes, @CreatedAt, 1)",
                        new {
                            item.Id, item.SaleId, item.ProductId, item.ProductName, item.Quantity,
                            item.UnitPrice, item.Discount, item.Total, item.Notes,
                            CreatedAt = item.CreatedAt.ToString("o")
                        }, transaction
                    );
                    
                    // Actualizar stock
                    connection.Execute(
                        "UPDATE products SET stock = stock - @Quantity, updated_at = @UpdatedAt, need_sync = 1 WHERE id = @Id",
                        new { Id = item.ProductId, Quantity = item.Quantity, UpdatedAt = DateTime.UtcNow.ToString("o") },
                        transaction
                    );
                }
                
                transaction.Commit();
                
                // Log para sincronizacion
                LogSync(connection, "sales", sale.Id, "insert");
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        
        public (decimal Total, int Count, decimal Average) GetTodayStats()
        {
            using var connection = DatabaseManager.GetConnection();
            var today = DateTime.Today.ToString("o");
            var tomorrow = DateTime.Today.AddDays(1).ToString("o");
            
            var result = connection.QueryFirstOrDefault<(decimal Total, int Count)>(
                @"SELECT COALESCE(SUM(total), 0) as Total, COUNT(*) as Count 
                  FROM sales 
                  WHERE restaurant_id = @RestaurantId 
                  AND status = 'completed'
                  AND created_at >= @Today AND created_at < @Tomorrow",
                new { RestaurantId = _restaurantId, Today = today, Tomorrow = tomorrow }
            );
            
            var average = result.Count > 0 ? result.Total / result.Count : 0;
            return (result.Total, result.Count, average);
        }
        
        private void LogSync(Microsoft.Data.Sqlite.SqliteConnection connection, string tableName, string recordId, string operation)
        {
            connection.Execute(@"
                INSERT INTO sync_log (table_name, record_id, operation, created_at)
                VALUES (@TableName, @RecordId, @Operation, @CreatedAt)",
                new { TableName = tableName, RecordId = recordId, Operation = operation, CreatedAt = DateTime.UtcNow.ToString("o") }
            );
        }
    }
}
