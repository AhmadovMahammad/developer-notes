/* Understanding Subqueries and Common Table Expressions (CTEs) in SQL

When working with SQL, sometimes you need to break down complex queries into smaller, manageable parts. 
Instead of writing a single complicated SQL statement, you can use 'subqueries' or 'CTEs' (Common Table Expressions) 
to improve readability, organization, and performance.

*/

/* Subqueries – Queries Inside Queries

A subquery is a query written inside another query. Think of it as a nested SQL query that helps fetch intermediate results, 
which the main query then uses.

A subquery can be used in different parts of an SQL statement:

1. Inside a SELECT statement (to calculate values)
2. Inside a FROM clause (to create a temporary table)
3. Inside a WHERE clause (to filter data dynamically)



1. Example: Using a Subquery in WHERE Clause

Imagine you need to find employees who work in the Engineering department, 
but you only have their department names, not DepartmentID.

Without a subquery, you would need to first find the DepartmentID of Engineering, then use it in another query. 
Instead, you can do it in one step:


SELECT Employees.FirstName, Employees.LastName, Employees.Salary
FROM Employees
WHERE Employees.DepartmentID = (
SELECT DepartmentID FROM Departments WHERE Departments.DepartmentName = 'Engineering'
)

FirstName	LastName	Salary
Leyla		Aliyeva		70000

Here, the inner query (SELECT DepartmentID FROM Departments WHERE DepartmentName = 'Engineering') 
fetches the DepartmentID of Engineering, and the outer query uses it to find employees from that department.



2. Example: Using a Subquery in SELECT Clause
You might want to display each employee’s salary compared to the company’s average salary.

SELECT 
	FirstName,
	LastName,
	Salary,
	(SELECT AVG(Salary) FROM Employees) AS 'Average Salary'
FROM Employees

FirstName	LastName	Salary	Average Salary
Mahammad	Ahmadov		50000	60000.000000
Aysel		Mammadova	60000	60000.000000
Ramin		Isayev		45000	60000.000000
Leyla		Aliyeva		70000	60000.000000
Nigar		Guliyeva	55000	60000.000000
Wrong		Data		80000	60000.000000

The subquery (SELECT AVG(Salary) FROM Employees) calculates the average salary for all employees. 
Each row then includes this value, allowing you to compare individual salaries against the company-wide average.



3. Example: Using a Subquery in FROM Clause
Sometimes, you need to treat a subquery as a temporary table. This can help when filtering or organizing data before using it.

SELECT DepartmentName, EmployeeCount
FROM (
	SELECT d.DepartmentName, COUNT(e.EmployeeID) AS EmployeeCount
	FROM Departments d
	LEFT JOIN Employees E ON d.DepartmentID = e.DepartmentID
	GROUP BY d.DepartmentName
) AS DepartmentStats
WHERE EmployeeCount >= 1;

DepartmentName	EmployeeCount
Engineering		1
Finance			2
HR				2

Here, the inner query creates a temporary table (DepartmentStats) that counts employees per department. 
The outer query then filters to show only departments with more than 1 employees.

*/

SELECT Employees.FirstName, Employees.LastName, Employees.Salary
FROM Employees
WHERE Employees.DepartmentID = 
(
	SELECT DepartmentID FROM Departments WHERE Departments.DepartmentName = 'Engineering'
)


SELECT 
	FirstName,
	LastName,
	Salary,
	(SELECT AVG(Salary) FROM Employees) AS 'Average Salary'
FROM Employees


SELECT DepartmentName, EmployeeCount
FROM (
	SELECT d.DepartmentName, COUNT(e.EmployeeID) AS EmployeeCount
	FROM Departments d
	LEFT JOIN Employees E ON d.DepartmentID = e.DepartmentID
	GROUP BY d.DepartmentName
) AS DepartmentStats
WHERE EmployeeCount >= 1;

/* Common Table Expressions (CTEs) – A Better Way to Write Complex Queries

Now, let’s talk about CTEs. They are similar to subqueries but much easier to read and manage. 
A CTE allows you to define a temporary result set with a name. 
This result set can then be used in the main query, just like a table.

How Does a CTE Work?

- A CTE is defined using the WITH keyword at the start of your query.
- The CTE holds a result set (a temporary table) that can be used later in the query.
- The main query can then refer to this result set.



Example 1: Using CTE for Better Readability
Let’s rewrite the previous query that counts employees per department using a CTE. 
Instead of using a complicated subquery, we’ll make it easier to read with a CTE.

WITH DepartmentStats AS
(
	SELECT d.DepartmentName, COUNT(e.EmployeeID) AS EmployeeCount
	FROM Departments d
	LEFT JOIN Employees E ON d.DepartmentID = e.DepartmentID
	GROUP BY d.DepartmentName
)
SELECT DepartmentName, EmployeeCount
FROM DepartmentStats
WHERE EmployeeCount >= 1

DepartmentName	EmployeeCount
Engineering		1
Finance			2
HR				2

Here’s what’s happening:

The CTE (DepartmentStats) calculates the number of employees in each department.
In the main query, we refer to this CTE (DepartmentStats) to get the result and 
filter departments with more than and equals to 1 employee.



Example 2: Recursive CTE – A Special Case for Hierarchical Data

Sometimes, you need to deal with hierarchical or self-referencing data. This is where a recursive CTE comes in handy. 
A recursive CTE allows you to query data that references itself, 
like an employee who has a manager, and that manager has a manager, and so on.

Example: Finding All Employees Reporting to a Manager
Let’s say we have a list of employees where each employee has a ManagerID. 
We want to find all employees who report to a specific manager, 
even if the manager is at different levels in the hierarchy.

WITH RecursiveCTE AS 
(
     -- Base case: Start with employees who have no manager (top-level managers)
    SELECT EmployeeID, FirstName, LastName, ManagerID, 0 as OrganizationLevel
    FROM SpecialEmployees
    WHERE ManagerID IS NULL

    UNION ALL

    -- Recursive case: Find employees who report to the current level
    SELECT e.EmployeeID, e.FirstName, e.LastName, e.ManagerID, OrganizationLevel + 1
    FROM SpecialEmployees e
    JOIN RecursiveCTE r 
	ON e.ManagerID = r.EmployeeID
)
SELECT * FROM RecursiveCTE;


---
Let’s break this down:

Base case: The first part of the CTE selects employees who have no ManagerID (i.e., top-level managers).
Recursive case: The second part of the CTE keeps joining the Employees table with itself, 
using the ManagerID to find all employees reporting to the current manager. 
It continues until all employees in the hierarchy are found.

---
How Does the Recursive CTE Work?

First, it starts with employees who do not report to anyone (i.e., top-level managers).
Then, it keeps looking for employees who report to those managers, and so on, 
until it has found all employees under the original top-level managers.

This is useful for situations where you need to explore hierarchical relationships, 
like finding all employees under a specific manager, or navigating organizational structures.

EmployeeID	FirstName	LastName		ManagerID	OrganizationLevel
1			Mahammad	Ahmadov			NULL		0
2			Aysel		Mammadova		1			1
3			Rauf		Aliyev			1			1
7			Lala		Zeynalova		3			2
8			Tural		Novruzov		3			2
9			Gunel		Mahmudova		3			2
4			Nigar		Mammadova		2			2
5			Elvin		Mammadov		2			2
6			Nijat		Ismailov		2			2
16			Zeynab		Mammadova		6			3
17			Fikrat		Yusifov			6			3
18			Amina		Aliyeva			6			3
13			Elvira		Guliyeva		5			3
14			Rashad		Huseynov		5			3
15			Kamran		Jafarov			5			3
10			Javid		Rzayev			4			3
11			Fidan		Sultanova		4			3
12			Samir		Abbasov			4			3

*/

/*

Key Differences Between Subqueries and CTEs

1. Subqueries are good when you need to embed a small query inside another query. 
They can be a little harder to read if they’re nested too deeply.

2. CTEs are easier to read because they allow you to define a temporary result with a name 
that can be used in the main query. 
They are especially useful for complex queries or recursive queries.

*/

WITH DepartmentStats AS
(
	SELECT d.DepartmentName, COUNT(e.EmployeeID) AS EmployeeCount
	FROM Departments d
	LEFT JOIN Employees E ON d.DepartmentID = e.DepartmentID
	GROUP BY d.DepartmentName
)
SELECT DepartmentName, EmployeeCount
FROM DepartmentStats
WHERE EmployeeCount >= 1

CREATE TABLE SpecialEmployees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    ManagerID INT,
    FOREIGN KEY (ManagerID) REFERENCES Employees(EmployeeID)
);

INSERT INTO SpecialEmployees (EmployeeID, FirstName, LastName, ManagerID) VALUES (1, 'Mahammad', 'Ahmadov', NULL);
INSERT INTO SpecialEmployees (EmployeeID, FirstName, LastName, ManagerID) VALUES (2, 'Aysel', 'Mammadova', 1);
INSERT INTO SpecialEmployees (EmployeeID, FirstName, LastName, ManagerID) VALUES (3, 'Rauf', 'Aliyev', 1);
INSERT INTO SpecialEmployees (EmployeeID, FirstName, LastName, ManagerID) VALUES (4, 'Nigar', 'Mammadova', 2);
INSERT INTO SpecialEmployees (EmployeeID, FirstName, LastName, ManagerID) VALUES (5, 'Elvin', 'Mammadov', 2);

WITH RecursiveCTE AS 
(
     -- Base case: Start with employees who have no manager (top-level managers)
    SELECT EmployeeID, FirstName, LastName, ManagerID, 0 as OrganizationLevel
    FROM SpecialEmployees
    WHERE ManagerID IS NULL

    UNION ALL

    -- Recursive case: Find employees who report to the current level
    SELECT e.EmployeeID, e.FirstName, e.LastName, e.ManagerID, OrganizationLevel + 1
    FROM SpecialEmployees e
    JOIN RecursiveCTE r 
	ON e.ManagerID = r.EmployeeID
)
SELECT * FROM RecursiveCTE;

/* Union and Union All

When working with databases, sometimes we need to merge the results of multiple queries into a single result set. 
This is where UNION and UNION ALL come into play. 
They allow combining the outputs of multiple SELECT statements into one dataset. 
However, they behave differently, which makes understanding them crucial for performance and accuracy.

Let's start with UNION. When two SELECT queries are joined using UNION, SQL ensures that the final result contains only distinct rows. 
This means that even if the same row appears in both queries, it will only be included once in the final output. 
SQL automatically removes duplicates, just like applying DISTINCT.

On the other hand, UNION ALL does not perform this duplicate removal. 
It simply stacks the results of multiple SELECT statements, keeping every row, even if there are duplicates. 
This makes UNION ALL faster because SQL does not need to check for and remove duplicate rows.

---

To understand this better, consider a simple table named Employees. 
Suppose we have two different queries—one that selects employees from the IT department 
and another that selects employees from the HR department.


SELECT e.FirstName, e.LastName, e.Salary, d.DepartmentName 
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID 
WHERE d.DepartmentName = 'Finance'

UNION

SELECT e.FirstName, e.LastName, e.Salary, d.DepartmentName 
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID 
WHERE d.DepartmentName = 'HR'


FirstName	LastName	Salary	DepartmentName
Aysel		Mammadova	60000	Finance
Mahammad	Ahmadov		50000	HR
Nigar		Guliyeva	55000	Finance
Ramin		Isayev		45000	HR

With UNION, if the same employee exists in both departments (which could happen in a system that allows multiple assignments), 
that employee will appear only once in the final output.

Now, if we use UNION ALL, SQL will not check for duplicates. 
It will return every row from both queries, even if they are identical.

SELECT e.FirstName, e.LastName, e.Salary, d.DepartmentName 
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID 
WHERE d.DepartmentName = 'Finance'

UNION ALL

SELECT e.FirstName, e.LastName, e.Salary, d.DepartmentName 
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID 
WHERE d.DepartmentName = 'HR'

This version will include all employees from both departments without removing duplicates.

---

To dive deeper into how this works internally, SQL processes UNION by first executing both SELECT statements separately. 
Then, before returning the result, it sorts and removes duplicates. 
This extra step of sorting and filtering requires additional computational effort, 
which is why UNION can be slower.

With UNION ALL, the process is much simpler. 
SQL executes both SELECT queries and immediately returns all rows without any further processing. 
Because of this, UNION ALL is more efficient in scenarios where duplicates do not need to be removed.

Imagine a case where a company maintains multiple employee records for different job contracts. 
If someone works in both IT and HR under different contracts, 
using UNION would hide this fact, while UNION ALL would preserve the complete data.

When deciding between the two, the key question is whether duplicate rows matter. 
If they do, UNION is necessary. 
If they do not and performance is a concern, UNION ALL is the better choice.


NOTE: UNION and UNION ALL work on queries that return the same number of columns, 
and those columns must have compatible data types. 
The order of execution follows a structured approach.

1. If there are two queries in the UNION, they each execute independently.
2. Each query's result is sorted to identify duplicate rows within itself before merging.
3. After sorting, the queries are combined into a single result set.
4. Once merged, SQL ensures that the final result contains only distinct rows, 
   removing any duplicates that exist across queries.

*/

SELECT e.FirstName, e.LastName, e.Salary, d.DepartmentName 
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID 
WHERE d.DepartmentName = 'Finance'

UNION

SELECT e.FirstName, e.LastName, e.Salary, d.DepartmentName 
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID 
WHERE d.DepartmentName = 'HR'


SELECT e.FirstName, e.LastName, e.Salary, d.DepartmentName 
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID 
WHERE d.DepartmentName = 'Finance'

UNION ALL

SELECT e.FirstName, e.LastName, e.Salary, d.DepartmentName 
FROM Employees e
LEFT JOIN Departments d ON e.DepartmentID = d.DepartmentID 
WHERE d.DepartmentName = 'HR'
