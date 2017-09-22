using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace TestTask
{
    public enum TypeImgPath { File, Net }
    
    public class ASCIIConverter
    {
        /// <summary>
        /// Кодирует изображение в набор символов
        /// </summary>
        /// <param name="pathImg">Путь до файла (пока только на компьютере) </param>
        /// <returns>Строку с результатом</returns>
        public static String Convert(String pathImg)
        {
            String tmp = "";          
            Bitmap workImg = new Bitmap(pathImg); // Открываем изобрежение

            for (int i = 0; i < workImg.Height; i++)
            {
                tmp += Environment.NewLine;
                for (int j = 0; j < workImg.Width; j++)
                {
                    Color k = workImg.GetPixel(j, i);
                    int c = (k.R + k.G + k.B) / 3;
                    int a = k.A;
                    if (c <= (255 / 2)) tmp += "*";
                    else tmp+= " ";
                }
            }

            return tmp;
        }
    }
}
