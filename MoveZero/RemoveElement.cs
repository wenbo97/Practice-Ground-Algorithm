namespace MoveZero;

public class RemoveElement
{
    public static int RemoveElementByLoop(int[] nums, int val)
    {
        int indexPos = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] != val)
            {
                nums[indexPos] = nums[i];
                indexPos++;
            }
        }

        Console.WriteLine("End. IndexPos = {0}", indexPos);
        return indexPos;
    }

    // Not Correct
    // 1.IndexPos is the unique boundary, it should only move one more step if the new number is found.
    // 2.Logic error - the unique element will be override.
    public static int RemoveDuplicatesByLoop(int[] nums)
    {
        int indexPos = 0;

        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[indexPos] != nums[i])
            {
                nums[indexPos] = nums[i];
                indexPos++;
            }
            else
            {
                indexPos = i;
            }

        }
        Console.WriteLine("End. IndexPos = {0}", indexPos);
        return indexPos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nums"></param>
    /// <returns></returns>
    public static int RemoveDuplicateByCorrectLoop(int[] nums)
    {
        if (nums.Length == 0) return 0;
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
}
