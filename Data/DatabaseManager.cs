using Microsoft.Data.Sqlite;
using Dapper;
using Newtonsoft.Json;

namespace SalvadoreXDesktop.Data
{
    public static class DatabaseManager
    {
        private static string _connectionString = string.Empty;
        private static readonly string DbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SalvadoreX",
            "salvadorex_local.db"
        );
        
        public static string ConnectionString => _connectionString;
        
        public static void Initialize()
        {
            // Crear directorio si no existe
            var directory = Path.GetDirectoryName(DbPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            _connectionString = $"Data Source={DbPath}";
            
            using var connection = GetConnection();
            connection.Open();
            
            // Crear tablas
            CreateTables(connection);
            
            // Insertar datos de prueba si la base esta vacia
            InsertSampleDataIfEmpty(connection);
        }
        
        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
        
        private static void CreateTables(SqliteConnection connection)
        {
            var sql = @"
                -- Tabla de categorias
                CREATE TABLE IF NOT EXISTS categories (
                    id TEXT PRIMARY KEY,
                    name TEXT NOT NULL,
                    description TEXT,
                    color TEXT DEFAULT '#3B82F6',
                    icon TEXT,
                    parent_id TEXT,
                    display_order INTEGER DEFAULT 0,
                    active INTEGER DEFAULT 1,
                    restaurant_id TEXT NOT NULL,
                    created_at TEXT NOT NULL,
                    updated_at TEXT NOT NULL,
                    need_sync INTEGER DEFAULT 0,
                    last_sync_at TEXT
                );
                
                -- Tabla de productos
                CREATE TABLE IF NOT EXISTS products (
                    id TEXT PRIMARY KEY,
                    name TEXT NOT NULL,
                    description TEXT,
                    price REAL NOT NULL,
                    cost REAL DEFAULT 0,
                    sku TEXT,
                    barcode TEXT,
                    stock INTEGER DEFAULT 0,
                    min_stock INTEGER DEFAULT 0,
                    category_id TEXT,
                    image_url TEXT,
                    active INTEGER DEFAULT 1,
                    available_pos INTEGER DEFAULT 1,
                    available_digital_menu INTEGER DEFAULT 1,
                    restaurant_id TEXT NOT NULL,
                    created_at TEXT NOT NULL,
                    updated_at TEXT NOT NULL,
                    need_sync INTEGER DEFAULT 0,
                    last_sync_at TEXT,
                    FOREIGN KEY (category_id) REFERENCES categories(id)
                );
                
                -- Tabla de clientes
                CREATE TABLE IF NOT EXISTS customers (
                    id TEXT PRIMARY KEY,
                    name TEXT NOT NULL,
                    email TEXT,
                    phone TEXT,
                    address TEXT,
                    rfc TEXT,
                    credit_limit REAL DEFAULT 0,
                    current_credit REAL DEFAULT 0,
                    loyalty_points INTEGER DEFAULT 0,
                    notes TEXT,
                    active INTEGER DEFAULT 1,
                    restaurant_id TEXT NOT NULL,
                    created_at TEXT NOT NULL,
                    updated_at TEXT NOT NULL,
                    need_sync INTEGER DEFAULT 0,
                    last_sync_at TEXT
                );
                
                -- Tabla de ventas
                CREATE TABLE IF NOT EXISTS sales (
                    id TEXT PRIMARY KEY,
                    customer_id TEXT,
                    customer_name TEXT,
                    subtotal REAL NOT NULL,
                    tax REAL DEFAULT 0,
                    discount REAL DEFAULT 0,
                    total REAL NOT NULL,
                    payment_method TEXT DEFAULT 'cash',
                    status TEXT DEFAULT 'completed',
                    notes TEXT,
                    receipt_number TEXT,
                    restaurant_id TEXT NOT NULL,
                    user_id TEXT,
                    created_at TEXT NOT NULL,
                    updated_at TEXT NOT NULL,
                    need_sync INTEGER DEFAULT 0,
                    last_sync_at TEXT,
                    FOREIGN KEY (customer_id) REFERENCES customers(id)
                );
                
                -- Tabla de items de venta
                CREATE TABLE IF NOT EXISTS sale_items (
                    id TEXT PRIMARY KEY,
                    sale_id TEXT NOT NULL,
                    product_id TEXT NOT NULL,
                    product_name TEXT NOT NULL,
                    quantity INTEGER NOT NULL,
                    unit_price REAL NOT NULL,
                    discount REAL DEFAULT 0,
                    total REAL NOT NULL,
                    notes TEXT,
                    created_at TEXT NOT NULL,
                    need_sync INTEGER DEFAULT 0,
                    FOREIGN KEY (sale_id) REFERENCES sales(id),
                    FOREIGN KEY (product_id) REFERENCES products(id)
                );
                
                -- Tabla de ingredientes
                CREATE TABLE IF NOT EXISTS ingredients (
                    id TEXT PRIMARY KEY,
                    name TEXT NOT NULL,
                    description TEXT,
                    sku TEXT,
                    category TEXT,
                    current_stock REAL DEFAULT 0,
                    min_stock REAL DEFAULT 0,
                    max_stock REAL DEFAULT 0,
                    unit_type TEXT DEFAULT 'weight',
                    unit_name TEXT DEFAULT 'kg',
                    cost_per_unit REAL DEFAULT 0,
                    active INTEGER DEFAULT 1,
                    restaurant_id TEXT NOT NULL,
                    created_at TEXT NOT NULL,
                    updated_at TEXT NOT NULL,
                    need_sync INTEGER DEFAULT 0,
                    last_sync_at TEXT
                );
                
                -- Tabla de log de sincronizacion
                CREATE TABLE IF NOT EXISTS sync_log (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    table_name TEXT NOT NULL,
                    record_id TEXT NOT NULL,
                    operation TEXT NOT NULL,
                    data TEXT,
                    synced INTEGER DEFAULT 0,
                    created_at TEXT NOT NULL,
                    synced_at TEXT,
                    error TEXT,
                    retry_count INTEGER DEFAULT 0
                );
                
                -- Tabla de configuracion
                CREATE TABLE IF NOT EXISTS settings (
                    key TEXT PRIMARY KEY,
                    value TEXT NOT NULL,
                    updated_at TEXT NOT NULL
                );
                
                -- Indices para mejor rendimiento
                CREATE INDEX IF NOT EXISTS idx_products_category ON products(category_id);
                CREATE INDEX IF NOT EXISTS idx_products_barcode ON products(barcode);
                CREATE INDEX IF NOT EXISTS idx_sales_date ON sales(created_at);
                CREATE INDEX IF NOT EXISTS idx_sale_items_sale ON sale_items(sale_id);
                CREATE INDEX IF NOT EXISTS idx_sync_log_pending ON sync_log(synced, table_name);
            ";
            
            connection.Execute(sql);
        }
        
        private static void InsertSampleDataIfEmpty(SqliteConnection connection)
        {
            var count = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM categories");
            if (count > 0) return;
            
            var restaurantId = "00000000-0000-0000-0000-000000000001";
            var now = DateTime.UtcNow.ToString("o");
            
            // Insertar categorias
            var categories = new[]
            {
                ("cat-001", "Hamburguesas", "Hamburguesas y sandwiches", "#EF4444"),
                ("cat-002", "Bebidas", "Refrescos y bebidas", "#3B82F6"),
                ("cat-003", "Tacos", "Tacos y antojitos", "#F59E0B"),
                ("cat-004", "Postres", "Postres y dulces", "#EC4899"),
                ("cat-005", "Extras", "Complementos y extras", "#10B981")
            };
            
            foreach (var (id, name, desc, color) in categories)
            {
                connection.Execute(
                    @"INSERT INTO categories (id, name, description, color, restaurant_id, created_at, updated_at)
                      VALUES (@Id, @Name, @Description, @Color, @RestaurantId, @CreatedAt, @UpdatedAt)",
                    new { Id = id, Name = name, Description = desc, Color = color, RestaurantId = restaurantId, CreatedAt = now, UpdatedAt = now }
                );
            }
            
            // Insertar productos
            var products = new[]
            {
                ("prod-001", "Hamburguesa Clasica", 89.00m, "cat-001", "HAM-001"),
                ("prod-002", "Hamburguesa Doble", 119.00m, "cat-001", "HAM-002"),
                ("prod-003", "Hamburguesa BBQ", 109.00m, "cat-001", "HAM-003"),
                ("prod-004", "Refresco Cola", 25.00m, "cat-002", "BEB-001"),
                ("prod-005", "Agua Natural", 18.00m, "cat-002", "BEB-002"),
                ("prod-006", "Jugo de Naranja", 35.00m, "cat-002", "BEB-003"),
                ("prod-007", "Taco de Bistec", 28.00m, "cat-003", "TAC-001"),
                ("prod-008", "Taco de Pastor", 25.00m, "cat-003", "TAC-002"),
                ("prod-009", "Helado de Vainilla", 45.00m, "cat-004", "POS-001"),
                ("prod-010", "Papas Fritas", 35.00m, "cat-005", "EXT-001")
            };
            
            foreach (var (id, name, price, catId, sku) in products)
            {
                connection.Execute(
                    @"INSERT INTO products (id, name, price, category_id, sku, stock, restaurant_id, created_at, updated_at)
                      VALUES (@Id, @Name, @Price, @CategoryId, @Sku, 100, @RestaurantId, @CreatedAt, @UpdatedAt)",
                    new { Id = id, Name = name, Price = price, CategoryId = catId, Sku = sku, RestaurantId = restaurantId, CreatedAt = now, UpdatedAt = now }
                );
            }
            
            // Insertar clientes de prueba
            var customers = new[]
            {
                ("cust-001", "Juan Perez", "juan@email.com", "555-1234"),
                ("cust-002", "Maria Garcia", "maria@email.com", "555-5678"),
                ("cust-003", "Carlos Lopez", "carlos@email.com", "555-9012")
            };
            
            foreach (var (id, name, email, phone) in customers)
            {
                connection.Execute(
                    @"INSERT INTO customers (id, name, email, phone, restaurant_id, created_at, updated_at)
                      VALUES (@Id, @Name, @Email, @Phone, @RestaurantId, @CreatedAt, @UpdatedAt)",
                    new { Id = id, Name = name, Email = email, Phone = phone, RestaurantId = restaurantId, CreatedAt = now, UpdatedAt = now }
                );
            }
            
            // Guardar configuracion inicial
            connection.Execute(
                @"INSERT OR REPLACE INTO settings (key, value, updated_at) VALUES (@Key, @Value, @UpdatedAt)",
                new { Key = "restaurant_id", Value = restaurantId, UpdatedAt = now }
            );
        }
        
        public static string GetSetting(string key, string defaultValue = "")
        {
            using var connection = GetConnection();
            var value = connection.ExecuteScalar<string>(
                "SELECT value FROM settings WHERE key = @Key", 
                new { Key = key }
            );
            return value ?? defaultValue;
        }
        
        public static void SetSetting(string key, string value)
        {
            using var connection = GetConnection();
            var now = DateTime.UtcNow.ToString("o");
            connection.Execute(
                @"INSERT OR REPLACE INTO settings (key, value, updated_at) VALUES (@Key, @Value, @UpdatedAt)",
                new { Key = key, Value = value, UpdatedAt = now }
            );
        }
    }
}
