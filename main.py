import re
import math

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
