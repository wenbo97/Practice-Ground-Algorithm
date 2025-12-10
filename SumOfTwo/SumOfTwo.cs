namespace SumOfTwo;

public class SumOfTwo
{
    public static int[] TwoSumO2(int[] nums, int target)
    {
        int len = nums.Length;
        int[] res = { 0, 0 };
        for (int i = 0; i < len; i++)
        {
            for (int j = i + 1; j < len; j++)
            {
                if (nums[i] + nums[j] == target)
                {
                    res[0] = i;
                    res[1] = j;
                }
            }
        }
        return res;
    }

    public static int[] TwoSumMap(int[] nums, int target)
    {
        int len = nums.Length;
        int[] res = { 0, 0 };

        Dictionary<int, int> map = new Dictionary<int, int>();

        for (int i = 0; i < len; i++)
        {
            int diff = target - nums[i];
            map.TryAdd(diff, i);
        }

        for (int i = 0; i < len; i++)
        {
            int diffPos = map.GetValueOrDefault(nums[i], -1);
            // attention - the position of diff from current value{nums[i]} cannot same as current value{nums[i]}, or we will return a same position
            // like [3,3] target = 6, without `diffPos != i` condition will cause return [0,0]
            if (diffPos != -1 && diffPos != i)
            {
                res[0] = diffPos;
                res[1] = i;
                return res;
            }
        }

        return res;
    }

    public static int[] TwoSumOneLoopMap(int[] nums, int target)
    {
        int len = nums.Length;
        Dictionary<int, int> map = new Dictionary<int, int>();

        for (int i = 0; i < len; i++)
        {
            int completion = target - nums[i];
            if (map.ContainsKey(completion))
            {
                return new int[] { map[completion], i };
            }
            map.TryAdd(nums[i], i);
        }

        return new int[] { 0, 0 };
    }
}