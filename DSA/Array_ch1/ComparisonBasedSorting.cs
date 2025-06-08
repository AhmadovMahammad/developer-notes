using System;
using System.Collections.Concurrent;

namespace Array_ch1;
public class ComparisonBasedSorting
{
    public void BubbleSort(int[] arr)
    {
        int n = arr.Length;

        for (int i = 0; i < arr.Length - 1; i++)
        {
            bool swapped = false;

            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    swapped = true;
                }
            }

            if (!swapped)
            {
                break;
            }
        }
    }

    public void SelectionSort(int[] arr)
    {
        int n = arr.Length;

        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;

            for (int j = i + 1; j < n; j++)
            {
                if (arr[j] < arr[minIndex])
                {
                    minIndex = j;
                }
            }

            if (minIndex != i)
            {
                (arr[minIndex], arr[i]) = (arr[i], arr[minIndex]);
            }
        }
    }

    public void InsertionSort(int[] arr)
    {
        int n = arr.Length;

        for (int i = 1; i < n; i++)
        {
            int check = arr[i];
            int j = i - 1;

            while (j >= 0 && arr[j] > check)
            {
                arr[j + 1] = arr[j];
                j -= 1;
            }

            arr[j + 1] = check;
        }
    }

    public void MergeSort(int[] arr)
    {
        // 8 7 6 5 4 3 2 1
        // 8 7 6 5 - 4 3 2 1
        // 8 7 - 6 5 - 4 3 - 2 1
        // 8 - 7 - 6 - 5 - 4 - 3 - 2 - 1

        if (arr.Length <= 1)
        {
            return;
        }

        int mid = arr.Length / 2;

        int[] left = new int[mid];
        int[] right = new int[arr.Length - mid];

        for (int i = 0; i < mid; i++) left[i] = arr[i];
        for (int j = 0; j < right.Length; j++) right[j] = arr[mid + j];

        MergeSort(left);
        MergeSort(right);

        Merge(arr, left, right);
    }

    public void Merge(int[] arr, int[] left, int[] right)
    {
        int i = 0;
        int j = 0;
        int k = 0;

        while (i < left.Length && j < right.Length)
        {
            if (left[i] <= right[j])
            {
                arr[k++] = left[i++];
            }
            else
            {
                arr[k++] = right[j++];
            }
        }

        while (i < left.Length)
        {
            arr[k++] = left[i++];
        }

        while (j < right.Length)
        {
            arr[k++] = right[j++];
        }
    }

    // 9, 3, 7, 6, 2, 5
    public void QuickSort(int[] arr, int low = int.MinValue, int high = int.MaxValue)
    {
        if (low == int.MinValue) low = 0;
        if (high == int.MaxValue) high = arr.Length - 1;

        if (low < high)
        {
            // p = 2
            int pivotIndex = Partition(arr, low, high); // arr = [3, 2, 5, 6, 9, 7], pivot = 2
            QuickSort(arr, low, pivotIndex - 1);
            QuickSort(arr, pivotIndex + 1, high);
        }
    }

    private int Partition(int[] arr, int low, int high)
    {
        int i = low;
        int pivotElement = arr[high];

        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivotElement)
            {
                (arr[i], arr[j]) = (arr[j], arr[i]);
                i += 1;
            }
        }

        (arr[i], arr[high]) = (arr[high], arr[i]);
        return i;
    }
}