namespace SortSeries;

public class CountNegatives
{
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
}
