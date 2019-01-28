using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ListInversions.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var smallTest = GetValuesFromFile("smallTest.txt");
            long smallTestInversions = SortAndCountInversions(smallTest, 0, smallTest.Count - 1);
            Console.WriteLine("Total inversions in small file: " + smallTestInversions);

            var bigTest = GetValuesFromFile("bigTest.txt");
            long bigTestInversions = SortAndCountInversions(bigTest, 0, bigTest.Count - 1);
            Console.WriteLine("Total inversions in big file: " + bigTestInversions);

            var reversed = new List<int> { 5, 4, 3, 2, 1, 0 };
            long reversedInversions = SortAndCountInversions(reversed, 0, reversed.Count - 1);
            Console.WriteLine("Total inversions in reversed 6 entry array: " + reversedInversions);

            var lectureExample = new List<int> { 1, 3, 5, 2, 4, 6 };
            long lectureInversions = SortAndCountInversions(lectureExample, 0, lectureExample.Count - 1);
            Console.WriteLine("Total inversions in lecture example: " + lectureInversions);

            var has28Inversions = new List<int> { 8, 7, 6, 5, 4, 3, 2, 1 };
            long inversions28 = SortAndCountInversions(has28Inversions, 0, has28Inversions.Count - 1);
            Console.WriteLine("Total inversions in example with 28 inversions: " + inversions28);

            var sortedSet = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
            long shouldBe0 = SortAndCountInversions(sortedSet, 0, sortedSet.Count - 1);
            Console.WriteLine("Total inversions sorted set: " + shouldBe0);
        }

        private static List<int> GetValuesFromFile(string fileName)
        {
            var values = new List<int>();
            var path = Path.Combine(Environment.CurrentDirectory, fileName);
            var file = new StreamReader(path);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (int.TryParse(line, out var val)) values.Add(val);
            }
            return values;
        }

        private static long SortAndCountInversions(List<int> values, int first, int last)
        {
            int entries = last - first + 1;
            if (entries <= 1) return 0;

            int mid = (first + last) / 2;
            long leftInversions = SortAndCountInversions(values, first, mid);
            long rightInversions = SortAndCountInversions(values, mid + 1, last);
            long splitInversions = MergeAndCountSplitInversions(values, first, last);

            return leftInversions + rightInversions + splitInversions;
        }

        private static long MergeAndCountSplitInversions(List<int> values, int first, int last)
        {
            int length = last - first + 1;
            int mid = (first + last) / 2;
            int left = first;
            int right = mid + 1;
            int[] copy = new int[length];
            long inversions = 0;
            for (int i = 0; i < length; i++)
            {
                if (left > mid) //nothing left in left, copy from right
                {
                    copy[i] = values[right++];
                }
                else if (right > last) //nothing left in right, copy from left
                {
                    copy[i] = values[left++];
                }
                else if (values[left] <= values[right]) //left < right, copy from left
                {
                    copy[i] = values[left++];
                }
                else // right < left, copy from right, count inversion
                {
                    copy[i] = values[right++];
                    inversions += (mid - left + 1);
                }
            }
            if (inversions > 0)
            {
                int copyIndex = 0;
                for (int i = first; i <= last; i++)
                {
                    values[i] = copy[copyIndex++];
                }
            }
            return inversions;
        }
    }
}
