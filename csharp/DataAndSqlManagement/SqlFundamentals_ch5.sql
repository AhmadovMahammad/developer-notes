CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    DepartmentID INT,
    Salary DECIMAL
);

CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(100)
);

DELETE FROM Employees
DELETE FROM Departments

INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary)
VALUES
(1, 'Mahammad', 'Ahmadov', 1, 50000),
(2, 'Aysel', 'Mammadova', 2, 60000),
(3, 'Ramin', 'Isayev', 1, 45000),
(4, 'Leyla', 'Aliyeva', 3, 70000),
(5, 'Nigar', 'Guliyeva', 2, 55000),
(6, 'Wrong', 'Data', 12, 80000);

INSERT INTO Departments (DepartmentID, DepartmentName)
VALUES
(1, 'HR'),
(2, 'Finance'),
(3, 'Engineering'),
(4, 'None');



/* 1. Creating Tables and Inserting Data

Let's start by creating two tables, Employees and Departments, and inserting some data into them. 
This will help us demonstrate the joins.

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    DepartmentID INT,
    Salary DECIMAL
);

CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(100)
);

INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary)
VALUES
(1, 'Mahammad', 'Ahmadov', 1, 50000),
(2, 'Aysel', 'Mammadova', 2, 60000),
(3, 'Ramin', 'Isayev', 1, 45000),
(4, 'Leyla', 'Aliyeva', 3, 70000),
(5, 'Nigar', 'Guliyeva', 2, 55000),
(6, 'Wrong', 'Data', 12, 80000);

INSERT INTO Departments (DepartmentID, DepartmentName)
VALUES
(1, 'HR'),
(2, 'Finance'),
(3, 'Engineering'),
(4, 'None');

*/

/* 2. Joins: Explanation
Now that we have our data, let's go over the types of SQL Joins and explain them in simple terms.



2.1. INNER JOIN

Definition: An INNER JOIN returns only the rows where there is a match in both tables.
Example: We want to find the employees and the departments they work in, 
but we only want employees that are assigned to a department.

SELECT e.FirstName, e.LastName, e.Salary, d.DepartmentName 
FROM Employees e
INNER JOIN Departments d
ON e.DepartmentID = d.DepartmentID

SELECT Employees.FirstName, Employees.LastName, Employees.Salary, Departments.DepartmentName
FROM Employees
INNER JOIN Departments
ON Employees.DepartmentID = Departments.DepartmentID

FirstName	LastName	Salary	DepartmentName
Mahammad	Ahmadov	    50000	HR
Aysel	    Mammadova	60000	Finance
Ramin	    Isayev	    45000	HR
Leyla	    Aliyeva	    70000	Engineering
Nigar	    Guliyeva	55000	Finance

Explanation: This query will return the employees along with their department names, 
but only for those employees who have a valid DepartmentID (i.e., those who belong to a department). 
Employees who don't belong to a department will be excluded.



2.2. LEFT JOIN (or LEFT OUTER JOIN)

Definition: A LEFT JOIN returns all rows from the left table and the matching rows from the right table. 
If there is no match, the result is NULL for columns from the right table.

Example: We want to list all employees and their departments, including those who are not assigned to a department.

SELECT Employees.FirstName, Employees.LastName, Employees.Salary, Departments.DepartmentName
FROM Employees
LEFT JOIN Departments
ON Employees.DepartmentID = Departments.DepartmentID


FirstName	LastName	Salary	DepartmentName
Mahammad	Ahmadov	    50000	HR
Aysel	    Mammadova	60000	Finance
Ramin	    Isayev	    45000	HR
Leyla	    Aliyeva	    70000	Engineering
Nigar	    Guliyeva	55000	Finance
Wrong	    Data	    80000	NULL ---------------> this employee is not belong to any of departmens, 
                                                      so department name is null

Explanation: This will return all employees. 
If an employee does not belong to a department, the DepartmentName will show as NULL.



2.3. RIGHT JOIN (or RIGHT OUTER JOIN)

Definition: A RIGHT JOIN returns all rows from the right table and the matching rows from the left table. 
If there is no match, the result is NULL for columns from the left table.

Example: We want to list all departments and the employees in those departments, including departments that do not have any employees.

SELECT Employees.FirstName, Employees.LastName, Departments.DepartmentName
FROM Employees
RIGHT JOIN Departments
ON Employees.DepartmentID = Departments.DepartmentID;

FirstName	LastName	Salary	DepartmentName
Mahammad	Ahmadov	    50000	HR
Ramin	    Isayev	    45000	HR
Aysel	    Mammadova	60000	Finance
Nigar	    Guliyeva	55000	Finance
Leyla	    Aliyeva	    70000	Engineering
NULL	    NULL	    NULL	None

Explanation: This will return all departments. 
If a department has no employees, the FirstName, LastName and Salary will be NULL.



2.4. FULL JOIN (or FULL OUTER JOIN)

Definition: A FULL JOIN returns all rows when there is a match in either the left or the right table. If there is no match, the result is NULL from the side that has no match.
Example: We want to list all employees and all departments, including employees who don't belong to any department and departments with no employees.

SELECT Employees.FirstName, Employees.LastName, Departments.DepartmentName
FROM Employees
FULL JOIN Departments
ON Employees.DepartmentID = Departments.DepartmentID;

Explanation: This will return all employees and all departments. 
If an employee does not belong to a department, the DepartmentName will be NULL. 
Similarly, if a department has no employees, the FirstName and LastName will be NULL.

How FULL JOIN Works Internally

2.4.1. Start with LEFT JOIN rows

Includes all rows from the left table (Employees), with matched values from the right table (Departments).
If no match is found, NULL is placed in the right table's columns.

2.4.2. Add RIGHT JOIN rows that were missing

Includes all rows from the right table (Departments) that were not already included from the LEFT JOIN.
If no match is found, NULL is placed in the left table's columns.



2.5. CROSS JOIN

Definition: A CROSS JOIN returns the Cartesian product of two tables, i.e., 
it returns all possible combinations of rows from the two tables. 
Be careful with this one because it can return a huge number of results.

Example: Let's combine every employee with every department.

SELECT Employees.FirstName, Employees.LastName, Employees.Salary, Departments.DepartmentName
FROM Employees
CROSS JOIN Departments;

FirstName	LastName	Salary	DepartmentName
Mahammad	Ahmadov	    50000	HR
Aysel	    Mammadova	60000	HR
Ramin	    Isayev	    45000	HR
Leyla	    Aliyeva	    70000	HR
Nigar	    Guliyeva	55000	HR
Wrong	    Data	    80000	HR
Mahammad	Ahmadov	    50000	Finance
Aysel	    Mammadova	60000	Finance
Ramin	    Isayev	    45000	Finance
Leyla	    Aliyeva	    70000	Finance
Nigar	    Guliyeva	55000	Finance
Wrong	    Data	    80000	Finance
Mahammad	Ahmadov	    50000	Engineering
Aysel	    Mammadova	60000	Engineering
Ramin	    Isayev	    45000	Engineering
Leyla	    Aliyeva	    70000	Engineering
Nigar	    Guliyeva	55000	Engineering
Wrong	    Data	    80000	Engineering
Mahammad	Ahmadov	    50000	None
Aysel	    Mammadova	60000	None
Ramin	    Isayev	    45000	None
Leyla	    Aliyeva	    70000	None
Nigar	    Guliyeva	55000	None
Wrong	    Data	    80000	None

Explanation: This will return every combination of employees and departments. 
For example, if there are 5 employees and 3 departments, this query will return 15 rows (5 * 3).


2.5.1. Why Use CROSS JOIN?
At first glance, CROSS JOIN might seem useless because it does not use a joining condition and returns a Cartesian product 
(every row from the first table is combined with every row from the second table). 
However, it has real-world use cases.

2.5.2. When CROSS JOIN Is Useful?

- Generating All Possible Combinations (Cartesian Product)
Example: If you want to pair every employee with every department, regardless of their actual assignment.
Useful in scenario modeling or testing combinations.

- Combinations in Scheduling or Assignments
Example: Pairing all students with all available courses.
Example: Generating all possible shifts for employees in different departments.



2.6. SELF JOIN

Definition: A SELF JOIN is when a table is joined with itself. 
This is useful when a table has hierarchical data, such as employees who have managers.

Example: Suppose we add a column ManagerID to the Employees table that refers to another EmployeeID, representing their manager.

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    DepartmentID INT,
    Salary DECIMAL,
    ManagerID INT
);

-- Insert data with manager relationships
INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary, ManagerID)
VALUES
(1, 'Mahammad', 'Ahmadov', 1, 50000, NULL),
(2, 'Aysel', 'Mammadova', 2, 60000, 1),
(3, 'Ramin', 'Isayev', 1, 45000, 1),
(4, 'Leyla', 'Aliyeva', 3, 70000, NULL),
(5, 'Nigar', 'Guliyeva', 2, 55000, 2);

-- Self Join Example
SELECT E1.FirstName AS Employee, E2.FirstName AS Manager
FROM Employees E1
LEFT JOIN Employees E2
ON E1.ManagerID = E2.EmployeeID;

Explanation: This query returns a list of employees and their managers. 
If an employee doesn’t have a manager (e.g., John or Alice), the Manager column will be NULL.

*/

SELECT Employees.FirstName, Employees.LastName, Employees.Salary, Departments.DepartmentName
FROM Employees
INNER JOIN Departments
ON Employees.DepartmentID = Departments.DepartmentID

SELECT Employees.FirstName, Employees.LastName, Employees.Salary, Departments.DepartmentName
FROM Employees
LEFT JOIN Departments
ON Employees.DepartmentID = Departments.DepartmentID

SELECT Employees.FirstName, Employees.LastName, Employees.Salary, Departments.DepartmentName
FROM Employees
RIGHT JOIN Departments
ON Employees.DepartmentID = Departments.DepartmentID

SELECT Employees.FirstName, Employees.LastName, Employees.Salary, Departments.DepartmentName
FROM Employees
FULL JOIN Departments
ON Employees.DepartmentID = Departments.DepartmentID;

SELECT Employees.FirstName, Employees.LastName, Employees.Salary, Departments.DepartmentName
FROM Employees
CROSS JOIN Departments;