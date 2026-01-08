**Map Solution**
```csharp
    /// <summary>
    /// pwwkew | abba | abcdef | abcabcbb
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static int LengthOfLongestSubstringBase(string s)
    {
        char[] chars = s.ToCharArray();
        if (chars.Length == 0)
        {
            return 0;
        }
        if (chars.Length == 1)
        {
            return 1;
        }

        if (chars.Length <= 2)
        {
            return chars[0] == chars[1] ? 1 : 2;
        }

        int low = 0;
        int fast = 0;
        int maxLen = 0;
        Dictionary<char, int> map = new Dictionary<char, int>();
        for (; fast < chars.Length;)
        {
            char c = chars[fast];
            if (map.ContainsKey(c))
            {
                low = Math.Max(map[c] + 1, low);
            }
            map[c] = fast;
            maxLen = Math.Max(maxLen, fast - low + 1);
            fast++;

        }
        return maxLen;
    }
```

**For Hashset:**
```csharp
    int slow = 0;
    int fast = 0;
    int maxLen = 0;
    HashSet<char> set = new HashSet<char>();
    while (fast < s.Length)
    {
        if (!set.Contains(s[fast]))
        {
            set.Add(s[fast++]);
            maxLen = Math.Max(maxLen, set.Count);
        }
        else
        {
            set.Remove(s[slow]);
            slow++;
        }
    }
    return maxLen;
```