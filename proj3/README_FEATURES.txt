UPDATED ASP.NET CORE MVC FEATURES
================================

1. CART QUANTITY +/-
   - + button increases quantity.
   - - button decreases quantity.
   - When quantity changes, item total = Price * Quantity is recalculated automatically on the next page render.
   - Cart grand total is recalculated from all item totals.
   - Quantity cannot exceed the product stock.

2. FAVORITES
   - Product page has Add to Favorites / Remove Favorite.
   - Favorites are stored in session.
   - /User/Favorites shows only products that the current session marked as favorites.
   - Header Favorites icon shows the favorites count.

3. MANAGE ACCOUNT
   - Login stores the registered user's id in Session["UserId"].
   - /User/ManageAccount loads the exact name, email and password of the logged-in registered user from dbo.register.
   - User can update name, email and password.
   - User can permanently delete their account.
   - Logout clears the session.

4. LOGIN
   - Successful login now stores UserId, UserName and UserEmail in session.
   - After login the user is redirected to User/home.

5. DATABASE
   - DATABASE_SETUP.sql is included.
   - Existing project connection string:
     Data Source=.\SQLEXPRESS;Initial Catalog=pr3;Integrated Security=True;pwd=123;TrustServerCertificate=True

IMPORTANT
---------
The project uses SQL Server table:
dbo.register(id, name, email, password)

Open the project in Visual Studio, restore NuGet packages, run the database script if the table does not exist, and start the project.
