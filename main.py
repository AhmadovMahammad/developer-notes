import re
import math
from leetcode import Solution

# from leetcode import ListNode


# firstName = "mahammad"
# print(f"hello, {firstName}")

# print("hello", "i'm", 21, "years", "old", end=" end of text\n")
# print("hello", "i'm", 21, "years", "old", sep=" ", end="\n")

# hello = "hello"
# world = "world"
# world = hello
# hello = "updated..."

# print(hello, world)

# name = input("enter your name: ")
# age = input("enter your age: ")
# print(f"hello, {name}. you are {age} years old.")

# x = 10
# y = 5.5

# result = x + y
# print(result)

# result = x - y
# print(result)

# result = x * y
# print(result)

# result = int(x / y)
# print(result)

# result = x**y
# print(result)

# result = x // y
# print(result)

# result = x % y
# print(result)


# num = input("number: ")


# 1. isdigit()
# Purpose: Returns True if all characters in the string are digits (0-9).

# if num.isdigit():
#     print(int(num) - 5)
# else:
#     print("exception: input is not a number.")

# "123".isdigit()   # True
# "-123".isdigit()  # False
# "3.14".isdigit()  # False
# "hello".isdigit() # False


# 2. isnumeric()
# Purpose: Similar to isdigit(),
# but it also returns True for numeric characters like fractions or superscripts.

# The isnumeric() method in Python can return True for characters that represent numbers,
# including fractions, superscripts, or other special numeric characters, not just the digits 0-9.
# This is different from isdigit(), which only returns True for digits.

# However, the isnumeric() method does not work with numbers like -3.14 or other floating-point numbers,
# because those include the negative sign (-) and the decimal point (.),
# which are not considered numeric characters by isnumeric().

# "123".isnumeric()  # True
# "ⅩⅩⅩ".isnumeric()  # True (Roman numerals)
# "⅓".isnumeric()  # True (fraction)

# "-3.14".isnumeric()  # False (includes "-" and ".")
# "3.14".isnumeric()  # False (includes ".")


# 3. isspace()
# Purpose: Returns True if the string contains only whitespace characters (spaces, tabs, etc.).

# "   ".isspace()  # True
# "abc".isspace()  # False
# print(f"string 'hello  ' contains only whitespace: {'hello   '.isspace()}")   # False


# 4. isalpha()
# Purpose: Returns True if all characters in the string are alphabetic (A-Z, a-z).

# "abc".isalpha()  # True
# "abc123".isalpha()  # False


# 5. isalnum()
# Purpose: Returns True if all characters in the string are alphanumeric (letters or digits).

# "abc123".isalnum()  # True
# "abc!".isalnum()  # False


# 6. startswith()
# Purpose: Returns True if the string starts with the specified prefix.

# "hello".startswith("he")  # True
# "hello".startswith("lo")  # False


# 7. endswith()
# Purpose: Returns True if the string ends with the specified suffix.

# "hello".endswith("lo")  # True
# "hello".endswith("he")  # False


# 8. strip() lstript() rstrip()
# Purpose: Removes leading and trailing whitespace characters.

# print("  hello  ".strip())  # "hello"
# print("  hello  ".rstrip())  # "   hello"
# print("  hello  ".lstrip())  # "hello   "


# 9. replace()
# Purpose: Replaces a specified substring with another substring.

# print("hello world".replace("world", "Python"))  # "hello Python"
# print("hello world".replace("world---", "Python"))  # "hello world"


# 10. split()
# Purpose: Splits the string into a list of substrings
# based on a specified delimiter (default is any whitespace).

# print("hello world".split())  # ['hello', 'world']
# print("apple,banana,orange".split(","))  # ['apple', 'banana', 'orange']
# print(
#     "apple,banana,orange,apple,banana,orange,apple,banana,orange".split(",", 2)
# )  # ['apple', 'banana', 'orange,apple,banana,orange,apple,banana,orange']


# 11. join()
# Purpose: Joins elements of an iterable (like a list) into a single string with a specified separator.

# print("".join(["apple", "banana", "cherry"]))  # "apple-banana-cherry"


# 12. upper() lower() title()
# print("hello, i am mahammad".upper())  # HELLO, I AM MAHAMMAD
# print("HELLO, I AM MAHAMMAD".lower())  # hello, i am mahammad
# print("hello, i am mahammad".title())  # Hello, I Am Mahammad

# 13. capitalize()

# print("i'm 21 years old".capitalize())  # I'm 21 years old

# ---------------------------------------
# if re.fullmatch(r"-?\d+(\.\d+)?", num):
#     print(float(num) - 5)
# else:
#     print("exception: input is not a number.")

# print(
#     """
# hello,
# it is
# multiline
# text
#       """
# )


# name = "mahammad"
# print(len(name))  # 8
# print(name[1])  # a
# print(name[-1])  # d
# print(name[0:3])  # mah
# print(name[0:])  # mahammad
# print(name[:5])  # maham
# print(name[:])  # mahammad


# name = "mahammad"
# name[0] = "test"  # errror because strings are immutable

# surname = "ahmadov"
# print(name + " " + surname)
# print(f"{name} {surname}")
# print(f"{len(name)}:{4+4+0}")
# print(name.find("m"))  # 0
# print(name.find("m1"))  # -1
# print("mah" in name)  # True
# print("mah" not in name)  # False


# print(round(2.4))  # 2
# print(round(2.61212, 2))  # 2.61
# print(abs(-212))  # 212
# print(math.ceil(2.1))

# squares = ["a", "A"]
# for val in squares:
#     print(ord(val))


# temperature = int(input("enter a temperature: "))

# if temperature > 30:
#     message = "Drink water"
# elif temperature > 20:
#     message = "It's nice"
# else:
#     message = "It's cold"
# print("Done.")

# message = (
#     "Drink water"
#     if temperature > 30
#     else "It's nice" if temperature > 20 else "It's cold"
# )

# print(message)

# high_income = False
# good_credit = True
# student = False

# if (high_income or good_credit) and not student:
#     print("Eligible")
# else:
#     print("Not eligible")

# chaining comparison operators
# age = 21

# if age >= 20 and age < 30:
# if 20 <= age < 30:
#     print("eligible")


# for i in range(3):
#     num = i + 1
#     print(f"attempt: {num} {num * '.'}")

# attempt: 1 .
# attempt: 2 ..
# attempt: 3 ...


# for i in range(1, 4):
#     print(f"attempt: {i} {i * '.'}")

# attempt: 1 .
# attempt: 2 ..
# attempt: 3 ...


# for i in range(1, 10, 2):
#     print(f"attempt: {i} {i * '.'}")

# attempt: 1 .
# attempt: 3 ...
# attempt: 5 .....
# attempt: 7 .......
# attempt: 9 .........


# In Python, a for loop can have an else block,
# which is executed only if the loop completes normally (i.e., it does not break early).

# for number in range(3):
#     print("Attempt", number)
#     if number == 1:
#         print("Successful")
#         break  # Loop exits early
# else:
#     print("Loop completed without break")

# Attempt 0
# Attempt 1
# Successful


# for number in range(3):
#     print("Attempt", number)
# else:
#     print("Loop completed without break")

# Attempt 0
# Attempt 1
# Attempt 2
# Loop completed without break

# The for-else structure is useful when searching for something.
# If we find the item, we break; if we don't find it, the else block executes.

# search_list = [1, 2, 3, 4, 7, 8, 9, 10]
# target = 5

# for num in search_list:
#     if num == target:
#         print("found", num)
#         break
# else:
#     print("target not found")


# for x in range(10):
#     for y in range(10):
#         print(f"({x},{y})", end=" ")
#     print("\n")


# num = 100
# while num > 0:
#     print(num)
#     num //= 2


# def greet_people():
#     print("hello guys")


# greet_people()


# def greet(first_name: str, last_name: str):
#     print(f"name: {first_name}. surname: {last_name}")


# greet("mahammad", "ahmadov")


# Python File Handling (Working with Files)
# In Python, we use the open() function to work with files.
# Here’s a clear explanation with all file modes, including reading and writing, and a comparison with C#.

# 1. Opening a File in Python

# file = open("c:\Users\mahammada\Documents\schedule.txt", mode="r")  # Opens the file in read mode
# open("filename", mode) → Opens the file with a specific mode.
# You must close the file after using it:

# File Modes (Understanding the Letters)

# Mode	    Meaning	                            Creates File?	    Overwrites?
# "r"	    Read (default)	                    ❌ No	           ❌ No
# "w"	    Write (erases content)	            ✅ Yes	           ✅ Yes
# "a"	    Append (adds content)	            ✅ Yes	           ❌ No
# "x"	    Exclusive create (fails if exists)	✅ Yes	           ❌ No
# "r+"	    Read & Write (no erase)	            ❌ No	           ❌ No
# "w+"	    Write & Read (erases content)	    ✅ Yes	           ✅ Yes
# "a+"	    Append & Read	                    ✅ Yes	           ❌ No
# "b"	    Binary mode (rb, wb, etc.)	        ✅ Yes	           ✅ Yes/No


# 2. Reading a File (r Mode)
# with open(r"C:\Users\mahammada\Documents\blazor documentaion ch5.txt", "r") as file:
#     content = file.read()
#     print(content)

# with open(...) automatically closes the file after use.


# Reading Line by Line
# with open(r"C:\Users\mahammada\Documents\blazor documentaion ch5.txt", "r") as file:
#     for line in file:
#         print(line.strip())  # strip() removes newline characters


# Reading a Specific Number of Characters
# with open(r"C:\Users\mahammada\Documents\blazor documentaion ch5.txt", "r") as file:

#     print(file.read(100))  # Reads only 100 characters
#     file.seek(0)

#     content = file.read()
#     print(content[:100])


# def increment(
#     num,
#     another,
#     by=1,
# ):
#     return num + by


# print(increment(num=5))


# def sum(*nums):
#     total = 0
#     for num in nums:
#         total += num
#     return total


# print(sum(1, 2, 3, 4))


# list = [1, "i", "am", 21.5, "years", "old", "true"]
# for item in list:
#     print(item)

# for index in range(len(list)):
#     print(list[index])

# list.append("calling append...")
# list.extend(range(5))

# for item in list:
#     print(item)

# while list:
#     popped = list.pop()
#     print(f"popped: {popped}")


# Python's ternary syntax follows this structure:
# value_if_true if condition else value_if_false

# For multiple conditions:
# value1 if condition1 else value2 if condition2 else value3


# Walrus Operator (:=) in Python
# The walrus operator (:=) is called the "assignment expression" in Python.
# It allows you to assign a value to a variable inside an expression
# (like in an if or while statement).


# Syntax & Meaning
# variable := expression

# := assigns the value of expression to variable.
# The whole expression evaluates to the assigned value.


# Example: Without Walrus
# Let's say we want to read user input and check if it's not "exit" before printing it.

# user_input = input("Enter something: ")
# while user_input != "exit":
#     print("You entered:", user_input)
#     user_input = input("Enter something: ")  # Repeating assignment

# We repeat input() twice.


# Example: With Walrus
# Now, let's remove repetition using :=:'

# while (user_input := input("Enter something: ")) != "exit":
#     print(f"you entered: {user_input}")

# list = [1, "i", "am", 21.5, "years", "old", "true"]
# list.append("calling append...")
# list.extend(range(5))

# while (popped := list.pop() if list else None) != None:
#     print(f"popped element: {popped}")


# some important list operations
# numbers = [10, 20, 30, 40, 50, 10]

# ---

# 1. List Slicing (Getting Subsets of a List)

# print(numbers[1:4])  # [20, 30, 40] (start index 1, end index 4 - 1)
# print(numbers[:3])  # [10, 20, 30] (start from 0, end at 3-1)
# print(numbers[2:])  # [30, 40, 50] (start from 2, go till end)
# print(numbers[-2:])  # [40, 50] (last 2 elements)

# Slicing helps extract parts of a list.

# ---

# 2. Inserting Elements (insert)

# numbers.insert(2, 25)  # Insert 25 at index 2
# print(numbers)  # [10, 20, 25, 30, 40, 50]

# ---

# 3. Extending a List (extend)

# list1 = [1, 2, 3]
# list2 = [4, 5, 6]
# list1.extend(list2)  # Adds elements of list2 to list1
# numbers.extend(list1)

# print(numbers)  # [10, 20, 30, 40, 50, 1, 2, 3, 4, 5, 6]
# Use extend() instead of append() when adding multiple items.

# ---

# 4. Removing Elements

# print(numbers)

# num_to_remove = 10
# if num_to_remove in numbers:
#     numbers.remove(num_to_remove)  # Removes first occurrence of 10
#     print(f"deleted. list: {numbers}")
# else:
#     print(f"{num_to_remove} not found")

# -

# del numbers[1]  # Deletes item at index 1
# print(numbers)  # [10, 30, 40, 50, 10]

# ---

# 5. Sorting & Reversing

# numbers.sort()  # Sorts in ascending order
# print(numbers)  # [10, 10, 20, 30, 40, 50]
#
# numbers.sort(reverse=True)  # Sorts in descending order
# print(numbers)  # [50, 40, 30, 20, 10, 10]
#
# numbers.reverse()  # Reverses the list
# print(numbers)  # [10, 10, 20, 30, 40, 50]

# ---

# 6. Checking for an Item (in Operator)

# nums = [1, 2, 3, 4]
# print(3 in nums)  # True
# print(5 in nums)  # False

# ---

# 7. Getting Index of an Item (index)

# fruits = ["apple", "banana", "cherry"]

# print(fruits.index("apple"))  # 0
# print(fruits.index("not known fruit"))  # throws exception if item does not exists
# in string find method, if char does not exists, it returns just -1

# ---

# 8️. Counting Occurrences (count)

# nums = [1, 2, 2, 3, 2]
# print(nums.count(2))  # 3 (2 appears 3 times)

# ---

# 9. Clearing a List (clear)

# nums.clear()
# print(nums)  # []


# nums = [1, 2, 3, 4, 5]

# for num in nums:
#     print(num)

# The enumerate() function in Python is used to loop over a list (or any iterable)
# and get both the index and the value of each item.
# This is especially useful when you need both the index (position) and the item itself
# while iterating over the list.

# What does enumerate() return?
# enumerate() returns an iterator that produces pairs of the form (index, value).

# Each time you iterate over the enumerate object, it yields a tuple containing:
# The index (starting from 0 by default).
# The value from the iterable.

# Syntax of enumerate()
# enumerate(iterable, start=0)

# iterable: The iterable you want to loop over (e.g., a list, string, etc.).
# start: The starting index (optional). By default, it starts at 0,
# but you can specify a different value if needed.

# my_list = ["apple", "banana", "cherry"]

# Using enumerate in a for loop
# for index, value in enumerate(my_list):
#     print(f"Index {index}: {value}")

# Index 0: apple
# Index 1: banana
# Index 2: cherry

# Starting the Index from a Different Value
# You can specify a different starting index by passing the start parameter to enumerate().

# Starting index from 1
# for index, value in enumerate(my_list, start=1):
#     print(f"Index {index}: {value}")

# Index 1: apple
# Index 2: banana
# Index 3: cherry


# Why use enumerate()?

# Avoid manually tracking the index:
# When you need both the index and the value of the items in the iterable,
# enumerate() makes it easier without manually managing an index variable.

# Cleaner and more Pythonic code:
# It makes your code more concise and easier to read,
# avoiding the need for a separate index counter.


# Using enumerate() with a List and Modifying Items
# You can also use enumerate() if you want to modify elements of the list using the index:

# my_list = ["apple", "banana", "cherry"]

# for index, value in enumerate(my_list):
#     my_list[index] = value.upper()

# print(my_list)  # ['APPLE', 'BANANA', 'CHERRY']

# print(my_list[2:0:-1])
# print(my_list[2:-1:-1])

# print(my_list[-1::-1])

# print(my_list[::-2])

# --------------------

# What is a Set?

# A set is a collection data type in Python that is unordered and does not allow duplicates.
# It is similar to a mathematical set, where you only store unique items.

# Key Characteristics of Sets:

#  1. Unordered: The elements in a set are not stored in any specific order,
#     meaning you can't rely on their position.

#  2. Unique elements: A set automatically removes duplicate values, so every element in a set is unique.
#  3. Mutable: You can add and remove elements from a set after it's created.
#  4. No indexing: Since sets are unordered, you cannot access elements using an index like you would with lists.


# Creating a Set
# You can create a set using curly braces {} or the set() function.

# Using curly braces
# my_set = {1, 2, 3, 4}

# Using set() function
# another_set = set([1, 2, 3, 4])

# Both methods create a set with the elements 1, 2, 3, 4.
# If there are any duplicates in the input, only one instance of each element will remain.


# Set Operations
# Here are some common operations you can perform with sets:
# my_set = {1, 2, 3}


# 1. Adding elements
# You can add elements to a set using the add() method.

# my_set.add(4)  # Adds 4 to the set
# my_set.add(1)  # Ignores adding 1 to the set
# print(my_set)  # Output: {1, 2, 3, 4}


# 2. Removing elements
# You can remove elements using the remove() method,
# which will raise an error if the element does not exist.

# my_set.remove(3)  # Removes 3 from the set
# print(my_set)  # Output: {1, 2}

# --- If you want to remove an element without raising an error, you can use discard():

# my_set.discard(5)  # Does nothing since 5 is not in the set
# print(my_set)  # Output: {1, 2, 3}


# 3. Set Union
# The union of two sets combines all the elements from both sets, removing duplicates.
# You can do this using the | operator or the union() method.

# set_a = {1, 2, 3}
# set_b = {3, 4, 5}

# union_set = set_a | set_b  # Union using operator
# print(union_set)  # Output: {1, 2, 3, 4, 5}
#
# union_set_method = set_a.union(set_b)  # Union using method
# print(union_set_method)  # Output: {1, 2, 3, 4, 5}


# 4. Set Intersection
# The intersection of two sets gives you a new set with only the elements that are present in both sets.
# You can use the & operator or the intersection() method.

# intersection_set = set_a & set_b  # Intersection using operator
# print(intersection_set)  # Output: {3}
#
# intersection_set_method = set_a.intersection(set_b)  # Intersection using method
# print(intersection_set_method)  # Output: {3}


# 5. Set Difference
# The difference of two sets returns a new set with elements that are in the first set but not in the second.
# You can use the - operator or the difference() method.

# difference_set = set_a - set_b  # Difference using operator
# print(difference_set)  # Output: {1, 2}
#
# difference_set_method = set_a.difference(set_b)  # Difference using method
# print(difference_set_method)  # Output: {1, 2}


# 6. Set Symmetric Difference
# The symmetric difference of two sets gives you a new set with elements that are in either of the sets,
# but not in both.
# You can use the ^ operator or the symmetric_difference() method.

# symmetric_difference_set = set_a ^ set_b  # Symmetric Difference using operator
# print(symmetric_difference_set)  # Output: {1, 2, 4, 5}
#
# symmetric_difference_set_method = set_a.symmetric_difference(set_b)  # Symmetric Difference using method
# print(symmetric_difference_set_method)  # Output: {1, 2, 4, 5}


# Set Properties
#
# Sets are unordered: This means the elements inside the set don’t have a guaranteed order.
# Sets do not allow duplicates: If you try to add an item that already exists in the set,
# it will not be added again.

# my_set = {1, 2, 3, 1, 2}
# print(my_set)  # Output: {1, 2, 3}

# --------------------

# What is a Dictionary?
# A dictionary is an unordered collection of items in Python, where each item consists of a key-value pair.
# It is similar to a real-world dictionary where you have a word (the key) and its definition (the value).

# Key Characteristics of Dictionaries:
#  1. Unordered: The items are stored in no particular order.
#     However, starting from Python 3.7, dictionaries maintain the insertion order
#     (the order in which items were added).
#  2. Key-Value Pairs: Each item in a dictionary is a pair where the key is unique,
#     and the value can be any data type.
#  3. Mutable: You can change, add, or remove items from a dictionary after it’s created.
#  4. Keys are Unique: You cannot have duplicate keys in a dictionary.
#     If you try to insert a new value for an existing key, the old value will be overwritten.
#  5. Unhashable Keys: Keys must be of a type that is immutable and hashable (e.g., strings, integers, tuples).
#     You cannot use mutable types like lists as dictionary keys.


# Creating a Dictionary
# You can create a dictionary using curly braces {} with key-value pairs, separated by a colon :.
# Alternatively, you can use the dict() constructor.

# Using curly braces
# my_dict = {
#     "name": "mahammad",
#     "surname": "ahmadov",
#     "age": 21,
# }

# Using dict() function
# another_dict = dict(name="mahammad", surname="ahmadov", age=21)


# Accessing Items in a Dictionary
# You can access dictionary values by referring to the key inside square brackets [] or using the get() method.

# Using square brackets
# print(my_dict["name"])  # Output: mahammad

# Using get() method
# print(my_dict.get("age"))  # Output: 21


# NOTE:
# The get() method is useful because it returns None (or a default value you provide) if the key doesn’t exist,
# instead of raising an error.

# print(my_dict.get("salary", "not found"))


# Adding Items to a Dictionary
# You can add a new key-value pair to a dictionary by simply assigning a value to a new key.

# my_dict["salary"] = 10_000
# print(my_dict)
# Output: {'name': 'mahammad', 'surname': 'ahmadov', 'age': 21, 'salary': 10000}


# Modifying Items in a Dictionary
# You can modify an existing value by using its key.

# my_dict["age"] = 20 + 1


# Removing Items from a Dictionary
# There are several ways to remove items from a dictionary:

# 1. Using del: This will remove a key-value pair by key.
# del my_dict["age"]
# print(my_dict)  # {'name': 'mahammad', 'surname': 'ahmadov'}

# 2. Using pop(): This method removes the key-value pair by key and returns the value.
# value = my_dict.pop("age")
# print(value)  # Output: 21
# print(my_dict)  # Output: {'name': 'mahammad', 'surname': ahmadov}

# 3. Using popitem(): This removes the last inserted key-value pair (from Python 3.7+) and returns it.
# item = my_dict.popitem()
# print(item)  # Output: ('age', 21)


# Checking for Keys or Values
# You can check if a key or value exists in a dictionary using the in keyword.

# Checking if a key exists
# print("name" in my_dict)  # Output: True
# print("address" in my_dict)  # Output: False
#
# Checking if a value exists
# print(30 in my_dict.values())  # Output: False
# print("mahammad" in my_dict.values())  # Output: True


# Dictionary Methods
# Here are some commonly used dictionary methods:

# print(my_dict.keys())  # Output: dict_keys(['name', 'salary'])
# print(my_dict.values())  # Output: dict_values(['Alice', 50000])
# print(my_dict.items())  # Output: dict_items([('name', 'Alice'), ('salary', 50000)])
# another_dict = {"age": 32, "city": "Los Angeles"}
# my_dict.update(another_dict)
# print(
#     my_dict
# )  # Output: {'name': 'mahammad', 'surname': 'ahmadov', 'age': 32, 'city': 'Los Angeles'}


# for key, value in my_dict.items():
#     print(f"Key: {key}, Value: {value}")

# --------------------------------
# 1. Understanding List Comprehensions
# A list comprehension is a more concise way to create lists compared to using loops.

# Basic Structure:
# [expression for item in iterable]


# Example 1: Creating a List of Squares
# squares = [square**2 for square in range(10)]
# print(squares)  # [0, 1, 4, 9, 16, 25, 36, 49, 64, 81]

# Equivalent loop:
# squares = []
# for x in range(10):
#     squares.append(x**2)


# 2. Adding Conditions (Filtering)
# You can add an if condition inside a list comprehension.

# Example 2: Even Numbers from 0 to 20
# even_nums = [num for num in range(21) if num % 2 == 0]
# print(even_nums)  # [0, 2, 4, 6, 8, 10, 12, 14, 16, 18, 20]

# Equivalent loop:
# evens = []
# for x in range(21):
#     if x % 2 == 0:
#         evens.append(x)


# 3. Nested Loops in Comprehensions
# You can use multiple loops inside a list comprehension.

# Example 3: Generating (x, y) pairs
# pairs = [(x, y) for x in range(3) for y in range(3)]
# print(pairs)

# for index, value in pairs:
# print(f"Index: {index} = {value}")  # list of tuples

# [(0, 0), (0, 1), (0, 2), (1, 0), (1, 1), (1, 2), (2, 0), (2, 1), (2, 2)]
# Index: 0 = 0
# Index: 0 = 1
# Index: 0 = 2
# Index: 1 = 0
# Index: 1 = 1
# Index: 1 = 2
# Index: 2 = 0
# Index: 2 = 1
# Index: 2 = 2


# 4. Using if-else in List Comprehensions
# You can include if-else statements inside list comprehensions.

# labels = ["even" if x % 2 == 0 else "odd" for x in range(20)]
# print(labels)

# ['even', 'odd', 'even', 'odd', 'even', 'odd', 'even', 'odd', 'even', 'odd', 'even', 'odd', 'even', 'odd', 'even', 'odd', 'even', 'odd', 'even', 'odd']


# Equivalent loop:
# labels = []
# for x in range(10):
#     if x % 2 == 0:
#         labels.append("Even")
#     else:
#         labels.append("Odd")


# 5. Multi-Dimensional Lists (Nested List Comprehensions)
# You can generate 2D lists using comprehensions.

# Example 5: Creating a 3×3 Matrix Filled with Zeros
# matrix = [[0 for _ in range(3)] for _ in range(3)]
# print(matrix)

# matrix[1][2] = 31
# print(matrix[1][2])  # Accessing row index 1, column index 2

# Equivalent loop:
# matrix = []
# for _ in range(3):
#     row = []
#     for _ in range(3):
#         row.append(0)
#     matrix.append(row)


# 6. Dictionary Comprehensions
# You can create dictionaries using comprehensions.

# squares_dict = {x: x**2 for x in range(5)}
# print(squares_dict)

# Equivalent loop:
# squares_dict = {}
# for x in range(5):
#     squares_dict[x] = x**2

# Output: {0: 0, 1: 1, 2: 4, 3: 9, 4: 16}

# -------------------------------------------------------

# Understanding *args and **kwargs in Python
# In Python, *args and **kwargs allow you to pass a variable number of arguments to a function.


# 1. *args (Non-Keyword Arguments)
#
# *args allows you to send any number of positional arguments to the function.
# Inside the function, args is treated as a tuple.


# def add_numbers(*args):
#     print(args)  # tuple containing all the arguments
#     return sum(args)  # sum all numbers


# print(add_numbers(1, 2, 3, 4, 5))  # Output: 15

# (1, 2, 3, 4, 5)
# 15


# Iterating Over *args

# def print_names(*args):
#     for name in args:
#         print(name)


# print_names("mahammad", "ahmadov")
# Here, 'mahammad' and 'ahmadov' are stored in names as a tuple.


# 2. **kwargs (Keyword Arguments)
#
# **kwargs allows you to send any number of keyword arguments (key=value) to the function.
# Inside the function, kwargs is treated as a dictionary.

# def print_info(**kwargs):
#     print(kwargs)
#     for key, value in kwargs.items():
#         print(f"{key}: {value}")


# print_info(name="mahammad", age=21)

# Output:
# {'name': 'mahammad', 'age': 21}
# name: mahammad
# age: 21


# 3. Unpacking With * and **
# * and ** can also unpack lists and dictionaries when calling a function.

# 3.1. Unpacking Lists into *args

# def add_numbers(a, b, c):
#     return a + b + c


# numbers = [1, 2, 3]
# print(add_numbers(*numbers))  # Output: 6


# 3.2. Unpacking Dictionaries into **kwargs

# def show_info(name, age, city):
#     print(f"{name} is {age} years old and lives in {city}")


# person = {"name": "mahammad", "age": 21, "city": "sumgait"}
# show_info(**person)

# ------------------------------------------------------------

# In Python, exceptions are a mechanism for handling errors and other exceptional situations
# that can occur during the execution of a program.
# Exceptions allow you to catch and handle errors in a controlled way,
# preventing your program from crashing abruptly.


# Basic Structure of Exceptions
# Python uses a try...except block to handle exceptions. Here's a simple structure:

# try:
#     # Code that might raise an exception
#     x = 1 / 0  # This will raise a ZeroDivisionError
# # except Exception as e:
# except ZeroDivisionError as e:
#     # Code that runs if the exception occurs
#     print(f"Error: {e}")


# Components of Exception Handling

# try block: The code inside the try block is executed. If an error (exception) occurs,
# the rest of the code in the try block is skipped, and Python moves to the except block.

# except block: If an error occurs in the try block,
# the program will jump to the corresponding except block to handle the error.

# else block: (Optional) Code inside the else block is executed if no exceptions are raised in the try block.

# finally block: (Optional) Code in the finally block runs no matter what,
# whether an exception was raised or not.
# It's typically used for cleanup, like closing files or releasing resources.

# try:
#     num = int(input("enter a number: "))
#     result = num / 0
# except ValueError as ve:
#     print(f"you should enter a valid number: {ve}")
# except ZeroDivisionError as ze:
#     print(f"you can't divide to zero: {ze}")
# else:
#     print("code finished without error.")
# finally:
#     print("execution complete")


# Catching Multiple Exceptions
# You can catch multiple exceptions in the same try block:

# try:
#     # Some code that may raise different exceptions
#     num = int(input("Enter a number: "))
#     result = 10 / num
# except (ValueError, ZeroDivisionError) as e:
#     print(f"Error: {e}")
# else:
#     print(f"Result: {round(result, 2)}")


# Raising Exceptions
# You can also raise exceptions manually using the raise keyword.
# This is useful for custom error handling.

# def divide(x, y):
#     if y == 0:
#         raise ZeroDivisionError("you can't divide by zero!")
#     return x / y


# try:
#     print(divide(10, 0))
# except ZeroDivisionError as e:
#     print(f"Error: {e}")

# ---------------------------------------

# 1. Classes in Python
# A class is a blueprint for creating objects (instances).
# It defines the properties (variables) and behaviors (methods) that the objects created from the class will have.
# You can think of a class as a template, and each object is an instance of that class.


# Basic Syntax of a Class:
# class MyClass:
#     # Constructor method (optional)
#     def __init__(self, param1, param2):
#         self.param1 = param1  # Instance variable
#         self.param2 = param2  # Instance variable

#     # Method (function defined inside a class)
#     def some_method(self):
#         print(f"Values are: {self.param1} and {self.param2}")


# self: In every method within a class, the first parameter is typically self.
# It refers to the instance of the object itself.
# When you call a method on an object,
# Python automatically passes the object as the first argument to the method.


# Creating an Object (Instance) of the Class:
# class Person:
#     def __init__(self, name, age):
#         self.name = name
#         self.age = age

#     def introduce(self):
#         print(f"Hi, I am {self.name} and I am {self.age} years old.")


# me = Person("mahammad", 21)
# me.introduce()


# Accessing and Modifying Object Attributes:


# class Car:
#     def __init__(self, make, model, year):
#         self.make = make
#         self.model = model
#         self.year = year

#     def display_info(self):
#         print(f"Car info: {self.year} {self.make} {self.model}")


# car1 = Car("Toyota", "Camry", 2021)
# car1.display_info()  # Output: Car info: 2021 Toyota Camry
# print(car1.make)  # Output: Toyota

# # Modifying an attribute
# car1.year = 2022
# print(car1.year)  # Output: 2022


# 2. Dunder Methods (Special Methods)
# Now that you understand the basic concept of classes, let's discuss the dunder methods
# (methods that begin and end with double underscores).

# These are special methods that Python automatically calls under specific circumstances,
# and they allow you to define the behavior of your objects in certain situations.


# 1. __init__ (Constructor)
# Purpose: Initializes an object's state when it is created. It is called automatically when an object is instantiated.


# class Dog:
#     def __init__(self, name, breed):
#         self.name = name
#         self.breed = breed


# dog = Dog("Rex", "German Shepherd")
# print(dog.name)  # Output: Rex
# print(dog.breed)  # Output: German Shepherd


# 2. __str__ (String Representation)
# Purpose: This method defines how an object should be represented as a string when it is printed.
# It is used by the print() function or str() on an object.


# class Person:
#     def __init__(self, name, age):
#         self.name = name
#         self.age = age

#     def __str__(self):
#         return f"Person(Name: {self.name}, Age: {self.age})"


# person = Person("mahammad", 21)
# print(person)  # Output: Person(Name: mahammad, Age: 21)


# 3. __len__ (Length of Object)
# Purpose: Used by the len() function to get the length of the object.


# class MyList:
#     def __init__(self, data):
#         self.data = data

#     def __len__(self):
#         return len(self.data)


# my_list = MyList([1, 2, 3, 4])
# print(len(my_list))  # Output: 4

# for num in my_list.data:
#     print(num)


# 4. __add__ (Addition)
# Purpose: This method is called when the + operator is used between objects.
# It allows you to define custom addition behavior for your objects.


# class Point:
#     def __init__(self, x, y):
#         self.x = x
#         self.y = y

#     def __add__(self, other):
#         return Point(self.x + other.x, self.y + other.y)


# point1 = Point(2, 3)
# point2 = Point(1, 1)
# point3 = point1 + point2  # Calls __add__
# print(point3.x, point3.y)  # Output: 3 4


# 5. __getitem__ (Indexing)
# Purpose: This method allows you to define custom behavior for index-based access,
# similar to how you access elements in a list.


# class MyList:
#     def __init__(self, data):
#         self.data = data

#     def __getitem__(self, index):
#         return self.data[index]


# my_list = MyList([10, 20, 30])
# print(my_list[1])  # Output: 20


# 6. __setitem__ (Index Assignment)
# Purpose: This method allows you to define custom behavior for assignment to indexed elements,
# such as obj[index] = value.


# class MyList:
#     def __init__(self, data):
#         self.data = data

#     def __setitem__(self, index, value):
#         self.data[index] = value


# my_list = MyList([10, 20, 30])
# my_list[1] = 50
# print(my_list.data)  # Output: [10, 50, 30]
