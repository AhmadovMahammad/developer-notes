namespace Array_ch1;
public class Searching
{
    // Leetcode: 74. Search a 2D Matrix
    public bool SearchMatrix(int[][] matrix, int target)
    {
        int m = matrix.Length;
        int n = matrix[0].Length;
        int left = 0, right = m * n - 1;

        // [ 1, 3, 5, 4] target = 1
        // mid = 0 + 3 / 2 = 1 = 1
        // arr[1] = 3 : left = 0 right = mid - 1, so check left <= rigth
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int midValue = matrix[mid / n][mid % n];

            if (midValue == target) return true;

            if (target < midValue) right = mid - 1;
            else left = mid + 1;
        }

        return false;
    }

    public int Search(int[] nums, int target)
    {
        int left = 0, right = nums.Length - 1;

        // 4, 5, 6, 7, 0, 1, 2
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (target == nums[mid]) return mid;

            if (nums[left] <= nums[mid])
            {
                if (nums[left] <= target && target < nums[mid]) right = mid - 1;
                else left = mid + 1;
            }
            else
            {
                if (nums[mid] < target && target <= nums[right]) left = mid + 1;
                else right = mid - 1;
            }
        }

        return -1;
    }
}
