# Python question from LeetCode
from typing import List


class ListNode:
    def __init__(self, x):
        self.val = x
        self.next = None


class Solution:
    # nums = [2,7,11,15], target = 9
    def twoSum(self, nums: List[int], target: int) -> List[int]:
        dict = {}
        for index, val in enumerate(nums):
            subtraction = target - val
            if subtraction in dict:
                return [index, dict[subtraction]]
            dict[val] = index
        return [-1, -1]

    # Input: head = [3,2,0,-4], pos = 1
    def hasCycle(self, head: ListNode) -> bool:
        memo = set()

        while head != None:
            if head in memo:
                return True
            else:
                memo.add(head)
                head = head.next

        return False

    # Input: nums1 = [1,2,3,0,0,0], m = 3, nums2 = [2,5,6], n = 3
    # Output: [1,2,2,3,5,6]
    def merge(self, nums1: List[int], m: int, nums2: List[int], n: int) -> None:
        end = m + n - 1

        while m != 0 and n != 0:
            if nums1[m - 1] > nums2[n - 1]:
                nums1[end] = nums1[m - 1]
                m -= 1
            else:
                nums1[end] = nums2[n - 1]
                n -= 1
            end -= 1

        while n > 0:
            nums1[end] = nums2[n - 1]
            end -= 1
            n -= 1

    # Input: nums = [0,0,1,1,1,2,2,3,3,4]
    # Output: 5, nums = [0,1,2,3,4,_,_,_,_,_]
    def removeDuplicates(self, nums: List[int]) -> int:
        my_set = set()
        left = 0

        for index, value in enumerate(nums):
            if value not in my_set:
                (nums[left], nums[index]) = (nums[index], nums[left])
                left += 1
                my_set.add(value)

        return left

    def removeDuplicates_II(self, nums: List[int]) -> int:
        my_dict = dict()
        left = 0

        for index, value in enumerate(nums):
            if value not in my_dict or (_ := my_dict.get(value)) == 1:
                (nums[left], nums[index]) = (nums[index], nums[left])
                left += 1

                my_dict.setdefault(value, 0)
                my_dict[value] += 1

        return left

    def majorityElement(self, nums: List[int]) -> int:
        my_dict = dict()

        for num in nums:
            my_dict.setdefault(num, 0)
            my_dict[num] += 1

        return max(my_dict, key=my_dict.get)

    # list1 = nums[-3:]
    # list2 = nums[: len(nums) - k]
    def rotate(self, nums: List[int], k: int) -> None:
        length = len(nums)
        k %= length
        nums[::] = nums[-k:] + nums[:-k]

    # [7, 6, 4, 3, 1] - time limit exceed exception
    # [7, 1, 5, 3, 6, 4]
    def maxProfit(self, prices: List[int]) -> int:
        (min_price, max_price) = (float("inf"), 0)
        max_profit = 0
        n = len(prices)

        for i in range(n):
            min_price = min(min_price, prices[i])
            max_price = 0

            for j in range(i + 1, n):
                max_price = max(max_price, prices[j])

            max_profit = max(max_profit, max_price - min_price)

        return max_profit

    def maxProfit_II(self, prices: List[int]) -> int:
        min_price = float("inf")
        max_profit = 0

        for price in prices:
            min_price = min(min_price, price)
            max_profit = max(max_profit, price - min_price)

        return max_profit
