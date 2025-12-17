namespace MaxArea;

public class LengthOfLongestSubstringTwoDistinct
{
    public static int LengthOfLongestSubstringTwoDistinctBase(string s)
    {
        int maxLen = 0;
        int slow = 0;
        int fast = 0;
        Dictionary<char, int> map = new Dictionary<char, int>();
        while (fast < s.Length)
        {
            if (!map.ContainsKey(s[fast]))
            {
                if (map.Keys.Count < 3)
                {
                    map[s[fast]] = map.GetValueOrDefault(s[fast], 0) + 1;
                }

            }
        }

        return maxLen;
    }
}
