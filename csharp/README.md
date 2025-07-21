# C# Learning Path

This document outlines a comprehensive learning path for C# programming and related technologies. The topics are categorized into six main sections, each containing essential subtopics crucial for building expertise in C# and .NET development.

*Last Updated: 09/12/2024*

## Table of Contents

1. Core Programming (C# Fundamentals)
2. API Development
3. Testing and Debugging
4. Data and SQL Management
5. Advanced Topics and Tools
6. Algorithms and Data Structures

## 1. Core Programming (C# Fundamentals)

- 1.1. Introducing C# and .NET
- 1.2. C# Language Basics
- 1.3. Creating Types in C#
- 1.4. Advanced C#
- 1.5. .NET Overview
- 1.6. .NET Fundamentals
- 1.7. Collections
- 1.8. LINQ Queries
- 1.9. LINQ Operators
- 1.10. LINQ to XML
- 1.11. Other XML and JSON Technologies
- 1.12. Disposal and Garbage Collection
- 1.13. Diagnostics
- 1.14. Concurrency and Asynchrony
- 1.15. Streams and I/O
- 1.16. Networking
- 1.17. Assemblies (basic info)
- 1.18. Reflection and Metadata
- 1.19. Dynamic Programming
- 1.20. Cryptography
- 1.21. Advanced Threading
- 1.22. Parallel Programming
- 1.23. Span\<T> and Memory\<T>
- 1.24. Native and COM Interoperability
- 1.25. Regular Expressions

## 2. API Development

- 2.1. RESTful Services with ASP.NET Core
- 2.2. Web API Design Principles (versioning, security, etc.)
- 2.3. Authentication and Authorization (JWT, OAuth)
- 2.4. gRPC and WebSockets

## 3. Testing and Debugging

- 3.1. Unit Testing with xUnit/NUnit/MSTest
- 3.2. Test-Driven Development (TDD)
- 3.3. Mocking and Dependency Injection for Testing
- 3.4. Integration and Functional Testing
- 3.5. Performance and Load Testing
- 3.6. Debugging Techniques and Tools
- 3.7. Selenium

## 4. Data and SQL Management

### 4.1. Database Fundamentals
- 4.1.1. Introduction to Databases
  - What is a database?
  - Relational vs. Non-relational databases
- 4.1.2. SQL Fundamentals
  - SELECT, FROM, WHERE
  - GROUP BY, HAVING, ORDER BY, DISTINCT
  - Joins (INNER, LEFT, RIGHT, FULL, CROSS, SELF)
  - Subqueries and Common Table Expressions (CTEs)
  - INSERT, UPDATE, DELETE, MERGE (UPSERT)
  - Transactions (BEGIN TRANSACTION, COMMIT, ROLLBACK, Savepoints)
  - Indexing (Clustered vs. Non-clustered, B-Tree structure, execution plans)
  - Stored Procedures, Functions, and Triggers
  - Views and Materialized Views
  - Cursors and Loops (performance considerations)

### 4.2. Advanced Database Concepts
- 4.2. ACID Principles
- 4.3. BASE Principles
- 4.4. SQL and NoSQL
- 4.5. SQL Committed and Uncommitted Messages
- 4.6. Materialized View
- 4.7. SQL Indexing Types and BTree
- 4.8. SQL Pessimistic and Optimistic Locking
- 4.9. Isolation Layers
- 4.10. Clustered and Non-clustered Index
- 4.11. Normalization and Denormalization
- 4.12. Database Migration Best Practices
- 4.13. Database Sharding vs Database Partitioning
- 4.14. Database Replication

## 5. Advanced Topics and Tools

- 5.1. Containers & Docker (Junior Level)
- 5.2. CI/CD Pipeline (Junior to Mid-Level)
- 5.3. Monitoring / Alerting / Logging (Junior to Mid-Level)
- 5.4. Microservices (Mid-Level)
- 5.5. Proxy / Reverse Proxy / Load Balancers (Mid-Level)
- 5.6. Distributed Systems / Designing High-Load Intensive Applications (Senior Level)
- 5.7. Kestrel (Optional, Relevant for .NET Developers)
- 5.8. Kubernetes (Optional, Advanced)
- 5.9. Cloud Platforms (AWS, Azure)
- 5.10. Message Brokers (RabbitMQ, Kafka)

## 6. Algorithms and Data Structures

- Arrays and Strings
- Sorting and Searching
- Linked Lists
- Stacks and Queues
- Recursion and Backtracking
- Trees
- Heaps and Priority Queues
- Graphs
- Dynamic Programming
- Greedy Algorithms

### 6.1 Arrays and Strings
#### 6.1.1 Theoretical Concepts:
- Array Basics: Memory layout, definition, and benefits of index-based access
- Types of Arrays:
  - Static Arrays: Fixed size, pros/cons, common applications
  - Dynamic Arrays: Array resizing and reallocation, ArrayList and memory efficiency
  - Jagged and Multidimensional Arrays: Structure and use cases

#### 6.1.2 Algorithms:
- Basic Algorithms:
  - Two-Pointer Technique: Efficient for pair finding and reversal
  - Array Reversal: Simple loop-based reversal
- Intermediate Algorithms:
  - Subarray Sum Problems: Including Kadane's Algorithm for min/max subarray sum
- Advanced Techniques:
  - Sliding Window Algorithms: Fixed and variable-sized windows for efficient searching
  - Prefix Sum Arrays: Useful for range-based calculations in arrays

### 6.2 Sorting and Searching
#### 6.2.1 Theoretical Concepts:
- Sorting Basics: Stability, time complexity, and in-place vs. out-of-place sorting
- Types of Sorting Algorithms:
  - Simple Sorting: Bubble Sort, Selection Sort, Insertion Sort
  - Divide and Conquer Sorting: Merge Sort and Quick Sort
  - Non-comparative Sorting: Counting Sort and Radix Sort, focusing on conditions for using these

#### 6.2.2 Algorithms:
- Sorting Implementations: Practice and analyze each sorting type, focusing on efficiency
- Searching Basics: Implement Linear Search and Binary Search with various edge cases
- Advanced Searching:
  - Binary Search Variations: Binary Search on Rotated Arrays
  - 2D Matrix Search: Efficient searching based on row/column constraints

### 6.3 Linked Lists
#### 6.3.1 Theoretical Concepts:
- Types of Linked Lists:
  - Singly Linked List: Node structure, creation, traversal
  - Doubly Linked List: Bidirectional pointers and additional memory use
  - Circular Linked List: Circular node connections and use cases
- Memory Allocation: Dynamic memory, pointers, and memory usage comparisons to arrays

#### 6.3.2 Algorithms:
- Basic Operations: Creation, insertion, deletion, and traversal
- Advanced Algorithms:
  - Linked List Reversal
  - Cycle Detection: Using Floyd's Cycle-Finding Algorithm
  - Merging Linked Lists: Merging sorted lists efficiently
  - Node Removal: Removing the N-th node from the end with a two-pointer technique

### 6.4 Stacks and Queues
#### 6.4.1 Theoretical Concepts:
- Stack: LIFO principle, uses, and common operations (push, pop, peek)
- Queue: FIFO principle, usage scenarios, and queue operations (enqueue, dequeue)
- Types of Queues: Circular Queue, Priority Queue, Deque (Double-ended Queue)
- Implementations: Array vs. Linked List implementations

#### 6.4.2 Algorithms:
- Stack Algorithms:
  - Balanced Parentheses Validation
  - Monotonic Stack: Tracking min/max for optimization
- Queue Algorithms:
  - Queue/Stack Implementations: Using two queues or stacks
  - Sliding Window Maximum: Using deque for efficient window tracking

### 6.5 Recursion and Backtracking
#### 6.5.1 Theoretical Concepts:
- Recursion Fundamentals: Base and recursive cases, recursion depth
- Stack Frames: Understanding recursion stacks
- Backtracking: Incremental solution building, pruning, and solution space exploration

#### 6.5.2 Algorithms:
- Recursion Problems: Factorial, Fibonacci, power functions
- Backtracking Algorithms:
  - Subsets and Permutations
  - N-Queens Problem
  - Sudoku Solver
  - Word Search in 2D Matrices

### 6.6 Trees
#### 6.6.1 Theoretical Concepts:
- Tree Basics: Nodes, roots, leaves, height, and binary tree properties
- Binary Search Trees (BST): Properties and advantages in searching
- Balanced Trees: AVL, Red-Black Trees, and maintaining balance

#### 6.6.2 Algorithms:
- Tree Traversals: Inorder, Preorder, Postorder
- Lowest Common Ancestor (LCA)
- Binary Tree to Linked List: Converting a binary tree to a doubly linked list
- Tries: Insert, search, and prefix search

### 6.7 Heaps and Priority Queues
#### 6.7.1 Theoretical Concepts:
- Heap Properties: Min-Heap and Max-Heap, array-based storage
- Priority Queues: Applications, implementation, and how they differ from queues

#### 6.7.2 Algorithms:
- Heap Sort: Sorting using heaps
- K-th Largest Element
- Median in a Stream: Efficient tracking of median using heaps

### 6.8 Graphs
#### 6.8.1 Theoretical Concepts:
- Graph Basics: Vertices, edges, types of graphs (directed, undirected, weighted)
- Graph Representations: Adjacency lists, matrices, edge lists
- Types of Graphs: Weighted, unweighted, cyclic, acyclic

#### 6.8.2 Algorithms:
- Graph Traversals: Depth-First Search (DFS), Breadth-First Search (BFS)
- Cycle Detection: Detect cycles in both directed and undirected graphs
- Shortest Path: Dijkstra's and Bellman-Ford
- Minimum Spanning Tree: Kruskal's and Prim's algorithms

### 6.9 Dynamic Programming (DP)
#### 6.9.1 Theoretical Concepts:
- DP Basics: Identifying overlapping subproblems, optimal substructure
- Memoization vs. Tabulation: Differences and scenarios for each

#### 6.9.2 Algorithms:
- DP Problems:
  - Longest Increasing Subsequence (LIS)
  - Knapsack Problem (0/1 and Unbounded)
  - Edit Distance Problem
  - Coin Change Problem

### 6.10 Greedy Algorithms
#### 6.10.1 Theoretical Concepts:
- Greedy Choice Property: Making optimal choices at each stage
- Comparing Greedy and DP: When each method is effective

#### 6.10.2 Algorithms:
- Activity Selection Problem
- Huffman Encoding: Compression and prefix-free code
- Minimum Platforms: Optimizing for limited resources

## Motivation

> When you feel exhausted, remember why you started. You didn't choose to learn programming because it was easy. You chose it because it challenges you, pushes you, and gives you the power to create and solve problems in ways many can't. Every line of code, every bug fixed, and every problem solved is one step closer to mastery.
>
> Tiredness is temporary, but the skills you're building are permanent. Every late-night session and frustrating error message is an investment in your future self. A version of you that's more skilled, more confident, and more capable of taking on any challenge that comes your way. Remember, even the greatest developers were once where you are now, battling through tiredness and self-doubt. They kept pushing, and so can you.
>
> There is no growth in comfort. When you feel tired, remember that pushing through that fatigue is what separates the average from the exceptional. You're building something amazing within yourself, even if it feels slow, even if it feels hard. Every single time you persist, you're developing not just your coding skills, but your resilience and that's a skill no one can take from you.
>
> You have the power to change your trajectory-your hard work today is the gateway to a better tomorrow. Imagine looking back a year from now and realizing that despite how tired you felt, you kept going, and you made it further than you ever thought possible.
>
> Programming is a superpower, and you're already on the path to wielding it. Rest if you need to, but never stop moving forward. Your future self will thank you for it.
