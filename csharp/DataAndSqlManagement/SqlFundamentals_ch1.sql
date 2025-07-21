/* SELECT

When working with databases and SQL, the most common task is to request data
from one or more tables and display it. The SELECT statement accomplishes this. 
But the SELECT can do far more than simply retrieve and display data.

-- Retrieving Data with SQL

The most common SQL operation is a SELECT statement, 
which pulls data from a table and then displays the results.


SELECT * FROM CUSTOMERS;

CustomerID	FirstName	LastName	Email						PhoneNumber		Address			City		 State	ZipCode		Country
1			Alice		Smith		alice.smith@example.com		555-1234		123	Maple St	Springfield	 IL		62704		USA
2			Bob			Johnson		bob.johnson@example.com		555-5678		456	Oak St		Shelbyville	 IL		62565		USA
3			Charlie		Brown		charlie.brown@example.com	555-9101		789	Pine St		Capital City IL		62701		USA
4			Diana		Prince		diana.prince@example.com	555-1122		321	Elm St		Metropolis	 NY		10001		USA
5			Edward		Cullen		edward.cullen@example.com	555-3344		654	Birch St	Forks		 WA		98331		USA


You do not have to pull all columns in a SELECT statement. You can also pick and
choose only the columns you are interested in. The following query will only pull the
CUSTOMERID and FIRSTNAME columns:


SELECT CustomerID, FirstName FROM CUSTOMERS;

CustomerID	FirstName	
1			Alice		
2			Bob			
3			Charlie		
4			Diana		
5			Edward

NOTE:
A single SQL statement can optionally end with a semicolon (;), as shown in the previous examples. 
However, the semicolon is necessary to run multiple SQL statements at once.


-- Expressions in SELECT Statements

The SELECT statement is not limited to just retrieving columns from a table. 
It can also perform calculations, manipulate data, and create new columns dynamically in the query result. 


1. Basic SELECT with Expressions
Suppose you want to calculate a taxed price for each product in the Products table. 
Let’s assume the tax rate is 7%. 
You can dynamically calculate this in your query without modifying the actual table.

Example Query:

SELECT 
	ProductID,
	ProductName,
	Price,
	Price * 1.07 as TaxedPrice
FROM Products;

ProductID	ProductName		Price	TaxedPrice
1			Laptop			999.99	1069.9893
2			Smartphone		799.99	855.9893
3			Tablet			399.99	427.9893
4			Smartwatch		199.99	213.9893
5			Headphones		149.99	160.4893

Explanation:

- Price * 1.07: This expression calculates the taxed price by multiplying the Price column by 1.07 
  (which represents a 7% increase).

- AS TaxedPrice: The AS keyword assigns an alias (TaxedPrice) to the calculated column. 
  This alias is only valid within the scope of this query and does not modify the table structure.



2. Formatting the Output with ROUND()
If you want to round the TaxedPrice to two decimal places (for better readability), you can use the ROUND() function.

Example Query:

SELECT 
	ProductID,
	ProductName,
	Price,
	ROUND(Price * 1.07, 2) as TaxedPrice
FROM Products;

Explanation:

- ROUND(Price * 1.07, 2): The ROUND() function takes two arguments:
    - The first argument is the expression to round (Price * 1.07).
    - The second argument is the number of decimal places to round to (2 in this case).

- This ensures that the TaxedPrice column displays values like 10.75 instead of 10.7523.



3. Aliasing Columns for Clarity

You can also use aliases to rename existing columns in the query result. 
For example, you might want to rename the Price column to UntaxedPrice for clarity.

Example Query:

SELECT 
	ProductID,
	ProductName,
	Price as UntaxedPrice,
	ROUND(Price * 1.07, 2) as TaxedPrice
FROM Products;


-- Text Concatenation in SQL

Text concatenation is the process of combining two or more strings (text values) into a single string. 
In SQL, this is often done using the double pipe operator (||) or the CONCAT() function, 
depending on the database system.


1. Basic Concatenation

Suppose you want to combine the City and State columns from the Customers table into a single column called Location, 
separated by a comma and a space.

Example Query:

SELECT
    FirstName,
    LastName,
    CONCAT(City, ', ', State) AS Location
FROM Customers;

SELECT
    FirstName,
    LastName,
    City + ', ' + State AS Location
FROM Customers;

FirstName	LastName	Location
Alice		Smith		Springfield, IL
Bob			Johnson		Shelbyville, IL
Charlie		Brown		Capital City, IL
Diana		Prince		Metropolis, NY
Edward		Cullen		Forks, WA


2. Handling Different Data Types

Concatenation works with different data types (e.g., numbers, dates) by implicitly converting them to text. 
For example, if ZipCode were stored as an integer, 
SQL Server would automatically convert it to a string during concatenation.

SELECT
    CONCAT(CustomerID, '. ', FirstName) as FirstName,
    LastName,
    City AS Location
FROM Customers;

or you can use Explicit Convesrion

SELECT
    CONCAT(CAST(CustomerID as varchar(10)), '. ', FirstName) as Name,
    LastName,
    City AS Location
FROM Customers;



3. Concatenating with NULL Values

If any column in the concatenation contains a NULL value, the entire result will be NULL when using the + operator. 
To handle this, you can use the ISNULL() or COALESCE() function to replace NULL with an empty string.

SELECT
    FirstName,
    LastName,
    ISNULL(Address, '') + ' ' + ISNULL(City, '') + ', ' + ISNULL(State, '') + ' ' + ISNULL(ZipCode, '') AS ShipAddress
FROM Customers;

ISNULL(Column, ''): Replaces NULL values in the column with an empty string ('').
This ensures that NULL values don’t break the concatenation.

*/

/* Where
The WHERE clause is used in SQL to filter records based on specific conditions. 
It allows you to select only the rows that meet certain criteria. 
Think of it as a way to narrow down your data to only what you’re interested in.


-- How to Use WHERE
You add the WHERE clause after the SELECT statement and 
specify the condition(s) you want to filter by. Here’s the basic structure:

SELECT * FROM TableName
WHERE condition;

- TableName: The name of your table (in your case, StationData).
- condition: The rule that determines which rows to include.


-- Examples Using Your StationData Table

1. Filter by Temperature
Let’s say you want to find all records where the temperature is above 25 degrees:

SELECT * FROM StationData
WHERE Temperature > 25;

This will return:
    Station A, 2023-10-01, 25.3°C
    Station A, 2023-10-03, 26.0°C


2. Filter by Weather Condition
If you want to find all records where the weather condition is "Rainy":

SELECT * FROM StationData
WHERE WeatherCondition = 'Rainy';

This will return:
    Station B, 2023-10-01, Rainy
    Station B, 2023-10-03, Rainy
    Station C, 2023-10-02, Rainy


3. Filter by Multiple Conditions
You can combine conditions using AND or OR. For example, 
let’s find records where the temperature is below 20°C and the humidity is above 85%:

SELECT * FROM StationData
WHERE Temperature < 20 AND Humidity > 85;

This will return:
    Station C, 2023-10-02, 17.2°C, 90%
    Station B, 2023-10-03, 19.8°C, 88%
    Station C, 2023-10-03, 16.5°C, 92%


4. Filter by Date
If you want to find records for a specific date, like 2023-10-02:

SELECT * FROM StationData
WHERE RecordDate = '2023-10-02';

This will return:
    Station A, 2023-10-02, 24.8°C
    Station B, 2023-10-02, 20.5°C
    Station C, 2023-10-02, 17.2°C


5. Filter Using BETWEEN
If you want to find records where the wind speed is between 10 and 20 km/h:

SELECT * FROM StationData
WHERE WindSpeed BETWEEN 10 AND 20;

This will return:
    Station A, 2023-10-01, 10.5 km/h
    Station B, 2023-10-01, 15.3 km/h
    Station A, 2023-10-02, 12.1 km/h
    Station B, 2023-10-02, 20.0 km/h
    Station B, 2023-10-03, 18.5 km/h


--- Key Points to Remember

- Conditions: You can use comparison operators like =, >, <, >=, <=, !=, or <>.
- Text Values: When filtering text (like WeatherCondition), use single quotes ('Rainy').
- Dates: Dates should also be in single quotes ('2023-10-02').
- Combining Conditions: Use AND to combine conditions (both must be true) or OR (either can be true).


---------------------------------------------------------------------------------------------------------------------
AND, OR, and IN Statements

1. AND Statement
The AND operator is used to combine multiple conditions in a WHERE clause. 
All conditions must be true for a row to be included in the result.

Example:
Let’s say you want to find records where the temperature is above 20°C and the humidity is below 80%:

SELECT StationID, StationName, Temperature, Humidity FROM StationData
WHERE Temperature > 20 AND Humidity < 80;

StationID	StationName	    Temperature	    Humidity
1	        Station A	    25.30	        60	    
2	        Station B	    22.10	        75	    
4	        Station A	    24.80	        65	    
7	        Station A	    26.00	        55	    


2. OR Statement
The OR operator is used to combine multiple conditions in a WHERE clause. 
At least one condition must be true for a row to be included in the result.

Example:
Let’s say you want to find records where the weather condition is either "Sunny" or "Cloudy":

SELECT StationName, WeatherCondition FROM StationData
WHERE WeatherCondition = 'Sunny' OR WeatherCondition = 'Cloudy';

This will return:
    Station A, 2023-10-01, Sunny
    Station C, 2023-10-01, Cloudy
    Station A, 2023-10-02, Sunny
    Station A, 2023-10-03, Sunny


3. IN Statement
The IN operator is used to specify multiple possible values for a column. 
It’s a shorthand for using multiple OR conditions.

Example:
Let’s say you want to find records where the weather condition is either "Rainy" or "Stormy":

SELECT StationName, WeatherCondition FROM StationData
WHERE WeatherCondition IN ('Rainy', 'Cloudy');

StationName	WeatherCondition
Station B	Rainy
Station C	Cloudy
Station C	Rainy
Station B	Rainy


4. NOT IN Statement
The NOT IN operator is used to exclude rows that match any of the specified values.

Example:
Let’s say you want to find records where the weather condition is not "Rainy" or "Stormy":

SELECT StationName, WeatherCondition FROM StationData
WHERE WeatherCondition NOT IN ('Rainy', 'Cloudy');

This will return:
    StationName	WeatherCondition
    Station A	Sunny
    Station A	Sunny
    Station B	Stormy
    Station A	Sunny
    Station C	Foggy


5. Using Modulus Operator (%)
The modulus operator (%) returns the remainder of a division. It’s useful for filtering records based on divisibility.

Example:
Let’s say you want to find records where the humidity is divisible by 10 (i.e., the remainder is 0 when divided by 10):

SELECT StationName, WeatherCondition FROM StationData
WHERE Humidity % 10 = 0;

This will return:
    StationName	Humidity
    Station A	60
    Station C	80
    Station C	90

---------------------------------------------------------------------------------------------------------------------
--- Using WHERE on Text

1. LEN Function
The LEN function is used to count the number of characters in a text field. 
It’s useful for validating or filtering data based on the length of text.

Example:
Let’s say you want to ensure that the WeatherCondition column always has at least 4 characters:

SELECT * FROM StationData
WHERE LEN(WeatherCondition) >= 4;

This will return:
    WeatherCondition
    ----------------
    Sunny
    Rainy
    Cloudy
    Sunny
    Stormy
    Rainy
    Sunny
    Rainy
    Foggy

If you wanted to find records where the WeatherCondition is not 5 characters long:

SELECT WeatherCondition 
FROM StationData
WHERE LEN(WeatherCondition) <> 5;


2. LIKE Operator with Wildcards
The LIKE operator is used to search for patterns in text. It supports two wildcards:

    1. %: Matches any number of characters (including zero).
    2. _: Matches exactly one character.


Example 1: Using %
Let’s say you want to find all records where the WeatherCondition starts with the letter "S":

SELECT * FROM StationData
WHERE WeatherCondition LIKE 'S%'

StationID	    StationName     WeatherCondition
1	            Station A       Sunny
4	            Station A       Sunny
5	            Station B       Stormy
7	            Station A       Sunny


Example 2: Using _
Let’s say you want to find all records where the WeatherCondition has exactly 5 characters and starts with "S":

SELECT * FROM StationData
WHERE WeatherCondition LIKE 'S____'

StationID	StationName     WeatherCondition
1	        Station A       Sunny
4	        Station A       Sunny
7	        Station A       Sunny


3. CHARINDEX Function

The CHARINDEX function is used to find the position of a substring within a string. 
It returns the 1-based position of the first occurrence of the substring. 
If the substring is not found, it returns 0.

Syntax:
CHARINDEX(substring, string, [start_position])

- substring: The text you’re searching for.
- string: The text you’re searching within.
- start_position (optional): The position in the string to start searching (default is 1).


Examples Using CHARINDEX

Example 1: Find Records Where "a" Appears in WeatherCondition
Let’s say you want to find all records where the letter "a" appears in the WeatherCondition:

SELECT * FROM StationData
WHERE CHARINDEX ('a', WeatherCondition) > 0;

StationName     WeatherCondition
Station B       Rainy
Station C       Rainy
Station B       Rainy


Example 2: Find Records Where "y" Appears in the 5th Position
Let’s say you want to find all records where the letter "y" appears in the 5th position of WeatherCondition:

SELECT * FROM StationData
WHERE CHARINDEX ('y', WeatherCondition) = 5;


Example 3: Find Records Where "o" Appears After the 3rd Position
Let’s say you want to find all records where the letter "o" appears after the 3rd position of WeatherCondition:

SELECT * FROM StationData
WHERE CHARINDEX ('o', WeatherCondition, 4) > 0;


4. SUBSTR Function
The SUBSTRING function is used to extract a portion of a string. It takes three arguments:

    1. The string to extract from.
    2. The starting position (1-based index).
    3. The number of characters to extract.

Syntax:
SUBSTRING(string, start_position, length)

- string: The text you’re extracting from.
- start_position: The position in the string to start extracting (1-based index).
- length: The number of characters to extract.


Examples Using SUBSTRING

Example 1: Extract First 3 Characters of WeatherCondition
Let’s say you want to extract the first 3 characters of the WeatherCondition:

SELECT 
    StationName, 
    RecordDate, 
    SUBSTRING(WEATHERCONDITION, 1, 3) AS ShortCondition
FROM StationData;

StationName     RecordDate      ShortCondition
Station A       2023-10-01      Sun
Station B       2023-10-01      Rai
Station C       2023-10-01      Clo
Station A       2023-10-02      Sun
Station B       2023-10-02      Sto
Station C       2023-10-02      Rai
Station A       2023-10-03      Sun
Station B       2023-10-03      Rai
Station C       2023-10-03      Fog



Example 2: Extract the Last 3 Characters of WeatherCondition
To extract the last 3 characters, 
you can combine SUBSTRING with the LEN function (which returns the length of the string):

SELECT 
    StationName, 
    RecordDate, 
    SUBSTRING(WEATHERCONDITION, LEN(WEATHERCONDITION) - 2, 3) AS LastThreeChars
FROM StationData;

This will return:

    Station A, 2023-10-01, nny
    Station B, 2023-10-01, iny
    Station C, 2023-10-01, udy
    Station A, 2023-10-02, nny
    Station B, 2023-10-02, rmy
    Station C, 2023-10-02, iny
    Station A, 2023-10-03, nny
    Station B, 2023-10-03, iny
    Station C, 2023-10-03, ggy

*/

/* Data Seed

-- Customers Table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    PhoneNumber VARCHAR(15),
    Address VARCHAR(255),
    City VARCHAR(100),
    State VARCHAR(50),
    ZipCode VARCHAR(10),
    Country VARCHAR(50)
);

-- Products Table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName VARCHAR(100) NOT NULL,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
    StockQuantity INT NOT NULL
);

-- CustomerProducts Table (Many-to-Many Relationship)
CREATE TABLE CustomerProducts (
    CustomerProductID INT IDENTITY(1,1),
	CustomerID INT,
    ProductID INT,
    PurchaseDate DATE NOT NULL,
    Quantity INT NOT NULL,
    PRIMARY KEY (CustomerID, ProductID),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);


CREATE TABLE StationData (
    StationID INT PRIMARY KEY IDENTITY(1,1),
    StationName VARCHAR(100) NOT NULL,
    RecordDate DATE NOT NULL,
    Temperature DECIMAL(5, 2),
    Humidity INT,
    Precipitation DECIMAL(5, 2),
    WindSpeed DECIMAL(5, 2),
    WeatherCondition VARCHAR(50)
);

INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber, Address, City, State, ZipCode, Country)
VALUES 
('Alice', 'Smith', 'alice.smith@example.com', '555-1234', '123 Maple St', 'Springfield', 'IL', '62704', 'USA'),
('Bob', 'Johnson', 'bob.johnson@example.com', '555-5678', '456 Oak St', 'Shelbyville', 'IL', '62565', 'USA'),
('Charlie', 'Brown', 'charlie.brown@example.com', '555-9101', '789 Pine St', 'Capital City', 'IL', '62701', 'USA'),
('Diana', 'Prince', 'diana.prince@example.com', '555-1122', '321 Elm St', 'Metropolis', 'NY', '10001', 'USA'),
('Edward', 'Cullen', 'edward.cullen@example.com', '555-3344', '654 Birch St', 'Forks', 'WA', '98331', 'USA');

INSERT INTO Products (ProductName, Description, Price, StockQuantity)
VALUES 
('Laptop', '15-inch, 8GB RAM, 256GB SSD', 999.99, 50),
('Smartphone', '6.5-inch, 128GB, 5G', 799.99, 100),
('Tablet', '10-inch, 64GB, Wi-Fi', 399.99, 75),
('Smartwatch', 'Heart rate monitor, GPS', 199.99, 150),
('Headphones', 'Noise-cancelling, wireless', 149.99, 200);

INSERT INTO CustomerProducts (CustomerID, ProductID, PurchaseDate, Quantity)
VALUES 
(1, 1, '2023-10-01', 1),
(1, 3, '2023-10-02', 2),
(2, 2, '2023-10-03', 1),
(3, 4, '2023-10-04', 3),
(4, 5, '2023-10-05', 1),
(5, 1, '2023-10-06', 1),
(5, 2, '2023-10-07', 1);


INSERT INTO StationData (StationName, RecordDate, Temperature, Humidity, Precipitation, WindSpeed, WeatherCondition)
VALUES
('Station A', '2023-10-01', 25.3, 60, 0.0, 10.5, 'Sunny'),
('Station B', '2023-10-01', 22.1, 75, 5.2, 15.3, 'Rainy'),
('Station C', '2023-10-01', 18.7, 80, 0.0, 8.7, 'Cloudy'),
('Station A', '2023-10-02', 24.8, 65, 0.0, 12.1, 'Sunny'),
('Station B', '2023-10-02', 20.5, 85, 12.4, 20.0, 'Stormy'),
('Station C', '2023-10-02', 17.2, 90, 3.7, 10.0, 'Rainy'),
('Station A', '2023-10-03', 26.0, 55, 0.0, 9.8, 'Sunny'),
('Station B', '2023-10-03', 19.8, 88, 8.9, 18.5, 'Rainy'),
('Station C', '2023-10-03', 16.5, 92, 1.2, 7.5, 'Foggy');

*/
