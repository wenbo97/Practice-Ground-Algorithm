
namespace ArrayAlgorithm;

public class NumMagicSquaresInside
{
    /// <summary>
    /// [4, 3, 8, 4]
    /// [9, 5, 1, 9]
    /// [2, 7, 6, 2]
    /// [1, 2, 3, 4]
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static int NumMagicSquaresInsideBase(int[][] array)
    {
        int sequenceCount = 0;
        if (array.Length < 3 || array[0].Length < 3)
        {
            return sequenceCount;
        }

        int row = 0;
        int col = 0;
        int rowLength = array.Length;
        int colLength = array[0].Length;

        for (; row < rowLength - 2; row++)
        {
            col = 0;
            for (; col < colLength - 2; col++)
            {
                if (CheckUniqueDigitsInMatrix(array, row, col, rowLength, colLength))
                {
                    sequenceCount++;
                }
            }
        }


        return sequenceCount;
    }


    private static bool CheckUniqueDigitsInMatrix(
        int[][] array,
        int currentRow,
        int currentCol,
        int rowLength,
        int colLength)
    {
        HashSet<int> set = new(9);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int val = array[currentRow + i][currentCol + j];
                if (val < 1 || val > 9)
                {
                    return false;
                }
                if (set.Contains(val))
                {
                    return false;
                }
                else
                {
                    set.Add(val);
                }
            }
        }

        if (set.Count < 9)
        {
            return false;
        }
        // row
        // [0][0] | [1][0] | [2][0]
        if (array[currentRow][currentCol] + array[currentRow][currentCol + 1] + array[currentRow][currentCol + 2] != 15)
            return false;
        if (array[currentRow + 1][currentCol] + array[currentRow + 1][currentCol + 1] + array[currentRow + 1][currentCol + 2] != 15)
            return false;
        if (array[currentRow + 2][currentCol] + array[currentRow + 2][currentCol + 1] + array[currentRow + 2][currentCol + 2] != 15)
            return false;

        // column
        if (array[currentRow][currentCol] + array[currentRow + 1][currentCol] + array[currentRow + 2][currentCol] != 15)
            return false;
        if (array[currentRow][currentCol + 1] + array[currentRow + 1][currentCol + 1] + array[currentRow + 2][currentCol + 1] != 15)
            return false;
        if (array[currentRow][currentCol + 2] + array[currentRow + 1][currentCol + 2] + array[currentRow + 2][currentCol + 2] != 15)
            return false;

        if (array[currentRow][currentCol] + array[currentRow + 1][currentCol + 1] + array[currentRow + 2][currentCol + 2] != 15)
            return false;

        if (array[currentRow + 2][currentCol] + array[currentRow + 1][currentCol + 1] + array[currentRow][currentCol + 2] != 15)
            return false;

        return true;
    }
}
