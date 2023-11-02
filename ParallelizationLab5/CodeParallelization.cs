using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab5
{
    internal class CodeParallelization : InitialData
    {
        /// <summary>
        /// Метод для параллельного расчета условия завершения программы
        /// </summary>
        public static bool ParallTermCond(List<List<int>> myList, int newSumN)
        {
            //Stopwatch time = new Stopwatch(); //время
            //time.Start(); //время

            Task<double>[] tasks = new Task<double>[Fluxes]; //массив для созданных потоков

            int range = height / Fluxes;

            for (int i = 0; i < Fluxes; i++)
            {
                var leftBor = range * i;
                var rightBor = range * (i + 1);

                if (i == Fluxes - 1) //нет потерь
                {
                    rightBor = height;
                }

                tasks[i] = Task.Run(() => TermCondWithBorders(leftBor, rightBor, myList, newSumN)); //создаем и запускаем новый поток с функцией расчета
            }

            Task.WaitAll(tasks);

            double deltaSum = 0.0;
            for (int i = 0; i < tasks.Length; i++)
            {
                deltaSum += tasks[i].Result;
            }

            const double distinction = 164580.9346; // - как задавать?

            //time.Stop(); //время
            //double myTime = time.ElapsedMilliseconds / 1000.0; //время
            //Console.WriteLine($"время метода ParallTermCond = {myTime} сек"); //время

            if (deltaSum < distinction)
            {
                Console.WriteLine($"deltaSum = {deltaSum}");
                return false;
            }
            else
            {
                return true;
            }
            //if (deltaSum <= distinction)
            //{
            //    return false;
            //}
        }

        /// <summary>
        /// Сами параллельные расчеты суммы дельт
        /// </summary>
        private static double TermCondWithBorders(int leftBor, int rightBor, List<List<int>> newDistr, int newSumN) //это долго считается
        {
            double deltaSum = 0.0;

            //int newSumN = ParallSum(newDistr);

            double coefDivN = Convert.ToDouble(SumReferData * 1.0 / newSumN);

            for (int i = leftBor; i < rightBor; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    deltaSum += Math.Abs((ReferenceDataList[i][j] * 1.0) - (coefDivN * 1.0) * newDistr[i][j]);
                }
            }

            return deltaSum;
        }

    }
}
