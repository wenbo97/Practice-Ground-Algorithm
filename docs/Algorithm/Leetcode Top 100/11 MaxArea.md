[Link](https://leetcode.cn/problems/container-with-most-water/submissions/685211773/)

```csharp
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
```

## Core Thoughts

Given an array `height`, where each element represents the height of a vertical line. Choose any two lines `(i,j)` with `i < j` together with the x-axis, they form a container.

The amount of water the container can hold(i.e the area) is determined by two factors:
```
Area = (j-i) x min(height[i],height[j])
```
`(j - i)` - the width of the container.
`min(height[i],height[j])` - the water level limit (the shorter line)

**Constraints**
- The water level is always determined by the shorter side.
- Under a fixed pair `(i, j)`:
	- Increasing the taller side does **not** increase the area.
	- To increase the area, the only option is to increase the **shorter side**.

Why the Brute Force Solution is $$O{(n^2)}$$
1. Enumerate all possible `(i, j)` pairs.
2. Compute the area for each pair.
3. No pruning is used.

However, in reality, a large number of states are **mathematically proven** to be impossible optimal solutions.

## Core Logic of the Two-Pointer Strategy

Initialization
```
start = 0
end = n - 1
```
After computing the current area at each step:
* If `height[start] < height[end]`:
	* The current limiting side (shorter side) is `start`.
	* If `start` does not move, the water level will always be $\leq$ `height[start]`
	* If we move `end--`:
		* The width decreases.
		* The height of water area cannot exceed `height[start]`.
	* Therefore, the area will **definitely decrease**, regardless of how tall `height[end - 1]` is. - 👉 The only valid move is `start++`.
* If `height[start] >= height[end]`:
	* Symmetric logic applies. 👉 The only valid move is `end--`

### Why an "Extremely Tall Line" Cannot Compensate

**Key Conclusion**
* As long as the shorter side remains unchanged, increasing the other side is useless.
* Decreasing width + unchanged water level => area strictly decreases.
* Therefore:
	* Whether the array is ordered is irrelevant.
	* "There is a very tall line later" cannot justify not moving the shorter side.