/* What Is a Database?

In the broadest definition, a database is anything that collects and organizes data. A
spreadsheet holding customer bookings is a database, and so is a plain-text file containing
flight schedule data. Plain-text data itself can be stored in a variety of formats,
including XML and CSV.

Professionally, however, when one refers to a “database” they likely are referring to a
relational database management system (RDBMS). This term may sound technical
and intimidating, but an RDBMS is simply a type of database that holds one or more
tables that may have relationships to each other.

*/

/* Exploring Relational vs Non-Relational Databases

Relational and non-relational are the two popular types of databases that are most widely used. 
They are very different in their features and trade-offs and appropriate for different use cases.


--- Relational databases
Relational databases, sometimes called SQL databases or Relational Database Management Systems (RDBMS), 
store data in tables with columns and rows.

The main advantage of relational databases is the integrity of references which means the data is always accurate and consistent. 
Using primary/foreign keys ensures that when a row is added or deleted, any references to other tables must also be valid and exist. 
This prevents orphaned records where the reference points to a row that does not exist, which can cause errors in your application.

Example relational databases: MySQL, PostgreSQL, Amazon RDS, Google Cloud SQL, Azure SQL
----------------------------------------------------------------------------------------

--- Non-relational databases
NoSQL databases are designed to have flexible schemas, making them great for modern applications where data structures might evolve quickly. 
They typically offer an easy development experience due to the JSON-like format and 
lack of structure imposed by tables, rows and columns.

NoSQL databases scale horizontally, allowing them to operate at a much larger scale and performance than relational databases. 
Not worrying about the accuracy of relations makes it much easier to share non-relational data sets.


NOTE: There are various types of NoSQL databases which we’ll cover next.
*/

/* Types of NoSQL databases

1. Document store
- Document databases store data in documents, which are typically JSON-like (or BSON for MongoDB) objects.
- Each document is a key-value pair where the key is the unique identifier and the value is a document (a JSON object).
- Documents can have a flexible structure, meaning fields can vary from one document to another within the same collection.

Structure (MongoDB Example)

{
    "_id": "127001",
    "name": "Mahammad Ahmadov",
    "email": "dev.ahmadov.mahammad@gmail.com",
    "orders": [
        {
            "order_id": "001",
            "product": "Laptop",
            "price": 1200
        },
        {
            "order_id": "002",
            "product": "Mouse",
            "price": 50
        }
    ]
}

db.users.find({ "name": "Mahammad Ahmadov" })

Additional Points:
- You don't need to define the structure beforehand. This makes it easy to evolve the data model over time.
- Data can be sharded across multiple nodes, making it highly scalable.

Examples: MongoDB, Amazon DocumentDB, Google Cloud Firestore, Azure CosmosDB.
----------------------------------------------------------------------------

2. In-Memory Cache
- Typically, in-memory caches store data as key-value pairs. 
  The key is a unique identifier, and the value can be a string, integer, object, or serialized data.
- The data resides entirely in RAM, enabling fast access times. 
  Some systems also provide persistent storage by periodically taking snapshots or writing data to disk.

Key: "user:12345"
Value: {"name": "Mahammad Ahmadov", "email": "dev.ahmadov.mahammad@gmail.com"}

Additional Points:
- Ideal for caching frequently accessed data to reduce load on databases and minimize response times.
- Data can be lost if the system crashes, unless persistent snapshots are enabled.

Examples: Redis, Amazon MemoryDB, Google Cloud Memorystore, Azure Cache.
------------------------------------------------------------------------

3. Graph Database
Graph databases store data as nodes and edges, forming a graph-like structure instead of traditional tables or documents.



1. What is a Node?
A node represents an entity or object in the database. It could be a person, product, place, event, or any other real-world entity.
Each node has properties (key-value pairs) that describe its attributes.

Example of Nodes > Consider an e-commerce system:

    A User is a node: {id: 1, name: "Mahammad Ahmadov", age: 21}
    A Product is a node: {id: 101, name: "Laptop", price: 1200}
    A Category is also a node: {id: 201, name: "Electronics"}



2. What is an Edge?
An edge represents a relationship between two nodes. It connects nodes and defines how they are related.
Each edge has a type (relationship name) and can have properties of its own.

Example of Edges

    A User (Mahammad Ahmadov) purchased a Product (Laptop)
        Edge: {type: "PURCHASED", date: "2025-01-30"}

    A Product (Laptop) belongs to a Category (Electronics)
        Edge: {type: "BELONGS_TO"}



3. Graph Representation in a Database

  (Mahammad Ahmadov) --[PURCHASED]--> (Laptop)
       │
       └── [FRIEND_OF] ──> (Ahmadov SomeOne)
  
  (Laptop) --[BELONGS_TO]--> (Electronics)



4. Querying a Graph Database
Graph databases like Neo4j use Cypher Query Language (CQL) to find relationships efficiently.

MATCH (u:User)-[:PURCHASED]->(p:Product)
WHERE u.name = "Mahammad Ahmadov"
RETURN p.name

This query finds all products purchased by Mahammad Ahmadov.

Examples: Neo4j, AWS Neptune, Apache TinkerPop.
-----------------------------------------------

4. Search Engine Database
- Search engine databases index data in a key-value pair format for fast retrieval, 
  but with advanced indexing and full-text search capabilities.
- The data is indexed by various attributes like keywords, metadata, or tags, 
  allowing complex queries to return relevant results.


Example (Indexing Text Data):

Index: "product_name: Laptop"
Associated document: {id: 101, description: "A high-performance laptop with 16GB RAM"}

Examples: Elasticsearch, Amazon OpenSearch, Azure Cognitive Search.
-------------------------------------------------------------------

5. Time Series Database (TSDB)
- Data is stored in a series of timestamped records, with each record representing a data point at a particular time.
- It is optimized for storing sequential data over time, like sensor readings, logs, or financial data.


Example (Time Series Data):
{
  "timestamp": "2025-01-30T12:00:00Z",
  "temperature": 22.5,
  "humidity": 60
}

- Time series databases are typically append-only because new data is added as it comes in, without modifying old data.
- TSDBs are designed for efficient aggregations over time, such as calculating averages, max/min, or sums over intervals.

Examples: Prometheus, Amazon Timestream.
----------------------------------------

*/

/* Horizontal vs Vertical Scaling

--- What Is Scaling?
In the context of databases and systems, 
scaling refers to the ability of a system to handle more data or more traffic (requests, users, etc.) as it grows.

Scaling Up: Making an individual machine or system more powerful.
Scaling Out: Adding more machines or systems to handle the load.


--- #1. Vertical Scaling (Scaling Up)

1. What Is Vertical Scaling?

    - Vertical scaling means upgrading the capacity of a single machine.
    - Think of it as making one server more powerful by adding resources like CPU, RAM, storage, or upgrading the hardware.

2. How Vertical Scaling Works:

    - Imagine you have a database server. If your system starts experiencing performance bottlenecks (e.g., high CPU usage or slow processing), 
	  you could add more RAM or a faster processor to that machine.
	- You increase the power of the machine to handle more requests or data.
    - This is "scaling up" because you are improving the performance of the existing machine.

3. Why Vertical Scaling?

    - Simplicity: It's relatively easier to scale vertically because you are just adding more resources to one machine.
    - No Complex Data Management: Since you're working with a single server, 
	  you don't need to manage the complexity of splitting or distributing data across multiple machines.

4. Limitations of Vertical Scaling:

    - Hardware Limitations: There’s a physical limit to how much you can upgrade a single machine. 
	  Eventually, you run out of space or processing power.
    - Cost: Upgrading to more powerful machines can become expensive.
    - Single Point of Failure: If that one powerful server fails, your entire system goes down.

- Example of Vertical Scaling:

SQL Databases like MySQL, PostgreSQL, and Microsoft SQL Server often use vertical scaling because:
Moving data across multiple servers would make it complex due to the strict consistency requirements.



--- #2. Horizontal Scaling (Scaling Out)

1. What Is Horizontal Scaling?

    - Horizontal scaling means adding more machines to handle the load.
    - Think of it as adding more servers (or nodes) to distribute the data and processing load across multiple systems, 
	  rather than improving just one machine.

2. How Horizontal Scaling Works:

    - Imagine you have a system handling requests, and it's overwhelmed. 
	  Instead of upgrading the existing server, you add more servers to the system and distribute the data across them.
    - Data Partitioning: The data gets split (or sharded) between multiple servers so that each server only manages a portion of the data, 
      thus reducing the load on any single server.
    - For example, one server may store users A-M while another stores users N-Z, 
	  so each server handles only part of the data.

3. Why Horizontal Scaling?

    - Handles Large Volumes of Data: As your data grows, you can continue to add more machines to the system, 
	  allowing it to handle massive datasets and high traffic volumes.
    - Fault Tolerance: With multiple machines, if one server goes down, others can continue to handle the load, making the system more resilient.
    - Cost-Effective: Horizontal scaling often uses more standard, cheaper hardware, and it allows for gradual scaling without the need to replace expensive hardware.

4. Limitations of Horizontal Scaling:

    - Complexity: Managing multiple servers (nodes) can be complex. 
	  You need to figure out how to distribute data, handle synchronization, and maintain consistency across all the servers.
    - Networking Overhead: Communication between servers requires network bandwidth and can introduce latency.

Example of Horizontal Scaling:

NoSQL Databases like MongoDB, Cassandra, and Couchbase are designed to scale horizontally because:
They often don't enforce strict relational models. This means that data can be split across multiple servers without concerns 
about relationships like foreign keys.
NoSQL databases are typically designed to handle massive amounts of data by distributing it across many servers.

*/