[Link]()

But the sliding window rule follows the **First In, First Out**(FIFO) principle, just like a queue.

Use map to save sliding window
```csharp
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

            while(map.Keys.Count > 2)
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
```