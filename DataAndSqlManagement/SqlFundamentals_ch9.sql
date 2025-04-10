/* What is a Stored Procedure?

A Stored Procedure is simply a set of SQL commands that are pre-written and stored in your database. 
Think of it as a saved set of instructions that can be executed whenever needed. 
It's like a batch file (in programming), but specifically for your database. 

The procedure is saved in the database and can be executed as a single operation, 
instead of writing the same SQL commands repeatedly.


--- Why Use Stored Procedures?

1. Performance: Since the procedures are precompiled and are stored in the database, 
it can execute faster than running regular SQL queries. 
The database doesn't have to recompile the query every time—it’s ready to go.

2. Reusability: You can reuse the same procedure across multiple applications or parts of your application. 
This saves time and effort, as you don’t have to write the same SQL code again and again.

3. Security: Stored procedures help enhance security. Instead of giving users direct access to the database tables, 
you can give them permission to execute specific procedures. 
This limits what they can do and reduces risk.

4. Encapsulation: Logic can be stored and managed in the database, simplifying code management. 
You can update the stored procedure once, and the changes will reflect in all the places where it's used.

5. Reduced Network Traffic: When you execute a stored procedure, the data doesn’t have to travel over the network multiple times. 
The procedure runs in the database, reducing the amount of data sent between the application and the database.

*/


/* How to Create and Use Stored Procedures?

Example 1: Basic Stored Procedure
Here’s how to create a simple stored procedure that gets users by their status (active or inactive):

``` sql

CREATE TABLE Users 
(
	UID UNIQUEIDENTIFIER,
	Name VARCHAR(100),
	Email VARCHAR(100),
	IsDisabled BIT,
	PRIMARY KEY (UID)
)

INSERT INTO Users (UID, Name, Email, IsDisabled)
VALUES
	(NEWID(), 'Mahammad Ahmadov', 'aysel.mammadova@example.com', 1),
    (NEWID(), 'Aysel Mammadova', 'aysel.mammadova@example.com', 0),
    (NEWID(), 'Farid Aliyev', 'farid.aliyev@example.com', 0),
    (NEWID(), 'Sevinj Hasanova', 'sevinj.hasanova@example.com', 0),
    (NEWID(), 'Rashad Guliyev', 'rashad.guliyev@example.com', 1),
    (NEWID(), 'Nigar Jafarova', 'nigar.jafarova@example.com', 0),
    (NEWID(), 'Elnur Huseynov', 'elnur.huseynov@example.com', 0),
    (NEWID(), 'Gunel Mammadli', 'gunel.mammadli@example.com', 0),
    (NEWID(), 'Tural Aliyev', 'tural.aliyev@example.com', 1),
    (NEWID(), 'Lamiya Aghayeva', 'lamiya.aghayeva@example.com', 0),
    (NEWID(), 'Vugar Sadiqli', 'vugar.sadiqli@example.com', 0);

```

CREATE PROCEDURE GetUsersByStatus
	@Status BIT
AS
BEGIN
	SELECT UID, Name, Email
    FROM USERS
    WHERE IsDisabled = @Status;
END;

@Status: This is a parameter that you pass into the procedure to filter the users by their active/inactive status.
BEGIN/END: These keywords define the body of the procedure. 
           All SQL commands inside these keywords will be executed when the procedure is called.



--- Execute Stored Procedure:
You can execute the stored procedure like this:

EXEC GetUsersByStatus 0;  -- Here 0 means inactive users
This will execute the procedure and return the users who are inactive (isDisabled = 0).


UID	                                    Name	        Email
44D7C4C7-E0F9-4612-AAAA-5070E5BF76AA	Elnur Huseynov	elnur.huseynov@example.com
9A1BA4A8-7003-4AA2-A4C7-751C3BCD1512	Lamiya Aghayeva	lamiya.aghayeva@example.com
8362EE06-F1CF-46E2-9240-8E6F45D2687E	Nigar Jafarova	nigar.jafarova@example.com
C80FB3C1-1BF5-415F-ABBE-9DF3CE67E2B5	Gunel Mammadli	gunel.mammadli@example.com
496AAAFC-995F-4791-8CA1-A6705F393958	Sevinj Hasanova	sevinj.hasanova@example.com
E6B51E1B-3D50-4882-A59F-BC3AF15B9CDE	Aysel Mammadova	aysel.mammadova@example.com
9AE78244-F971-4B8A-B37C-E66D1222FA3A	Farid Aliyev	farid.aliyev@example.com
2B1FB023-03CB-47D5-967F-FF08786C204F	Vugar Sadiqli	vugar.sadiqli@example.com



--- Stored Procedures vs. Functions
Stored procedures and functions are both used in SQL, but they have differences:

Stored Procedures:

: They can perform actions but do not necessarily return a value.
: They can modify data (e.g., INSERT, UPDATE, DELETE).
: Can have IN, OUT, and INOUT parameters.
: They can commit or rollback transactions.
: Called using EXEC or CALL.

Functions:

: They must return a value (usually scalar values or a result set).
: Can be used within queries (e.g., SELECT statement).
: Cannot perform transactions (no COMMIT or ROLLBACK).
: Functions are typically used for calculations or data retrieval.

*/

CREATE PROCEDURE GetUsersByStatus
	@Status BIT
AS
BEGIN
	SELECT UID, Name, Email
    FROM USERS
    WHERE IsDisabled = @Status;
END;

EXEC GetUsersByStatus 1;  -- Here 0 means inactive users


/* Why Prefer Stored Procedures Over Entity Framework (EF)?

While Entity Framework (EF) is widely used for database interaction in .NET, 
Stored Procedures can often outperform EF in certain scenarios:

1. Precompiled: Stored procedures are already compiled and optimized by the database engine. 
EF queries are compiled at runtime, which can add overhead, making stored procedures faster for complex or frequently executed queries.

2. Optimized Execution Plans: SQL Server (or other DBMS) can create optimized execution plans for stored procedures, 
which improve performance by reusing the plan. 
EF queries, however, may not benefit from this kind of optimization.

3. Less Network Traffic: Stored procedures reduce network traffic because multiple SQL commands can be executed in a single database call. 
EF may require multiple requests to the database, increasing the amount of data transmitted.

4. Transaction Handling: Stored procedures can handle transactions directly using BEGIN TRANSACTION, COMMIT, and ROLLBACK, 
making them ideal for operations that need to be atomic (all or nothing). 
EF, on the other hand, handles transactions at a higher level.

4.1. Transaction with Stored Procedures, lower level.

CREATE PROCEDURE UpdateUserAndLog
AS
BEGIN
    BEGIN TRANSACTION

    UPDATE Users SET Name = 'Ali' WHERE Id = 1
    INSERT INTO Logs VALUES ('User updated')

    COMMIT
END

4.2. Handling Transaction at higher level. in C# for example:

using var transaction = connection.BeginTransaction();

try
{
    // 1. Call stored procedure
    var cmd1 = new SqlCommand("EXEC UpdateUser @Id, @Name", connection, transaction);
    // 2. Call another one
    var cmd2 = new SqlCommand("EXEC LogAction @Message", connection, transaction);

    cmd1.ExecuteNonQuery();
    cmd2.ExecuteNonQuery();

    transaction.Commit();
}
catch
{
    transaction.Rollback();
}

*/

/* Integration with .NET (EF or ADO.NET)
You can easily call a stored procedure in .NET using ADO.NET or Entity Framework (EF):

const string connectionString = "Data Source=ServerName;Initial Catalog=DbName;Integrated Security=true;TrustServerCertificate=True";
using SqlConnection connection = new SqlConnection(connectionString);
SqlCommand command = new SqlCommand("GetUsersByStatus", connection)
{
    CommandType = CommandType.StoredProcedure
};

command.Parameters.Add(new SqlParameter()
{
    ParameterName = "Status",
    Value = 0
});

connection.Open();
SqlDataReader reader = command.ExecuteReader();

while (reader.Read())
{
    Console.WriteLine($"{reader.GetGuid(0)}: {reader.GetString(1)} - {reader.GetString(2)}");
}

reader.Close();




--- EF Core Example (Calling a Stored Procedure):
var result = dbContext.Users.FromSqlRaw("EXEC GetUsersByStatus @Status", new SqlParameter("@Status", 0)).ToList();

*/


/* User Registration with Validations

CREATE PROCEDURE RegisterUser
	@Name VARCHAR(100),
	@Email NVARCHAR(100),
    @ResultMessage NVARCHAR(200) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION

		IF EXISTS (SELECT 1 FROM USERS WHERE Name = @Name)
		BEGIN
            SET @ResultMessage = 'Username already exists.';
            ROLLBACK TRANSACTION;
            RETURN;
        END

		INSERT INTO Users (UID, Name, Email, IsDisabled)
        VALUES (NEWID(), @Name, @Email, 1);

		SET @ResultMessage = 'User registered successfully.';
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;

		SET @ResultMessage = 'An error occurred: ' + ERROR_MESSAGE(); 
	END CATCH

END;



--- Calling from AdoNet

private static void Main(string[] args)
{
    const string connectionString = "";
    using SqlConnection connection = new SqlConnection(connectionString);
    connection.Open();

    SqlCommand command = new SqlCommand("RegisterUser", connection)
    {
        CommandType = CommandType.StoredProcedure
    };

    command.Parameters.AddWithValue("@Name", "Mahammad Ahmadov"); // existing user
    command.Parameters.AddWithValue("@Email", "mahammad@example.com");

    SqlParameter resultParameter = new SqlParameter()
    {
        ParameterName = "ResultMessage",
        SqlDbType = SqlDbType.NVarChar,
        Size = 200,
        Direction = ParameterDirection.Output,
    };
    command.Parameters.Add(resultParameter);

    command.ExecuteNonQuery();
    Console.WriteLine(resultParameter.Value);
}

Output: Username already exists.
and with different Username: User registered successfully.

*/

CREATE PROCEDURE RegisterUser
	@Name VARCHAR(100),
	@Email NVARCHAR(100),
    @ResultMessage NVARCHAR(200) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION

		IF EXISTS (SELECT 1 FROM USERS WHERE Name = @Name)
		BEGIN
            SET @ResultMessage = 'Username already exists.';
            ROLLBACK TRANSACTION;
            RETURN;
        END

		INSERT INTO Users (UID, Name, Email, IsDisabled)
        VALUES (NEWID(), @Name, @Email, 1);

		SET @ResultMessage = 'User registered successfully.';
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;

		SET @ResultMessage = 'An error occurred: ' + ERROR_MESSAGE(); 
	END CATCH

END;


/* SET NOCOUNT ON

When you run SQL commands like INSERT, UPDATE, or DELETE, 
SQL Server automatically returns a message like this:

(3 rows affected)
This is called the row count message.


---  What does SET NOCOUNT ON do?
It turns off that message, so SQL Server doesn't send "X rows affected" after every command.
It makes procedures faster by reducing unnecessary messages.

*/