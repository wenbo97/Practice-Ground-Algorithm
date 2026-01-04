[Link](https://leetcode.cn/problems/count-negative-numbers-in-a-sorted-matrix/description/?envType=daily-question&envId=2025-12-28)
```csharp
    public static int CountNegativesBase(int[][] grid)
    {
        int result = 0;
        int col = grid[0].Length - 1;
        int row = 0;
  
        while (col > 0 && row < grid.Length)
        {
            if (grid[row][col] <= 0)
            {
                result += grid.Length - col;
                col--;
            }
            else
            {
                row++;
            }
        }
        return result;
    }
```

**Error in above solution:**
1. `result += grid.Length - col;` we should calculate by `grid.Length - row` **NOT** `grid.Length - col`. Because, if current `grid[row][col] < 0`, `grid[0][col]` to `grid[<last-row>][col]` are all less then zero.
2.  `if (grid[row][col] <= 0)` - this will add when `grid[row][col] == 0` which is incorrect. Only add when `grid[row][col] < 0`
3. col will never scan on `grid[row][0]` column. Modify `while (col > 0 && row < grid.Length)` to `while (col **>=** 0 && row < grid.Length)`

**Correct Version:**
```csharp
    /// <summary>
    /// [4,3,2,-1]
    /// [3,2,1,-1]
    /// [1,1,-1,-2]
    /// [-1,-1,-2,-3]
    /// </summary>
    /// <param name="grid"></param>
    /// <returns></returns>
    public static int CountNegativesBase(int[][] grid)
    {
        int result = 0;

        int col = grid[0].Length - 1;
        int row = 0;

        while (col >= 0 && row < grid.Length)
        {
            if (grid[row][col] < 0)
            {
                Console.WriteLine(grid[row][col]);
                result += grid.Length - row;
                col--;
            }
            else
            {
                row++;
            }
        }
        return result;
    }
```