[Link](https://leetcode.cn/problems/remove-element/)
```csharp
        public static int RemoveElement1231(int[] nums, int val)
        {
            int slow = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] != val)
                {
                    nums[slow] = nums[i];
                    slow++;
                }
            }

            return slow;
        }
```