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