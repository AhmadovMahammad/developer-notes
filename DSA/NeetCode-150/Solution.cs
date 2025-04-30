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
}
