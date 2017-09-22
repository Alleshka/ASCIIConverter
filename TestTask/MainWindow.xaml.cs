using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace TestTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Запуск программы
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        /// <summary>
        /// Выбрать файл для преобразования
        /// </summary>
        private void BtnChoose_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
               Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png;*.gif",
               InitialDirectory = "Test2"
            };

            if (dialog.ShowDialog() == true)
            {
                _pathToPicture.Text = dialog.FileName;
                _toFileText.Text = "Сохранить в файл: " + dialog.FileName + ".txt";
            }
        }

        /// <summary>
        /// Действие при открытии экспандера
        /// </summary>
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (_listSyb.Items.Count == 0)
            {
                List<String> lst = ASCIIConverter.GetListSyb();
                foreach (var str in lst)
                {
                    _listSyb.Items.Add(str);
                }

                _listSyb.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Удаление символа из списка символов
        /// </summary>
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {      
                _listSyb.Items.RemoveAt(_listSyb.SelectedIndex);
                _listSyb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Сохранение символов
        /// </summary>
        private void SaveSyb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_listSyb.Items.Count < 2) throw new Exception("Не может быть меньше двух символов");
                List<String> tmp = new List<string>();

                foreach (var t in _listSyb.Items)
                {
                    tmp.Add(t.ToString());
                }

                ASCIIConverter.SetSyb(tmp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Добавление символа 
        /// </summary>
        private void BtnAddNewSyb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_newSyb.Text == "") throw new Exception("Символ не может быть пустым");

                _listSyb.Items.Add(_newSyb.Text);
                _newSyb.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Запускаем преобразование
        private void Start()
        {
            try
            {
                if (_pathToPicture.Text == "") throw new Exception("Не указан путь до файла");

                string responce = "";
                string result = ""; // Тут будет храниться результат

                String path = _pathToPicture.Text;
                Task<String> run;

                // Если стоит галка на сжатии
                // Так как может обрабатываться долго, запускаю в отдельном потоке
                if (Compress.IsChecked==true)
                {
                    int w = Convert.ToInt32(_compressWidth.Text);
                    int h = Convert.ToInt32(_compressHeight.Text);

                    run = new Task<String>(()=>ASCIIConverter.ConvertToSize(path, w, h));
                }
                else // Если не стоит
                {  
                    run = new Task<String>(() => ASCIIConverter.ConvertToRealSize(path));
                }

                // Запускаем поток обработки 
                run.Start();

                // Ждём результата
                result = run.Result;

                // Сохранить результат в буфер обмена
                if (_toClipboard.IsChecked == true)
                {
                    Clipboard.SetText(result);
                    responce += "Сохранено в буфер обмена" + Environment.NewLine;
                }

                // Сохранить результат в файл
                if ((bool)_toFile.IsChecked)
                {
                    String pathNewFile = _pathToPicture.Text + ".txt";

                    StreamWriter write = new StreamWriter(new FileStream(pathNewFile, FileMode.Create));
                    write.WriteLine(result);
                    write.Close();

                    // Открыть файл
                    if (_opetnNewFile.IsChecked == true) System.Diagnostics.Process.Start(pathNewFile);

                    responce += $"Сохранено в новый файл: {pathNewFile}";
                }

                // Открыть в новом окне
                if (_toTextBox.IsChecked == true)
                {
                    ResultWindow res = new ResultWindow(_pathToPicture.Text, result);
                    res.ShowDialog();
                }

                if (responce == "") MessageBox.Show("Обработано");
                else MessageBox.Show(responce);
                        
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + Environment.NewLine + ex.Message);
            }
        }

        private void _toFile_Click(object sender, RoutedEventArgs e)
        {
            if (_toFile != null)
            {
                if (_toFile.IsChecked == true)
                {
                    _opetnNewFile.IsEnabled = true;
                }
                else _opetnNewFile.IsEnabled = false;
            }
        }

        private void Compress_Click(object sender, RoutedEventArgs e)
        {
            if (Compress.IsChecked == true)
            {
                _setCompressExp.IsEnabled = true;
            }
            else
            {
                _setCompressExp.IsEnabled = false;
                _setCompressExp.IsExpanded = false;
            }
        }
    }
}
