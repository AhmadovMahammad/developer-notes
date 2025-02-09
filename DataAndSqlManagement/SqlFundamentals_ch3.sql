/* Aggregate Functions
Aggregate functions are specialized SQL operations that process multiple rows of data to produce a single summarizing value. 
These functions are used for calculations like 

1. counting records, 
2. summing numerical values, 
3. computing averages, and 
4. finding minimum or maximum values.

-- Key Characteristics of Aggregate Functions

1. They process multiple rows and return a single value.
Unlike row-wise operations, which return individual results for each row, 
aggregate functions collapse multiple rows into a single summary value.

2. They ignore NULL values by default.
This behavior ensures that missing data does not distort calculations. 

3. They often work with GROUP BY to summarize data for specific categories.
Without GROUP BY, an aggregate function produces a single result for all rows. 
With GROUP BY, it calculates a result for each group of values.

SELECT COUNT(FirstName) FROM Customers


NOTE: In chapter 2, we explored aggregate functions very deeply by using group by.
*/


/* The HAVING Statement

1. What is the HAVING Statement?
The HAVING clause is used to filter groups of rows based on the result of an aggregate function (like SUM, AVG, COUNT, etc.). 
It is similar to the WHERE clause, 
but while WHERE filters individual rows, HAVING filters groups of rows after aggregation.

2. Why Use HAVING?
    Filter Aggregated Results: You can filter groups based on calculations like sums, averages, or counts.
    Post-Aggregation Filtering: HAVING is applied after the GROUP BY and aggregate functions are calculated.

Key Differences Between WHERE and HAVING

WHERE	                                                HAVING
-------------------------------------------------------------------------------------------------
Filters individual rows before aggregation.	            Filters groups of rows after aggregation.
Cannot use aggregate functions.	                        Can use aggregate functions.
Used before GROUP BY.	                                Used after GROUP BY.

Syntax:
SELECT 
    column1, aggregate_function(column2)
FROM table_name
GROUP BY column1
HAVING condition;


--- Examples Using Your StationData Table

Example 1: Filter Stations with Total Precipitation Greater Than 10
Let’s say you want to find stations where the total precipitation is greater than 10:

Code: 

SELECT
    StationName, SUM(Precipitation) AS TotalPrecipitation    
FROM StationData
GROUP BY StationName
HAVING SUM(Precipitation) > 10;


Explanation:

    - Groups rows by StationName.
    - Calculates the total precipitation for each station.
    - Filters out groups where the total precipitation is not greater than 10.


Result:

StationName	TotalPrecipitation
Station B	26.5


Example 2: Filter Stations with Average Temperature Below 20
Let’s say you want to find stations where the average temperature is below 20°C:

Code:

SELECT
    StationName, AVG(Temperature) AS AvgTemperature
FROM StationData
GROUP BY StationName
HAVING AVG(Temperature) < 20;


Explanation:

    - Groups rows by StationName.
    - Calculates the average temperature for each station.
    - Filters out groups where the average temperature is not below 20°C.


Result:

StationName	AvgTemperature
Station C	17.47


--- Key Points to Remember

1. Use HAVING for Aggregations:  Use HAVING to filter groups based on aggregate functions like SUM, AVG, COUNT, etc.
2. Order Matters:  HAVING comes after GROUP BY and before ORDER BY.
Aliases in HAVING: Some databases (like Oracle) do not support aliases in HAVING. 
                   You must repeat the aggregate function (e.g., HAVING SUM(column) > 10).


--- Why Use HAVING?

1. Summarize Data: It helps you summarize and filter large datasets based on aggregated results.
2. Analyze Trends: You can analyze trends (e.g., stations with high precipitation).
3. Simplify Reporting: It simplifies reporting by providing filtered aggregated results.

*/


/* Getting Distinct Records

To remove duplicate entries from the result set in a SQL SELECT query, use the DISTINCT term. 
Accordingly, if you apply DISTINCT to a query, 
duplicate values will be eliminated and you will be given a list of unique values. 
Let’s examine this keyword’s operation by starting with the fundamentals.

Imagine a situation in which you have a table named Customers that contains customer data, such as names. 
From this data, you wish to extract a list of distinct customer names. 
Here’s how to utilize DISTINCT to make this happen:

SELECT DISTINCT CustomerName
FROM Customers;

The Customers table’s CustomerName column is chosen by the query in this example. 
Only distinct customer names are returned in the result set thanks to the DISTINCT keyword.
From the output, any duplicate client names are immediately eliminated.


-- Understanding Duplicate Data
Before we dive deeper into the capabilities of the DISTINCT keyword, 
it’s essential to understand why duplicate data exists in a database. 
Duplicate data can be the result of various factors, including:

- Data Entry Errors: Human errors during data entry can lead to duplicate records.
- Data Integration: When data is collected from multiple sources and integrated into a single database, 
  duplicate entries can occur.
- Data Migration: During data migration processes, such as moving data from one system to another, 
  duplicates can be inadvertently created.
- Historical Data: In some cases, historical data might include multiple entries for the same entity over time.

Understanding the source of duplicate data is crucial because it can guide you in dealing with duplicates effectively.


-- Using DISTINCT with Multiple Columns

DISTINCT has only been used with a single column thus far. 
However, what if you need to obtain distinct value combinations from many columns? 
This may be accomplished by using DISTINCT in the SELECT query with several columns. 
Let’s use an example to further explain this:

SELECT DISTINCT FirstName, LastName
FROM Employees;

We are getting a list of distinct first- and last-name combinations from the Employees table using this query. 
Multiple workers that have the same last name and initial name will be eliminated from the result set as duplicates.


-- Performance Considerations

While the DISTINCT keyword is a powerful tool for retrieving unique data, 
it’s essential to consider its impact on query performance, especially when working with large datasets. 
Here are some performance considerations:

1. Resource Usage: Applying DISTINCT requires additional processing by the database server to identify and eliminate duplicates. 
This can consume CPU and memory resources, so it’s crucial to assess the impact on your system’s performance.

2. Indexes: Ensure that the columns used with DISTINCT are properly indexed. 
Indexes can significantly improve query performance when using DISTINCT, 
as they help the database engine locate unique values more efficiently.

3. Alternative Approaches: In some cases, you can achieve the same result without using DISTINCT. 
Consider whether other SQL techniques like GROUP BY or subqueries are more efficient for your specific use case.


Nevertheless, DISTINCT should be applied carefully as it affects query response time, particularly for huge data sets. 
You may also try using other methods such as indexing, analyzing queries for appropriate data distribution, 
or GROUP BY among others.

*/
