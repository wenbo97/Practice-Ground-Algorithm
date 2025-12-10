[Link](https://leetcode.cn/problems/remove-duplicates-from-sorted-array/description/)

```csharp
    public static int RemoveDuplicateByCorrectLoop(int[] nums)
    {
        if(nums.Length == 0) return 0;
        int slow = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[slow] != nums[i])
            {
                nums[++slow] = nums[i];
            }
        }

        Console.WriteLine("End. 'slow' Index = {0}", slow);
        return slow + 1;
    }
```

## Question

- Why won't the slow pointer go out of bounds
	* `slow` will always **slow** or **equal** `fast` - i pointer has defined in loop code segment.
	* And `fast` - i pointer will never over `nums.length`