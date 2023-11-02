using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab5
{
    internal class RecreationOfDistribution : InitialData
    {
        public static List<List<int>> NewDistrib()
        {
            List<List<int>> newDistr = new List<List<int>>(); //самый важный список в этом методе
            for (int i = 0; i < height; i++)
            {
                newDistr.Add(new List<int>());
                for (int j = 0; j < width; j++)
                {
                    newDistr[i].Add(0);
                }
            }

            Point basePoint = new Point();
            basePoint.X = 0;
            basePoint.Y = 0;

            newDistr[basePoint.Y][basePoint.X] += 1;

            Stopwatch time = new Stopwatch(); //время
            time.Start(); //время

            int sumGenerN = 1;

            while (CodeParallelization.ParallTermCond(newDistr, sumGenerN))
            //for (int opp = 0; opp < height * width * 255; opp++)
            {
                List<Point> coordsList = CoordinatesList(basePoint.X, basePoint.Y);

                List<double> deltaRo = new List<double>();
                for (int i = 0; i < coordsList.Count; i++)
                {
                    int n = newDistr[coordsList[i].Y][coordsList[i].X];
                    //double nowRo = NewRo(n, SumOfDataInFlux(newDistr));
                    double nowRo = NewRo(n, /*CodeParallelization.ParallSum(newDistr)*/ sumGenerN);
                    double refRo = ReferDistributionDataList[coordsList[i].Y][coordsList[i].X];
                    deltaRo.Add(refRo - nowRo);
                }
                int indexOfMax = deltaRo.IndexOf(deltaRo.Max());
                basePoint = coordsList[indexOfMax];

                newDistr[basePoint.Y][basePoint.X] += 1;

                sumGenerN++;
            }

            //Console.WriteLine($"deltaSum / SumReferData < distinction ? = {!CodeParallelization.ParallTermCond(newDistr)}");

            time.Stop(); //время
            double myTime = time.ElapsedMilliseconds / 1000.0; //время
            Console.WriteLine($"общее время = {myTime} сек"); //время
            mySeconds.Add(myTime);

            return newDistr;
        }

        public static List<double> mySeconds = new List<double>();

        private static List<Point> CoordinatesList(int x, int y)
        {
            List<Point> coordsList = new List<Point>();
            int cntrPts = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (x + i < 0 || y + j < 0 || x + i >= width || y + j >= height || (x + i == x && y + j == y))
                    {
                        continue;
                    }
                    coordsList.Add(new Point(x, y));

                    Point bufferPoint = coordsList[cntrPts];
                    bufferPoint.X += i;
                    bufferPoint.Y += j;
                    coordsList[cntrPts] = bufferPoint;
                    cntrPts++;
                }
            }
            return coordsList;
        }

        private static double NewRo(int n, int sumN)
        {
            return Convert.ToDouble(n * 1.0 / sumN);
        }

    }
}
