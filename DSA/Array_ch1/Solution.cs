namespace Array_ch1
{
    //These explanations and code examples should give you a comprehensive foundation in each array algorithm.
    public class Solution
    {
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
                if (sum == target) return true;

                if (sum > target) right--;
                else left++;
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
        public int RangeSum(int[] prefix, int start, int end) // [2, 6, 12, 20, 28]
        {
            if (start == 0) return prefix[end];
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

                if (_keyValuePairs.ContainsKey(subTarget)) return new int[] { _keyValuePairs[subTarget], i };
                _keyValuePairs[key: current] = i;
            }

            return new[] { -1, -1 };
        }
        public bool IsPalindrome(int x)
        {
            /*
                Input: x = 121
                Output: true
                
                Input: x = -121
                Output: false
                
                Input: x = 10
                Output: false
            */

            if (x < 0) return false;
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

                if (keyValuePairs.TryGetValue(c, out int value) && value >= previousVal) sum += value;
                else sum -= value;

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
        public double FindMaxAverage2(int[] nums, int k) // 643, Easy
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
                prevWindowSum += nums[i] - nums[i - k];
                maxAverage = Math.Max(maxAverage, prevWindowSum / k);
            }

            return maxAverage;
        }
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
    }
}
