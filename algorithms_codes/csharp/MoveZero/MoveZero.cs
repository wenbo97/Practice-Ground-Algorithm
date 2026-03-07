namespace MoveZero;

public class MoveZero
{
    public static void MoveZeroesWithTwoPointer(int[] nums)
    {
        int fast = 0;
        int slow = 0;
        int len = nums.Length;

        while (fast < len)
        {
            if (nums[fast] != 0)
            {
                if (fast != slow)
                {
                    int temp = nums[slow];
                    nums[slow] = nums[fast];
                    nums[fast] = temp;
                }
                slow++;
            }
            fast++;
        }
    }

    public static void MoveZeroWithLoop(int[] nums)
    {
        int indexPos = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] != 0)
            {
                nums[indexPos++] = nums[i];
            }
        }

        while (indexPos < nums.Length)
        {
            nums[indexPos++] = 0;
        }
    }
}
