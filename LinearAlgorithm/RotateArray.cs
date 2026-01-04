namespace LinearAlgorithm;

public class RotateArray
{
    /// <summary>
    /// [1, 2, 3, 4, 5, 6, 7]
    /// </summary>
    /// <param name="nums"></param>
    /// <param name="k"></param>
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


    /// <summary>
    /// [1, 2, 3, 4, 5, 6, 7]
    /// </summary>
    /// <param name="nums"></param>
    /// <param name="k"></param>
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
}
