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
            char c = s[fast];

            if (map.ContainsKey(c))
            {
                map[c] = map[c] + 1;
            }
            else
            {
                map[c] = 1;
            }

            while (map.Keys.Count > 2)
            {
                map[s[slow]]--;
                if (map[s[slow]] == 0)
                {
                    map.Remove(s[slow]);
                }
                slow++;
            }

            maxLen = Math.Max(maxLen, fast - slow + 1);
            fast++;
        }

        return maxLen;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s">Target string</param>
    /// <returns>Maximum of the substring</returns>
    public static int LengthOfLongestSubstringTwoDistinctViaArray(string s)
    {
        int maxLen = 0;
        int slow = 0;
        int fast = 0;

        int[] map = new int[128];
        Array.Fill(map, 0);
        int distinctCharCount = 0;

        while (fast < s.Length)
        {
            char fastChar = s[fast];
            if (map[fastChar] == 0)
            {
                distinctCharCount++;
            }
            map[fastChar]++;

            while (distinctCharCount > 2)
            {
                char slowChar = s[slow];
                map[slowChar]--;
                if (map[slowChar] == 0)
                {
                    distinctCharCount--;
                }
                slow++;
            }
            fast++;
            maxLen = Math.Max(maxLen, fast - slow);
        }
        return maxLen;
    }
}
