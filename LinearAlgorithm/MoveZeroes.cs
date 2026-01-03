namespace LinearAlgorithm;

public class MoveZeroes
{
    public static void MoveZeroesBase(int[] nums)
    {
        int slow = 0;
        int fast = 0;

        while (fast < nums.Length)
        {
            if (nums[fast] != 0)
            {
                // int temp = nums[fast];
                // nums[fast] = nums[slow];
                // nums[slow] = temp;
                (nums[slow], nums[fast]) = (nums[fast], nums[slow]);
                slow++;
            }
            fast++;
        }
    }

    public static void MoveZeroesBase2(int[] nums)
    {
        Span<int> span = stackalloc int[nums.Length];
    }
}
