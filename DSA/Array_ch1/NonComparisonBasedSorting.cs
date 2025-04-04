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
}
