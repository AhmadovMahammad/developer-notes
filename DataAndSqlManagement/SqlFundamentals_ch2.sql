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
Cloudy	1
Foggy	1
Rainy	3
Stormy	1
Sunny	3

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

SELECT * FROM StationData;

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