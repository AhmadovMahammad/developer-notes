/* What is GROUP BY?

The GROUP BY clause is used in SQL to group rows that have the same values in specified columns. 
It’s often used with aggregate functions like COUNT, SUM, AVG, MIN, and MAX to perform calculations on each group.

Think of it like this:

- You have a table with many rows.
- You want to group rows that share a common value in one or more columns.
- Then, you want to perform calculations (like counting or averaging) on each group.

*/

/* How GROUP BY Works

Grouping: Rows are grouped based on the columns specified in the GROUP BY clause.
Aggregation: Aggregate functions (like COUNT, SUM, etc.) are applied to each group.
Result: The result is a summary of each group, with one row per group.

Syntax:
SELECT 
	column1, column2, ..., aggregate_function(column)
FROM table_name
GROUP BY column1, column2, ...;


-- Examples Using Your StationData Table

Example 1: Count Records by Weather Condition
Let’s say you want to count how many records exist for each WeatherCondition:


SELECT
	WeatherCondition, COUNT(*) AS RecordCount 
FROM StationData
GROUP BY WeatherCondition;

WeatherCondition	RecordCount
Cloudy	            1
Foggy	            1
Rainy	            3
Stormy	            1
Sunny	            3

~ Explanation ~

- The query groups records by WeatherCondition.
- It then counts the number of records in each group.
- The result shows the WeatherCondition and the number of records for each condition.



Example 2: Calculate Average Temperature by Station
Let’s say you want to calculate the average temperature for each station:

SELECT
	StationName, AVG(Temperature) AS AverageTemperature 
FROM StationData
GROUP BY STATIONNAME;

StationName		AverageTemperature
Station A		25.366666
Station B		20.800000
Station C		17.466666

~ Explanation ~

- The query groups records by StationName.
- It then calculates the average temperature for each station.
- The result shows the StationName and the average temperature for each station.



Example 3: Maximum Wind Speed by Weather Condition
Let’s say you want to find the maximum wind speed for each weather condition:

SELECT 
	WeatherCondition, MAX(WindSpeed) AS MaxWindSpeed
FROM StationData
GROUP BY WeatherCondition;

WeatherCondition	MaxWindSpeed
Cloudy				8.70
Foggy				7.50
Rainy				18.50
Stormy				20.00
Sunny				12.10

~ Explanation ~

- The query groups records by WeatherCondition.
- It then finds the maximum wind speed for each weather condition.
- The result shows the WeatherCondition and the maximum wind speed for each condition.


Example 4: Total Precipitation by Station
Let’s say you want to calculate the total precipitation for each station:

SELECT 
	StationName, SUM(Precipitation) AS TotalPrecipitation
FROM StationData
GROUP BY StationName;

StationName		TotalPrecipitation
Station A		0.00
Station B		26.50
Station C		4.90

~ Explanation ~

- The query groups records by StationName.
- It then calculates the total precipitation for each station.
- The result shows the StationName and the total precipitation for each station.

*/

/* Ordering Records
The ORDER BY clause is used to sort the result set of a query based on one or more columns. 
You can sort in:

- Ascending order (default, using ASC).
- Descending order (using DESC).


-- How ORDER BY Works
Sorting: Rows are sorted based on the columns specified in the ORDER BY clause.
Direction: You can specify ASC (ascending) or DESC (descending) for each column.
Multiple Columns: You can sort by multiple columns, separated by commas.

Syntax:

SELECT 
	column1, column2, ...
FROM table_name
ORDER BY column1 [ASC|DESC], column2 [ASC|DESC], ...;


Examples Using Your StationData Table

Example 1: Sort by Temperature in Ascending Order
Let’s say you want to list all records sorted by Temperature from lowest to highest:

SELECT 
	StationName, Temperature 
FROM StationData
ORDER BY Temperature ASC;

Result:

    Station C, 2023-10-03, 16.5°C (Lowest temperature)
    Station C, 2023-10-02, 17.2°C
    Station C, 2023-10-01, 18.7°C
    Station B, 2023-10-03, 19.8°C
    Station B, 2023-10-02, 20.5°C
    Station B, 2023-10-01, 22.1°C
    Station A, 2023-10-02, 24.8°C
    Station A, 2023-10-01, 25.3°C
    Station A, 2023-10-03, 26.0°C (Highest temperature)


Example 2: Sort by RecordDate in Descending Order
Let’s say you want to list all records sorted by RecordDate from most recent to oldest:

SELECT 
	StationName, RecordDate	
FROM StationData
ORDER BY RecordDate DESC;

Result:

    Station A, 2023-10-03, 26.0°C (Most recent)
    Station B, 2023-10-03, 19.8°C
    Station C, 2023-10-03, 16.5°C
    Station A, 2023-10-02, 24.8°C
    Station B, 2023-10-02, 20.5°C
    Station C, 2023-10-02, 17.2°C
    Station A, 2023-10-01, 25.3°C
    Station B, 2023-10-01, 22.1°C
    Station C, 2023-10-01, 18.7°C (Oldest)


Example 3: Sort by StationName and Temperature
Let’s say you want to list all records sorted first by StationName in ascending order and 
then by Temperature in descending order:

SELECT
    StationName, Temperature
FROM StationData
ORDER BY StationName ASC, Temperature DESC;

Result:

    Station A, 2023-10-03, 26.0°C (Highest temperature for Station A)
    Station A, 2023-10-01, 25.3°C
    Station A, 2023-10-02, 24.8°C

    Station B, 2023-10-01, 22.1°C (Highest temperature for Station B)
    Station B, 2023-10-02, 20.5°C
    Station B, 2023-10-03, 19.8°C

    Station C, 2023-10-01, 18.7°C (Highest temperature for Station C)
    Station C, 2023-10-02, 17.2°C
    Station C, 2023-10-03, 16.5°C

*/

/* Using the LIMIT Clause

Structured Query Language (SQL) is a powerful tool for managing and extracting data from databases. 
In this article, we'll explore an essential aspect of SQL that can help you control the volume of data you retrieve — the "LIMIT" clause. 

When working with databases, it's crucial to strike a balance between obtaining enough information and not overwhelming yourself with excessive data. 
We'll walk you through various ways to use the LIMIT clause effectively, 
helping you fine-tune your SQL queries to get exactly the information you need.


--- Understanding the Default Limit

In SQL, when you run a query, there is often a default limit on the number of rows displayed in the result set. 
This default limit is usually set to 1,000 rows. 
This can be convenient in some cases, as it prevents accidentally retrieving massive datasets, 
but it can also be restrictive when you need more extensive information.

*/

SELECT
    StationName, Temperature
FROM StationData
ORDER BY StationName ASC, Temperature DESC;

SELECT 
	StationName, RecordDate	
FROM StationData
ORDER BY RecordDate DESC;

SELECT 
	StationName, Temperature 
FROM StationData
ORDER BY Temperature ASC;

SELECT
	WeatherCondition, COUNT(*) AS RecordCount 
FROM StationData
GROUP BY WeatherCondition;

SELECT
	StationName, AVG(Temperature) AS AverageTemperature 
FROM StationData
GROUP BY STATIONNAME;

SELECT 
	WeatherCondition, MAX(WindSpeed) AS MaxWindSpeed
FROM StationData
GROUP BY WeatherCondition;

SELECT 
	StationName, SUM(Precipitation) AS TotalPrecipitation
FROM StationData
GROUP BY StationName;