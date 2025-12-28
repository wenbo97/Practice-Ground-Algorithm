namespace MaxArea_SlidingWindow;

public class MaxArea
{
    public static int MaxAreaBase(int[] height)
    {
        int start = 0;
        int end = height.Length - 1;

        int maxArea = -1;

        while (start < end)
        {
            int curHigh = Math.Min(height[start], height[end]);
            int curWide = end - start;

            maxArea = Math.Max((curHigh * curWide), maxArea);
            if (height[start] < height[end])
            {
                start++;
            }
            else
            {
                end--;
            }
        }


        return maxArea;
    }
}
