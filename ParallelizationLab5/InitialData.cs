using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab5
{
    internal class InitialData
    {
        public static int Fluxes = 0;

        public static List<List<int>> ReferenceDataList = null;
        public static int height = 0;
        public static int width = 0;
        public static int SumReferData = 0;
        public static List<List<double>> ReferDistributionDataList = null;

        /// <summary>
        /// Начальные условия для эталонного распределения
        /// </summary>
        /// <param name="referenceDistributionList"></param>
        public static void InitialConditions(List<List<int>> referenceDistributionList, int flux)
        {
            Fluxes = flux;

            ReferenceDataList = referenceDistributionList;
            height = ReferenceDataList.Count;
            width = ReferenceDataList[0].Count;

            SumReferData = SumOfDistr(ReferenceDataList);
            ReferDistributionDataList = NewNormDistr(ReferenceDataList, SumReferData);

            Console.WriteLine($"Sum all bytes = {SumReferData}");
            Console.WriteLine($"height = {height}");
            Console.WriteLine($"width = {width}");
        }

        private static List<List<double>> NewNormDistr(List<List<int>> myList, int sumN)
        {
            int height = myList.Count;
            int width = myList[0].Count;

            List<List<double>> normalizedDistribution = new List<List<double>>();
            for (int i = 0; i < height; i++)
            {
                normalizedDistribution.Add(new List<double>());
                for (int j = 0; j < width; j++)
                {
                    normalizedDistribution[i].Add(Convert.ToDouble((myList[i][j] * 1.0) / sumN));
                }
            }

            return normalizedDistribution;
        }

        public static int SumOfDistr(List<List<int>> myList)
        {
            int sumAllN = 0;
            for (int i = 0; i < height; i++)
            {
                sumAllN += myList[i].Sum();
            }
            return sumAllN;
        }
    }
}
