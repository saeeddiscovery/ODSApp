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
using System.Windows.Shapes;
using System.IO;
using System.Windows.Interop;
using CsvHelper;

namespace RepApp
{
    /// <summary>
    /// Interaction logic for EvalWindow.xaml
    /// </summary>
    public partial class EvalWindow : Window
    {
        private static string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        private static string folderPath = appPath + "/نتایج";
        private static string filePath = folderPath + "/survey.csv";
        private static int currId = 0;
        public static string info;
        public static string info_filePath;
        public int imgIdx = 0;
        public List<string> imgList;
        string[] q1_answers, q2_answers;
        public EvalWindow()
        {
            InitializeComponent();
        }

        private void EvalWin_Closed(object sender, EventArgs e)
        {
            //Application.Current.Shutdown();
        }

        private void saveAnswers(string[] q1, string[] q2)
        {
            var csv = new StringBuilder();
            csv.AppendLine(info);
            File.AppendAllText(info_filePath, csv.ToString(), Encoding.UTF8);

            IEnumerable<dynamic> records;
            using (TextReader fileReader = File.OpenText(filePath))
            {
                var curr_csv = new CsvReader(fileReader);
                records = curr_csv.GetRecords<dynamic>();
            }

            if (currId > 1) currId = (currId+2) / 2;
            string q1_line = currId.ToString() + ',' + "Q1";
            for (int i = 0; i < q1.Length; i++)
            {
                q1_line = q1_line + ',' + q1[i];
            }
            string q2_line = currId.ToString() + ',' + "Q2";
            for (int i = 0; i < q2.Length; i++)
            {
                q2_line = q2_line + ',' + q2[i];
            }

            List<string> answers = new List<string>();
            answers.Add(q1_line);
            answers.Add(q2_line);
            //csv.AppendLine(q1_line);
            //csv.AppendLine(q2_line);
            File.AppendAllLines(filePath, answers);
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            int q1 = 0;
            int q2 = 0;
            if (rb_1.IsChecked == true) q1 = 1;
            else if (rb_2.IsChecked == true) q1 = 2;
            else if (rb_3.IsChecked == true) q1 = 3;
            else if (rb_4.IsChecked == true) q1 = 4;
            else if (rb_5.IsChecked == true) q1 = 5;
            else if (rb_6.IsChecked == true) q1 = 6;
            else if (rb_7.IsChecked == true) q1 = 7;
            else if (rb_8.IsChecked == true) q1 = 8;
            else if (rb_9.IsChecked == true) q1 = 9;
            else if (rb_10.IsChecked == true) q1 = 10;
            else { MessageBox.Show("لطفا به سوال اول پاسخ دهید"); return; }

            if (rb_11.IsChecked == true) q2 = 1;
            else if (rb_12.IsChecked == true) q2 = 2;
            else if (rb_13.IsChecked == true) q2 = 3;
            else if (rb_14.IsChecked == true) q2 = 4;
            else if (rb_15.IsChecked == true) q2 = 5;
            else if (rb_16.IsChecked == true) q2 = 6;
            else if (rb_17.IsChecked == true) q2 = 7;
            else if (rb_18.IsChecked == true) q2 = 8;
            else if (rb_19.IsChecked == true) q2 = 9;
            else if (rb_20.IsChecked == true) q2 = 10;
            else { MessageBox.Show("لطفا به سوال دوم پاسخ دهید"); return; }


            q1_answers[imgIdx] = q1.ToString();
            q2_answers[imgIdx] = q2.ToString();

            if (imgIdx < imgList.Count-1)
            {
                imgIdx += 1;
                if (btn_Prev.IsEnabled == false) btn_Prev.IsEnabled = true;
                image.Source = new BitmapImage(new Uri(imgList[imgIdx]));
                tb_Index.Text = (imgIdx+1).ToString() + " / " + imgList.Count.ToString();
            }
            else
            {
                saveAnswers(q1_answers, q2_answers);
                MessageBox.Show("پرسشنامه به اتمام رسید\nسپاسگذاریم ", "پایان ارزیابی", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                btn_Next.IsEnabled = false;
                this.Close();
            }

            rb_1.IsChecked = false;
            rb_2.IsChecked = false;
            rb_3.IsChecked = false;
            rb_4.IsChecked = false;
            rb_5.IsChecked = false;
            rb_6.IsChecked = false;
            rb_7.IsChecked = false;
            rb_8.IsChecked = false;
            rb_9.IsChecked = false;
            rb_10.IsChecked = false;
            rb_11.IsChecked = false;
            rb_12.IsChecked = false;
            rb_13.IsChecked = false;
            rb_14.IsChecked = false;
            rb_15.IsChecked = false;
            rb_16.IsChecked = false;
            rb_17.IsChecked = false;
            rb_18.IsChecked = false;
            rb_19.IsChecked = false;
            rb_20.IsChecked = false;
        }

        private void btn_Prev_Click(object sender, RoutedEventArgs e)
        {
            imgIdx -= 1;
            if (imgIdx > 0)
            {
                if (btn_Next.IsEnabled == false) btn_Next.IsEnabled = true;
                image.Source = new BitmapImage(new Uri(imgList[imgIdx]));
                tb_Index.Text = (imgIdx+1).ToString() + " / " + imgList.Count.ToString();
            }
            else
            {
                image.Source = new BitmapImage(new Uri(imgList[imgIdx]));
                tb_Index.Text = (imgIdx+1).ToString() + " / " + imgList.Count.ToString();
                btn_Prev.IsEnabled = false;
            }
        }

        private void Win_Eval_Loaded(object sender, RoutedEventArgs e)
        {
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string folderName = appPath + "\\تصاویر";
            if (Directory.Exists(folderName) == false)
            {
                MessageBox.Show("لطفا عکس ها را در پوشه ''تصاویر'' قرار دهید", "پوشه ''تصاویر'' پیدا نشد", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                Environment.Exit(0);
            }
            //imgList = Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories).ToList();
            imgList = Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories).OrderBy(f => int.Parse(System.IO.Path.GetFileNameWithoutExtension(f))).ToList();
            if (imgList.Count == 0)
            {
                MessageBox.Show("لطفا عکس ها را در پوشه ''تصاویر'' قرار دهید", "تصویری در پوشه ''تصاویر'' پیدا نشد", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                Environment.Exit(0);
            }
            q1_answers = new string[imgList.Count];
            q2_answers = new string[imgList.Count];
            image.Source = new BitmapImage(new Uri(imgList[imgIdx]));
            tb_Index.Text = "1 / " + imgList.Count.ToString();

            if (Directory.Exists(folderPath) == false)
                Directory.CreateDirectory(folderPath);

            if (File.Exists(filePath) == false)
            {
                var csv = new StringBuilder();
                string title = "ID, Question";
                for (int i = 1; i <= imgList.Count; i++)
                {
                    title = title + ',' + i.ToString() + ".jpg";
                }
                csv.AppendLine(title);
                File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
                currId = 1;
            }
            else
            {
                using (TextReader fileReader = File.OpenText(filePath))
                {
                    var csv = new CsvReader(fileReader);
                    var records = csv.GetRecords<dynamic>();
                    currId = records.Count<dynamic>();
                }
            }
        }
    }
}
