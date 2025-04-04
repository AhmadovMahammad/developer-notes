namespace Array_ch1;
public class NonComparisonBasedSorting
{
    public void CountingSort(int[] arr)
    {
        //new int[] { 4, 2, 2, 8, 3, 3, 1 },
        int max = 0;
        int n = arr.Length;

        for (int i = 0; i < n; i++)
        {
            if (arr[i] > max) max = arr[i];
        }
        // max = 8

        int[] counting = new int[max + 1]; // 0, 0, 0, 0, 0, 0, 0, 0, 0
        for (int i = 0; i < n; i++)
        {
            counting[arr[i]]++;
        }
        // 0, 1, 2, 2, 1, 0, 0, 0, 1

        int j = 0;
        for (int i = 0; i < counting.Length; i++)
        {
            // i is the value, and counting[i] is its frequency
            int count = counting[i];
            while (count > 0)
            {
                arr[j] = i; // Place the value i into the original array
                count--;
                j++;
            }
        }
    }

    public void RadixSort(int[] arr)
    {
        int max = GetMax(arr);
        int n = arr.Length;
        int exp = 1;

        Dictionary<int, List<int>> buckets = new Dictionary<int, List<int>>();
        for (int i = 0; i < 10; i++) buckets[i] = new List<int>();

        while (max / exp > 0)
        {
            for (int i = 0; i < 10; i++) buckets[i] = new List<int>();
            for (int j = 0; j < n; j++)
            {
                int digit = (arr[j] / exp) % 10;
                buckets[digit].Add(arr[j]);
            }

            int k = 0;
            foreach (var bucket in buckets.Values)
            {
                foreach (var num in bucket) arr[k++] = num;
            }

            for (int i = 0; i < 10; i++) buckets[i].Clear();
            exp *= 10;
        }
    }

    public void BucketSort(double[] arr)
    {
        int n = arr.Length;
        if (n <= 1) return;

        (double min, double max) = (arr.Min(), arr.Max());
        double size = Math.Max(1, (max - min) / n + 1);

        Dictionary<int, List<double>> dict = new Dictionary<int, List<double>>();
        for (int i = 0; i < n; i++) dict[i] = new List<double>();

        for (int i = 0; i < n; i++)
        {
            int bucketIndex = (int)Math.Floor((arr[i] - min) / size);
            dict[bucketIndex].Add(arr[i]);
        }

        int k = 0;
        foreach (var bucket in dict.OrderBy(b => b.Key).Select(b => b.Value))
        {
            bucket.Sort();
            foreach (var val in bucket)
            {
                arr[k++] = val;
            }
        }
    }

    // When we calculate bucket size as (max - min) / n, we are dividing the value range
    // into 'n' equal-width buckets. However, these buckets use half-open intervals,
    // meaning they include the lower bound but exclude the upper bound.
    //
    // For example, if min = 0, max = 1, and n = 5:
    //   size = (1 - 0) / 5 = 0.2
    //   Buckets become: [0.0 - 0.2), [0.2 - 0.4), [0.4 - 0.6), [0.6 - 0.8), [0.8 - 1.0)
    //
    // The issue here is that the last value (1.0) does not fall into any of these buckets,
    // because 1.0 is not *strictly less than* 1.0 (it's at the boundary).
    //
    // What does '+1' fix?
    // By adding +1 to the bucket count or increasing the size slightly,
    // we ensure the last element (max) also fits inside the final bucket.
    // That way, we avoid index out of bounds errors or missed values when inserting into buckets.

    private int GetMax(int[] arr)
    {
        int max = arr[0];
        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] > max) max = arr[i];
        }

        return max;
    }
}
