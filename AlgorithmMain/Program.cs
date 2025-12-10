using SumOfTwo;

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

    public static void Main(string[] args)
    {
        SumOfTwoTest();
    }
}
