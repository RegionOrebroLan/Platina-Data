Scripts to set up database and then scaffold db-contxt. To test howto, then code-first.

- Create a LocalDB-database named "07390b07-29ed-4944-aced-df2ad5b4dbc2".
- Run SqlServer-database.sql in it
- Run: Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=07390b07-29ed-4944-aced-df2ad5b4dbc2" Microsoft.EntityFrameworkCore.SqlServer -ContextDir "" -OutputDir "Your-directory"
- Delete the database