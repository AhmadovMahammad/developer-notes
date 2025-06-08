using System.Text;

namespace NeetCode_150;
internal class Solution
{
    private readonly HashSet<int> _set = [];
    private readonly Dictionary<string, List<string>> _map = [];
    private readonly Dictionary<int, int> _valIndexDict = [];
    private readonly Dictionary<int, int> _numFrequencyDict = [];

    public bool HasDuplicate(int[] nums)
    {
        foreach (int num in nums)
        {
            if (_set.Contains(num)) return true;
            _set.Add(num);
        }

        return false;
    }

    public bool IsAnagram(string s, string t)
    {
        if (s.Length != t.Length) return false;

        int[] counts = new int[26];
        for (int i = 0; i < s.Length; i++)
        {
            counts[s[i] - 'a']++;
            counts[t[i] - 'a']--;
        }

        return counts.All(m => m == 0);
    }

    public int[] TwoSum(int[] nums, int target)
    {
        for (int i = 0; i < nums.Length; i++)
        {
            int subtract = target - nums[i];
            if (_valIndexDict.TryGetValue(subtract, out int index))
            {
                return [index, i];
            }

            _valIndexDict[nums[i]] = i;
        }

        return [-1, -1];
    }

    public IList<IList<string>> GroupAnagrams(string[] strs)
    {
        foreach (var word in strs)
        {
            var count = new int[26];
            foreach (var c in word)
            {
                count[c - 'a']++;
            }

            var key = string.Join(",", count);
            if (!_map.TryGetValue(key, out List<string>? value))
            {
                value = [];
                _map[key] = value;
            }

            value.Add(word);
        }

        return [.. _map.Values];
    }

    public int[] TopKFrequent(int[] nums, int k)
    {
        // Input: nums = [1,1,1,2,2,3], k = 2
        // Output: [1, 2]

        foreach (int num in nums)
        {
            if (!_numFrequencyDict.TryGetValue(num, out int frequency))
            {
                frequency = 0;
                _numFrequencyDict[num] = frequency;
            }

            _numFrequencyDict[num] = ++frequency;
        }

        // 1, 1, 1, 1, 1
        // dict: 1 => 3 ; 2 => 2 ; 3 => 1
        var buckets = new List<int>[nums.Length + 1];
        foreach (KeyValuePair<int, int> pair in _numFrequencyDict)
        {
            int frequency = pair.Value;
            if (buckets[frequency] == null)
            {
                buckets[frequency] = [];
            }

            buckets[frequency].Add(pair.Key);
        }

        List<int> result = [];
        int n = 0;

        for (int i = buckets.Length - 1; i >= 0; i--)
        {
            if (buckets[i] != null)
            {
                result.AddRange(buckets[i]);
                if (n == k)
                {
                    break;
                }
            }
        }

        return [.. result.Take(k)];
    }

    // encode and decode strings:
    // Constraints:
    // strs[i] contains only UTF-8 characters.
    // 0 <= strs[i].length < 200
    // so we will use not UTF-8 character as delimitter and store string length also

    public string Encode(IList<string> strs)
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (string str in strs)
        {
            string delimitter = str.Length.ToString("D3");

            stringBuilder.Append(delimitter);
            stringBuilder.Append(str);
        }

        return stringBuilder.ToString();
    }

    public List<string> Decode(string s)
    {
        List<string> result = new List<string>();
        int n = 0;
        int formatLength = 3;

        while (n < s.Length)
        {
            if (int.TryParse(s.AsSpan(n, formatLength).ToString(), out int length))
            {
                n += formatLength;
                result.Add(s.AsSpan(n, length).ToString());
                n += length;
            }
        }

        return result;
    }

    // Time Complexity : O(nlogn) + O(n), drop the non dominant terms
    // #Approach 1

    //public int LongestConsecutive(int[] nums)
    //{
    //    QuickSort(nums);

    //    // Constraints:
    //    // 0 <= nums.length <= 105
    //    // - 109 <= nums[i] <= 109

    //    // Input: nums = [100,4,200,1,3,2]
    //    // after sorting: [ 1, 2, 3, 4, 100, 200 ]

    //    int prev = nums[0];
    //    int maxSequence = 1;
    //    int count = 1;

    //    for (int i = 1; i < nums.Length; i++)
    //    {
    //        int cur = nums[i];

    //        if (cur == prev) continue;
    //        if (cur - 1 == prev)
    //        {
    //            count++;
    //        }
    //        else
    //        {
    //            maxSequence = Math.Max(maxSequence, count);
    //            count = 1;
    //        }

    //        prev = cur;
    //    }

    //    return Math.Max(maxSequence, count);
    //}

    // related to LC question
    //private void QuickSort(int[] arr)
    //{
    //    QuickSort(arr, 0, arr.Length - 1);
    //}

    //private void QuickSort(int[] arr, int low, int high)
    //{
    //    if (low < high)
    //    {
    //        // p = 2
    //        int pivotIndex = Partition(arr, low, high); // arr = [3, 2, 5, 6, 9, 7], pivot = 2
    //        QuickSort(arr, low, pivotIndex - 1);
    //        QuickSort(arr, pivotIndex + 1, high);
    //    }
    //}

    //private int Partition(int[] arr, int low, int high)
    //{
    //    int i = low;
    //    int pivotElement = arr[high];

    //    for (int j = low; j < high; j++)
    //    {
    //        if (arr[j] < pivotElement)
    //        {
    //            (arr[i], arr[j]) = (arr[j], arr[i]);
    //            i += 1;
    //        }
    //    }

    //    (arr[i], arr[high]) = (arr[high], arr[i]);
    //    return i;
    //}

    // # Approach 2


    /*

     Example 1:
     Input: nums = [100,4,200,1,3,2]
     Output: 4
     
     Example 2:
     Input: nums = [0,3,7,2,5,8,4,6,0,1]
     Output: 9
     
     
    Example 3:
    Input: nums = [1,0,1,2]
    Output: 3
     
     */


    // [100, 4, 200, 1, 3, 2]
    public int LongestConsecutive(int[] nums)
    {
        // base case
        if (nums.Length == 0) return 0;

        HashSet<int> set = new HashSet<int>(nums);
        int maxSequence = 0;
        int count = 0;

        for (int i = 0; i < nums.Length; i++)
        {
            int cur = nums[i];
            count = 1;

            int check = cur - 1;
            while (set.Contains(check))
            {
                count++;
                check -= 1;
            }

            maxSequence = Math.Max(maxSequence, count);
        }

        return maxSequence;
    }
}