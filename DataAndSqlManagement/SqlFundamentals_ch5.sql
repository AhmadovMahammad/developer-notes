-- Create CUSTOMER table
CREATE TABLE CUSTOMER (
    CUSTOMER_ID INT PRIMARY KEY,
    CUSTOMER_NAME VARCHAR(100),
    CONTACT_NAME VARCHAR(100),
    EMAIL VARCHAR(100),
    PHONE VARCHAR(20),
    ADDRESS VARCHAR(200)
);

-- Create CUSTOMER_ORDER table
CREATE TABLE CUSTOMER_ORDER (
    ORDER_ID INT PRIMARY KEY,
    CUSTOMER_ID INT,
    ORDER_DATE DATE,
    TOTAL_AMOUNT DECIMAL(10,2),
    STATUS VARCHAR(20),
    SHIPPING_ADDRESS VARCHAR(200),
    FOREIGN KEY (CUSTOMER_ID) REFERENCES CUSTOMER(CUSTOMER_ID)
);

-- Insert data into CUSTOMER table
INSERT INTO CUSTOMER (CUSTOMER_ID, CUSTOMER_NAME, CONTACT_NAME, EMAIL, PHONE, ADDRESS)
VALUES 
(1, 'Acme Corporation', 'John Smith', 'john@acme.com', '555-1234', '123 Main St'),
(2, 'TechSolutions Inc', 'Sarah Johnson', 'sarah@techsolutions.com', '555-5678', '456 Tech Blvd'),
(3, 'Re-Barre Construction', 'Robert Brown', 'robert@rebarre.com', '555-9012', '789 Builder Ave'),
(4, 'Global Traders', 'Emma Wilson', 'emma@globaltraders.com', '555-3456', '101 Trade Center'),
(5, 'Fresh Foods Market', 'Michael Davis', 'michael@freshfoods.com', '555-7890', '202 Grocery Lane');

-- Insert data into CUSTOMER_ORDER table
INSERT INTO CUSTOMER_ORDER (ORDER_ID, CUSTOMER_ID, ORDER_DATE, TOTAL_AMOUNT, STATUS, SHIPPING_ADDRESS)
VALUES 
(101, 1, '2023-01-15', 1250.99, 'Delivered', '123 Main St'),
(102, 2, '2023-01-20', 876.50, 'Shipped', '456 Tech Blvd'),
(103, 3, '2023-01-25', 2340.00, 'Processing', '789 Builder Ave'),
(104, 3, '2023-02-05', 1875.25, 'Shipped', '789 Builder Ave'),
(105, 3, '2023-02-12', 945.75, 'Delivered', '789 Builder Ave'),
(106, 4, '2023-02-18', 1568.30, 'Processing', '101 Trade Center'),
(107, 1, '2023-02-22', 2250.00, 'Processing', '123 Main St'),
(108, 5, '2023-03-01', 780.40, 'Shipped', '202 Grocery Lane'),
(109, 2, '2023-03-05', 1340.60, 'Processing', '456 Tech Blvd'),
(110, 4, '2023-03-10', 2680.15, 'Pending', '101 Trade Center');


SELECT
    SUM(TOTAL_AMOUNT) AS [Total Amount Per Date]
FROM CUSTOMER_ORDER
GROUP BY ORDER_DATE;

/* What is a Join?
A join in SQL allows you to combine rows from two or more tables based on a related column between them. 
In our example, the CUSTOMER_ID is the column that relates the CUSTOMER and CUSTOMER_ORDER tables.


-- Parent-Child Relationship
In our example:

CUSTOMER is the parent table
CUSTOMER_ORDER is the child table

This is because CUSTOMER_ORDER depends on CUSTOMER for information - it uses the CUSTOMER_ID to reference customer details. 
The parent table doesn't depend on the child table for any information.


-- One-to-Many Relationship
The example illustrates a one-to-many relationship:

One customer can have multiple orders
Each order belongs to exactly one customer

In our data, customer "Re-Barre Construction" (CUSTOMER_ID 3) has three orders (ORDER_ID 103, 104, and 105).


-- Why Joins Matter
Without joins, you would need to:

- Query the CUSTOMER_ORDER table to get order information
- For each order, make a separate query to the CUSTOMER table to get customer details
- Manually combine this information

Here's a simple explanation of how you would retrieve and combine information from `CUSTOMER_ORDER` and `CUSTOMER` tables without using a JOIN:
### Step 1: Query the CUSTOMER_ORDER table

First, you would retrieve order information:
SELECT * FROM CUSTOMER_ORDER WHERE ORDER_ID = 103;

This returns a row with order details:
```
ORDER_ID: 103
CUSTOMER_ID: 3
ORDER_DATE: 2023-01-25
TOTAL_AMOUNT: 2340.00
STATUS: Processing
SHIPPING_ADDRESS: 789 Builder Ave
```

### Step 2: Make a separate query to the CUSTOMER table
Now you need to look up the customer information using the CUSTOMER_ID (3) from the first query:
SELECT * FROM CUSTOMER WHERE CUSTOMER_ID = 3;

This returns:
```
CUSTOMER_ID: 3
CUSTOMER_NAME: Re-Barre Construction
CONTACT_NAME: Robert Brown
EMAIL: robert@rebarre.com
PHONE: 555-9012
ADDRESS: 789 Builder Ave
```

### Step 3: Manually combine the information
Finally, you'd combine these results, either in your application code or in a report:

```
ORDER_ID: 103
ORDER_DATE: 2023-01-25
TOTAL_AMOUNT: 2340.00
STATUS: Processing
SHIPPING_ADDRESS: 789 Builder Ave
CUSTOMER_NAME: Re-Barre Construction
CONTACT_NAME: Robert Brown
EMAIL: robert@rebarre.com
PHONE: 555-9012
```

This manual process is inefficient because:
1. It requires multiple database queries
2. The application needs to handle the data combination logic
3. It becomes extremely cumbersome when dealing with multiple orders

This is exactly why JOINs were created - to handle this relationship in a single query rather than through multiple separate queries.
Joins allow you to retrieve all this related information in a single query, making data retrieval much more efficient.


-- Basic JOIN Syntax
Here's how you would join these two tables:

SELECT *
FROM CUSTOMER
JOIN CUSTOMER_ORDER ON CUSTOMER_ORDER.CUSTOMER_ID = CUSTOMER.CUSTOMER_ID

This query will return all columns from both tables where the CUSTOMER_ID matches, 
effectively giving you a complete picture of each order with its associated customer information.

CUSTOMER_ID	CUSTOMER_NAME	CONTACT_NAME	EMAIL	PHONE	ADDRESS	ORDER_ID	CUSTOMER_ID	ORDER_DATE	TOTAL_AMOUNT	STATUS	SHIPPING_ADDRESS
1	Acme Corporation	John Smith	john@acme.com	555-1234	123 Main St	101	1	2023-01-15	1250.99	Delivered	123 Main St
2	TechSolutions Inc	Sarah Johnson	sarah@techsolutions.com	555-5678	456 Tech Blvd	102	2	2023-01-20	876.50	Shipped	456 Tech Blvd
3	Re-Barre Construction	Robert Brown	robert@rebarre.com	555-9012	789 Builder Ave	103	3	2023-01-25	2340.00	Processing	789 Builder Ave
3	Re-Barre Construction	Robert Brown	robert@rebarre.com	555-9012	789 Builder Ave	104	3	2023-02-05	1875.25	Shipped	789 Builder Ave
3	Re-Barre Construction	Robert Brown	robert@rebarre.com	555-9012	789 Builder Ave	105	3	2023-02-12	945.75	Delivered	789 Builder Ave
4	Global Traders	Emma Wilson	emma@globaltraders.com	555-3456	101 Trade Center	106	4	2023-02-18	1568.30	Processing	101 Trade Center
1	Acme Corporation	John Smith	john@acme.com	555-1234	123 Main St	107	1	2023-02-22	2250.00	Processing	123 Main St
5	Fresh Foods Market	Michael Davis	michael@freshfoods.com	555-7890	202 Grocery Lane	108	5	2023-03-01	780.40	Shipped	202 Grocery Lane
2	TechSolutions Inc	Sarah Johnson	sarah@techsolutions.com	555-5678	456 Tech Blvd	109	2	2023-03-05	1340.60	Processing	456 Tech Blvd
4	Global Traders	Emma Wilson	emma@globaltraders.com	555-3456	101 Trade Center	110	4	2023-03-10	2680.15	Pending	101 Trade Center

*/

SELECT *
FROM CUSTOMER
JOIN CUSTOMER_ORDER ON CUSTOMER_ORDER.CUSTOMER_ID = CUSTOMER.CUSTOMER_ID