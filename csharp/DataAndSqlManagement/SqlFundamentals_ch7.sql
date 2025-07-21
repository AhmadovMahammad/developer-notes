-- Database Design

/* The SurgeTech Conference

You are a staff member for the SurgeTech conference, a gathering of tech startup
companies seeking publicity and investors. The organizer has tasked you with creating
a database to manage the attendees, companies, presentations, rooms, and presentation
attendance. How should this database be designed?

First, review the different entities and start thinking about how they will be structured
into tables. This may seem like a large number of business asks to capture, but any
complex problem can be broken down into simple components.


-- ATTENDEE
The attendees are registered guests (including some VIPs) who are checking out the
tech startups. Each attendee’s ID, name, phone number, email, and VIP status will
need to be tracked.

Taking all this information, we may design the ATTENDEE table with these columns:

-- ATTENDEE
ATTENDEE ID
FIRST_NAME
LAST NAME
PHONE
EMAIL
VIP


-- COMPANY
The startup companies need to be tracked as well. The company ID, company name,
company description, and primary contact (who should be listed as an attendee) for
each must be tracked:

-- COMPANY
COMPANY_ID
NAME
DESCRIPTION
PRIMARY_CONTACT_ATTENDEE_ID


-- PRESENTATION
Some companies will schedule to do a presentation for a specific slot of time (with a
start time and end time). The company leading the presentation as well as a room
number must also be booked for each presentation slot:


-- ROOM
There will be rooms available for the presentations, each with a room ID number, a
floor number, and a seating capacity:


-- PRESENTATION_ATTENDANCE
If attendees are interested in hearing a company’s presentation, they can acquire a
ticket (with a ticket ID) and be allowed in. This will help keep track of who attended
what presentations. 

To implement this, the PRESENTATION_ATTENDANCE table will track
the ticket IDs and pair the presentations with the attendees through their respective
IDs to show who was where:

*/


/* Primary and Foreign Keys

You should always strive to have a primary key on any table. 
A primary key is a special field (or combination of fields: composite keys) 
that provides a unique identity to each record. 


A primary key often defines a relationship and is frequently joined on. 
The ATTENDEE table has an ATTENDEE_ID field as its primary key, COMPANY has COMPANY_ID, 
and so on. 


While you do not need to designate a field as a primary key to join on it, 
it allows the database software to execute queries much more efficiently. 
No duplicates are allowed on the primary key, 
which means you cannot have two ATTENDEE records both with an ATTENDEE_ID of 2. 
The database will forbid this from happening and throw an error.


To focus our scope in this book, we will not compose a primary key
off more than one field. But be aware that multiple fields can act as
a primary key, and you can never have duplicate combinations of
those fields. For example, if you specified your primary key on the
fields REPORT_ID and APPROVER_ID, you can never have two records
with the same combination of REPORT_ID and APPROVER_ID.


Do not confuse the primary key with a foreign key.
The primary key exists in the parent table, but the foreign key exists in the child table. 
The foreign key in a child table points to the primary key in its parent table. 


For example, the ATTENDEE_ID in the ATTENDEE table is a primary key, 
but the ATTENDEE_ID in the PRESENTATION_ATTENDANCE table is a foreign key. 
The two are joined together for a one-to-many relationship. 

Unlike a primary key, a foreign key does not enforce uniqueness, 
as it is the “many” in a “one-to-many” relationship.
The primary key and foreign key do not have to share the same field name. The
BOOKED_COMPANY_ID in the PRESENTATION table is a foreign key pointing to the
COMPANY_ID in its parent table COMPANY. The field name can be different on the foreign
key to make it more descriptive of its usage. 

In this case, BOOKED_COMPANY_ID is more descriptive than just COMPANY_ID. 

*/


/* Understanding Table Creation with Primary and Foreign Keys

Designing a database for a conference management system requires understanding 
how tables relate to one another. 
Each table represents a distinct entity (such as attendees, companies, or presentations), 
and the relationships between them are defined using primary keys and foreign keys.


---
Primary Keys: The Unique Identifiers
A primary key (PK) ensures that each record in a table is unique and identifiable. 
No two rows can have the same primary key value, and it cannot be NULL. 
This uniqueness makes it easier to retrieve, update, and delete records efficiently.

For example:

The ATTENDEE table has ATTENDEE_ID as its primary key. Every attendee must have a unique ID.
The COMPANY table has COMPANY_ID as its primary key. Every startup must have a unique identifier.
The ROOM table has ROOM_ID as its primary key, ensuring that no two rooms have the same ID.

Each table’s primary key serves as the foundation for relationships between tables.


---
Foreign Keys: Connecting Related Data
A foreign key (FK) is a column in one table that references the primary key in another table. 
It establishes a relationship between the two tables, ensuring that data remains consistent and valid.

For example:

1. The PRESENTATION table needs to track which company is giving the presentation.

- It has BOOKED_COMPANY_ID as a foreign key, which links to COMPANY_ID in the COMPANY table.
- This means a presentation must be associated with a valid company.

2. The PRESENTATION_ATTENDANCE table tracks which attendees go to which presentations.

- ATTENDEE_ID is a foreign key referencing ATTENDEE_ID in the ATTENDEE table.
- PRESENTATION_ID is a foreign key referencing PRESENTATION_ID in the PRESENTATION table.
- This ensures that attendance records only contain existing attendees and presentations.

Foreign keys preventing orphaned records (e.g., preventing a presentation from being booked by a nonexistent company).


---
Referential Actions and Constraints
When defining foreign keys, databases enforce rules to maintain integrity:

1. ON DELETE CASCADE:

- If a company is deleted, all its presentations are also deleted automatically.
- If an attendee is deleted, their attendance records are also removed.
- This prevents orphaned records (e.g., a presentation referencing a nonexistent company).


CREATE TABLE Company (
    company_id INT PRIMARY KEY, -- creates clustered index by default on primary key
    name VARCHAR(255) NOT NULL
)

CREATE TABLE Presentation (
    presentation_id INT PRIMARY KEY,
    booked_company_id INT,
    title VARCHAR(255) NOT NULL,
    FOREIGN KEY (booked_company_id) 
        REFERENCES Company(company_id) 
        ON DELETE CASCADE
);

CREATE TABLE Attendee (
    attendee_id INT PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE Presentation_Attendance (
    attendee_id INT,
    presentation_id INT,
    PRIMARY KEY(attendee_id, presentation_id),
    FOREIGN KEY(attendee_id) 
        REFERENCES Attendee(attendee_id)
        ON DELETE CASCADE,
    FOREIGN KEY(presentation_id)
        REFERENCES Presentation(presentation_id) 
        ON DELETE CASCADE
)


- Behaviors -

: Deleting a Company will delete all related Presentations.
: Deleting an Attendee will remove their attendance records.
: Deleting a Presentation will remove its attendance records.


2. ON DELETE SET NULL:

- If a company is deleted, its presentations still exist, but their BOOKED_COMPANY_ID is set to NULL.
- This keeps the presentation data while allowing flexibility.

CREATE TABLE Company (
    company_id INT PRIMARY KEY, -- creates clustered index by default on primary key
    name VARCHAR(255) NOT NULL
)

CREATE TABLE Presentation (
    presentation_id INT PRIMARY KEY,
    booked_company_id INT,
    title VARCHAR(255) NOT NULL,
    FOREIGN KEY (booked_company_id) 
        REFERENCES Company(company_id) 
        ON DELETE SET NULL
);

- Behavior -

: If a Company is deleted, the related Presentations are not deleted 
  but their BOOKED_COMPANY_ID is set to NULL.

3. ON DELETE RESTRICT (DEFAULT):

- Prevents deletion if related records exist.
- Example: If a company has booked presentations, it cannot be deleted unless the presentations are deleted first.

*/

CREATE TABLE Company (
    company_id INT PRIMARY KEY, -- creates clustered index by default on primary key
    name VARCHAR(255) NOT NULL
)

CREATE TABLE Presentation (
    presentation_id INT PRIMARY KEY,
    booked_company_id INT,
    title VARCHAR(255) NOT NULL,
    FOREIGN KEY (booked_company_id) 
        REFERENCES Company(company_id) 
        ON DELETE CASCADE
);

CREATE TABLE Attendee (
    attendee_id INT PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE Presentation_Attendance (
    attendee_id INT,
    presentation_id INT,
    PRIMARY KEY(attendee_id, presentation_id),
    FOREIGN KEY(attendee_id) 
        REFERENCES Attendee(attendee_id)
        ON DELETE CASCADE,
    FOREIGN KEY(presentation_id)
        REFERENCES Presentation(presentation_id) 
        ON DELETE CASCADE
)



/* What is the MERGE Statement?

The MERGE statement in SQL is used to perform 
1. insert, 2. update, and 3. delete operations on a target table based on a source table's data. 
The idea is to merge the data from the source table into the target table in a single operation. 

This can be really useful when you're dealing with two tables that should be synchronized, 
but you don't want to write separate INSERT, UPDATE, and DELETE queries.



--- Why Would Someone Need MERGE?

Imagine you are dealing with two tables: 
1. one that holds the current state of data, and 2. another that holds updates or new data.

The common scenario where you'd use MERGE is when you're importing new data
or updating an existing table based on changes or additions made in another table.

Here’s an example of when you might need to use MERGE:

1. Synchronizing Data: 

You might have an existing table (say, Employees) and a new table (say, UpdatedEmployees) 
that has updated or new information about the employees. 
You want to update the existing employee records with the new data 
or insert new employee records from the second table. 

2. ETL Processes (Extract, Transform, Load): 

ETL stands for Extract, Transform, Load. 
It’s a process used primarily in data warehousing to collect data from various sources, 
transform it into a format that’s useful for analysis, 
and then load it into a database or data warehouse for storage and querying.

Here’s a simple breakdown of each part of ETL:

2.1. Extract (E)
In this step, data is extracted from different sources. 
These sources can be databases, APIs, flat files (like CSVs or Excel files), or even data streams. 
The goal is to get the raw data that needs to be analyzed or processed.

For example, you might extract data from:

    A sales database
    Social media APIs
    A customer relationship management (CRM) system
    Log files or other forms of data storage

2.2. Transform (T)
Once the data is extracted, it often needs to be transformed. 
This step involves cleaning, changing the format, 
or processing the data so it’s consistent and ready for analysis. This could include:

- Removing duplicates
- Changing data types (e.g., converting date formats)
- Joining data from multiple sources
- Filtering unnecessary data
- Aggregating data (like summing sales totals)
- Calculating new values based on existing data

For example, you might take sales data from two different regions, clean it up, 
and calculate a "total sales" field that combines the two regions' data.

2.3. Load (L)
In the final step, the transformed data is loaded into the destination system, 
usually a data warehouse or a database. 
The data can then be queried and analyzed to generate reports or insights. 
The process of loading data can involve:

- Appending new data to existing data tables.
- Replacing or updating existing data (in some cases, you might want to use MERGE here to synchronize the data).
- Deleting outdated or irrelevant data.


Why Is MERGE Used in ETL?
In ETL processes, you might use the MERGE statement when you're loading data into a destination table 
(such as a data warehouse) and need to:

- Update existing records if the data in the source table is more recent or has changed.
- Insert new records if the source table has data that doesn't exist in the destination table.
- Delete outdated records that no longer exist in the source table.


--------------

What Does MERGE Do?
The MERGE statement allows you to:

1. Update records that already exist in the target table (based on matching data between the source and target).
2. Insert new records into the target table that do not exist.
3. Delete records from the target table that no longer exist in the source.


The Basic Syntax of MERGE

MERGE INTO target_table AS target
USING source_table AS source
ON target.match_column = source.match_column
WHEN MATCHED THEN
    -- Action to take if rows match (e.g., UPDATE)
WHEN NOT MATCHED THEN
    -- Action to take if rows do not match (e.g., INSERT)
WHEN MATCHED AND condition THEN
    -- Action to take if rows match and a condition is met (e.g., DELETE)


MERGE INTO target_table AS target: 
This part tells SQL which table you're going to modify (this is the "target" table). 
The "AS target" part is just an alias for the table name, which makes it easier to refer to it in the query.

USING source_table AS source: 
This tells SQL which table you're comparing the target table with (the "source" table). 
The source table contains the new or updated data you want to apply to the target table.

ON target.match_column = source.match_column: 
This is the condition used to compare rows between the target and source tables. 
Typically, you compare a key column, such as an ID. 
If a record in the target table has the same value as a record in the source table, 
the row is considered a "match."


Example with All Operations: Insert, Update, and Delete
Let’s go step by step with a practical example. Imagine we have two tables:

1. Employees Table (Target Table)

EmployeeID	Name	    Salary
------------------------------
1	        John	    50000
2	        Alice	    60000
3	        Bob	        70000

2. UpdatedEmployees Table (Source Table)

EmployeeID	Name	    Salary
------------------------------
2	        Alice	    65000
4	        Charlie	    55000


We want to:

Update Alice's salary because the UpdatedEmployees table has a higher salary for her.
Insert Charlie because he's not in the Employees table.
Delete Bob from the Employees table because he no longer exists in the UpdatedEmployees table. (OPTIONAL)


MERGE INTO Employees AS target
USING Employees AS source
ON target.EmployeeID = source.EmployeeID
WHEN MATCHED AND target.Salary != source.Salary THEN
    UPDATE SET target.Salary = source.Salary
WHEN NOT MATCHED THEN
    INSERT (EmployeeID, Name, Salary)
    VALUES (source.EmployeeID, source.Name, source.Salary)
WHEN MATCHED AND source.EmployeeID NOT IN (SELECT EmployeeID FROM Employees) THEN
    DELETE;

After running this query, the Employees table will look like this:

EmployeeID	Name	    Salary
------------------------------
1	        John	    50000
2	        Alice	    65000
4	        Charlie	    55000


Conclusion

In summary, MERGE is a powerful SQL feature for merging data from two tables by performing 
insert, update, or delete operations based on certain conditions. 
It is especially useful in situations like data synchronization, ETL processes, 
or when you're working with a source of data that may have new, updated, or missing records.

*/

