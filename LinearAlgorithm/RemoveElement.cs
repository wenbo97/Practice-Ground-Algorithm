namespace LinearAlgorithm
{
    public class RemoveElement
    {
        public static int RemoveElementBase(int[] nums, int val)
        {
            int newLength = 0;

            if (nums.Length <= 1)
            {
                return 1;
            }

            int slow = 0;
            int fast = 1;
            Array.Sort(nums);
            // [2,2,3,3]
            while (fast < nums.Length)
            {
                if (nums[fast] == nums[slow])
                {
                    slow++;
                }
                else
                {
                    nums[slow] = nums[fast];
                    slow++;
                    newLength++;
                }
                fast++;
            }

            return newLength;
        }

        public static int RemoveElement1231(int[] nums, int val)
        {
            int slow = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] != val)
                {
                    nums[slow] = nums[i];
                    slow++;
                }
            }

            return slow;
        }
    }
}
