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
(
	@Name VARCHAR(100),
	@Email NVARCHAR(100),
    @ResultMessage NVARCHAR(200) OUTPUT
)
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



/* What Is a UDF? And When Should You Use One?
A User Defined Function (UDF) in SQL is something developers create to perform an action and return a result. 
SQL already has many Built-in Functions that can do common tasks like work with strings, numbers, dates, and more. 


------------------------------
Built-in SQL Functions
Built-in functions (also called native functions) are ready-made tools that come with SQL. 


--- String Functions

- `UPPER()`: Changes text to uppercase
  SELECT UPPER('hello') -- Returns 'HELLO'
  
- `LOWER()`: Changes text to lowercase
  SELECT LOWER('HELLO') -- Returns 'hello'
  
- `LENGTH()` or `LEN()`: Counts characters in text
  SELECT LEN('hello world') -- Returns 11
  
- `SUBSTRING()`: Gets part of a text string
  SELECT SUBSTRING('hello world', 1, 5) -- Returns 'hello'
  

--- Numeric Functions

- `ROUND()`: Rounds a number to specified decimal places
  SELECT ROUND(5.678, 1) -- Returns 5.7
  
- `ABS()`: Gets absolute value (removes negative sign)
  SELECT ABS(-10) -- Returns 10
  
- `CEILING()`: Rounds up to nearest whole number
  SELECT CEILING(5.1) -- Returns 6
  
- `FLOOR()`: Rounds down to nearest whole number
  SELECT FLOOR(5.9) -- Returns 5
  

--- Date Functions

- `GETDATE()` or `NOW()`: Gets current date and time
  SELECT GETDATE() -- Returns current date and time
  
- `YEAR()`: Gets just the year from a date
  SELECT YEAR('2023-05-15') -- Returns 2023
  
- `MONTH()`: Gets just the month from a date
  SELECT MONTH('2023-05-15') -- Returns 5
  
- `DATEDIFF()`: Calculates difference between dates
  SELECT DATEDIFF(day, '2023-01-01', '2023-01-10') -- Returns 9

  
------------------------------

But when developers need to do complex calculations or transform data in special ways not covered by these built-in functions, 
they can create their own functions and save them in the database for future use. 

UDFs offer several benefits:

1. Making queries simpler: UDFs organize complex calculations and repeated operations into one place,
making your main query easier to read and maintain.

2. Reusing code: UDFs package logic and calculations into a single function that you can use in different queries, 
views, or procedures, reducing duplicate code.

3. Improving security: UDFs can limit direct access to certain data or operations. 
Sensitive information can be protected this way.

4. Better performance: UDFs are compiled and stored in the database, helping SQL queries run faster. 
They can also prevent unnecessary data transfers between the database and applications.


*/


/* Types of SQL UDFs
In SQL Server, there are three main types of UDFs:

1. Scalar UDFs: These work on a single row and return a single value.
2. Aggregate UDFs: These process multiple rows and return one aggregated value. 
   Creating these requires knowledge of programming languages like C# or VB.NET.
3. Table UDFs: These return a table that can be used like a regular table in queries.

Note: MySQL doesn't directly support Table UDFs, though similar results can be achieved using Stored Procedures or Views.

---
Basic Structure of a SQL UDF
Here's a general structure that works for creating most SQL UDFs:

CREATE FUNCTION [schema_name.]function_name
(
    @parameter_name1 datatype [= default_value],
    @parameter_name2 datatype [= default_value],
    ...
)
RETURNS return_type
[WITH <function_options>]
AS
BEGIN
    -- Function body: the logic of the function
    RETURN [value or table_expression];
END;
GO

*/


/* Example of a Scalar UDF
A scalar UDF produces a single value from one row of data. Here's a simple example that multiplies two numbers:

CREATE FUNCTION dbo.func_multiply
(
    @num_1 FLOAT,
    @num_2 FLOAT
)
RETURNS FLOAT
AS
BEGIN
    RETURN @num_1 * @num_2
END;

SELECT dbo.func_multiply(10.5, 2) AS MULTIPLY
and it returns 21

*/


/* Table UDFs Examples
Table UDFs return a table instead of a single value. There are two types:

In my other article, "The Most Useful Advanced SQL Techniques to Succeed in the Tech Industry," 
I used the mock sales data from a promotion at Star Department Store to introduce some advanced SQL techniques.

CREATE TABLE promo_sales
(
  Sale_Person_ID VARCHAR(40) PRIMARY KEY,
  Department VARCHAR(40),
  Sales_Amount INT
);

INSERT INTO promo_sales VALUES ('001', 'Cosmetics', 500);
INSERT INTO promo_sales VALUES ('002', 'Cosmetics', 700);
INSERT INTO promo_sales VALUES ('003', 'Fashion', 1000);
INSERT INTO promo_sales VALUES ('004', 'Jewellery', 800);
INSERT INTO promo_sales VALUES ('005', 'Fashion', 850);
INSERT INTO promo_sales VALUES ('006', 'Kid', 500);
INSERT INTO promo_sales VALUES ('007', 'Cosmetics', 900);
INSERT INTO promo_sales VALUES ('008', 'Fashion', 600);
INSERT INTO promo_sales VALUES ('009', 'Fashion', 1200);
INSERT INTO promo_sales VALUES ('010', 'Jewellery', 900);
INSERT INTO promo_sales VALUES ('011', 'Kid', 700);
INSERT INTO promo_sales VALUES ('012', 'Fashion', 1500);
INSERT INTO promo_sales VALUES ('013', 'Cosmetics', 850);
INSERT INTO promo_sales VALUES ('014', 'Kid', 750);
INSERT INTO promo_sales VALUES ('015', 'Jewellery', 950);

1. Inline Table-Valued UDFs
These contain a single SELECT statement and don't need a BEGIN...END block:

Sale_Person_ID	Department	    Sales_Amount
001	            Cosmetics	    500
002	            Cosmetics	    700
003	            Fashion	        1000
004	            Jewellery	    800
005	            Fashion	        850
006	            Kid	            500
007	            Cosmetics	    900
008	            Fashion	        600
009	            Fashion	        1200
010	            Jewellery	    900
011	            Kid	            700
012	            Fashion	        1500
013	            Cosmetics	    850
014	            Kid	            750
015	            Jewellery	    950

The first task is to obtain the summary of all departments, 
including the number of sales persons and the total sales in each department. 
Similar to Scalar UDFs, we can use the universal structure by replacing the following parts and get the syntax.

[schema_name.]function_name -> `dbo.GetDepartmentSummary` 
Parameters -> None 
Return Type ->`TABLE` 
Function Body-> Returns a table showing the department summary


CREATE FUNCTION dbo.GetDepartmentSummary()
RETURNS TABLE
AS
RETURN 
SELECT
    Department,
    COUNT(Sale_Person_ID) AS Number_of_Sales_Person,
    SUM(Sales_Amount) as Total_Revenue
FROM promo_sales
GROUP BY Department

Now we can call this function:
SELECT * FROM dbo.GetDepartmentSummary();
SELECT * FROM dbo.GetDepartmentSummary() WHERE Department = 'Cosmetics';
SELECT * FROM dbo.GetDepartmentSummary() WHERE Total_Revenue > 2000;
SELECT TOP 1 * FROM dbo.GetDepartmentSummary()

The table UDF can be called to retrieve the department summary, filter by specific criteria, or perform some other actions.



The second task is also to obtain the department summary, but this time, 
the summary involves the average sales per person and a bonus for each department. 
Later, the company management decided to change the bonus rule — 
departments with sales over 2000K USD would have their bonus rate increased by 10%. 
Unlike the first task which was completed with an Inline Table-Valued UDF, 
this task will be conducted with a Multistatement Table-Valued UDF.

The key differences between Inline Table-Valued UDFs and Multistatement Table-Valued UDFs can be observed from the two main areas:

1. Complexity of logic: Inline Table-Valued UDFs contain a single query and don't require BEGIN…ENDblock, 
which is ideal for simple logic while 
Multistatement Table-Valued UDFs employ BEGIN…END block to accommodate multiple SQL statements and more complex logic.

2. Performance: SQL server's query optimizer treats Inline Table-Valued UDFs as part of the calling query, 
which generally leads to faster performance. On the other hand, 
Multistatement Table-Valued UDFs must build the entire table in memory before returning it, which can impact performance.

---
For the second task, the following elements can be filled into the universal structure 
to create the Multistatement Table-Valued UDF:

[schema_name.]function_name -> `dbo.MultiStmt_GetDepartmentSummary` 
Parameters -> None 
Return Type ->`TABLE` 
Function Body-> Returns the final table showing department summary


CREATE FUNCTION dbo.GetDepartmentSummary_Multi()
RETURNS @DeptSummary TABLE 
(
    Department VARCHAR(40),
    Number_of_Sales_Person INT,
    Total_Revenue INT,
    Avg_Sales_Per_Person DECIMAL(10, 2),
    Bonus DECIMAL(10, 2) 
)
AS
BEGIN
    INSERT INTO @DeptSummary
    SELECT
        Department,
        COUNT(Sale_Person_ID) AS Number_of_Sales_Person,
        SUM(Sales_Amount) as Total_Revenue,
        AVG(Sales_Amount) AS Avg_Sales_Per_Person,
        SUM(Sales_Amount) * 0.2 as Bonus
    FROM promo_sales
    GROUP BY Department

    UPDATE @DeptSummary
    SET Bonus = Bonus * 1.1
    WHERE Total_Revenue > 2000

    RETURN
END

Department	    Number_of_Sales_Person	Total_Revenue	Avg_Sales_Per_Person	Bonus
Cosmetics	    4	                    2950	        737.00	                649.00
Fashion	        5	                    5150	        1030.00	                1133.00
Jewellery	    3	                    2650	        883.00	                583.00
Kid	            3	                    1950	        650.00	                390.00

*/

-- Scalar UDF
CREATE FUNCTION dbo.func_multiply
(
    @num_1 FLOAT,
    @num_2 FLOAT
)
RETURNS FLOAT
AS
BEGIN
    RETURN @num_1 * @num_2
END;

SELECT dbo.func_multiply(10.5, 2) AS MULTIPLY

-- Inline Table-Valued UDF
CREATE FUNCTION dbo.GetDepartmentSummary()
RETURNS TABLE
AS
RETURN 
SELECT
    Department,
    COUNT(Sale_Person_ID) AS Number_of_Sales_Person,
    SUM(Sales_Amount) as Total_Revenue
FROM promo_sales
GROUP BY Department;

SELECT * FROM dbo.GetDepartmentSummary();
SELECT * FROM dbo.GetDepartmentSummary() WHERE Department = 'Cosmetics';
SELECT * FROM dbo.GetDepartmentSummary() WHERE Total_Revenue > 2000;
SELECT TOP 1 * FROM dbo.GetDepartmentSummary()

-- Multistatement Table-Valued UDF
CREATE FUNCTION dbo.GetDepartmentSummary_Multi()
RETURNS @DeptSummary TABLE 
(
    Department VARCHAR(40),
    Number_of_Sales_Person INT,
    Total_Revenue INT,
    Avg_Sales_Per_Person DECIMAL(10, 2),
    Bonus DECIMAL(10, 2) 
)
AS
BEGIN
    INSERT INTO @DeptSummary
    SELECT
        Department,
        COUNT(Sale_Person_ID) AS Number_of_Sales_Person,
        SUM(Sales_Amount) as Total_Revenue,
        AVG(Sales_Amount) AS Avg_Sales_Per_Person,
        SUM(Sales_Amount) * 0.2 as Bonus
    FROM promo_sales
    GROUP BY Department

    UPDATE @DeptSummary
    SET Bonus = Bonus * 1.1
    WHERE Total_Revenue > 2000

    RETURN
END

SELECT * FROM dbo.GetDepartmentSummary_Multi()


/* Differences Between Table UDFs and Stored Procedures

It's common that people who are new to SQL UDFs often confuse Table UDFs with Stored Procedures 
because they often appear similar and can sometimes conduct the same functionalities. 
A Stored Procedure is a precompiled collection of SQL statement(s) which can be executed as a single unit and generate result sets.

The key differences between Table Functions and Stored Procedures are:

1. Return Type: Table UDFs always return a table but Stored Procedures return result sets.
2. Flexibility: Stored Procedures are more flexible than Table UDFs because they allow a wider range of SQL statements and operations.
3. Parameters: Table UDFs only allow input parameters but Stored Procedures accept both input and output parameters.

*/


/* What Are SQL Triggers?

A trigger in SQL is a special type of stored procedure that runs automatically when specific events happen in a database.
Think of it like an automatic response - 
when something happens to your data, the trigger "fires" and performs actions that you've defined before.


Classification of Triggers
Triggers can be categorized based on various criteria:

1. Timing of Execution

    BEFORE Triggers: Execute prior to the triggering event, allowing for validation or modification of data 
                     before it is committed to the database.
    AFTER Triggers: Execute subsequent to the triggering event, typically used for logging or cascading changes to related tables.

2. Scope of Application

    Row-Level Triggers: Fire once for each row affected by the triggering event, providing granular control over data modifications.
    Statement-Level Triggers: Fire once per triggering SQL statement, regardless of the number of rows affected, suitable for broad-scope operations.

3. Type of Triggering Event

    Data Manipulation Language (DML) Triggers: Respond to data changes such as INSERT, UPDATE, or DELETE operations.
    Data Definition Language (DDL) Triggers: Respond to schema changes like CREATE, ALTER, or DROP statements.
    Logon Triggers: Execute in response to user login events, often used for session initialization or security enforcement.

CREATE TRIGGER trg_AuditOrders
ON Orders
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @Action VARCHAR(10)
    IF EXISTS (SELECT * FROM INSERTED) AND EXISTS (SELECT * FROM DELETED)
        SET @Action = 'UPDATE'
    ELSE IF EXISTS (SELECT * FROM INSERTED)
        SET @Action = 'INSERT'
    ELSE
        SET @Action = 'DELETE'

    INSERT INTO AuditLog (Action, TableName, ChangeDate)
    VALUES (@Action, 'Orders', GETDATE())
END

In this example, the trigger determines the type of DML operation performed and logs it accordingly.

*/