using Dapper;
using SalvadoreXDesktop.Models;

namespace SalvadoreXDesktop.Data
{
    public class ProductRepository
    {
        private readonly string _restaurantId;
        
        public ProductRepository()
        {
            _restaurantId = DatabaseManager.GetSetting("restaurant_id", "00000000-0000-0000-0000-000000000001");
        }
        
        public List<Product> GetAll(bool activeOnly = true)
        {
            using var connection = DatabaseManager.GetConnection();
            var sql = @"SELECT p.*, c.name as CategoryName, c.color as CategoryColor
                        FROM products p
                        LEFT JOIN categories c ON p.category_id = c.id
                        WHERE p.restaurant_id = @RestaurantId";
            
            if (activeOnly)
                sql += " AND p.active = 1";
                
            sql += " ORDER BY c.display_order, p.name";
            
            var products = connection.Query<Product>(sql, new { RestaurantId = _restaurantId }).ToList();
            return products;
        }
        
        public List<Product> GetByCategory(string categoryId)
        {
            using var connection = DatabaseManager.GetConnection();
            var sql = @"SELECT * FROM products 
                        WHERE restaurant_id = @RestaurantId 
                        AND category_id = @CategoryId 
                        AND active = 1
                        ORDER BY name";
            return connection.Query<Product>(sql, new { RestaurantId = _restaurantId, CategoryId = categoryId }).ToList();
        }
        
        public Product? GetById(string id)
        {
            using var connection = DatabaseManager.GetConnection();
            return connection.QueryFirstOrDefault<Product>(
                "SELECT * FROM products WHERE id = @Id",
                new { Id = id }
            );
        }
        
        public Product? GetByBarcode(string barcode)
        {
            using var connection = DatabaseManager.GetConnection();
            return connection.QueryFirstOrDefault<Product>(
                "SELECT * FROM products WHERE barcode = @Barcode AND active = 1",
                new { Barcode = barcode }
            );
        }
        
        public List<Product> Search(string query)
        {
            using var connection = DatabaseManager.GetConnection();
            var searchTerm = $"%{query}%";
            var sql = @"SELECT * FROM products 
                        WHERE restaurant_id = @RestaurantId 
                        AND active = 1
                        AND (name LIKE @Search OR sku LIKE @Search OR barcode LIKE @Search)
                        ORDER BY name
                        LIMIT 50";
            return connection.Query<Product>(sql, new { RestaurantId = _restaurantId, Search = searchTerm }).ToList();
        }
        
        public void Insert(Product product)
        {
            product.RestaurantId = _restaurantId;
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            product.NeedSync = true;
            
            using var connection = DatabaseManager.GetConnection();
            connection.Execute(@"
                INSERT INTO products (id, name, description, price, cost, sku, barcode, stock, min_stock, 
                    category_id, image_url, active, available_pos, available_digital_menu, 
                    restaurant_id, created_at, updated_at, need_sync)
                VALUES (@Id, @Name, @Description, @Price, @Cost, @Sku, @Barcode, @Stock, @MinStock,
                    @CategoryId, @ImageUrl, @Active, @AvailablePos, @AvailableDigitalMenu,
                    @RestaurantId, @CreatedAt, @UpdatedAt, @NeedSync)",
                new {
                    product.Id, product.Name, product.Description, product.Price, product.Cost,
                    product.Sku, product.Barcode, product.Stock, product.MinStock, product.CategoryId,
                    product.ImageUrl, Active = product.Active ? 1 : 0, 
                    AvailablePos = product.AvailablePos ? 1 : 0,
                    AvailableDigitalMenu = product.AvailableDigitalMenu ? 1 : 0,
                    product.RestaurantId, 
                    CreatedAt = product.CreatedAt.ToString("o"), 
                    UpdatedAt = product.UpdatedAt.ToString("o"),
                    NeedSync = 1
                }
            );
            
            LogSync("products", product.Id, "insert");
        }
        
        public void Update(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            product.NeedSync = true;
            
            using var connection = DatabaseManager.GetConnection();
            connection.Execute(@"
                UPDATE products SET 
                    name = @Name, description = @Description, price = @Price, cost = @Cost,
                    sku = @Sku, barcode = @Barcode, stock = @Stock, min_stock = @MinStock,
                    category_id = @CategoryId, image_url = @ImageUrl, active = @Active,
                    available_pos = @AvailablePos, available_digital_menu = @AvailableDigitalMenu,
                    updated_at = @UpdatedAt, need_sync = 1
                WHERE id = @Id",
                new {
                    product.Id, product.Name, product.Description, product.Price, product.Cost,
                    product.Sku, product.Barcode, product.Stock, product.MinStock, product.CategoryId,
                    product.ImageUrl, Active = product.Active ? 1 : 0, 
                    AvailablePos = product.AvailablePos ? 1 : 0,
                    AvailableDigitalMenu = product.AvailableDigitalMenu ? 1 : 0,
                    UpdatedAt = product.UpdatedAt.ToString("o")
                }
            );
            
            LogSync("products", product.Id, "update");
        }
        
        public void UpdateStock(string productId, int newStock)
        {
            using var connection = DatabaseManager.GetConnection();
            var now = DateTime.UtcNow.ToString("o");
            connection.Execute(
                "UPDATE products SET stock = @Stock, updated_at = @UpdatedAt, need_sync = 1 WHERE id = @Id",
                new { Id = productId, Stock = newStock, UpdatedAt = now }
            );
            
            LogSync("products", productId, "update");
        }
        
        public void Delete(string id)
        {
            using var connection = DatabaseManager.GetConnection();
            var now = DateTime.UtcNow.ToString("o");
            connection.Execute(
                "UPDATE products SET active = 0, updated_at = @UpdatedAt, need_sync = 1 WHERE id = @Id",
                new { Id = id, UpdatedAt = now }
            );
            
            LogSync("products", id, "delete");
        }
        
        private void LogSync(string tableName, string recordId, string operation)
        {
            using var connection = DatabaseManager.GetConnection();
            connection.Execute(@"
                INSERT INTO sync_log (table_name, record_id, operation, created_at)
                VALUES (@TableName, @RecordId, @Operation, @CreatedAt)",
                new { TableName = tableName, RecordId = recordId, Operation = operation, CreatedAt = DateTime.UtcNow.ToString("o") }
            );
        }
    }
}
