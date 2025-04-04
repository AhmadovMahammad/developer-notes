using System;
using System.Collections.Concurrent;

namespace Array_ch1;
public class ComparisonBasedSorting
{
    public void BubbleSort(int[] arr)
    {
        int n = arr.Length;
        bool swapped = false;

        for (int i = 0; i < n; i++)
        {
            swapped = false;

            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    swapped = true;
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                }
            }

            if (!swapped) break;
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
                (arr[i], arr[minIndex]) = (arr[minIndex], arr[i]);
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
        if (arr.Length <= 1) return;

        int mid = arr.Length / 2;
        int[] left = new int[mid];
        int[] right = new int[arr.Length - mid];

        for (int i = 0; i < left.Length; i++) left[i] = arr[i];
        for (int i = 0; i < right.Length; i++) right[i] = arr[mid + i];

        MergeSort(left);
        MergeSort(right);

        Merge(arr, left, right);
    }

    private void Merge(int[] arr, int[] left, int[] right)
    {
        int l = 0, r = 0;
        int i = 0;

        while (l < left.Length && r < right.Length)
        {
            if (left[l] < right[r])
            {
                arr[i] = left[l];
                l++;
            }
            else
            {
                arr[i] = right[r];
                r++;
            }

            i++;
        }

        while (l < left.Length)
        {
            arr[i] = left[l];
            l++;
            i++;
        }

        while (r < right.Length)
        {
            arr[i] = right[r];
            r++;
            i++;
        }
    }

    // 9, 3, 7, 6, 2, 5
    public void QuickSort(int[] arr)
    {
        QuickSort(arr, 0, arr.Length - 1);
    }

    private void QuickSort(int[] arr, int low, int high)
    {
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
