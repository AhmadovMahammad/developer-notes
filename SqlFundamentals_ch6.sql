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
WHERE Employees.DepartmentID = (
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