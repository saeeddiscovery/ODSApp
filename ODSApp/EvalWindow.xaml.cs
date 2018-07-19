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

namespace ODSApp
{
    /// <summary>
    /// Interaction logic for EvalWindow.xaml
    /// </summary>
    public partial class EvalWindow : Window
    {
        private static string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        private static string folderPath = appPath + "/نتایج";
        private static string filePath = folderPath + "/survey.csv";
        public static int currId = 0;
        private static int currQ = 0;
        private static int numTitles = 0;
        public static string info;
        public static string info_filePath;
        public List<string> titles = new List<string>();
        public List<string> questions = new List<string>();
        private string[] q1_answers, q2_answers, q3_answers, q4_answers, q5_answers;
        string q1, q2, q3, q4, q5;
        public EvalWindow()
        {
            InitializeComponent();
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void Win_Eval_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (currQ < titles.Count - 1)
            {
                if (MessageBox.Show("اگر قبل از پاسخ به تمام پرسش ها پنجره را ببندید پاسخ های شما ذخیره نخواهد شد\nآیا مایل به بستن این پنجره هستید؟",
                    "پرسش ها به اتمام نرسیده است", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign) == MessageBoxResult.No)
                    e.Cancel = true;
            }
            currQ = 0;
            tb_Index.Text = (currQ + 1).ToString() + " / " + numTitles.ToString();
        }

        private void saveAnswers(string[] q1, string[] q2, string[] q3, string[] q4, string[] q5)
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

            //string q1_line = q1[0];
            //for (int i = 1; i < q1.Length; i++)
            //{
            //    q1_line = q1_line + ',' + q1[i];
            //}
            //string q2_line = q2[0];
            //for (int i = 1; i < q2.Length; i++)
            //{
            //    q2_line = q2_line + ',' + q2[i];
            //}
            //string q3_line = q3[0];
            //for (int i = 1; i < q3.Length; i++)
            //{
            //    q3_line = q3_line + ',' + q3[i];
            //}
            //string q4_line = q4[0];
            //for (int i = 1; i < q4.Length; i++)
            //{
            //    q4_line = q4_line + ',' + q4[i];
            //}
            //string q5_line = q5[0];
            //for (int i = 1; i < q5.Length; i++)
            //{
            //    q5_line = q5_line + ',' + q5[i];
            //}

            string ansLine = currId.ToString();
            for (int i = 0; i < titles.Count; i++)
                ansLine = ansLine + "," + q1[i] + "," + q2[i] + "," + q3[i] + "," + q4[i] + "," + q5[i];
            List<string> answers = new List<string>();
            answers.Add(ansLine);
            File.AppendAllLines(filePath, answers);
        }

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            bool answered = false;
            foreach (RadioButton rb in FindVisualChildren<RadioButton>(Grid_Q1))
            {
                if (rb.IsChecked == true)
                {
                    answered = true;
                    int id = int.Parse(rb.Name.Substring(rb.Name.Length - 1));
                    q1 = id.ToString();
                }
            }
            if (answered == false) { MessageBox.Show("لطفا به سوال اول پاسخ دهید"); return; }
            answered = false;
            foreach (RadioButton rb in FindVisualChildren<RadioButton>(Grid_Q2))
            {
                if (rb.IsChecked == true)
                {
                    answered = true;
                    int id = Math.Abs(int.Parse(rb.Name.Substring(rb.Name.Length - 1))-5);
                    q2 = id.ToString();
                }
            }
            if (answered == false) { MessageBox.Show("لطفا به سوال دوم پاسخ دهید"); return; }
            answered = false;
            foreach (RadioButton rb in FindVisualChildren<RadioButton>(Grid_Q3))
            {
                if (rb.IsChecked == true)
                {
                    answered = true;
                    int id = int.Parse(rb.Name.Substring(rb.Name.Length - 1));
                    q3 = id.ToString();
                }
            }
            if (answered == false) { MessageBox.Show("لطفا به سوال سوم پاسخ دهید"); return; }
            answered = false;
            foreach (RadioButton rb in FindVisualChildren<RadioButton>(Grid_Q4))
            {
                if (rb.IsChecked == true)
                {
                    answered = true;
                    int id = int.Parse(rb.Name.Substring(rb.Name.Length - 2))-15;
                    q4 = id.ToString();
                }
            }
            if (answered == false) { MessageBox.Show("لطفا به سوال چهارم پاسخ دهید"); return; }
            answered = false;
            foreach (RadioButton rb in FindVisualChildren<RadioButton>(Grid_Q5))
            {
                if (rb.IsChecked == true)
                {
                    answered = true;
                    int id = int.Parse(rb.Name.Substring(rb.Name.Length - 1));
                    q5 = id.ToString();
                }
            }
            if (answered == false) { MessageBox.Show("لطفا به سوال پنجم پاسخ دهید"); return; }

            q1_answers[currQ] = q1.ToString();
            q2_answers[currQ] = q2.ToString();
            q3_answers[currQ] = q3.ToString();
            q4_answers[currQ] = q4.ToString();
            q5_answers[currQ] = q5.ToString();

            if (currQ < titles.Count-1)
            {
                currQ += 1;
                if (btn_Prev.IsEnabled == false) btn_Prev.IsEnabled = true;
                tb_Index.Text = (currQ + 1).ToString() + " / " + titles.Count.ToString();
                lbl_title.Content = titles[currQ];
                int qID = currQ * 5;
                lbl_Q1.Text = questions[qID];
                lbl_Q2.Text = questions[qID+1];
                lbl_Q3.Text = questions[qID+2];
                lbl_Q4.Text = questions[qID+3];
                lbl_Q5.Text = questions[qID+4];
            }
            else
            {
                saveAnswers(q1_answers, q2_answers, q3_answers, q4_answers, q5_answers);
                MessageBox.Show("پرسشنامه به اتمام رسید\nسپاسگذاریم ", "پایان ارزیابی", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                btn_Next.IsEnabled = false;
                this.Close();
            }

            foreach (RadioButton rb in FindVisualChildren<RadioButton>(Win_Eval))
            {
                rb.IsChecked = false;
            }
        }

        private void btn_Prev_Click(object sender, RoutedEventArgs e)
        {
            currQ -= 1;
            if (currQ > 0)
            {
                if (btn_Next.IsEnabled == false) btn_Next.IsEnabled = true;
                tb_Index.Text = (currQ + 1).ToString() + " / " + numTitles.ToString();
            }
            else
            {
                tb_Index.Text = (currQ + 1).ToString() + " / " + numTitles.ToString();
                btn_Prev.IsEnabled = false;
            }
            tb_Index.Text = (currQ + 1).ToString() + " / " + titles.Count.ToString();
            lbl_title.Content = titles[currQ];
            int qID = currQ * 5;
            lbl_Q1.Text = questions[qID];
            lbl_Q2.Text = questions[qID + 1];
            lbl_Q3.Text = questions[qID + 2];
            lbl_Q4.Text = questions[qID + 3];
            lbl_Q5.Text = questions[qID + 4];
        }

        private void Win_Eval_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists("ODS_Questions.txt")==false)
                {
                MessageBox.Show("فایل سوالات یافت نشد\nODS_Questions.txt", "فایل موجود نیست", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            string line;
            int i = 0;
            using (StreamReader sr = new StreamReader("ODS_Questions.txt"))
            {
                line = sr.ReadLine();
                while (line != null)
                {
                    if (i % 6 == 0)
                        titles.Add(line.Split('\t')[1]);
                    else
                        questions.Add(line.Split('\t')[1].Replace(".", " • "));
                    line = sr.ReadLine();
                    i++;
                }
            }

            q1_answers = new string[titles.Count];
            q2_answers = new string[titles.Count];
            q3_answers = new string[titles.Count];
            q4_answers = new string[titles.Count];
            q5_answers = new string[titles.Count];

            numTitles = titles.Count;
            tb_Index.Text = (currQ+1).ToString() + " / " + numTitles.ToString();

            lbl_title.Content = titles[0];
            lbl_Q1.Text = questions[0];
            lbl_Q2.Text = questions[1];
            lbl_Q3.Text = questions[2];
            lbl_Q4.Text = questions[3];
            lbl_Q5.Text = questions[4];


            if (Directory.Exists(folderPath) == false)
                Directory.CreateDirectory(folderPath);

            if (File.Exists(filePath) == false)
            {
                var csv = new StringBuilder();
                string title = "ID, 1-1, 1-2, 1-3, 1-4, 1-5, 2-1, 2-2, 2-3, 2-4, 2-5, 3-1, 3-2, 3-3, 3-4, 3-5, 4-1, 4-2, 4-3, 4-4, 4-5" +
                    ", 5-1, 5-2, 5-3, 5-4, 5-5, 6-1, 6-2, 6-3, 6-4, 6-5, 7-1, 7-2, 7-3, 7-4, 7-5, 8-1, 8-2, 8-3, 8-4, 8-5" +
                    ", 9-1, 9-2, 9-3, 9-4, 9-5, 10-1, 10-2, 10-3, 10-4, 10-5, 11-1, 11-2, 11-3, 11-4, 11-5, 12-1, 12-2, 12-3, 12-4, 12-5" +
                    ", 13-1, 13-2, 13-3, 13-4, 13-5, 14-1, 14-2, 14-3, 14-4, 14-5, 15-1, 15-2, 15-3, 15-4, 15-5, 16-1, 16-2, 16-3, 16-4, 16-5" +
                    ", 17-1, 17-2, 17-3, 17-4, 17-5, 18-1, 18-2, 18-3, 18-4, 18-5, 19-1, 19-2, 19-3, 19-4, 19-5, 20-1, 20-2, 20-3, 20-4, 20-5";
                csv.AppendLine(title);
                File.WriteAllText(filePath, csv.ToString(), Encoding.UTF8);
                //currId = 1;
            }
            else
            {
                using (TextReader fileReader = File.OpenText(filePath))
                {
                    var csv = new CsvReader(fileReader);
                    var records = csv.GetRecords<dynamic>();
                    //currId = records.Count<dynamic>();
                }
            }
            //string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            //string folderName = appPath + "\\تصاویر";
            //if (Directory.Exists(folderName) == false)
            //{
            //    MessageBox.Show("لطفا عکس ها را در پوشه ''تصاویر'' قرار دهید", "پوشه ''تصاویر'' پیدا نشد", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            //    Environment.Exit(0);
            //}
            ////imgList = Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories).ToList();
            //imgList = Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories).OrderBy(f => int.Parse(System.IO.Path.GetFileNameWithoutExtension(f))).ToList();
            //if (imgList.Count == 0)
            //{
            //    MessageBox.Show("لطفا عکس ها را در پوشه ''تصاویر'' قرار دهید", "تصویری در پوشه ''تصاویر'' پیدا نشد", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            //    Environment.Exit(0);
            //}
            //q1_answers = new string[imgList.Count];
            //q2_answers = new string[imgList.Count];
            //image.Source = new BitmapImage(new Uri(imgList[imgIdx]));
            //tb_Index.Text = "1 / " + imgList.Count.ToString();
        }
    }
}
