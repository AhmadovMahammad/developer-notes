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

        string encoded = solution.Encode(["mahammad", "ahmadov"]);
        foreach (var item in solution.Decode(encoded))
        {
            Console.WriteLine(item);
        };
    }
}