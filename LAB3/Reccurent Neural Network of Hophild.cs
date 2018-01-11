using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace LAB_3
{
    class Reccurent_Neural_Network_of_Hophild
    {
        private int[,] weighting_coefficients = new int[0, 0];

        private int[] vectorization(int[,] Matrix_image)
        {
            int[] vect_matrix = new int[Matrix_image.Length];
            int i3_index = 0;
            for (int i1_index = 0; i1_index < Matrix_image.Length / 5; i1_index++)
            {
                for (int i2_index = 0; i2_index < Matrix_image.Length / 5; i2_index++)
                {
                    vect_matrix[i3_index] = Matrix_image[i2_index, i1_index];
                    i3_index++;
                }
            }
            return vect_matrix;
        }

        private int[,] multiplying_of_matrixes(int[] Matrix)
        {
            int[,] result = new int[Matrix.Length, Matrix.Length];
            for (int i1_index = 0; i1_index < Matrix.Length; i1_index++)//По строкам
            {
                for (int i2_index = 0; i2_index < Matrix.Length; i2_index++)//По столбцам
                {
                    if (i1_index == i2_index)
                        result[i1_index, i2_index] = 0;
                    else
                        result[i1_index, i2_index] = Matrix[i1_index] * Matrix[i2_index];
                }
            }
            return result;
        }

        private int[,] setting_of_coefficients(int[] X, int[] E, int[] D)
        {
            int[,] first_matrix = multiplying_of_matrixes(X);
            int[,] second_matrix = multiplying_of_matrixes(E);
            int[,] third_matrix = multiplying_of_matrixes(D);
            weighting_coefficients = new int[X.Length, X.Length];
            for (int i1_index = 0; i1_index < X.Length; i1_index++)
            {
                for (int i2_index = 0; i2_index < X.Length; i2_index++)
                {
                    weighting_coefficients[i1_index, i2_index] = first_matrix[i1_index, i2_index] +
                        second_matrix[i1_index, i2_index] + third_matrix[i1_index, i2_index];
                }
            }
            return weighting_coefficients;
        }

        private void neuron_learning(int[] vectorized_matrix, out int[,] Matrix)
        {
            /*int[,]*/ Matrix = new int[vectorized_matrix.Length / 5, vectorized_matrix.Length / 5];
            int[] net = new int[vectorized_matrix.Length];
            int[] image = new int[vectorized_matrix.Length];
            for (int i1_index = 0; i1_index < vectorized_matrix.Length; i1_index++)
            {
                for (int i2_index = 0; i2_index < vectorized_matrix.Length; i2_index++)
                    net[i1_index] += vectorized_matrix[i2_index] * weighting_coefficients[i2_index, i1_index];
            }
            for (int index = 0; index < net.Length; index++)
            {
                if (net[index] > 0)
                    image[index] = 1;
                if (net[index] < 0)
                    image[index] = -1;
                   if (net[index] == 0)
                      image[index] = 0;
            }
            int indexx = 0;
            for (int i1_index = 0; i1_index < image.Length / 5; i1_index++)
            {
                for (int i2_index = 0; i2_index < image.Length / 5; i2_index++)
                {
                    Matrix[i1_index, i2_index] = image[indexx];
                    indexx++;
                }
            }
            print(Matrix);
        }

        private void print(int[,] image)
        {
            for (int i1_index = 0; i1_index < 5; i1_index++)
            {
                for (int index = 0; index < 5; index++)
                {
                    if (image[index, i1_index] == -1 || image[index, i1_index] == 0)
                        Console.Write(" {0}", image[index, i1_index]);
                    if (image[index, i1_index] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("  {0}", image[index, i1_index]);
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void print_2(int[,] image)
        {
            for (int i1_index = 0; i1_index < 5; i1_index++)
            {
                for (int index = 0; index < 5; index++)
                {
                    if (image[i1_index, index] == -1)
                        Console.Write(" {0}", image[i1_index, index]);
                    if (image[i1_index, index] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("  {0}", image[i1_index, index]);
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private void comparison_images(int[,] test_image, int[,] repaired_image)
        {
            for (int i1_index = 0; i1_index < test_image.Length / 5; i1_index++)
            {
                for (int i2_index = 0; i2_index < test_image.Length / 5; i2_index++)
                {
                    if (test_image[i1_index, i2_index] != repaired_image[i1_index, i2_index])
                    {
                        Console.WriteLine("Образ не распознан");
                        return;
                    }
                }
            }
            Console.WriteLine("Образ распознан");
        }

        public void Point_of_entry()
        {
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.Clear();
                Console.WriteLine("Распознавание образов X, E и D");
                int[,] X = { { 1, -1, -1, -1, 1 }, { -1, 1, -1, 1, -1 }, { -1, -1, 1, -1, -1 }, { -1, 1, -1, 1, -1 },
                           { 1, -1, -1, -1, 1 } };
                int[,] E = { { 1, 1, 1, 1, 1 }, { 1, -1, -1, -1, -1 }, { 1, 1, 1, 1, 1 }, { 1, -1, -1, -1, -1 },
                           { 1, 1, 1, 1, 1 } };
                int[,] D = { { 1, 1, 1, -1, -1 }, { 1, -1, -1, 1, -1 }, { 1, -1, -1, 1, -1 }, { 1, -1, -1, 1, -1 },
                           { 1, 1, 1, -1, -1 } };
                int[] vectorized_X = vectorization(X);
                int[] vectorized_E = vectorization(E);
                int[] vectorized_D = vectorization(D);
                Console.WriteLine("Тест РНС Хопфилда:");
                setting_of_coefficients(vectorized_X, vectorized_E, vectorized_D);
                Console.WriteLine("X:");
                int[,] test_image_X = new int[0, 0];
                neuron_learning(vectorized_X, out test_image_X);
                Console.WriteLine("E:");
                int[,] test_image_E = new int[0, 0];
                neuron_learning(vectorized_E, out test_image_E);
                Console.WriteLine("D:");
                int[,] test_image_D = new int[0, 0];
                neuron_learning(vectorized_D, out test_image_D);
                string letter = "";
                Console.Write("Введите искаженный образ буквы: ");
                letter = Console.ReadLine();
                if (letter != "E" && letter != "D" && letter != "X")
                    return;
                Console.Write("Ввод образа осуществляется построчно. Цифры только 1 и -1, отделяются пробелом.");
                Console.WriteLine("После ввода строки нажимать ENTER");
                int[,] wrong_image = new int[X.Length / 5, X.Length / 5];
                for (int number_of_string = 0; number_of_string < X.Length / 5; number_of_string++)
                {
                    string input = Console.ReadLine();
                    if (input.Length < 9 || input.Length > 14)
                        return;
                    string[] massive_input = input.Split(new Char[] { ' ' });
                    for (int index = 0; index < massive_input.Length; index++)
                    {
                        if (massive_input[index] != "1" && massive_input[index] != "-1")
                        {
                            Console.WriteLine("Неправильный ввод. Вводите только 1 или -1");
                            return;
                        }
                        wrong_image[number_of_string, index] = int.Parse(massive_input[index]);
                    }
                }
                Console.WriteLine("Искаженный образ буквы {0} введен:", letter);
                print_2(wrong_image);
                int[] vectorized_wrong_image = vectorization(wrong_image);
                Console.WriteLine("Исправленный образ буквы {0}:", letter);
                int[,] repaired_image = new int[0, 0];
                neuron_learning(vectorized_wrong_image, out repaired_image);
                if (letter == "X")
                    comparison_images(test_image_X, repaired_image);
                if (letter == "E")
                    comparison_images(test_image_E, repaired_image);
                if (letter == "D")
                    comparison_images(test_image_D, repaired_image);
                Console.WriteLine("Для продолжения нажмите ENTER, для выхода - любую другую клавишу");
                keyInfo = Console.ReadKey();
            } while (keyInfo.Key == ConsoleKey.Enter);
        }
    }
}
