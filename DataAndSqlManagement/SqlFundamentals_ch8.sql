/* Transaction Fundamentals

A transaction is a sequence of database operations that must all complete successfully as a single unit. 
The importance of transactions is ensuring that data integrity is maintained. 

---
Data integrity in a Database Management System (DBMS) ensures that the data remains correct and valid throughout its lifecycle, 
even when multiple users are accessing or modifying it.

Without data integrity, databases can become corrupted, inconsistent, or unreliable, leading to incorrect results, 
duplicate records, or loss of critical information.
---

For example, think of a situation where you're making a payment: 
1. money needs to be deducted from one account and 2. added to another. 
If both operations don't succeed, you don’t want to be in a situation where the money is deducted, but not added to the other account. 
This is where transactions come in. They ensure that either both operations succeed, or neither does.

A transaction is like a "wrapper" around a set of database operations. 
It guarantees that these operations are treated as a single, atomic unit, 
and if something goes wrong, all changes made by the transaction are rolled back.


Core Principles of Transactions (ACID)

Before jumping into SQL code, it's important to understand ACID at -basic- level, 
the set of principles that make transactions reliable:

1. Atomicity:
This means that all operations within a transaction are treated as a single "atomic" unit. 
Either all operations succeed, or none of them are applied. 
If anything goes wrong, everything is undone.

2. Consistency:
The database must always be in a valid state before and after a transaction. 
A transaction will not violate any database constraints (e.g., foreign keys, unique constraints).

3. Isolation:
Transactions are isolated from each other, 
meaning one transaction's operations are invisible to others until the transaction is committed.

4. Durability:
Once a transaction is committed, its changes are permanent and will survive even if the system crashes.

-----
The Transaction Commands: A Detailed Look

1. BEGIN TRANSACTION

The BEGIN TRANSACTION command starts a new transaction. After this command, any changes made to the database 
(like INSERT, UPDATE, DELETE) 
will not be permanent until you explicitly commit them.

BEGIN TRANSACTION
INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary)
VALUES (10, 'Name', 'Surname', 1, 100000)

It’s important to use this command whenever you want to group operations together and ensure they are treated as a single unit. 
Without BEGIN TRANSACTION, each database operation is applied immediately.

Here is an example:
BEGIN TRANSACTION;

Once you issue BEGIN TRANSACTION, the database is now in a "transaction mode." 
Any operations you perform next are not permanent until you commit them.

BEGIN TRANSACTION;

-- Deduct money from Account A
UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;

-- Add money to Account B
UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2;

In this example, we’re transferring money from one account to another. 
The transaction starts with BEGIN TRANSACTION. 
If something goes wrong during these updates (e.g., a network failure or a problem with one of the updates), 
nothing will be changed in the database.


---
2. COMMIT

After you have made changes to the database within a transaction and are satisfied that everything is correct, 
you issue the COMMIT command. 
This command applies all the changes made during the transaction permanently.

When you commit, the changes you made become visible to other users and applications. 
A commit also ends the transaction.


Here’s how you’d use COMMIT:

-- Assuming we were inside a transaction
COMMIT;

This confirms the changes made by the transaction are now part of the database. 
The money transfer (debit from one account and credit to another) is now final.

BEGIN TRANSACTION;

-- Step 1: Deduct money from Account A
UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;

-- Step 2: Add money to Account B
UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2;

-- If everything is successful, commit the transaction
COMMIT;


If the two UPDATE commands succeed, we COMMIT the changes, making them permanent in the database. 
However, if there’s a problem (e.g., a connection failure after the first update), 
we can roll back to the point where the transaction started, and no changes will be made.


---
3. ROLLBACK

ROLLBACK is the opposite of COMMIT. It undoes all the changes made during the current transaction. 
If anything goes wrong during the transaction, you can issue a ROLLBACK to undo all changes, 
returning the database to its state before the transaction began.

For example:

BEGIN TRANSACTION

-- Increase salary on first employee
UPDATE Employees
SET Salary = Salary + 100 WHERE EmployeeID = 1

-- Something goes wrong or an error is detected
ROLLBACK

If we had used COMMIT instead of ROLLBACK, the changes would have been permanent.


---
SAVEPOINTS

Savepoints allow you to set intermediate points inside a transaction, 
where you can later roll back to if needed, without undoing the entire transaction. 

Savepoints are useful in large transactions where you want to commit some changes 
but be able to "undo" certain parts of the transaction if something goes wrong later.

To set a savepoint, you use SAVEPOINT <name>:

-----
BEGIN TRANSACTION;

-- Step 1: Deduct money from Account A
UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;

-- Create a savepoint after this step
SAVEPOINT after_debit;

-- Step 2: Add money to Account B
UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2;

-- If something goes wrong here, rollback to the savepoint
ROLLBACK TO SAVEPOINT after_debit;

-- If everything is okay, commit the entire transaction
COMMIT;
-----

In this example, we use SAVEPOINT after_debit to mark the point after the debit operation. 
If something goes wrong while trying to update Account B, we can roll back to this savepoint, undoing only the second update. 
The debit from Account A will still remain.

This approach provides more fine-grained control over which parts of the transaction to keep and which to discard.


-- Why Do We Need Transactions?

Transactions are crucial for maintaining data consistency and integrity. 
Let’s go back to the example of transferring money from one account to another. 
Without transactions, you risk partial updates. 

For example, if you successfully decrease money from one account but fail to deposit it into the other account, 
the transaction will leave the system in an inconsistent state.

With transactions, you can ensure that either both operations succeed, or neither happens at all. 
This protects your data from unexpected failures, 
and ensures that every transaction is either fully completed or completely undone.


*/

BEGIN TRANSACTION
INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary)
VALUES (10, 'Name', 'Surname', 1, 100000)
ROLLBACK


/* Understanding the ACID Principles: Database Design Basics

When we store and manage data in databases, we need to make sure that the data is correct, safe, and reliable. 
Imagine you are using a banking app, transferring money between accounts. 
You wouldn't want any errors to cause money to disappear or show the wrong balance.

To protect data from mistakes, crashes, or system failures, databases follow ACID principles. 
ACID stands for:

1. Atomicity → Everything happens completely or nothing happens at all
2. Consistency → The database always follows the rules
3. Isolation → Transactions don’t interfere with each other
4. Durability → Once saved, data is permanent, even if the system crashes

-----------

1. Atomicity: All or Nothing

Think of atomicity as "all or nothing" rule. A transaction (a set of database operations) 
should either fully complete or not happen at all.

Example: Bank Transfer
Imagine you want to transfer $100 from Account A to Account B:

: Withdraw $100 from Account A
: Deposit $100 into Account B

Now, what if something goes wrong after Step 1? Maybe the app crashes before Step 2 completes. 
Without atomicity, $100 is gone from Account A but never reaches Account B!

To prevent this, databases use transactions. If any step fails, everything is rolled back (undone). 
So, either both steps succeed, or nothing happens at all.

``` SQL Example

BEGIN TRANSACTION;  -- Start a transaction

UPDATE Accounts SET Balance = Balance - 100 WHERE AccountID = 1;  -- Withdraw $100
UPDATE Accounts SET Balance = Balance + 100 WHERE AccountID = 2;  -- Deposit $100

COMMIT;  -- Confirm the transaction (both updates succeed)

```
If an error happens before COMMIT, the database automatically cancels all changes.

-----------

2. Consistency: Follow the Rules

Consistency ensures that data always follows the rules of the database. 
If a transaction would break the rules, the database rejects it.

Example: Age Rule in a Student Database
A school database has a rule that students must be at least 18 years old.
If someone tries to insert a student with age 16, the database rejects the transaction to keep the data valid.

``` SQL Example

CREATE TABLE Students (
    StudentID INT PRIMARY KEY,
    Name VARCHAR(100),
    Age INT CHECK (Age >= 18)  -- Rule: Age must be 18 or more
);

```

If you try this: 

INSERT INTO Students (StudentID, Name, Age) VALUES (1, 'Name Surname', 16);
The database refuses the operation because it breaks the Consistency rule.

-----------

3. Isolation: Transactions Don't Interfere

When many people use a database at the same time, isolation makes sure transactions don’t conflict with each other.

Example: Two Customers Buying the Same Product
Imagine an online store has 1 phone in stock, and two customers try to buy it at the same time.

: Customer A clicks "Buy"
: Customer B clicks "Buy" at the same time

Without isolation, both could be charged, but the store only has one phone! 
This leads to wrong data.

Databases lock the product until the first transaction finishes, so only one customer can complete the purchase.

``` SQL Example (Using Isolation)

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;  -- Prevents interference

BEGIN TRANSACTION;
UPDATE Products SET Stock = Stock - 1 WHERE ProductID = 101;
COMMIT;

```

--- Isolation Levels in Databases

When multiple transactions execute at the same time, they may attempt to read, write, or update the same data. 
Isolation levels control how these transactions interact, ensuring data consistency while balancing performance. 
The goal is to define how much one transaction can see the changes made by another transaction before those changes are committed.



1. Read Uncommitted

The lowest isolation level, Read Uncommitted, allows transactions to read data that has been modified but not yet committed by other transactions. 
This means that a transaction might read data that is later rolled back, leading to inconsistencies. 
This phenomenon is known as a [ dirty read ].

How It Works
Imagine a banking system where an account balance is stored.

Initial Data:
AccountID	Balance
1	        $500

Scenario:
    Transaction A starts and updates the balance of Account 1 to $400 but has not committed yet.
    Transaction B reads the balance and sees $400, even though the change is not yet permanent.
    Transaction A rolls back (cancels) the change, reverting the balance to $500.
    Transaction B now has incorrect information because it saw a temporary value that no longer exists.

``` Code Example (Read Uncommitted in SQL)

SELECT * FROM Employees

--EmployeeID	FirstName	LastName	DepartmentID	Salary
--1				Mahammad	Ahmadov		1				50000

-- Transaction A (Updating balance)
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
BEGIN TRANSACTION

UPDATE Employees SET Salary = Salary + 500 WHERE EmployeeID = 1
-- No COMMIT yet

-- Transaction B (Reading balance)
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT * FROM Employees WHERE EmployeeID = 1  -- Might see 50500, even if Transaction A will rollback

--EmployeeID	FirstName	LastName	DepartmentID	Salary
--1				Mahammad	Ahmadov		1				50500


ROLLBACK  -- Traansaction A cancel changes

SELECT * FROM Employees WHERE EmployeeID = 1

--EmployeeID	FirstName	LastName	DepartmentID	Salary
--1				Mahammad	Ahmadov		1				50000

```


2. Read Committed (Default Isolation Level in Many Databases)

A transaction can only read data that has been committed. It never sees uncommitted changes made by other transactions. 
However, this level does not prevent non-repeatable reads—if you read the same data twice, 
it might change if another transaction commits a modification in between.

Scenario:
    Transaction A updates the balance to $400 but has not committed yet.
    Transaction B wants to check the balance but can only see committed data, so it still sees $500.
    Transaction A commits the changes.
    Transaction B reads again and now sees $400.

Read Committed ensures that no dirty reads occur, but it does not prevent [ non-repeatable reads ] - problem name.



3. Repeatable Read

If a transaction reads data once, it will always see the same data for the rest of the transaction, 
even if another transaction modifies it. 
This prevents [ non-repeatable reads ] but does not prevent [ phantom reads ].

Imagine an online store where two customers are checking the stock of a product.

Scenario:

    Transaction A reads the stock of Product 101 and sees 10.
    Transaction B purchases 2 items and commits the change, reducing stock to 8.
    Transaction A reads the stock again in the same transaction.
    It still sees 10 instead of 8, because Repeatable Read ensures that data does not change during the transaction.


Prevents [ non-repeatable reads ], but [ phantom reads ] can still occur.

```

-- Transaction A (Checking stock)
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
BEGIN TRANSACTION;
SELECT Stock FROM Products WHERE ProductID = 101;  -- Sees 10

-- Transaction B (Purchasing items)
BEGIN TRANSACTION;
UPDATE Products SET Stock = Stock - 2 WHERE ProductID = 101;
COMMIT;  -- Now stock is 8

-- Transaction A (Checking stock again)
SELECT Stock FROM Products WHERE ProductID = 101;  -- Still sees 10
COMMIT;

```


--- Explaining Problems

1. Dirty Reads

A dirty read happens when a transaction reads data that has been modified but not yet committed by another transaction. 
If the other transaction rolls back the changes, the first transaction will have read incorrect (temporary) data.

Example of Dirty Read:
Imagine two bank employees checking the same account balance.

    Transaction A starts and deducts $100 from Account 1 (balance becomes $400) but has not committed yet.
    Transaction B checks the balance and sees $400.
    Now, Transaction A rolls back (cancels its changes), so the balance goes back to $500.
    But Transaction B has already seen $400, which was never actually committed—this is a dirty read.

- How to Prevent Dirty Reads?
To prevent dirty reads, a transaction should only see committed data. 
The Read Committed isolation level and above prevent dirty reads.


2. Non-Repeatable Reads

A non-repeatable read happens when a transaction reads the same data multiple times, 
but the data changes between reads because another transaction modified and committed it in between.

Example of Non-Repeatable Read:
Imagine an online shopping website:

    Transaction A starts and reads the stock of a product—it sees 10 items in stock.
    Transaction B buys 2 items and commits the change (stock becomes 8).
    Transaction A reads the stock again and now sees 8 items instead of 10—this is a non-repeatable read.

- How to Prevent Non-Repeatable Reads?
To prevent non-repeatable reads, the database should lock the rows so that
no other transaction can change them during the first transaction. 
The Repeatable Read isolation level and above prevent non-repeatable reads.


3. Phantom Reads

A phantom read happens when a transaction runs a query multiple times and
sees new rows appear (or disappear) between queries because another transaction inserted or deleted rows.

Example of Phantom Read:
Imagine a student registration system:

    Transaction A starts and checks how many students are registered for a class—it sees 50 students.
    Transaction B registers 5 more students and commits the change (total students = 55).
    Transaction A checks the student count again and now sees 55 instead of 50—this is a phantom read.

How to Prevent Phantom Reads?
To prevent phantom reads, the database must lock the entire range of data being queried. 
The Serializable isolation level is the only level that fully prevents phantom reads.

--- End Problems



4. Serializable

This is the strictest isolation level. Transactions act as if they are executed one after another, without overlapping. 
It prevents dirty reads, non-repeatable reads, and phantom reads, but it is the slowest.


Imagine a class registration system where two students try to register for the last available seat.

Initial Data:
ClassID	    SeatsAvailable
201	        1

Scenario:

    Transaction A starts and sees 1 seat available.
    Transaction B also starts and sees 1 seat available.
    Transaction A books the seat and commits, setting SeatsAvailable = 0.
    Transaction B tries to book, but Serializable ensures strict order, so it must wait.
    When Transaction B runs again, it sees that no seats are left.

Ensures complete isolation but can cause delays.

```

CREATE TABLE Classes
(
	ClassID INT PRIMARY KEY,
	SeatsAvailable INT
)

ALTER TABLE Classes
ADD CONSTRAINT chk_seats_available CHECK (SeatsAvailable >= 0)

ALTER TABLE Classes
DROP CONSTRAINT chk_seats_available;

INSERT INTO Classes VALUES (100, 1)
UPDATE Classes SET SeatsAvailable = 1


SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
BEGIN TRANSACTION
SELECT SeatsAvailable FROM Classes WHERE ClassID = 100;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;
SELECT SeatsAvailable FROM Classes WHERE ClassID = 100;

UPDATE Classes SET SeatsAvailable = SeatsAvailable - 1 WHERE ClassID = 100
COMMIT

UPDATE Classes SET SeatsAvailable = SeatsAvailable - 1 WHERE ClassID = 100;
COMMIT

SELECT * FROM Classes

```

*/

/*

IF @@ERROR <> 0 ROLLBACK
ELSE COMMIT

-- OR

BEGIN TRANSACTION
	BEGIN TRY
		-- QUERIES
		SELECT @@TRANCOUNT
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK
	END CATCH

*/

