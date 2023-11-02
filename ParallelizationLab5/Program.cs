using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelizationLab5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int fluxes = 1;

            //var dataList = ReadingFile.DataFromImage("SmallReferencePic.bmp");
            //var dataList = ReadingFile.DataFromImage("RefPattern.bmp");
            var dataList = ReadingFile.DataFromImage("SmallCat.bmp");
            //var dataList = ReadingFile.DataFromImage("ReferenceCat.bmp");
            //var dataList = ReadingFile.DataFromImage("FullBlack.bmp");
            //var dataList = ReadingFile.DataFromImage("FullWhite.bmp");

            InitialData.InitialConditions(dataList, fluxes);

            //List<List<int>> newData = null;
            //for (int i = 0; i < 12; i++)
            //{
            //    InitialData.Fluxes = i + 1;
            //    Console.Write($"потоков = {InitialData.Fluxes};\t");
            //    for (int j = 0; j < 10; j++)
            //    {
            //        newData = RecreationOfDistribution.NewDistrib();
            //    }
            //    //WritingFile.ImageForDistribution(newData);
            //}
            //WritingFile.AvergeTimes(RecreationOfDistribution.mySeconds, "Time");

            List<List<int>> newData = null;
            InitialData.Fluxes = 12;
            newData = RecreationOfDistribution.NewDistrib();


            WritingFile.ImageForDistribution(newData);

            Console.ReadLine();
        }

        /// <summary>
        /// Метод просто для эксперимента
        /// </summary>
        public static void writeBMP()
        {
            int size = 1000;
            List<byte> data = new List<byte>();
            for (int i = size / -2; i < size / 2; i++)
            {
                for (int j = size / -2; j < size / 2; j++)
                {
                    //double func = i * j * 1.0;
                    double func = 255 - DecarCoordBirdFunk(i * 1.0 / (size / 20.0), j * 1.0 / (size / 20.0));
                    //double func = 255 - (i * j);
                    byte myByte = (byte)(Math.Round(func) % 256);

                    for (int k = 0; k < 3; k++) //ЧБ картинка
                    {
                        data.Add(myByte);
                    }
                }
            }
            WritingFile.GenerateBmp(size, size, data, "newBMP.bmp");
        }

        /// <summary>
        /// Можно нарисовать птичку в файл по этой формуле
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double DecarCoordBirdFunk(double x, double y)
        {
            double opOne = -1 * Math.Pow(x * x + y * y, 0.5);
            double opTwo = 4;
            double opThree = (x * x - y * y) / (x * x + y * y);

            double opFour = (4.5 * x) / Math.Pow(x * x + y * y, 0.5);
            double opFive = (4 * x * y) / (x * x + y * y);
            double opSix = (x * x - y * y) / (x * x + y * y);

            double opGeneric3 = opFour * opFive * opSix;

            double sumOfOperands = opOne + opTwo + opThree + opGeneric3;

            return sumOfOperands;
        }
    }
}
