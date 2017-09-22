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

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void BtnChoose_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
               Filter = "Изображения (*.jpg;*.png)|*.jpg;*.png;*.gif"
            };

            if (dialog.ShowDialog() == true)
            {
                _pathToPicture.Text = dialog.FileName;
            }
        }

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

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            _listSyb.Items.RemoveAt(_listSyb.SelectedIndex);
            _listSyb.SelectedIndex = 0;
        }

        private void SaveSyb_Click(object sender, RoutedEventArgs e)
        {
            List<String> tmp = new List<string>();

            foreach (var t in _listSyb.Items)
            {
                tmp.Add(t.ToString());
            }

            ASCIIConverter.SetSyb(tmp);
        }

        private void BtnAddNewSyb_Click(object sender, RoutedEventArgs e)
        {
            _listSyb.Items.Add(_newSyb.Text);
            _newSyb.Text = "";
        }

        // Запускаем преобразование
        private void Start()
        {
            string result = "";

            if (Compress.IsEnabled)
            {
                result = ASCIIConverter.ConvertToSize(_pathToPicture.Text, Convert.ToInt32(_compressWidth.Text), Convert.ToInt32(_compressHeight.Text));
            }
            else
            {
                result = ASCIIConverter.ConvertToRealSize(_pathToPicture.Text);
            }


            if (_toClipboard.IsEnabled) Clipboard.SetText(result);

            String pathNewFile = _pathToPicture.Text + ".txt";

            if ((bool)_toFile.IsChecked)
            {
                StreamWriter write = new StreamWriter(new FileStream(pathNewFile, FileMode.Create));
                write.WriteLine(result);
                write.Close();

                if (_opetnNewFile.IsChecked == true) System.Diagnostics.Process.Start(pathNewFile);
            }

            if (_toTextBox.IsChecked == true)
            {
                ResultWindow res = new ResultWindow(_pathToPicture.Text, result);
                res.ShowDialog();
            }
        }
    }
}
