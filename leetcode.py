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
