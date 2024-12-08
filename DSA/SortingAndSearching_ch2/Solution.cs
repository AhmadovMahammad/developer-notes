namespace SortingAndSearching_ch2;

public class Solution
{
    public void BubbleSort(int[] nums)
    {
        int n = nums.Length;
        bool swapped = false;

        // { 5, 4, 3, 2, 1 }

        // i = 0, j until 4, j max val is 3
        // j:0 -> { 4, 5, 3, 2, 1 } => j:1 -> { 4, 3, 5, 2, 1 } => j:2 -> { 4, 3, 2, 5, 1 } => j:3 -> { 4, 3, 2, 1, 5 }

        // i = 1, j until 3, j max val is 2
        // j:0 -> { 3, 4, 2, 1, 5 } => j:1 -> { 3, 2, 4, 1, 5 } => j:2 -> { 3, 2, 1, 4, 5 }

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (nums[j] > nums[j + 1])
                {
                    (nums[j], nums[j + 1]) = (nums[j + 1], nums[j]);
                    swapped = true;
                }
            }

            if (!swapped) break;
        }
    }

    public void SelectionSort(int[] nums)
    {
        // { 5, 4, 3, 2, 1 }

        //    Sorted                  Unsorted
        //    { }                     { 5, 4, 3, 2, 1 }
        //    { 1 }                   { 4, 3, 2, 5 }
        //    { 1, 2 }                { 4, 3, 5 }
        //    { 1, 2, 3 }             { 4, 5 }
        //    { 1, 2, 3, 4 }          { 5 }
        //    { 1, 2, 3, 4, 5 }       

        int n = nums.Length;

        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;

            for (int j = i + 1; j < n; j++)
            {
                // find the minimum element's index.
                if (nums[j] < nums[minIndex])
                {
                    minIndex = j;
                }
            }

            (nums[i], nums[minIndex]) = (nums[minIndex], nums[i]);
        }
    }
}
