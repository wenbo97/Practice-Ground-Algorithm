using MaxArea_SlidingWindow;
using MoveZero;
using SortSeries;

namespace AlgorithmMain;

public class Program
{
    public static void SumOfTwoTest()
    {
        //int[] result = SumOfTwo.SumOfTwo.TwoSumO2([2, 7, 11, 15], 9);
        //int[] result = SumOfTwo.SumOfTwo.TwoSumMap([3, 2, 4], 6);
        int[] result = SumOfTwo.SumOfTwo.TwoSumOneLoopMap([3, 2, 4], 6);

        foreach (int res in result)
        {
            Console.WriteLine(res);
        }
    }

    public static void MoveZeroTest()
    {
        int[] array = [0, 1, 0, 3, 12];

        //MoveZero.MoveZero.MoveZeroesWithTwoPointer(array);
        MoveZero.MoveZero.MoveZeroWithLoop(array);

        foreach (var val in array)
        {
            Console.Write(val + " ");
        }
    }

    public static void RemoveElementTest()
    {
        //int[] nums = [3, 2, 2, 3];// val = 3
        int[] nums = [0, 1, 2, 2, 3, 0, 4, 2];// val = 2
        RemoveElement.RemoveElementByLoop(nums, 2);
        foreach (var val in nums)
        {
            Console.Write(val + " ");
        }
    }

    public static void RemoveDuplicatesTest()
    {
        //int[] nums = [1, 1, 2];
        int[] nums = [0, 0, 1, 1, 1, 2, 2, 3, 3, 4];
        RemoveElement.RemoveDuplicateByCorrectLoop(nums);
        foreach (var val in nums)
        {
            Console.Write(val + " ");
        }
    }

    public static void MergeTest()
    {
        //int[] nums = [1, 1, 2];
        int[] nums1 = [1, 2, 3, 0, 0, 0];
        int[] nums2 = [2, 5, 6];
        Merge.MergeByLoop(nums1, 3, nums2, 3);
        foreach (var val in nums1)
        {
            Console.Write(val + " ");
        }
    }

    public static void MaxAreaTest()
    {
        //int[] nums = [1, 1, 2];
        int[] nums1 = [1, 8, 6, 2, 5, 4, 8, 3, 7];

        int result = MaxArea.MaxAreaBase(nums1);
        Console.WriteLine(result);
    }


    public static void LengthOfLongestSubstringTest()
    {
        //int[] nums = [1, 1, 2];
        //string s = "abcabcbb";
        string s = "pwwkew";

        int result = LengthOfLongestSubstring.LengthOfLongestSubstringBase(s);
        Console.WriteLine(result);
    }

    public static void CountNegativeNumbersInASortedMatrix()
    {
        //int[] nums = [1, 1, 2];
        //string s = "abcabcbb";
        int[][] grid = [
            [4,3,2,-1],
            [3,2,1,-1],
            [1,1,-1,-2],
            [-1,-1,-2,-3]
        ];

        int result = CountNegatives.CountNegativesBase(grid);
        Console.WriteLine(result);
    }

    public static void Main(string[] args)
    {
        //SumOfTwoTest();
        //MoveZeroTest();
        //RemoveElementTest();
        //RemoveDuplicatesTest();
        //MergeTest();
        //MaxAreaTest();
        //LengthOfLongestSubstringTest();
        CountNegativeNumbersInASortedMatrix();
    }
}
