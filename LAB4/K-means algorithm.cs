using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAST_LABA
{
    class K_means_algorithm
    {
        private double Counting_min_distance(double X_point, double Y_point, double X_center, double Y_center,
            bool function)
        {
            if (function == true)//Евклидово расстояние
                return Math.Sqrt(Math.Pow(X_center - X_point, 2) + Math.Pow(Y_center - Y_point, 2));
            if (function == false)//Манхэттенское расстояние
                return (Math.Abs(X_center - X_point) + Math.Abs(Y_center - Y_point));
            return 0f;
        }

        public void Clustering(List<double[]> points, double[,] centers, List<List<double[]>> clusters, bool function)
        {
            //Инициализируем кластеры (уже известно, солько их есть)
            for (int cluster = 0; cluster < centers.Length / 2; cluster++)
                clusters.Add(new List<double[]>());
            foreach(var point in points)
            {
                double[] distance_to_center = new double[centers.Length / 2];
                for (int cluster = 0; cluster < centers.Length / 2; cluster++)//Ищем наименьшее расстояние
                {
                    distance_to_center[cluster] = Counting_min_distance(point[0], point[1], centers[cluster, 0],
                        centers[cluster, 1], function);
                }
                double min_dist_to_center = distance_to_center[0];
                int index_of_min_dist_to_center = 0;
                for (int dist = 1; dist < distance_to_center.Length; dist++)
                {
                    if (distance_to_center[dist] < min_dist_to_center)
                    {
                        min_dist_to_center = distance_to_center[dist];//Нашли наименьшее, сохраняем
                        index_of_min_dist_to_center = dist;
                    }
                }
                //Относим точку к соответствующему кластеру
                clusters[index_of_min_dist_to_center].Add(point);
            }
        }

        public void recount_centers_of_clusters(List<List<double[]>> clusters, double[,] centers_of_clusters)
        {
            for (int cluster = 0; cluster < clusters.Count; cluster++)
            {
                if (clusters[cluster].Count == 0)
                    continue;
                double[] new_coordinates = new double[2];
                foreach (var point in clusters[cluster])
                {
                    new_coordinates[0] += point[0];//X
                    new_coordinates[1] += point[1];//Y
                }
                centers_of_clusters[cluster, 0] = Math.Round(new_coordinates[0] / clusters[cluster].Count);
                centers_of_clusters[cluster, 1] = Math.Round(new_coordinates[1] / clusters[cluster].Count);
            }
        }
    }
}
