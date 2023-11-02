using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ParallelizationLab5
{
    internal class WritingFile
    {
        public static void AvergeTimes(List<double> myList, string fileName)
        {
            string pathWriteData = Path.Combine(Environment.CurrentDirectory, $"{fileName}.csv");

            using (StreamWriter sw = new StreamWriter(pathWriteData, false, Encoding.UTF8))
            {
                int counter = 0;
                for (int i = 0; i < 12; i++)
                {
                    sw.Write($"{i + 1};");
                    for (int j = 0; j < 10; j++)
                    {
                        sw.Write($"{myList[counter]};");
                        counter++;
                    }
                    sw.WriteLine();
                }
            }
        }

        public static void ImageForDistribution(List<List<int>> newDistr)
        {
            int height = newDistr.Count;
            int width = newDistr[0].Count;

            List<int> normValues = new List<int>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    normValues.Add(newDistr[i][j]);
                }
            }
            double max = Convert.ToDouble(normValues.Max());
            double min = Convert.ToDouble(normValues.Min());

            List<byte> myBytes = new List<byte>();
            for (int i = 0; i < normValues.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    myBytes.Add(Convert.ToByte(Math.Round(((normValues[i] * 1.0 - min) / max) * 255.0)));
                }
            }

            GenerateBmp(width, height, myBytes, "NewPicture.bmp");
        }



        /// <summary>
        /// метод перевода цветного BMP файла в черно-белый
        /// </summary>
        /// <param name="nameColorFile"></param>
        /// <param name="nameBlackAndWhiteFile"></param>
        public static void ConversionToBlackAndWhiteBMPFile(string nameColorFile, string nameBlackAndWhiteFile)
        {
            var refBMPlist = ReadingFile.ReferenceBMPRead($"{nameColorFile}.bmp");
            var bufBMPlist = new List<byte>();

            int offsetData = Convert.ToInt32($"{Convert.ToString(refBMPlist[13], 16)}{Convert.ToString(refBMPlist[12], 16)}{Convert.ToString(refBMPlist[11], 16)}{Convert.ToString(refBMPlist[10], 16)}", 16);
            int widthBMP = Convert.ToInt32($"{Convert.ToString(refBMPlist[21], 16)}{Convert.ToString(refBMPlist[20], 16)}{Convert.ToString(refBMPlist[19], 16)}{Convert.ToString(refBMPlist[18], 16)}", 16);
            int heightBMP = Convert.ToInt32($"{Convert.ToString(refBMPlist[25], 16)}{Convert.ToString(refBMPlist[24], 16)}{Convert.ToString(refBMPlist[23], 16)}{Convert.ToString(refBMPlist[22], 16)}", 16);
            int bitPerPixel = Convert.ToInt32($"{Convert.ToString(refBMPlist[29], 16)}{Convert.ToString(refBMPlist[28], 16)}", 16) / 8;

            int averageNumber = 0;
            //int counter = 0;
            for (int i = offsetData; i < refBMPlist.Count; i++)
            {
                if (i % bitPerPixel != 1)
                {
                    averageNumber += Convert.ToInt32(refBMPlist[i]);
                }
                else
                {
                    averageNumber = (int)Math.Round(averageNumber / 3.0);
                    for (int j = 0; j < 3; j++)
                    {
                        bufBMPlist.Add(Convert.ToByte(averageNumber));
                        //counter++;
                    }
                    averageNumber = 0;
                }
            }
            refBMPlist.Clear();
            bufBMPlist.Reverse();
            for (int i = 0; i < heightBMP; i++)
            {
                bufBMPlist.Reverse(widthBMP * i * 3, widthBMP * 3);
            }
            GenerateBmp(widthBMP, heightBMP, bufBMPlist, $"{nameBlackAndWhiteFile}.bmp");
        }

        /// <summary>
        /// метод создания BMP файла
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pixelBytes"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void GenerateBmp(int width, int height, List<byte> pixelBytes, string name)
        {
            if (pixelBytes.Count != width * height * 3)
            {
                throw new ArgumentException("Неверное количество байтов пикселей");
            }

            Bitmap bitmap = new Bitmap(width, height);
            int byteIndex = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte blue = pixelBytes[byteIndex++];
                    byte green = pixelBytes[byteIndex++];
                    byte red = pixelBytes[byteIndex++];

                    Color color = Color.FromArgb(red, green, blue);
                    bitmap.SetPixel(x, y, color);
                }
            }

            bitmap.Save(name, ImageFormat.Bmp);
        }
    }
}


