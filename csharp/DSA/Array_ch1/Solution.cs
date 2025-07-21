using System;
using System.Text;

namespace Array_ch1;

//These explanations and code examples should give you a comprehensive foundation in each array algorithm.
public class Solution
{
    private readonly Dictionary<int, string> _odds = new Dictionary<int, string>()
    {
        { 1, "One" }, { 2, "Two" }, { 3, "Three" }, { 4, "Four" }, { 5, "Five" },
        { 6, "Six" }, { 7, "Seven" }, { 8, "Eight" }, { 9, "Nine" }

    };

    private readonly Dictionary<int, string> _teens = new Dictionary<int, string>()
    {
        { 10, "Ten" }, { 11, "Eleven" }, { 12, "Twelve" }, { 13, "Thirteen" }, { 14, "Fourteen" },
        { 15, "Fiveteen" }, { 16, "Sixteen" }, { 17, "Seventeen" }, { 18, "Eighteen" }, { 19, "Nineteen" }
    };

    private readonly Dictionary<int, string> _tens = new Dictionary<int, string>()
    {
        { 20, "Twenty" }, { 30, "Thirty" }, { 40, "Fourty" }, { 50, "Fifty" },
        { 60, "Sixty" }, { 70, "Seventy" }, { 80, "Seventy" }, { 90, "Ninety" },
    };

    public string NumberToWords(int num)
    {
        StringBuilder sb = new StringBuilder();
        string[] units = new string[] { "", "Thousand", "Million", "Billion" }; // empty one for the first section
        int i = 0;

        while (num > 0)
        {
            if (num % 1000 != 0)
            {
                string unit = units[i];
                string section = HandleSection(num % 1000).Trim();

                if (sb.Length > 0)
                    sb.Insert(0, " ");
                sb.Insert(0, section + (unit != "" ? $" {unit}" : ""));
            }

            i++;
            num /= 1000;
        }

        return num == 0 ? "Zero" : sb.ToString();
    }

    private string HandleSection(int num)
    {
        StringBuilder sb = new StringBuilder();

        if (num >= 100)
        {
            sb.Append(_odds[num / 100] + " Hundred");
            num %= 100;
        }

        (int odd, int ten) = (num % 10, num / 10);
        switch (ten)
        {
            case int _ when odd == 0 && ten == 0:
                break;
            case 0:
                sb.Append($" {_odds[odd]}");
                break;
            case 1:
                sb.Append($" {_teens[num]}");
                break;
            case int _ when ten > 1:
                sb.Append($" {_tens[ten * 10]}");
                if (odd != 0) // we do not need to add the last digit if it is zero
                {
                    sb.Append($" {_odds[odd]}");
                }
                break;
        }

        return sb.ToString();
    }

    public int MaxSumSlidingWindow(int[] nums, int k)
    {
        int n = nums.Length;
        if (k > nums.Length)
            return -1; // edge case: if array size is less than k, just stop code;

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

    public int[] PlusOne(int[] digits)
    {
        int n = digits.Length;

        for (int i = n - 1; i >= 0; i--)
        {
            if (digits[i] < 9)
            {
                digits[i]++;
                return digits;
            }

            digits[i] = 0;
        }

        int[] res = new int[n + 1];
        res[0] = 1;

        return res;
    }

    public bool HasPairWithSum(int[] nums, int target)
    {
        // Problem:
        // Check if a sorted array has a pair of numbers that add up to a specific target sum.

        int left = 0;
        int right = nums.Length - 1;

        // 1, 2, 3, 4, 5  target = 4 | current sum = 6, so we should decrase sum and shift right to left
        while (left < right)
        {
            int sum = nums[left] + nums[right];
            if (sum == target)
                return true;

            if (sum > target)
                right--;
            else
                left++;
        }

        return false;
    }

    public void ReverseArray(int[] nums)
    {
        // Problem:
        // Reverse an array using the two-pointer technique.

        int left = 0;
        int right = nums.Length - 1;

        while (left < right)
        {
            (nums[left], nums[right]) = (nums[right], nums[left]);
            left++;
            right--;
        }
    }

    // #Kadane's Algorithm
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

    public int MinSubArraySum(int[] nums)
    {
        return 0;
    }

    public int MaxProduct(int[] nums)
    {
        if (nums.Length == 1)
        {
            return nums[0];
        }

        int prev_max = nums[0];
        int prev_min = nums[0];
        int global_max = prev_max;

        for (int i = 1; i < nums.Length; i++)
        {
            int num = nums[i];

            int cur_max = Math.Max(num, Math.Max((prev_max * num), (prev_min * num)));
            int cur_min = Math.Min(num, Math.Min((prev_max * num), (prev_min * num)));

            global_max = Math.Max(global_max, Math.Max(cur_max, cur_min));

            prev_max = cur_max;
            prev_min = cur_min;
        }

        return global_max;
    }


    // #Sliding Window
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

    public int MinSizeSubarraySlidingWindow(int[] nums, int S)
    {
        // nums = { 2, 1, 5, 2, 3, 2 }, S = 7
        int minLength = int.MaxValue;
        int windowSum = 0, start = 0;

        for (int i = 0; i < nums.Length; i++)
        {
            windowSum += nums[i]; // Expand the window by adding the current element

            // Shrink the window until the sum is less than S
            while (windowSum >= S)
            {
                minLength = Math.Min(minLength, i - start + 1);
                windowSum -= nums[start]; // Shrink the window from the left
                start++;
            }
        }

        return minLength == int.MaxValue ? 0 : minLength;
    }

    // #Prefix Sum
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

    public int RangeSum(int[] prefix, int start, int end) // [2, 6, 12, 20, 30]
    {
        if (start == 0)
            return prefix[end];
        return prefix[end] - prefix[start - 1];
    }

    // #leetcode solutions related to array
    public int[] TwoSum(int[] nums, int target)
    {
        /* LeetCode Task 1

            nums = [2,7,11,15], target = 9
            nums = [3,2,4], target = 6
            nums = [3,3], target = 6
        */

        Dictionary<int, int> _keyValuePairs = new();

        for (int i = 0; i < nums.Length; i++)
        {
            int current = nums[i];
            int subTarget = target - current;

            if (_keyValuePairs.ContainsKey(subTarget))
                return new int[] { _keyValuePairs[subTarget], i };
            _keyValuePairs[key: current] = i;
        }

        return new[] { -1, -1 };
    }

    public bool IsPalindrome(int x)
    {
        if (x < 0)
            return false;
        int temp = x;
        int reverse = 0;

        while (temp > 0)
        {
            reverse = reverse * 10 + temp % 10;
            temp /= 10;
        }

        return x == reverse;
    }

    public int RomanToInt(string s)
    {
        int sum = 0, previousVal = 0;

        Dictionary<char, int> keyValuePairs = new()
        {
            { 'I', 1 }, { 'V', 5 }, { 'X', 10 },
            { 'L', 50 }, { 'C', 100 }, { 'D', 500 },
            { 'M', 1000 },
        };

        for (int i = s.Length - 1; i >= 0; i--)
        {
            char c = s[i];

            if (keyValuePairs.TryGetValue(c, out int value) && value >= previousVal)
                sum += value;
            else
                sum -= value;

            previousVal = value;
        }

        return sum;
    }

    public int MinSubArrayLen(int target, int[] nums) // 209, Medium
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

    //public double FindMaxAverage(int[] nums, int k) // 643, Easy
    //{
    //    int n = nums.Length;
    //    if (k > nums.Length)
    //        return -1;

    //    double prevWindowSum = 0;
    //    for (int i = 0; i < k; i++)
    //    {
    //        prevWindowSum += nums[i];
    //    }

    //    double maxAverage = prevWindowSum / k;

    //    for (int i = k; i < n; i++)
    //    {
    //        double newWindowSum = prevWindowSum + nums[i] - nums[i - k];
    //        if (newWindowSum > prevWindowSum)
    //            maxAverage = Math.Max(maxAverage, newWindowSum / k);

    //        prevWindowSum = newWindowSum;
    //    }

    //    return maxAverage;
    //}

    //public double FindMaxAverage2(int[] nums, int k) // 643, Easy
    //{
    //    int n = nums.Length;
    //    if (k > nums.Length)
    //        return -1;

    //    double prevWindowSum = 0;
    //    for (int i = 0; i < k; i++)
    //    {
    //        prevWindowSum += nums[i];
    //    }

    //    double maxAverage = prevWindowSum / k;

    //    for (int i = k; i < n; i++)
    //    {
    //        prevWindowSum += nums[i] - nums[i - k];
    //        maxAverage = Math.Max(maxAverage, prevWindowSum / k);
    //    }

    //    return maxAverage;
    //}

    //public int SubarraySum(int[] nums, int k)
    //{
    //    Dictionary<int, int> keyValuePairs = new() { { 0, 1 } };
    //    int prefixSum = 0, count = 0;

    //    foreach (int num in nums)
    //    {
    //        prefixSum += num;

    //        if (keyValuePairs.TryGetValue(prefixSum - k, out int value))
    //        {
    //            count += value;
    //        }

    //        if (keyValuePairs.ContainsKey(prefixSum)) keyValuePairs[prefixSum]++;
    //        else keyValuePairs[prefixSum] = 1;
    //    }

    //    return count;
    //}

    public int RemoveElement(int[] nums, int val)
    {
        int left = 0;

        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] != val)
            {
                nums[left] = nums[i];
                left++;
            }
        }

        return left;
    }

    public int[] PlusOne2(int[] digits)
    {
        int n = digits.Length;
        int carry = 1;

        for (int i = n - 1; i >= 0; i--)
        {
            int sum = digits[i] + carry;
            digits[i] = sum % 10;
            carry = sum / 10;

            if (carry == 0)
                return digits;
        }

        int[] res = new int[n + 1];
        res[0] = 1;

        return res;
    }

    public string AddBinary(string a, string b)
    {
        int leftPadding = Math.Max(a.Length, b.Length);
        a = a.PadLeft(leftPadding, '0');
        b = b.PadLeft(leftPadding, '0');

        int carry = 0;
        StringBuilder sb = new StringBuilder();

        // a = 11 b = 01
        for (int i = a.Length - 1; i >= 0; i--)
        {
            int sum = a[i] + b[i] + carry - (2 * '0');
            sb.Append(sum % 2);
            carry = sum / 2;
        }

        if (carry != 0)
            sb.Append(carry);

        char[] response = sb.ToString().ToCharArray();
        Array.Reverse(response);

        return new string(response);
    }

    public string Convert(string s, int numRows)
    {
        // early exits
        if (numRows == 1 || numRows > s.Length)
            return s;

        List<List<char>> builder = new();
        for (int i = 0; i < numRows; i++)
        {
            builder.Add(new List<char>());
        }

        bool toBottom = false;
        int row = 0;

        foreach (char c in s)
        {
            builder[row].Add(c);

            if (row == 0 || row == numRows - 1)
                toBottom = !toBottom;
            row = toBottom ? row + 1 : row - 1;
        }

        return string.Join("", builder.SelectMany(list => list));
    }

    public void Permutation(string str)
    {
        Permute(str, string.Empty);
    }

    private void Permute(string remains, string prefix)
    {
        if (string.IsNullOrEmpty(remains))
            Console.WriteLine(prefix);
        else
        {
            for (int i = 0; i < remains.Length; i++)
            {
                string nextRemains = string.Concat(remains[..i], remains[(i + 1)..]);
                string nextPrefix = string.Concat(prefix, remains[i]);

                Permute(nextRemains, nextPrefix);
            }
        }
    }

    public void Permutation_Iterative(string str)
    {
        Queue<(string prefix, string remains)> queue = new();
        queue.Enqueue(("", str));

        while (queue.Count > 0)
        {
            var (prefix, remains) = queue.Dequeue();
            if (string.IsNullOrEmpty(remains))
                Console.WriteLine(prefix);
            else
            {
                for (int i = 0; i < remains.Length; i++)
                {
                    string nextPrefix = prefix + remains[i];
                    string nextRemains = remains[..i] + remains[(i + 1)..];
                    queue.Enqueue((nextPrefix, nextRemains));
                }
            }
        }
    }

    public void FindDuplicates(int[] arr)
    {
        int bitmask = 0;

        foreach (int num in arr)
        {
            int bit = 1 << num;

            if ((bitmask & bit) != 0)
            {
                Console.WriteLine("Duplicate found: " + num);
            }
            else
            {
                bitmask |= bit;
            }
        }
    }

    private readonly HashSet<int> _set = new HashSet<int>();
    public bool hasDuplicate(int[] nums)
    {
        foreach (int num in nums)
        {
            if (_set.Contains(num))
                return true;
            _set.Add(num);
        }

        return false;
    }

    public bool CheckInclusion(string s1, string s2)
    {
        if (s1.Length > s2.Length)
            return false;

        int[] s1Freq = new int[26];
        int[] winFreq = new int[26];

        for (int i = 0; i < s1.Length; i++)
        {
            s1Freq[s1[i] - 'a']++;
            winFreq[s2[i] - 'a']++;
        }

        if (s1Freq.SequenceEqual(winFreq))
            return true;

        for (int i = s1.Length; i < s2.Length; i++)
        {
            winFreq[s2[i] - 'a']++;
            winFreq[s2[i - s1.Length] - 'a']--;

            if (s1Freq.SequenceEqual(winFreq))
                return true;
        }

        return false;
    }

    // Input: nums     = [ 0, 1, 1, 1, 1, 1, 0, 0, 0]
    // Cumulative sums = [-1, 0, 1, 2, 3, 4, 3, 2, 1]

    // if cumulative sum is same between 2 indices,
    // so subarray sum between these 2 indices are same

    // example apart from that example:
    // 5,  1,  2,   3,  -2,  -1,  -3
    // 5,  6,  8,  11,   9,   8,   5

    // here 5 is found on indices 0 and 6
    // so sum between [1, 6] are 0 and it gives same result

    public int FindMaxLength(int[] nums)
    {
        // sum -> first_occurrence_index
        Dictionary<int, int> sum_index_pair = new Dictionary<int, int>()
        {
            { 0, -1 }
        };

        int cumulative_sum = 0;
        int global_max = 0;

        for (int i = 0; i < nums.Length; i++)
        {
            int num = nums[i];

            int next = num == 0 ? -1 : num;
            cumulative_sum += next;

            if (sum_index_pair.TryGetValue(cumulative_sum, out int index))
            {
                global_max = Math.Max(global_max, i - index);
            }
            else
            {
                sum_index_pair[cumulative_sum] = i;
            }
        }

        return global_max;
    }

    /*
    
    You are given an array prices where prices[i] is the price of a given stock on the ith day.
    You want to maximize your profit by choosing a single day to buy one stock and 
    choosing a different day in the future to sell that stock.
    Return the maximum profit you can achieve from this transaction. If you cannot achieve any profit, return 0.

    Example 1:

    Input: prices = [7,1,5,3,6,4]
    Output: 5
    Explanation: Buy on day 2 (price = 1) and sell on day 5 (price = 6), profit = 6-1 = 5.
    Note that buying on day 2 and selling on day 1 is not allowed because you must buy before you sell.
    
    Example 2:
    
    Input: prices = [7,6,4,3,1]
    Output: 0
    Explanation: In this case, no transactions are done and the max profit = 0.


    */

    public int MaxProfit(int[] prices)
    {
        int min_price = prices[0];
        int max_profit = 0;

        for (int i = 1; i < prices.Length; i++)
        {
            max_profit = Math.Max(max_profit, prices[i] - min_price);
            min_price = Math.Min(min_price, prices[i]);
        }

        return max_profit;
    }

    /*
    
        Given an integer n, return all the numbers in the range [1, n] sorted in lexicographical order.
        You must write an algorithm that runs in O(n) time and uses O(1) extra space. 
         
        
        Example 1:
        
        Input: n = 13
        Output: [1,10,11,12,13,2,3,4,5,6,7,8,9]
        

        Example 2:
        
        Input: n = 2
        Output: [1,2]
     
        img for help:
        https://assets.leetcode.com/users/images/c6aa8a16-9df8-4a19-8b9c-cf9755ea1082_1716309559.368739.jpeg
    */

    /*
    
    Output:
    [
        1,10,100,
        101,102,103,104,105,106,107,108,109,
        
        11,110,
        111,112,113,114,115,116,117,118,119,
        
        12,120,
        121,122,123,124,125,126,127,128,129,
        
        13,130,
        131,132,133,134,135,136,137,138,139,
        
        14,140,
        141,142,143,144,145,146,147,148,149,
        
        15,150,
        151,152,153,154,155,156,157,158,159,
        
        16,160,
        161,162,163,164,165,166,167,168,169,
        
        17,170,
        171,172,173,174,175,176,177,178,179,
            
        18,180,
        181,182,183,184,185,186,187,188,189,
        
        19,190,
        191,192,
        
        2,20,
        21,22,23,24,25,26,27,28,29,
        
        3,30,
        31,32,33,34,35,36,37,38,39,
    
        4,40,
        41,42,43,44,45,46,47,48,49,
    
        5,50,
        51,52,53,54,55,56,57,58,59,
    
        6,60,
        61,62,63,64,65,66,67,68,69,
    
        7,70,
        71,72,73,74,75,76,77,78,79,
    
        8,80,
        81,82,83,84,85,86,87,88,89,
    
        9,90,
        91,92,93,94,95,96,97,98,99
    ]
    */

    public IList<int> LexicalOrder(int n)   // n = 192
    {
        /*
        1
        ├── 10
        │   ├── 100
        │   ├── 101
        │   ...
        ├── 11
        │   ├── 110
        │   ├── 111
        │   ...
        ...
        9
        ├── 90
        │   ├── 900
        │   ...
 
        */

        List<int> res = new List<int>();
        int cur = 1;
        int count = 0;

        while (count < n)
        {
            //  1, 10, 100,
            //  101, 102, 103, 104, 105, 106, 107, 108, 109,

            //  19, 190, 
            //  191, 192, 

            //  2, 20, 
            //  21, 22, 23, 24, 25, 26, 27, 28, 29, 

            res.Add(cur);
            count++;

            // go deeper
            if (cur * 10 <= n)
            {
                cur *= 10; // 10, 100, 11, 110, ... , 19, 190
            }
            else
            {
                while (cur % 10 != 9 && cur < n)
                {
                    cur += 1; // 101, 102, 103, 104, ... , 109, 111, 112, ... , 119, ... , 191, 192

                    res.Add(cur);
                    count += 1;
                }

                cur /= 10;
                cur += 1;

                while (cur % 10 == 0)
                {
                    cur /= 10;
                }
            }
        }

        return res;
    }

    public int SubarraySum(int[] nums, int k)
    {
        // Input: nums = [1, 2, 3], k = 3
        // Input: nums = [2, 2, 2, 4, 6, -4], k = 6

        // sum -> count that makes k on this prefix sum
        // so, in next cycles if we found this prefix sum + k, so we can increase value
        Dictionary<int, int> sum_count_pair = new Dictionary<int, int>()
        {
            { 0, 1 }
        };

        // nums:            2,  2,  2,  4,  6,  -4, -2
        // prefix sum:      2,  4,  6,  10, 16, 12, 10
        // cur - k:        -4, -2,  0,  4,  10,  6,  4  
        // found? :         -   -   +   +   +,   +   +

        int cumulative_sum = 0;
        int count = 0;

        for (int i = 0; i < nums.Length; i++)
        {
            cumulative_sum += nums[i];
            int subtract = cumulative_sum - k;

            if (sum_count_pair.TryGetValue(subtract, out int value))
            {
                count += value;
            }

            sum_count_pair[cumulative_sum] = sum_count_pair.GetValueOrDefault(cumulative_sum) + 1;
        }

        return count;
    }

    public double FindMaxAverage(int[] nums, int k)
    {
        int n = nums.Length;
        double avg = 0;
        double sum = 0;

        for (int i = 0; i < n; i++)
        {
            if (i < k)
            {
                sum += nums[i];
            }
            else
            {
                sum = sum + nums[i] - nums[i - k];
                avg = Math.Max(avg, sum / k);
            }

            if (i + 1 == k)
            {
                avg = sum / k;
            }
        }

        return avg;
    }

    public int[] MaxSlidingWindow(int[] nums, int k)
    {
        int[] result = new int[nums.Length - k + 1];
        int max = nums[0];
        int max_index = 0;

        for (int i = 1; i < k; i++)
        {
            if (nums[i] > max)
            {
                max = nums[i];
                max_index = i;
            }
        }

        result[0] = max;

        // [7, 5, 4] k = 2
        // 1. [7, 5] max = 7, sum = 12
        // 2. [5, 4] max = 5, 16 - 7 = 9

        for (int i = k; i < nums.Length; i++)
        {
            int add = nums[i];

            /*
               [1   3  -1] -3  5  3  6   7       3
                1  [3  -1  -3] 5  3  6   7       3
                
                1   3 [-1  -3  5] 3  6   7       5
                1   3  -1 [-3  5  3] 6   7       5
                1   3  -1  -3 [5  3  6]  7       6
                1   3  -1  -3  5 [3  6   7]      7
             */

            if (max_index != i - k)
            {
                max = Math.Max(max, add);
            }
            else
            {
                max = int.MinValue;

                for (int j = i; j > i - k; j--)
                {
                    if (nums[j] > max)
                    {
                        max = nums[j];
                        max_index = j;
                    }
                }
            }

            result[i - k + 1] = max;
        }

        return result;
    }
}