using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.Clear();
                Console.WriteLine("Обучение с пороговой ФА выходного нейрона и гауссовой ФА J скрытых RBF-нейронов");
                Console.WriteLine("Выберите способ обучения:");
                Console.WriteLine("1 - Пороговая ФА и все комбинации переменных");
                Console.WriteLine("2 - Пороговая ФА и часть комбинаций переменных");
                string choose = Console.ReadLine();
                Console.WriteLine();
                double[,] set_of_training_vectors = 
                {
                    {0,0,0,0}, {0,0,0,1}, {0,0,1,0}, {0,0,1,1}, {0,1,0,0}, {0,1,0,1}, {0,1,1,0}, {0,1,1,1},
                    {1,0,0,0}, {1,0,0,1}, {1,0,1,0}, {1,0,1,1}, {1,1,0,0}, {1,1,0,1}, {1,1,1,0}, {1,1,1,1}
                };
                double[,] centers_of_RBF_neurons = { { 0, 1, 1, 1 }, { 1, 0, 1, 1 }, { 1, 1, 1, 1 } };
                double[] Function = { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0 };
                Neuron first = new Neuron();
                switch (choose)
                {
                    case "1":
                        first.gauss_funtion_of_activation(set_of_training_vectors, centers_of_RBF_neurons);
                        first.neuron_learning(true, Function);
                        break;
                    case "2":
                        first.choose_set_of_training_vectors(Function, set_of_training_vectors,
                            centers_of_RBF_neurons);
                        break;
                    default:
                        Console.WriteLine("Недопустимое значение");
                        break;
                }
                Console.WriteLine("Для продолжения нажмите ENTER, для выхода - любую другую клавишу");
                keyInfo = Console.ReadKey();
            } while (keyInfo.Key == ConsoleKey.Enter);
        }
    }
}
