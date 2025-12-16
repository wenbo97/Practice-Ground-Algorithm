using System;

namespace MaxArea;

public class LengthOfLongestSubstring
{
    /// <summary>
    /// pwwkew
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int LengthOfLongestSubstringBase(string s)
    {
        int len = s.Length;
        int slow = 0;
        int fast = 0;
        int maxWindow = 0;
        Dictionary<char, int> map = new();

        while (fast < len)
        {
            if (map.ContainsKey(s[fast]))
            {
                slow = Math.Max(slow, map[s[fast]] + 1);
            }
            map[s[fast]] = fast;
            maxWindow = Math.Max(maxWindow, fast - slow + 1);
            fast++;
        }
        return maxWindow;
    }

    public static int LengthOfLongestSubstringSet(string s)
    {
        int slow = 0;
        int fast = 0;
        int maxLen = 0;

        // A set to maintain the valid char sequence
        HashSet<char> set = new HashSet<char>();
        while (fast < s.Length)
        {
            if (set.Contains(s[fast]))
            {
                // set.Remove(s[fast]); // Incorrect - s[fast] is not in set.
                set.Remove(s[slow]);
                slow++;
            }
            else
            {
                set.Add(s[fast++]);
                maxLen = Math.Max(maxLen, set.Count);
            }
        }
        return maxLen;
    }
}
