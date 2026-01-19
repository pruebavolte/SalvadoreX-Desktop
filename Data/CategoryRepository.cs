using Dapper;
using SalvadoreXDesktop.Models;

namespace SalvadoreXDesktop.Data
{
    public class CategoryRepository
    {
        private readonly string _restaurantId;
        
        public CategoryRepository()
        {
            _restaurantId = DatabaseManager.GetSetting("restaurant_id", "00000000-0000-0000-0000-000000000001");
        }
        
        public List<Category> GetAll(bool activeOnly = true)
        {
            using var connection = DatabaseManager.GetConnection();
            var sql = "SELECT * FROM categories WHERE restaurant_id = @RestaurantId";
            
            if (activeOnly)
                sql += " AND active = 1";
                
            sql += " ORDER BY display_order, name";
            
            return connection.Query<Category>(sql, new { RestaurantId = _restaurantId }).ToList();
        }
        
        public Category? GetById(string id)
        {
            using var connection = DatabaseManager.GetConnection();
            return connection.QueryFirstOrDefault<Category>(
                "SELECT * FROM categories WHERE id = @Id",
                new { Id = id }
            );
        }
        
        public void Insert(Category category)
        {
            category.RestaurantId = _restaurantId;
            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            category.NeedSync = true;
            
            using var connection = DatabaseManager.GetConnection();
            connection.Execute(@"
                INSERT INTO categories (id, name, description, color, icon, parent_id, display_order,
                    active, restaurant_id, created_at, updated_at, need_sync)
                VALUES (@Id, @Name, @Description, @Color, @Icon, @ParentId, @DisplayOrder,
                    @Active, @RestaurantId, @CreatedAt, @UpdatedAt, 1)",
                new {
                    category.Id, category.Name, category.Description, category.Color, category.Icon,
                    category.ParentId, category.DisplayOrder, Active = category.Active ? 1 : 0,
                    category.RestaurantId, 
                    CreatedAt = category.CreatedAt.ToString("o"), 
                    UpdatedAt = category.UpdatedAt.ToString("o")
                }
            );
        }
        
        public void Update(Category category)
        {
            category.UpdatedAt = DateTime.UtcNow;
            category.NeedSync = true;
            
            using var connection = DatabaseManager.GetConnection();
            connection.Execute(@"
                UPDATE categories SET 
                    name = @Name, description = @Description, color = @Color, icon = @Icon,
                    parent_id = @ParentId, display_order = @DisplayOrder, active = @Active,
                    updated_at = @UpdatedAt, need_sync = 1
                WHERE id = @Id",
                new {
                    category.Id, category.Name, category.Description, category.Color, category.Icon,
                    category.ParentId, category.DisplayOrder, Active = category.Active ? 1 : 0,
                    UpdatedAt = category.UpdatedAt.ToString("o")
                }
            );
        }
        
        public void Delete(string id)
        {
            using var connection = DatabaseManager.GetConnection();
            var now = DateTime.UtcNow.ToString("o");
            connection.Execute(
                "UPDATE categories SET active = 0, updated_at = @UpdatedAt, need_sync = 1 WHERE id = @Id",
                new { Id = id, UpdatedAt = now }
            );
        }
    }
}
