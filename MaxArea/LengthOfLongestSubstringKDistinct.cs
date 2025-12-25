namespace MaxArea;

public class LengthOfLongestSubstringKDistinct
{
    public static int LengthOfLongestSubstringKDistinctBase(string s, int k)
    {
        int maxLen = 0;
        int slow = 0;
        int fast = 0;

        Dictionary<char, int> map = new();

        while (fast < s.Length)
        {
            char c = s[fast];
            if (map.ContainsKey(c))
            {
                map[c] = map[c] + 1;
            }
            else
            {
                map[c] = 1;
            }

            while (map.Keys.Count > k)
            {
                char charFromSlowPos = s[slow];
                map[charFromSlowPos]--;
                if (map[charFromSlowPos] == 0)
                {
                    map.Remove(charFromSlowPos);
                }
                slow++;
            }

            maxLen = Math.Max(maxLen, fast - slow + 1);
            fast++;
        }
        return maxLen;
    }
}
