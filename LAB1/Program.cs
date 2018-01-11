using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.Clear();
                Neuron first = new Neuron();
                Console.WriteLine("Выберите функцию и способ обучения:");
                Console.WriteLine("1 - Пороговая ФА и все комбинации переменных");
                Console.WriteLine("2 - Тангенциальная ФА и все комбинации переменных");
                Console.WriteLine("3 - Пороговая ФА и часть комбинаций переменных");
                Console.WriteLine("4 - Тангенциальная ФА и часть комбинаций переменных");
                string choose = Console.ReadLine();
                Console.WriteLine();
                switch (choose)
                {
                    case "1":
                        first.neuron_learning(false, true, new int[0, 0], new int[0]);
                        break;
                    case "2":
                        first.neuron_learning(true, true, new int[0, 0], new int[0]);
                        break;
                    case "3":
                        first.choose_set_of_training_vectors(false);
                        break;
                    case "4":
                        first.choose_set_of_training_vectors(true);
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
