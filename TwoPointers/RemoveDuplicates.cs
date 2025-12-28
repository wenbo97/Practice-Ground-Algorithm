namespace TwoPointers;

public class RemoveDuplicates
{
    public static int RemoveDuplicatesBase(int[] nums)
    {
        int newLength = 2;

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
                newLength++;
            }
            fast++;
        }

        return newLength;
    }
}
