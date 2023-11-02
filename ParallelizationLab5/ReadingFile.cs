using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParallelizationLab5
{
    internal class ReadingFile
    {
        public static List<List<int>> DataFromImage(string nameBMP)
        {
            var allBytes = ReferenceBMPRead(nameBMP);

            int offsetData = Convert.ToInt32($"{Convert.ToString(allBytes[13], 16)}{Convert.ToString(allBytes[12], 16)}{Convert.ToString(allBytes[11], 16)}{Convert.ToString(allBytes[10], 16)}", 16);
            int widthBMP = Convert.ToInt32($"{Convert.ToString(allBytes[21], 16)}{Convert.ToString(allBytes[20], 16)}{Convert.ToString(allBytes[19], 16)}{Convert.ToString(allBytes[18], 16)}", 16);
            int heightBMP = Convert.ToInt32($"{Convert.ToString(allBytes[25], 16)}{Convert.ToString(allBytes[24], 16)}{Convert.ToString(allBytes[23], 16)}{Convert.ToString(allBytes[22], 16)}", 16);
            int bitPerPixel = Convert.ToInt32($"{Convert.ToString(allBytes[29], 16)}{Convert.ToString(allBytes[28], 16)}", 16) / 8;
            List<List<int>> resList = new List<List<int>>();

            int counter = offsetData;
            for (int i = 0; i < heightBMP; i++)
            {
                resList.Add(new List<int>());
                for (int j = 0; j < widthBMP; j++)
                {
                    resList[i].Add(allBytes[counter]);
                    counter += bitPerPixel;
                }
            }

            resList.Reverse();
            return resList;
        }

        public static List<byte> ReferenceBMPRead(string name)
        {
            using (FileStream fstream = File.OpenRead(name))
            {
                byte[] buffer = new byte[fstream.Length];

                fstream.Read(buffer, 0, buffer.Length);

                return new List<byte>(buffer);
            }
        }

    }
}
