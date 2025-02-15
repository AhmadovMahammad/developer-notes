/* CASE Statements

The CASE statement is used to perform conditional logic in SQL. 
It evaluates a set of conditions and returns a value based on the first condition that is true. 

If no conditions are met, it returns the value specified in the ELSE clause (if provided). 
If there is no ELSE clause and no conditions are met, it returns NULL.

- Syntax

SELECT column1, column2, ...
    CASE
        WHEN condition1 THEN result1
        WHEN condition2 THEN result2
        ...
        ELSE default_result
    END AS new_column_name
FROM table_name;


Key Points About CASE

1. Order of Evaluation:
- The CASE statement evaluates conditions from top to bottom.
- The first condition that evaluates to TRUE is executed, and the corresponding result is returned.
- Subsequent conditions are not evaluated.

2. ELSE Clause:
- The ELSE clause is optional. If no conditions are true and there is no ELSE, the result is NULL.

3. Use Cases:
- Categorizing data (e.g., low, medium, high).
- Creating flags or indicators (e.g., "Yes" or "No").
- Transforming data based on conditions.

*/

/* Examples Using Your StationData Table

Example 1: Categorize Wind Speed
Let’s say you want to categorize the WindSpeed into three categories:

SELECT
    StationName, RecordDate, WindSpeed,
CASE
    WHEN WindSpeed >= 15 THEN 'HIGH'
    WHEN WindSpeed >= 10 AND WindSpeed <= 15 THEN 'MODERATE'
    ELSE 'LOW'
END AS WindSeverity
FROM StationData

Explanation:
    The CASE statement evaluates WindSpeed and assigns a category (High, Moderate, or Low).
    The first condition that is true determines the result.

Output:
StationName	    RecordDate	    WindSpeed	WindSeverity
Station A	    2023-10-01	    10.50	    MODERATE
Station B	    2023-10-01	    15.30	    HIGH
Station C	    2023-10-01	    8.70	    LOW
Station A	    2023-10-02	    12.10	    MODERATE
Station B	    2023-10-02	    20.00	    HIGH
Station C	    2023-10-02	    10.00	    MODERATE
Station A	    2023-10-03	    9.80	    LOW
Station B	    2023-10-03	    18.50	    HIGH
Station C	    2023-10-03	    7.50	    LOW

*/

/* What is Grouping CASE Statements?
When you use a CASE statement in combination with GROUP BY, you can:

- Categorize Data: Use CASE to create new categories or groups.
- Aggregate Data: Use GROUP BY to summarize data based on those categories.

This is particularly useful when you want to analyze data based on custom conditions or groupings.
SELECT 
    CASE
        WHEN condition1 THEN result1
        WHEN condition2 THEN result2
        ...
        ELSE default_result
    END AS new_column_name,
    aggregate_function(column)
FROM table_name
GROUP BY new_column_name;



Examples Using Your StationData Table

Example 1: Group by Wind Severity and Count Records
Let’s say you want to categorize WindSpeed into three categories (High, Moderate, Low) and 
count the number of records in each category:

SELECT
    CASE
        WHEN WindSpeed >= 15 THEN 'High'
        WHEN WindSpeed >= 10 THEN 'Moderate'
        ELSE 'Low'
    END AS WindSeverity,
    COUNT(*) AS RecordCount
FROM StationData
GROUP BY 
    CASE
        WHEN WindSpeed >= 15 THEN 'High'
        WHEN WindSpeed >= 10 THEN 'Moderate'
        ELSE 'Low'
    END;


WindSeverity	RecordCount
High	        3
Low	            3
Moderate	    3


Example 2: Group by Temperature Category and Calculate Average Humidity
Let’s say you want to categorize Temperature into three categories (Hot, Warm, Cool) and 
calculate the average humidity for each category:

SELECT 
    CASE
        WHEN Temperature >= 25 THEN 'Hot'
        WHEN Temperature >= 20 THEN 'Warm'
        ELSE 'Cool'
    END AS TemperatureCategory,
    AVG(Humidity) AS AvgHumidity
FROM StationData
GROUP BY 
    CASE
        WHEN Temperature >= 25 THEN 'Hot'
        WHEN Temperature >= 20 THEN 'Warm'
        ELSE 'Cool'
    END;


TemperatureCategory	    AvgHumidity
COOL	                87
HOT	                    57
WARM	                75

*/

SELECT 
    --CASE
    --    WHEN Temperature >= 25 THEN 'Hot'
    --    WHEN Temperature >= 20 THEN 'Warm'
    --    ELSE 'Cool'
    --END AS TemperatureCategory,
    AVG(Humidity) AS AvgHumidity
FROM StationData
GROUP BY 
    CASE
        WHEN Temperature >= 25 THEN 'Hot'
        WHEN Temperature >= 20 THEN 'Warm'
        ELSE 'Cool'
    END;

SELECT
    CASE
        WHEN WindSpeed >= 15 THEN 'High'
        WHEN WindSpeed >= 10 THEN 'Moderate'
        ELSE 'Low'
    END AS WindSeverity,
    COUNT(*) AS RecordCount
FROM StationData
GROUP BY 
    CASE
        WHEN WindSpeed >= 15 THEN 'High'
        WHEN WindSpeed >= 10 THEN 'Moderate'
        ELSE 'Low'
    END;