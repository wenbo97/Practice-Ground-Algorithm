[Link](https://leetcode.cn/problems/rotate-array/)

### Temp Array
```csharp
    public static void Rotate(int[] nums, int k)
    {
        int length = nums.Length;
        int[] copy = new int[length];
        int pos = 0;
        foreach (var e in nums)
        {
            copy[pos] = nums[pos];
            pos++;
        }
        for (int i = 0; i < length; i++)
        {
            int newPos = (i + k) % length;
            nums[newPos] = copy[i];
        }
    }
```

### In-Place solution
- For practices that are involving `reverse`, `parlindrome` or `pairwise swapping`, always start with a two-pointer approach using `while(left < right)`. DO NOT try to overcomplicate things with index math in a `for` loop.

##### Correct Version
```csharp
 public static void RotateNoNewArray(int[] nums, int k)
 {
     int start = 0; int end = nums.Length - 1;
     k = k % nums.Length;
     while (start < end)
     {
         nums[start] ^= nums[end];
         nums[end] ^= nums[start];
         nums[start] ^= nums[end];
         start++; end--;
     }

     start = 0;end = k - 1;
     while (start < end)
     {
         nums[start] ^= nums[end];
         nums[end] ^= nums[start];
         nums[start] ^= nums[end];
         start++; end--;
     }

     start = k; end = nums.Length - 1;
     while (start < end)
     {
         nums[start] ^= nums[end];
         nums[end] ^= nums[start];
         nums[start] ^= nums[end];

         start++;end--;
     }
 }
```

##### Incorrect Version
```csharp

    // This solution is incorrect and over complicate to use.
    for (int i = 0; i < nums.Length / 2; i++)
       {
           nums[i] ^= nums[nums.Length - i - 1];
           nums[nums.Length - i - 1] ^= nums[i];
           nums[i] ^= nums[nums.Length - i - 1];
       }

  
       for (int i = 0; i < k / 2; i++)
       {
           nums[i] ^= nums[k - i - 1];
           nums[k - i - 1] ^= nums[i];
           nums[i] ^= nums[k - i - 1];
       }

       for (int i = nums.Length - k - 1; i < (nums.Length - k) / 2; i++)
       {
           nums[i] ^= nums[nums.Length - k - 1]; // incorrect
           nums[nums.Length - k - 1] ^= nums[i];
           nums[i] ^= nums[nums.Length - k - 1];
       }
```

##### Overcomplicate Version
```csharp
public void Rotate(int[] nums, int k) {
    int n = nums.Length;
    k = k % n; // 必不可少！

    // 1. 翻转全部 [0, n-1]
    // 循环次数：总长度的一半
    for (int i = 0; i < n / 2; i++) 
    {
        // 左边：0 + i
        // 右边：(n - 1) - i
        nums[i] ^= nums[n - 1 - i];
        nums[n - 1 - i] ^= nums[i];
        nums[i] ^= nums[n - 1 - i];
    }

    // 2. 翻转前 k 个 [0, k-1]
    // 循环次数：k 的一半
    for (int i = 0; i < k / 2; i++) 
    {
        // 左边：0 + i
        // 右边：(k - 1) - i
        nums[i] ^= nums[k - 1 - i];
        nums[k - 1 - i] ^= nums[i];
        nums[i] ^= nums[k - 1 - i];
    }

    // 3. 翻转剩余部分 [k, n-1] <-- 这里的数学最麻烦
    // 区间长度：n - k
    // 循环次数：(n - k) / 2
    for (int i = 0; i < (n - k) / 2; i++) 
    {
        // 关键在这里！
        // 左指针起点是 k，偏移量是 i -> (k + i)
        // 右指针起点是 n-1，偏移量是 i -> (n - 1 - i)
        
        int left = k + i; 
        int right = n - 1 - i;

        nums[left] ^= nums[right];
        nums[right] ^= nums[left];
        nums[left] ^= nums[right];
    }
}
```