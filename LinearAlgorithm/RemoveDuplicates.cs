namespace LinearAlgorithm;

public class RemoveDuplicates
{
    public static int RemoveDuplicatesBase(int[] nums)
    {
        int slow = 1;
        int fast = 1;

        while (fast < nums.Length)
        {
            if (nums[fast] != nums[fast - 1])
            {
                nums[slow] = nums[fast];
                slow++;
            }
            fast++;
        }
        return slow;
    }
}
