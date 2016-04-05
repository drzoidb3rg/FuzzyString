using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzyString;

namespace FuzzyStringConsole
{
    class Program
    {
        private static IEnumerable<string> Titles()
        {
            yield return  "Sclater's monkey in Nigeria: Outlook good or just hanging on? Ecological and human dimensions of hunted and sacred populations.";
            yield return  "Molecular cloning and pharmacological characterization of monkey MT1 and MT2 melatonin receptors showing high affinity for the agonist ramelteon.";
            yield return  "sclaters monkey on nigeria, outlook good or bad. Ecological and human dimensions in hunting sacred populations";

        }
        static void Main(string[] args)
        { 
            string kevin = "kevin";
            string kevyn = "kevyn";

            var title1 = "Sclater's monkey in Nigeria: Outlook good or just hanging on? Ecological and human dimensions of hunted and sacred populations.";
            var title2 = "Molecular cloning and pharmacological characterization of monkey MT1 and MT2 melatonin receptors showing high affinity for the agonist ramelteon.";
            //var title3 = "sclaters monkey on nigeria, outlook good or bad. Ecological and human dimensions in hunting sacred populations";
            var title3 = "Pubmed bla bla : Sclater's monkey in Nigeria: Outlook good or just hanging on? Ecological and human dimensions of hunted and sacred populations.";



            var timer = new Stopwatch();
            timer.Start();
            var dataList = new List<string>();
            for (var i = 0; i < 334; i++)
            {
                dataList.AddRange(Titles().ToList());
            }
            List<FuzzyStringComparisonOptions> options = new List<FuzzyStringComparisonOptions>();
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
            options.Add(FuzzyStringComparisonOptions.CaseSensitive);
            var optArray = options.ToArray();
            var compCount = 0;
            var loopCount = 0;
            var length = dataList.Count;
            //for (var outer = 0; outer < dataList.Count; outer++)
            Enumerable.Range(900, length - 1).AsParallel().ForAll(outer =>
                {
                    for (var inner = outer + 1; inner < dataList.Count; inner++)
                    {
                        var outerString = dataList[outer];

                        //var result = outerString.ApproximatelyEquals(dataList[inner],
                        //                                                 FuzzyStringComparisonTolerance.Normal,
                        //                                                 optArray);

                        //var result = outerString == dataList[inner];
                        var distance2 = EditDistance(outerString, dataList[inner]);

                        compCount++;
                        loopCount++;
                    }
                    //Console.WriteLine(loopCount);
                    loopCount = 0;
                });
            Console.WriteLine("Compared new records in {0}", timer.ElapsedMilliseconds);
            Enumerable.Range(0, 899).AsParallel().ForAll(outer =>
                {
                    var outerString = dataList[outer];
                    for (var inner = 900; inner < dataList.Count; inner++)
                    {
                        //var result = outerString == dataList[inner];
                        //var result = outerString.ApproximatelyEquals(dataList[inner],
                        //                                                 FuzzyStringComparisonTolerance.Normal,
                        //                                                 optArray);

                        var distance3 = EditDistance(outerString, dataList[inner]);

                        compCount++;
                        loopCount++;
                    }
                    //Console.WriteLine(loopCount);
                    loopCount = 0;
                });
            Console.WriteLine(compCount);

            timer.Stop();
            Console.WriteLine("Took {0} ms", timer.ElapsedMilliseconds);

            var timer2 = new Stopwatch();
            timer2.Start();
            Console.WriteLine("match");
            //Console.WriteLine(title1.ApproximatelyEquals(title3, FuzzyStringComparisonTolerance.Normal, optArray.ToArray()));
            //Console.WriteLine(title1.ApproximatelyEquals(title3, FuzzyStringComparisonTolerance.Strong, options.ToArray()));

            var distance = EditDistance(title1, title3);

            Console.WriteLine("dist is : {0}", distance );

            Console.WriteLine("fuzzy time {0}", timer2.ElapsedMilliseconds);

            Console.WriteLine("no match");
            Console.WriteLine(title1.ApproximatelyEquals(title2, FuzzyStringComparisonTolerance.Normal, options.ToArray()));
            Console.WriteLine(title1.ApproximatelyEquals(title2, FuzzyStringComparisonTolerance.Strong, options.ToArray()));



            Console.ReadLine();
        }

        public static int EditDistance(string original, string modified)
        {
            int len_orig = original.Length;
            int len_diff = modified.Length;

            var matrix = new int[len_orig + 1, len_diff + 1];
            for (int i = 0; i <= len_orig; i++)
                matrix[i, 0] = i;
            for (int j = 0; j <= len_diff; j++)
                matrix[0, j] = j;

            for (int i = 1; i <= len_orig; i++)
            {
                for (int j = 1; j <= len_diff; j++)
                {
                    int cost = modified[j - 1] == original[i - 1] ? 0 : 1;
                    var vals = new int[] {
    matrix[i - 1, j] + 1,
    matrix[i, j - 1] + 1,
    matrix[i - 1, j - 1] + cost
   };
                    matrix[i, j] = vals.Min();
                    if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                }
            }
            return matrix[len_orig, len_diff];
        }
    }
}
