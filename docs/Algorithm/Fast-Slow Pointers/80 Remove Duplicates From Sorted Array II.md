```csharp
    public int RemoveDuplicates(int[] nums) {
        if (nums.Length <= 2)
        {
            return nums.Length;
        }
        
        int slow = 2;
        int fast = 2;
        while (fast < nums.Length)
        {
            if (nums[slow - 2] != nums[fast])
            {
                nums[slow] = nums[fast];
                slow++;
            }
            fast++;
        }
        return slow;
    }
```