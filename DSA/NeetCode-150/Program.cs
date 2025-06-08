using NeetCode_150;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("NeetCode solutions...");
        Solution solution = new Solution();

        //foreach (var item in solution.TopKFrequent([1, 1, 1, 2, 2, 3], 2))
        //{
        //    Console.WriteLine(item);
        //};

        //string encoded = solution.Encode(["mahammad", "ahmadov"]);
        //foreach (var item in solution.Decode(encoded))
        //{
        //    Console.WriteLine(item);
        //};

        IList<int>[] nums = [
            [100, 4, 200, 1, 3, 2],
            [0, 3, 7, 2, 5, 8, 4, 6, 0, 1],
            [1,0,1,2]
        ];

        foreach (var item in nums)
        {
            Console.WriteLine(solution.LongestConsecutive([.. item]));
        }
    }
}