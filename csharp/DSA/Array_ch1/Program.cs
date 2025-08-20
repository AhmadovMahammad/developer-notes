using Array_ch1;

internal class Program
{
    private static void Main(string[] args)
    {
        #region Basics

        /* Array Basics

        Arrays are one of the simplest data structures, 
        known for storing elements in a contiguous block of memory, which allows for constant-time access to elements via their index.

        In memory, an array is allocated as a single block.
        Given an array arr with integer elements, the address of an element arr[i] is computed as:

        Address of arr[i] = Base Address of arr + ( i × size of each element)

        This is why array access is so fast: 
        the element's position can be calculated with a simple formula, so no iteration is required to locate it.

        ---Benefits of Index-Based Access:
        a) You can directly retrieve any element in constant time (O(1)), 
        which makes arrays ideal for applications where fast read access is necessary.

        b) Arrays have low memory overhead since all elements are of a fixed size and laid out contiguously.

        */

        /* Array types

        a) Static Arrays
        Static arrays have a fixed size determined at the time of creation. Once defined, you can’t change the array’s length.

        Pros:
            Simple and efficient in terms of memory.
        Cons:
            Reallocation (copying data into a new, larger array) is necessary if you need more space.

        int[] staticArray = new int[5];  // Array of 5 integers
        staticArray[0] = 10;
        Console.WriteLine(staticArray[0]);


        b) Dynamic Arrays
        Dynamic arrays can grow or shrink in size as needed, with reallocation handled behind the scenes.
        ArrayList in C# acts as a dynamic array, allowing flexible resizing.
        When a dynamic array needs more space, it typically doubles its size and copies the existing elements to the new space. 
        This reallocation is costly (O(n) for n elements) but infrequent due to the resizing strategy.

        ArrayList dynamicArray = new ArrayList();
        dynamicArray.Add(10);
        dynamicArray.Add(20);
        Console.WriteLine(dynamicArray[0]);  // Outputs: 10


        c) Jagged Arrays
        A jagged array is an array of arrays, where each “row” can have a different length. 
        It’s a useful structure when dealing with variable-sized datasets, 
        like representing data where each row has a different number of elements.

        int[][] jaggedArray = new int[3][]; // Declares a jagged array with 3 rows
        jaggedArray[0] = new int[] { 1, 2 };
        jaggedArray[1] = new int[] { 3, 4, 5 };
        jaggedArray[2] = new int[] { 6, 7, 8, 9 };


        d) Multidimensional Arrays
        Multidimensional arrays are arrays with more than one dimension, commonly 2D or 3D arrays. 
        In a 2D array, for example, elements are stored in a grid-like structure.

        int[,] multiArray = new int[3, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
        Console.WriteLine(multiArray[2, 1]); // Accessing the element in row 3, column 2, Output: 6

        -----Array Properties and Methods
        C# provides various properties and methods for accessing information about an array, which include:

        1) Length Property
        The Length property returns the total number of elements in an array. 
        This is useful for looping over all elements, as it tells you the exact number of items.

        Console.WriteLine(numbers.Length); // Output: 5

        2) GetLength(int dimension) Method
        If you're working with multidimensional arrays, 
        GetLength(dimension) helps you retrieve the length along a specific dimension.

        int[,] matrix = new int[3, 5];
        Console.WriteLine(matrix.GetLength(0)); // Output: 3 (number of rows)
        Console.WriteLine(matrix.GetLength(1)); // Output: 5 (number of columns)

        3) GetUpperBound(int dimension) Method
        For multidimensional arrays, GetUpperBound(dimension) returns the index of the last element along a specified dimension. 
        It’s useful when you need the upper limit of an array dimension.

        Console.WriteLine(matrix.GetUpperBound(0)); // Output: 2 (last row index)
        Console.WriteLine(matrix.GetUpperBound(1)); // Output: 4 (last column index)

        */

        /* Algorithms

        -----1. Two-Pointer Technique

        The Two-Pointer technique involves using two indices (or pointers) 
        that traverse the array from opposite directions or move towards a goal. 

        It’s highly efficient in terms of time complexity and memory usage, 
        often yielding O(n) time complexity solutions for problems 
        involving pairs or subarray constraints.

        This technique is versatile and can be applied to a variety of problems:
        a) Pair Finding: Useful for checking if a pair exists with a certain sum in a sorted array.
        b) Array Reversal: Reversing an array by swapping elements from the start and end towards the center.

        -----2. Kadane’s Algorithm

        Kadane’s Algorithm is a classic technique for solving the Maximum Subarray Sum Problem, 
        where the goal is to find the largest sum of contiguous elements in an array. 
        This algorithm is efficient and has a linear time complexity of O(n), 
        which is optimal for this problem.

        Problem Overview:

        The Maximum Subarray Sum Problem is to find the contiguous subarray 
        within a one-dimensional array of numbers that has the largest sum. For example:

        Input: [-2, 1, -3, 4, -1, 2, 1, -5, 4]
        Output: 6 (from the subarray [4, -1, 2, 1])

        This problem might sound simple, but it’s challenging due to the presence of negative numbers. 
        When negative numbers are present, the sum of a subarray can decrease, 
        and blindly including all elements won’t yield the maximum possible sum.

        public int MaxSubArraySumBruteForce(int[] nums)
        {
            int maxSum = 0;

            for (int start = 0; start < nums.Length; start++)
            {
                int sum = 0;

                for (int end = start; end < nums.Length; end++)
                {
                    sum += nums[end];
                    maxSum = Math.Max(maxSum, sum);
                }
            }

            return maxSum;
        }

        --Why Simple Approaches Fall Short?

        1. BruteForce Approach:

        In a brute force solution, you would try every possible subarray and calculate their sums. 
        For an array of size n, there are about n(n+1)/2n possible subarrays. 
        This would result in an O(n2) or O(n3) time complexity, which becomes infeasible for large arrays.

        ---Theory Behind Kadane’s Algorithm

        As you traverse the array, you encounter each element. For each element, you have two choices:
        1_ Include the current element in the existing subarray (extend the current subarray).
        2_ Start a new subarray from the current element (discard previous subarray and start fresh).

        [-2, -3, 4, -1, -2, 1, 5, -3]

        First Element (-2): currentSum = -2, globalMax = -2
        Second Element (-3): currentSum = Math.Max(-3, -2 + -3) = -3, globalMax = -2
        Third Element (4): currentSum = Math.Max(4, -3 + 4) = 4, globalMax = 4
        Fourth Element (-1): currentSum = Math.Max(-1, 4 + -1) = 3, globalMax = 4
        Fifth Element (-2): currentSum = Math.Max(-2, 3 + -2) = 1, globalMax = 4
        Sixth Element (1): currentSum = Math.Max(1, 1 + 1) = 2, globalMax = 4
        Seventh Element (5): currentSum = Math.Max(5, 2 + 5) = 7, globalMax = 7
        Eighth Element (-3): currentSum = Math.Max(-3, 7 + -3) = 4, globalMax = 7


        currentSum = Math.Max(arr[i], currentSum + arr[i]);
        is critical to Kadane’s Algorithm and determines whether 
        1. to include the current element in the existing subarray or 2. start a new subarray from the current element.

        public int MaxSubArraySum(int[] nums)
        {
            //  -2, 1, -3, 4, -1, 2, 1, -5, 4 

            int maxSum = nums[0];
            int currentSum = nums[0];

            for (int i = 1; i < nums.Length; i++)
            {
                currentSum = Math.Max(nums[i], currentSum + nums[i]);
                maxSum = Math.Max(maxSum, currentSum);
            }

            return maxSum;
        }

        -----3. Sliding-Window Techniques

        A sliding window is a way to iterate over subsets of a larger dataset in a linear way 
        to find optimal solutions to subarray problems. 
        Sliding window algorithms are effective for problems where:

        1. The solution involves contiguous subarrays for subsequences.
        2. We want to minimize redundant calculations over overlapping sections.

        There are 2 main types of sliding window algorithms:
        1. Fixed-side sliding window
        2. Variable-size sliding windoww

        ---Why Sliding Window Is Effective

        In many cases, brute force solutions require recalculating overlapping subarrays repeatedly, 
        which makes them inefficient. 
        Sliding window algorithms solve this by only adjusting the parts that change as the window slides. 
        This technique reduces the time complexity significantly, often from O(n^2) to O(n).


        1) Fixed-Size Sliding Window

        Problem Example: Maximum Sum of Subarrays of Size k
        Problem Statement: Given an array of integers and a positive integer k, 
        find the maximum sum of any contiguous subarray of size k.

        Step 1: Brute Force Solution
        We calculate the sum of every subarray of size k.
        This approach has a time complexity of O(n * k) because we repeatedly calculate the sum of each subarray.

        public int MaxSumBruteForce(int[] nums, int k)
        {
            int maxSum = 0;

            for (int i = 0; i <= nums.Length - k; i++)
            {
                int sum = 0;

                for (int j = i; j < i + k; j++)
                {
                    sum += nums[j];
                }

                maxSum = Math.Max(maxSum, sum);
            }

            return maxSum;
        }

        This solution recalculates the sum for each subarray, making it inefficient.
        Each window overlaps with the previous by k - 1 elements, 
        but brute force recalculates the sum of all elements in each window.

        Step 2: Sliding Window Solution

        We calculate the sum of the first window (the first k elements).
        For each subsequent window, we slide by removing the element going out of the window and 
        adding the new element.

        This reduces the time complexity to O(n).

        public int MaxSumSlidingWindow(int[] nums, int k)
        {
            int n = nums.Length;
            if (k > nums.Length) return -1; // edge case: if array size is less than k, just stop code;

            int initialWindowSum = 0;
            for (int i = 0; i < k; i++)
            {
                initialWindowSum += nums[i];
            }

            int maxSum = initialWindowSum;

            for (int i = k; i < n; i++)
            {
                initialWindowSum += (nums[i] - nums[i - k]);
                maxSum = Math.Max(maxSum, initialWindowSum);
            }

            return maxSum;
        }

        ---Why Sliding Window is Effective Here

        Instead of recalculating each window's sum from scratch, 
        we adjust the previous sum by subtracting the element that falls out and adding the new element. 
        This cuts down redundant operations.


        2) Variable-Size Sliding Window.

        Problem Statement: 
        Given an array of positive integers and an integer S, 
        find the length of the smallest contiguous subarray whose sum is greater than or equal to S. 
        If no such subarray exists, return 0.

        Step 1: Brute Force Solution

        In the brute force approach for every possible starting point of the subarray, 
        we expand the subarray to check if the sum meets or exceeds S.

        public int MinSizeSubarrayBruteForce(int[] nums, int S)
        {
            int minLength = int.MaxValue, n = nums.Length;

            for (int i = 0; i < n; i++)
            {
                int sum = 0;

                for (int j = i; j < n; j++)
                {
                    sum += nums[j];

                    if (sum >= S)
                    {
                        minLength = Math.Min(minLength, j - i + 1);
                        break;
                    }
                }
            }

            return minLength == int.MaxValue ? 0 : minLength;
        }

        This solution recalculates the sum for each subarray from scratch, making it inefficient.
        For large arrays, this approach is impractical due to O(n^2) time complexity.

        Step 2: Sliding Window Solution

        Using a variable-size sliding window:

        1_ Start with an empty window and expand it by adding elements until the sum is greater than or equal to S.
        2_ Once the sum condition is met, try shrinking the window from the left to find the smallest possible subarray.

        This reduces the time complexity to O(n).

        public int MinSizeSubarraySlidingWindow(int[] nums, int S)
        {
            // nums = { 2, 1, 5, 2, 3, 2 }, S = 7
            int minLength = int.MaxValue;
            int windowSum = 0, start = 0;

            for (int i = 0; i < nums.Length; i++)
            {
                windowSum += nums[i];

                while (windowSum >= S)
                {
                    minLength = Math.Min(minLength, i - start + 1);
                    windowSum -= nums[start];
                    start++; // 2
                }
            }

            return minLength == int.MaxValue ? 0 : minLength;
        }

        By expanding until the sum condition is met, then shrinking to minimize the subarray size, 
        we avoid recalculating sums repeatedly.


        4) What is a Prefix Sum Array?

        A prefix sum array is a way of storing cumulative sums of an array so that 
        we can efficiently calculate the sum of elements over any subarray or 
        range within the original array.

        The idea is to create a new array prefix where 
        each element at index i stores the sum of all elements from the start of the array up to index i.

        Prefix sums are useful when we frequently need to calculate the sum of elements in a specific range (from index i to index j). 
        Without prefix sums, calculating the sum for each query would require a new loop through the elements in the range, 
        leading to O(n) time complexity per query.

        With prefix sums, we can reduce this to O(1) time complexity per query 
        after an initial O(n) preprocessing step to build the prefix sum array. 
        This is a significant efficiency gain for large arrays or when there are multiple range sum queries.

        public int[] BuildPrefixSumArray(int[] nums)
        {
            // { 2, 4, 6, 8, 10 }
            int[] prefix = new int[nums.Length];
            int prevSum = 0;

            for (int i = 0; i < nums.Length; i++)
            {
                int sum = nums[i] + prevSum;
                prevSum = prefix[i] = sum;
            }

            return prefix;
        }

        public int RangeSum(int[] prefix, int start, int end) // [2, 6, 12, 20, 28]
        {
            if (start == 0) return prefix[end];
            return prefix[end] - prefix[start - 1];
        }

        -----Practical Problems Using Prefix Sums

        LeetCode 560: Subarray Sum Equals K: Find the total number of continuous subarrays whose sum equals a target k.

        Problem Statement: 
        Given an integer array nums and an integer k, find the number of continuous subarrays whose sum equals k.

        Challenge: 
        Finding all subarrays and calculating their sums would take O(n²) time, 
        which is inefficient for large arrays. 
        Using a prefix sum approach with a hashmap can reduce the complexity to O(n).

        public int SubarraySum(int[] nums, int k)
        {
            Dictionary<int, int> keyValuePairs = new() { { 0, 1 } };
            int prefixSum = 0, count = 0;

            foreach (int num in nums)
            {
                prefixSum += num;

                if (keyValuePairs.TryGetValue(prefixSum - k, out int value))
                {
                    count += value;
                }

                if (keyValuePairs.ContainsKey(prefixSum)) keyValuePairs[prefixSum]++;
                else keyValuePairs[prefixSum] = 1;
            }

            return count;
        }

        */

        /* Notes

        Here are a few popular sliding window tasks on LeetCode that 
        apply both fixed-size and variable-size sliding window techniques. 
        They cover common use cases and allow you to practice different aspects of the sliding window approach.

        1. LeetCode 209: Minimum Size Subarray Sum

        Problem: Given an array of positive integers nums and a positive integer target, 
        find the minimal length of a contiguous subarray of which the sum is greater than or equal to target. 
        If there is no such subarray, return 0 instead.

        Difficulty: Medium
        Sliding Window Type: Variable-size
        Solution Insight: Expand the window by adding elements until the sum meets or exceeds target, 
        then shrink it from the left to minimize the length.
        Approach: Sliding window to expand and contract dynamically based on the sum.

        Example 1:
        Input: target = 7, nums = [2,3,1,2,4,3]
        Output: 2

        Example 2:
        Input: target = 4, nums = [1,4,4]
        Output: 1

        Example 3:
        Input: target = 11, nums = [1,1,1,1,1,1,1,1]
        Output: 0

        public int MinSubArrayLen(int target, int[] nums) // 209
        {
            int minLen = int.MaxValue;
            int windowSum = 0;
            int start = 0;

            for (int i = 0; i < nums.Length; i++)
            {
                windowSum += nums[i];

                while (windowSum >= target)
                {
                    minLen = Math.Min(minLen, i - start + 1);
                    windowSum -= nums[start];
                    start += 1;
                }
            }

            return minLen == int.MaxValue ? 0 : minLen;
        }

        2. LeetCode 643: Maximum Average Subarray I

        Problem: Given an array nums consisting of n elements, 
        find the contiguous subarray of length k that has the maximum average, and return this maximum average.

        Difficulty: Easy
        Sliding Window Type: Fixed-size
        Solution Insight: Use a fixed-size sliding window to calculate the sum of the first k elements, 
        then slide the window across the array by adding the next element and removing the first element of the previous window.
        Approach: This avoids recalculating sums repeatedly, providing an efficient solution with O(n) time complexity.

        public double FindMaxAverage(int[] nums, int k) // 643, Easy
        {
            int n = nums.Length;
            if (k > nums.Length) return -1;

            double prevWindowSum = 0;
            for (int i = 0; i < k; i++)
            {
                prevWindowSum += nums[i];
            }

            double maxAverage = prevWindowSum / k;

            for (int i = k; i < n; i++)
            {
                double newWindowSum = prevWindowSum + nums[i] - nums[i - k];
                if (newWindowSum > prevWindowSum) maxAverage = Math.Max(maxAverage, newWindowSum / k);

                prevWindowSum = newWindowSum;
            }

            return maxAverage;
        }

        3. LeetCode 567: Permutation in String

        Problem: Given two strings s1 and s2, return true if s2 contains a permutation of s1, or false otherwise. 
        In other words, return true if one of s1's permutations is the substring of s2.

        Difficulty: Medium
        Sliding Window Type: Fixed-size (using character count comparison)
        Solution Insight: This problem uses a fixed-size window to track the counts of characters. 
        Check if the character counts of the window in s2 match s1.
        Approach: Use a sliding window with a character frequency map for comparison, 
        adjusting the map as the window moves across s2.

        */

        #region Solutions
        //Solution solution = new Solution();

        // 1. HasPairWithSum Solution

        //int[] nums = new int[] { 1, 2, 3, 4, 5, 6 };
        //int target = 10;
        //Console.WriteLine($"Pair Exists: {solution.HasPairWithSum(nums, target)}");


        // 2. ReverseArray Solution
        //int[] nums = new int[] { 1, 2, 3, 4, 5, 6 };

        //Console.WriteLine($"Before: {string.Join(',', nums)}");
        //solution.ReverseArray(nums);
        //Console.WriteLine($"After: {string.Join(',', nums)}");


        // 3. Max Subarray Sum : Brute Force Code Implementation
        //int[] nums = { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
        //Console.WriteLine($"Max SubArray Sum: {solution.MaxSubArraySumBruteForce(nums)}");


        // 4. Max Subarray Sum : Kadane's Algorithm
        //int[] nums = { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
        //Console.WriteLine($"Max SubArray Sum: {solution.MaxSubArraySum(nums)}");


        // 5. Max Sum BruteForce
        //int maxSum = solution.MaxSumBruteForce(new int[] { 2, 1, 5, 1, 3, 2 }, 3);
        //Console.WriteLine($"Output: {maxSum}"); // Output: 9 (5, 1, 3)


        // 6. Max Sum Sliding Window (Fixed-Size)
        //int maxSum = solution.MaxSumSlidingWindow(new int[] { 2, 1, 5, 1, 3, 2 }, 3);
        //Console.WriteLine($"Output: {maxSum}"); // Output: 9 (5, 1, 3)


        // 7. Roman to Integer
        //Console.WriteLine(solution.RomanToInt("III"));
        //Console.WriteLine(solution.RomanToInt("LVIII"));
        //Console.WriteLine(solution.RomanToInt("MCMXCIV"));


        // 8. Minimum Size Subarray Sum (Brute-Force)
        //int[] nums = new int[] { 2, 1, 5, 2, 3, 2 };
        //int minSize = solution.MinSizeSubarrayBruteForce(nums, 7);
        //Console.WriteLine(minSize);


        // 9. Minimum Size Subarray Sum - Sliding Window (Variable-Size)
        //int[] nums = new int[] { 2, 1, 5, 2, 3, 2 };
        //int minSize = solution.MinSizeSubarraySlidingWindow(nums, 7);
        //Console.WriteLine(minSize);


        // 10. Maximum Average Subarray I
        //Console.WriteLine(solution.FindMaxAverage(new int[] { 4, 2, 1, 3, 3 }, 2));


        // 11. Prefix Related
        //int[] prefix = solution.BuildPrefixSumArray(new int[] { 2, 4, 6, 8, 10 });
        //Console.WriteLine($"Range Sum between 1:3 = {solution.RangeSum(prefix, 1, 3)}");


        // 12. SubArray Sum equals to K
        //Console.WriteLine(solution.SubarraySum(new int[] { 1, -1, 0 }, 0));
        #endregion

        #endregion

        ComparisonBasedSorting sorting = new ComparisonBasedSorting();
        NonComparisonBasedSorting sorting_2 = new NonComparisonBasedSorting();
        Solution solution = new Solution();

        solution.Permutation("abc");

        //Console.WriteLine(string.Join(',', solution.MaxSlidingWindow(new int[] { 1, 3, -1, -3, 5, 3, 6, 7 }, 3)));
        //Console.WriteLine(string.Join(',', solution.MaxSlidingWindow(new int[] { 7, 5, 4 }, 2)));


        //Console.WriteLine(solution.FindMaxAverage(new[] { 1, 12, -5, -6, 50, 3 }, 4));
        //Console.WriteLine(solution.FindMaxAverage(new[] { 5 }, 1));


        //Console.WriteLine(solution.MaxProduct(new int[] { -4, -3, -2 }));


        //Console.WriteLine(solution.FindMaxLength(new int[] { 0, 1, 1, 1, 1, 1, 0, 0, 0 }));
        //Console.WriteLine(solution.FindMaxLength(new int[] { 0, 1, 1 }));
        //Console.WriteLine(solution.FindMaxLength(new int[] { 0, 1 }));


        //Console.WriteLine(solution.MaxProfit(new int[] { 7, 1, 5, 3, 6, 4 }));
        //Console.WriteLine(solution.MaxProfit(new int[] { 7, 6, 4, 3, 1 }));


        //new List<int>() { 192, /*13, 2, 100*/ }.ForEach(test_case =>
        //{
        //    IList<int> res = solution.LexicalOrder(test_case);

        //    Console.WriteLine($"case: {test_case}\n");

        //    Console.Write(string.Join(',', res));

        //    Console.WriteLine();
        //});

        // -----------------------------------------------------------------------

        //List<int[]> unsortedArrays = new()
        //{
        //    new int[] { 237, 146, 259, 348, 152, 163, 235, 48, 36, 62 },
        //    new int[] { 8, 3, 7, 4 },
        //    new int[] { 9, 3, 7, 5, 6, 4, 8, 2 },
        //    new int[] { 4, 2, 2, 8, 3, 3, 1 },
        //    new int[] { 5, 4, 3, 2, 1 },
        //    new int[] { 1, 2, 3, 4, 5 },
        //    new int[] { 5, 1 },
        //    new int[] { 1, 2, 10, 7, 3, 6, 9, 0 },
        //    new int[] { 9, 3, 7, 6, 2, 5 },
        //};

        //Console.WriteLine("==== Sorting Algorithms Visualization ====");

        //{
        //    Console.WriteLine("\n--- Bubble Sort ---");
        //    foreach (int[] array in unsortedArrays)
        //    {
        //        int[] arrayCopy = (int[])array.Clone();

        //        Console.WriteLine($"Before sorting: {string.Join(", ", arrayCopy)}");
        //        sorting.BubbleSort(arrayCopy);
        //        Console.WriteLine($"After sorting: {string.Join(", ", arrayCopy)}");
        //    }
        //}


        //{
        //    Console.WriteLine("\n--- Selection Sort ---");
        //    foreach (int[] array in unsortedArrays)
        //    {
        //        int[] arrayCopy = (int[])array.Clone();

        //        Console.WriteLine($"Before sorting: {string.Join(", ", arrayCopy)}");
        //        sorting.SelectionSort(arrayCopy);
        //        Console.WriteLine($"After sorting: {string.Join(", ", arrayCopy)}");
        //    }
        //}


        //{
        //    Console.WriteLine("\n--- Insertion Sort ---");
        //    foreach (int[] array in unsortedArrays)
        //    {
        //        int[] arrayCopy = (int[])array.Clone();

        //        Console.WriteLine($"Before sorting: {string.Join(", ", arrayCopy)}");
        //        sorting.InsertionSort(arrayCopy);
        //        Console.WriteLine($"After sorting: {string.Join(", ", arrayCopy)}");
        //    }
        //}


        //{
        //    Console.WriteLine("\n--- Merge Sort ---");
        //    foreach (int[] array in unsortedArrays)
        //    {
        //        int[] arrayCopy = (int[])array.Clone();

        //        Console.WriteLine($"Before sorting: {string.Join(", ", arrayCopy)}");
        //        sorting.MergeSort(arrayCopy);
        //        Console.WriteLine($"After sorting: {string.Join(", ", arrayCopy)}");
        //    }
        //}


        //{
        //    Console.WriteLine("\n--- Quick Sort ---");
        //    foreach (int[] array in unsortedArrays)
        //    {
        //        int[] arrayCopy = (int[])array.Clone();

        //        Console.WriteLine($"Before sorting: {string.Join(", ", arrayCopy)}");
        //        sorting.QuickSort(arrayCopy);
        //        Console.WriteLine($"After sorting: {string.Join(", ", arrayCopy)}");
        //    }
        //}


        //{
        //    Console.WriteLine("\n--- Counting Sort ---");
        //    foreach (int[] array in unsortedArrays)
        //    {
        //        int[] arrayCopy = (int[])array.Clone();

        //        Console.WriteLine($"Before sorting: {string.Join(", ", arrayCopy)}");
        //        sorting_2.CountingSort(arrayCopy);
        //        Console.WriteLine($"After sorting: {string.Join(", ", arrayCopy)}");
        //    }
        //}


        //{
        //    Console.WriteLine("\n--- Radix Sort ---");
        //    foreach (int[] array in unsortedArrays)
        //    {
        //        int[] arrayCopy = (int[])array.Clone();

        //        Console.WriteLine($"Before sorting: {string.Join(", ", arrayCopy)}");
        //        sorting_2.RadixSort(arrayCopy);
        //        Console.WriteLine($"After sorting: {string.Join(", ", arrayCopy)}");
        //    }
        //}


        //{
        //    Console.WriteLine("\n--- Bucket Sort ---");
        //    foreach (double[] array in new List<double[]>
        //    {
        //        new double[]{0.13, 0.25, 0.36, 0.58, 0.41, 0.29, 0.22, 0.45}
        //    })
        //    {
        //        Console.WriteLine($"Before sorting: {string.Join(", ", array)}");
        //        sorting_2.BucketSort(array);
        //        Console.WriteLine($"After sorting: {string.Join(", ", array)}");
        //    }
        //}

        //Console.WriteLine("\n=== End of Sorting Algorithms Visualization ===");

        //sorting_2.Permutation("abc");
        //sorting_2.Permutation_Iterative("abc");

        //Solution solution = new Solution();
        //Console.WriteLine(solution.CheckInclusion("ab", "eidbaooo"));
    }
}