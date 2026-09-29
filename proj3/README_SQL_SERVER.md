# POJ3 - Dynamic Electronics Store (ASP.NET Core MVC + SQL Server)

## Technology
- ASP.NET Core MVC (.NET 10)
- C#
- SQL Server / SQL Server Express
- ADO.NET (`Microsoft.Data.SqlClient`)
- Bootstrap / AdminLTE / Electro UI template

## Database setup
1. Open SQL Server Management Studio (SSMS).
2. Open `DATABASE_SETUP.sql`.
3. Execute the complete script.
4. It creates database `pr3` and tables:
   - `register`
   - `products`
   - `favorites`
   - `orders`
   - `order_items`
5. It also inserts 8 sample products.

## Connection string
Open `appsettings.json`:

`Data Source=.\SQLEXPRESS;Initial Catalog=pr3;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True`

If your SQL Server instance is different, change only `Data Source`.
Examples:
- `Data Source=localhost`
- `Data Source=.\SQLEXPRESS`
- `Data Source=DESKTOP-PC\SQLEXPRESS`

If you use SQL Server username/password instead of Windows Authentication, use:
`Data Source=SERVER;Initial Catalog=pr3;User ID=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True`

## Dynamic functionality now connected to SQL Server
- Registration -> `register`
- Login/session -> `register`
- Forgot password -> `register`
- Manage account update/delete -> `register`
- Product listing/search/category -> `products`
- Product details -> `products`
- Favorites add/remove -> `favorites`
- Cart -> session (temporary shopping cart)
- Checkout -> `orders` + `order_items`
- Stock is reduced in SQL Server when an order is placed
- Order number and total are generated automatically

## Run
From Visual Studio:
1. Open `poj3.slnx` / project.
2. Make sure SQL Server is running.
3. Run `DATABASE_SETUP.sql`.
4. Run the ASP.NET project.
5. Register a new account and login.
6. Products are loaded from SQL Server.

## Important
The original project contained a hard-coded static product list. It has been replaced with SQL Server product retrieval, so changing rows in `dbo.products` changes the store without editing C# code.

The shopping cart intentionally remains session-based so the existing UI works immediately. Orders, favorites, users, and stock are persistent in SQL Server.
