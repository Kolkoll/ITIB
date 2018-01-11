using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.IO;
using Newtonsoft.Json;

namespace LAST_LABA
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<double[]> points = new List<double[]>();
        List<List<double[]>> clusters = new List<List<double[]>>();//Здесь храним кластеры
        double[,] centers_of_clusters = new double[0, 0];
        int[,] colors_of_centers = new int[0, 0];
        double previous_total_deviation = 0f;

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void mark_clusters()
        {//Отмечаем кластеры
            var pic = pictureBox1.CreateGraphics();
            SolidBrush myBrush = new SolidBrush(Color.Red);
            for (int cluster = 0; cluster < clusters.Count; cluster++)//по кластерам
            {
                myBrush.Color = Color.FromArgb(colors_of_centers[cluster, 0], colors_of_centers[cluster, 1],
                       colors_of_centers[cluster, 2]);
                foreach (var point in clusters[cluster])
                    pic.FillRectangle(myBrush, Convert.ToInt32(point[0]), Convert.ToInt32(point[1]), 6, 6);
            }
        }

        private void clear_point(double[] coordinates)
        {
            var pic = pictureBox1.CreateGraphics();
            pic.FillRectangle(Brushes.White, Convert.ToInt32(coordinates[0]), Convert.ToInt32(coordinates[1]), 6, 6);
        }

        private void mark_centers_of_clusters(double[,] centers_of_clusters, int indicator)
        {//Отмечаем центры на форме
            var pic = pictureBox1.CreateGraphics();
            SolidBrush myBrush = new SolidBrush(Color.Red);
            if (indicator == 0)//Начальная установка центроидов
            {
                colors_of_centers = new int[centers_of_clusters.Length / 2, 3];
                Random rand = new Random();
                for (int center = 0; center < centers_of_clusters.Length / 2; center++)
                {
                    for (int part_color = 0; part_color < 3; part_color++)
                    {
                        colors_of_centers[center, part_color] = rand.Next(0, 255);
                    }
                    myBrush.Color = Color.FromArgb(colors_of_centers[center, 0], colors_of_centers[center, 1],
                       colors_of_centers[center, 2]);
                    pic.FillEllipse(myBrush, Convert.ToInt32(centers_of_clusters[center, 0]),
                            Convert.ToInt32(centers_of_clusters[center, 1]), 20, 20);
                }
            }
            if (indicator == 1)//Стираем старые центроиды
            {
                for (int center = 0; center < centers_of_clusters.Length / 2; center++)
                {
                    pic.FillEllipse(Brushes.White, Convert.ToInt32(centers_of_clusters[center, 0]),
                        Convert.ToInt32(centers_of_clusters[center, 1]), 20, 20);
                }
            }
            if (indicator == 2)//Дальнейшая перерисовка центроидов
            {
                for (int center = 0; center < centers_of_clusters.Length / 2; center++)
                {
                    myBrush.Color = Color.FromArgb(colors_of_centers[center, 0], colors_of_centers[center, 1],
                       colors_of_centers[center, 2]);
                    pic.FillEllipse(myBrush, Convert.ToInt32(centers_of_clusters[center, 0]),
                        Convert.ToInt32(centers_of_clusters[center, 1]), 20, 20);
                }
            }
        }
        //первая координата - X, вторая - Y
        private void metroButton1_Click(object sender, EventArgs e)
        {
            one_step_of_clusterisation();
        }

        private bool one_step_of_clusterisation()
        {
            if (points.Count == 0 || centers_of_clusters.Length == 0)
            {
                MessageBox.Show("Отсутствуют центры кластеров или точки");
                return false;
            }
            //Непосредственно кластеризация
            K_means_algorithm k = new K_means_algorithm();
            if (metroRadioButton1.Checked)
                k.Clustering(points, centers_of_clusters, clusters, true);
            if (metroRadioButton2.Checked)
                k.Clustering(points, centers_of_clusters, clusters, false);
            //Отмечаем на форме кластеры
            mark_clusters();
            //Стираем с формы центры кластеров для начала их пересчета
            mark_centers_of_clusters(centers_of_clusters, 1);
            //Пересчет центроидов
            k.recount_centers_of_clusters(clusters, centers_of_clusters);
            //Отмечаем новые центроиды на форме
            mark_centers_of_clusters(centers_of_clusters, 2);
            //Подсчет среднеквадратичного отклонения
            double total_deviation = count_total_deviation();
            if (total_deviation == previous_total_deviation)
            {
                MessageBox.Show("Кластеризация завершена");
                clear_all_data();
                return true;
            }
            else
                previous_total_deviation = total_deviation;
            //Очистка хранилища кластеров для перекластеризации
            clusters.Clear();
            return false;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)//Добавляем точки
        {
            int amount_of_points = 0;
            var pic = (PictureBox)sender;
            var g = pic.CreateGraphics();
            Random rand = new Random();
            for (int index = 0; index < 350; index++)
            {
                double[] coord = new double[2];
                amount_of_points += 1;
                if (amount_of_points > (pictureBox1.Height - 5) * (pictureBox1.Width - 5))
                {
                    MessageBox.Show("Недопустимое количество точек");
                    return;
                }
                else
                {
                    int X = rand.Next(0, pictureBox1.Width - 5);
                    int Y = rand.Next(0, pictureBox1.Height - 5);
                    g.FillRectangle(Brushes.Black, X, Y, 6, 6);
                    coord[0] = Convert.ToDouble(X);
                    coord[1] = Convert.ToDouble(Y);
                    points.Add(coord);
                }
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (centers_of_clusters.Length != 0)
            {
                MessageBox.Show("Центры кластеров уже были добавлены");
                return;
            }
            if (metroTextBox1.Text != "")
            {
                string text = metroTextBox1.Text;
                for (int symbol = 0; symbol < text.Length; symbol++)
                {
                    if (text[symbol] < 48 || text[symbol] > 57)
                    {
                        MessageBox.Show("В числе кластеров должны быть только цифры");
                        return;
                    }
                }
                centers_of_clusters = new double[Convert.ToInt32(metroTextBox1.Text), 2];
            }
            if (points.Count == 0)
            {
                MessageBox.Show("Отсутствуют точки");
                return;
            }
            else
            {
                Random rand = new Random();
                for (int center = 0; center < centers_of_clusters.Length / 2; center++)
                {
                    centers_of_clusters[center, 0] = rand.Next(0, pictureBox1.Width - 20);
                    centers_of_clusters[center, 1] = rand.Next(0, pictureBox1.Height - 20);
                }
                //Проверка на совпадение центра кластера с координатами точки
                for (int center = 0; center < centers_of_clusters.Length / 2; center++)
                {
                    for (int point = 0; point < points.Count; point++)
                    {
                        if (centers_of_clusters[center, 0] == points[point][0] &&
                            centers_of_clusters[center, 1] == points[point][1])
                        {
                            clear_point(points[point]);
                            points.RemoveAt(point);
                        }
                    }
                }
                mark_centers_of_clusters(centers_of_clusters, 0);//Отметим их на форме
            }
        }

        private void metroTextBox1_Click(object sender, EventArgs e)
        {
            metroTextBox1.Clear();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            clear_all_data();
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            if (points.Count == 0 || centers_of_clusters.Length == 0)
            {
                MessageBox.Show("Отсутствуют центры кластеров или точки");
                return;
            }
            while (true)
                if (one_step_of_clusterisation() == true)
                    break;
        }

        private double count_total_deviation()//Подсчет среднеквадратичного отклонения
        {
            double total_deviationn = 0f;
            for (int cluster = 0; cluster < clusters.Count; cluster++)
            {
                foreach (var point in clusters[cluster])
                {
                    total_deviationn += Math.Pow((point[0] - centers_of_clusters[cluster, 0]) +
                        (point[1] - centers_of_clusters[cluster, 1]), 2);
                }
            }
            return total_deviationn;
        }

        private void clear_all_data()
        {
            pictureBox1.Image = null;
            points.Clear();
            centers_of_clusters = new double[0, 0];
            colors_of_centers = new int[0, 0];
            pictureBox1.Invalidate();
        }

    }
}