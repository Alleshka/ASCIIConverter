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
        private static List<String> _syb = new List<string>()
        {
            "*", " "
        };

        /// <summary>
        /// Сжимает изображение и конвертирует в ASCII
        /// </summary>
        /// <param name="_pathImg">Путь до файла</param>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <returns>Строку результат</returns>
        public static String ConvertToSize(String _pathImg, int width, int height)
        {
            return ActionConvert(OpenSmallFile(_pathImg, width, height));
        }

        /// <summary>
        /// Кодирует изображение в набор символов с таким же количеством символов, что и изображение
        /// </summary>
        /// <param name="pathImg">Путь до файла (пока только на компьютере) </param>
        /// <returns>Строку с результатом</returns>
        public static String ConvertToRealSize(String pathImg) => ActionConvert(OpenFullFile(pathImg)); 

        public static List<String> GetListSyb() => _syb;
        public static void SetSyb(List<String> lst)
        {
            _syb = lst;
        }

        /// <summary>
        /// Открывает изображение в полном размере
        /// </summary>
        /// <param name="pathImg">Путь до файа</param>
        private static Bitmap OpenFullFile(String pathImg)
        {
            return new Bitmap(pathImg); // Открываем изобрежение
        }
        /// <summary>
        /// Открывает изображение с указанными размерами
        /// </summary>
        /// <param name="pathImg">Путь до файла</param>
        /// <param name="width">Ширина файла</param>
        /// <param name="height">Высота файла</param>
        private static Bitmap OpenSmallFile(String pathImg, int width=50, int height=50)
        {
            return new Bitmap(Image.FromFile(pathImg), width, height);
        }
        /// <summary>
        /// Выполняет кодирование
        /// </summary>
        /// <param name="workImg">Изображение</param>
        /// <returns>Строку с результатом</returns>
        private static String ActionConvert(Bitmap workImg)
        {
            String tmp = ""; // Тут будет храниться набор символов

            for (int i = 0; i < workImg.Height; i++)
            {
                tmp += Environment.NewLine;
                for (int j = 0; j < workImg.Width; j++)
                {
                    Color k = workImg.GetPixel(j, i);
                    int c = (k.R + k.G + k.B) / 3;

                    tmp += GetCurSyb(c);
                }
            }

            return tmp;
        }
        /// <summary>
        /// Достаёт символ, который необходимо подставить
        /// </summary>
        /// <param name="c">Полученный коэффициент</param>
        /// <returns>Требуемый символ</returns>
        private static String GetCurSyb(int c)
        {
            for (int i = 0; i < _syb.Count() - 1; i++)
            {
                if ((c>255*i/_syb.Count()) && (c < 255 * (i + 1) / _syb.Count())) return Convert.ToString(_syb[i]);
            }

            return _syb.Last();
        }
    }
}
