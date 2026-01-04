[Link](https://leetcode.cn/problems/move-zeroes/)


```csharp
    public void MoveZeroes(int[] nums) {
        int fast = 0;
        int slow = 0;
        
        while(fast < nums.Length){
            if(nums[fast] != 0){
                int temp = nums[fast];
                nums[fast] = nums[slow];
                nums[slow] = temp;
                slow++;
            }
            fast++;
        }
    }
```