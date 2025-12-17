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
        int slow = 0;
        int fast = 0;
        int maxSubLen = 0;

        Dictionary<char, int> postionMap = new Dictionary<char, int>();

        while (fast < s.Length)
        {
            char c = s[fast];

            if (postionMap.ContainsKey(c) && postionMap[c] > slow)
            { 
                slow = postionMap[c] + 1;
            }
            postionMap[c] = fast;
            maxSubLen = Math.Max(maxSubLen, fast - slow + 1);
            fast++;
        } 
        return maxSubLen;
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
