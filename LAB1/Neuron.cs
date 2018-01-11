using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LAB_1
{
    class Neuron
    {
        private int[] Function = { 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 };
    //    private int[] Function = { 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0 };
    //    private int[] Function = { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0 };
        private double[] weighting_coefficients = { 0f, 0f, 0f, 0f, 0f };
        private int[,] set_of_training_vectors = 
        {
            {1,0,0,0,0}, {1,0,0,0,1}, {1,0,0,1,0}, {1,0,0,1,1}, {1,0,1,0,0}, {1,0,1,0,1}, {1,0,1,1,0}, {1,0,1,1,1},
            {1,1,0,0,0}, {1,1,0,0,1}, {1,1,0,1,0}, {1,1,0,1,1}, {1,1,1,0,0}, {1,1,1,0,1}, {1,1,1,1,0}, {1,1,1,1,1}
        };

        public int neuron_learning(bool type_of_function, bool amount_of_vectors, int[,] training_vectors, 
            int[] new_Function)
        {
            if (amount_of_vectors == true)//Если обучаем на полной выборке
            {
                int count = 0;
                while (true)
                {
                    int total_error_E = one_age_neuron_learning(type_of_function, set_of_training_vectors, Function,
                        "Полная выборка");
                    count++;
                    if (total_error_E == 0)
                        break;
                }
                Console.WriteLine("Количество эпох = {0}", count);
                return 0;
            }
            if (amount_of_vectors == false)//Если ищем минимальную выборку, на которой достижима 0 ошибка
                return one_age_neuron_learning(type_of_function, training_vectors, new_Function, "");//1 эпоха
            return 0;
        }

        private int one_age_neuron_learning(bool type_of_function, int[,] training_vectors, int[] new_Function,
            string action)
        {
            double[] net_exit = counting_net_exit(training_vectors, weighting_coefficients, new_Function.Length);
            int[] Y_real_exit = new int[new_Function.Length];
            for (int index = 0; index < new_Function.Length; index++)
            {
                if (type_of_function == false) //Если ФАЛ пороговая
                {
                    if (net_exit[index] >= 0)
                        Y_real_exit[index] = 1;
                    else
                        Y_real_exit[index] = 0;
                }
                if (type_of_function == true) //Если ФАЛ сигмоидальная ЭТА ЧАСТЬ ИЗМЕНЕНА
                {
                    double out_exit = 0.5f * (1 + (net_exit[index] / (1 + Math.Abs(net_exit[index]))));
 //                   double out_exit = 0.5f * (1 + Math.Tanh(net_exit[index]));
                    if (out_exit >= 0.5)
                        Y_real_exit[index] = 1;
                    else
                        Y_real_exit[index] = 0;
                }
            }
            double[] delta_error = new double[new_Function.Length];
            for (int index = 0; index < new_Function.Length; index++)
                delta_error[index] = new_Function[index] - Y_real_exit[index];
            int total_error_E = 0;
            for (int index = 0; index < new_Function.Length; index++)
                total_error_E = total_error_E + (new_Function[index] ^ Y_real_exit[index]);
            if (action == "Полная выборка" || action == "Проверка")
            {   
/*                StreamWriter str = new StreamWriter("C:\\Users\\KOL\\Desktop\\1.txt", true);
                str.Write("Y = ");
                for (int i = 0; i < Y_real_exit.Length; i++)
                {
                    str.Write(Y_real_exit[i]);
                    str.Write(", ");
                }
                str.Write(" W = ");
                for (int i = 0; i < 5; i++)
                {
                    str.Write("{0:f2}", weighting_coefficients[i]);
                    str.Write(", ");
                }
                str.Write(" E = {0}", total_error_E);
                str.WriteLine();
                str.Close();   */
                Console.Write("Y = ");
                for (int index = 0; index < Y_real_exit.Length; index++)
                    Console.Write(Y_real_exit[index]);
                Console.Write("  W = ");
                for (int index = 0; index < weighting_coefficients.Length; index++)
                    Console.Write("{0:f2}  ", weighting_coefficients[index]);
                Console.Write("  E = " + total_error_E + "\n");
            }
            for (int i1_index = 0; i1_index < weighting_coefficients.Length; i1_index++)//Коррекция вес. коэффициентов
            {
                double weight_change = 0f;
                for (int i2_index = 0; i2_index < delta_error.Length; i2_index++)
                {
                    if (type_of_function == false)
                        weight_change += (0.3f * delta_error[i2_index] * training_vectors[i2_index, i1_index]);
                    if (type_of_function == true)
                        weight_change += (0.3f * delta_error[i2_index] * training_vectors[i2_index, i1_index] *
                            derivative(net_exit[i2_index]));
                }
                weighting_coefficients[i1_index] = weighting_coefficients[i1_index] + weight_change;
            }
            return total_error_E;
        }

        private double[] counting_net_exit(int[,] training_vectors, double[] weighting_coefficients, int Length)
        {
            double[] net_exit = new double[Length];
            for (int i1_index = 0; i1_index < Length; i1_index++)
            {
                double net = 0f;
                for (int i2_index = 0; i2_index < weighting_coefficients.Length; i2_index++)
                    net = net + weighting_coefficients[i2_index] * training_vectors[i1_index, i2_index];
                net = net + weighting_coefficients[0];
                net_exit[i1_index] = net;
            }
            return net_exit;
        }

        private double derivative(double net_exit)//ЭТА ЧАСТЬ БЫЛА ИЗМЕНЕНА
        {
            double value_of_derivative = Math.Pow(1 - Math.Abs(net_exit / (1 + Math.Abs(net_exit))), 2f);
    //        double value_of_derivative = 2f / (Math.Pow(Math.Exp(net_exit) + Math.Exp(-net_exit),2f));
            return value_of_derivative;
        }

        public void choose_set_of_training_vectors(bool type_of_function)
        {
            bool indicator = false;
            int[,] stored_sample = new int[0, 0];//Инициализация хранилища минимальной выборки
            int[] Funct = new int[0];
            for (int capacity = 1; capacity < 16; capacity++)//Цикл смены разрядности сочетаний
            {
                int[] indexes; // массив для получения результатов
                bool flag; int count_of_combinations = 0; int count_2 = 0;
                Choosing_of_combinations choice = new Choosing_of_combinations(capacity, capacity, 16);
                while (choice.Get_Indexes(out indexes) == true) // получили в curN значения индексов
                {
                    int[,] not_full_training_vectors = new int[16 - indexes.Length, 5];
                    int[,] testing_vectors = new int[indexes.Length, 5];
                    int[] not_full_Function = new int[16 - indexes.Length];
                    int[] testing_Function = new int[indexes.Length];
                    int index = 0; int index_1 = 0;
                    for (int i1_index = 0; i1_index < 16; i1_index++)//Формируем в цикле новую выборку и учителя
                    {
                        flag = false;
                        for (int i2_index = 0; i2_index < indexes.Length; i2_index++)//Новая выборка
                        {
                            if (indexes[i2_index] == i1_index)
                            {
                                for (int i3_index = 0; i3_index < 5; i3_index++)
                                    testing_vectors[index_1, i3_index] = set_of_training_vectors[i1_index, i3_index];
                                testing_Function[index_1] = Function[i1_index];
                                index_1++;
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            for (int i3_index = 0; i3_index < 5; i3_index++)
                                not_full_training_vectors[index, i3_index] = set_of_training_vectors[i1_index, i3_index];
                            not_full_Function[index] = Function[i1_index];
                            index++;
                        }
                    }//Выборка сформирована
                    for (int iindex = 0; iindex < 5; iindex++)//Обнуляем весовые коэффициенты
                        weighting_coefficients[iindex] = 0f;
                    int max_possible_ages = 0;
                    while (max_possible_ages < 100)//Предполагаем, что обучится за 100 эпох
                    {
                        int total_error_E = neuron_learning(type_of_function, false, not_full_training_vectors, 
                            not_full_Function);
                        max_possible_ages++;
                        if (total_error_E == 0)
                            break;
                    }
                    count_of_combinations++;//Счетчик числа комбинаций
                    if (max_possible_ages == 100)//Не обучился
                    {
                        if (count_of_combinations == factorial(16) / (factorial(capacity) * factorial(16 - capacity)))
                        {
                            indicator = true;
                            break;
                        }
                        else
                            continue;
                    }
                    if (max_possible_ages < 100)//Обучился
                    {//Проверяем на тестовой комбинации полученные синаптические веса
                        int total_error_E = neuron_learning(type_of_function,false,testing_vectors,testing_Function);
                        count_2++;
                        if (total_error_E == 0)//Если тест пройден, выборка верна
                        {
                            stored_sample = new int[16 - indexes.Length, 5];
                            //Сохраняем текущую выборку
                            Array.Copy(not_full_training_vectors, stored_sample, (16 - indexes.Length) * 5);
                            Funct = new int[16 - indexes.Length];
                            Array.Copy(not_full_Function, Funct, 16 - indexes.Length);
                            break;
                        }
                        else//Если тест не пройден, выборка не подходит
                        {
                            if (count_of_combinations == factorial(16) / (factorial(capacity) * factorial(16 - capacity)))
                            {
                                indicator = true;
                                break;
                            }
                            else
                                continue;
                        }
                    }
                }
                if (indicator == true)//Не предусматривает случай последней комбинации
                {
                    Console.WriteLine("Обучение закончено, минимальная обучающая выборка: ");
                    for (int index = 0; index < stored_sample.Length / 5; index++)
                    {
                        for (int i2_index = 0; i2_index < 5; i2_index++)
                            Console.Write(stored_sample[index, i2_index]);
                        Console.Write(", ");
                    }
                    //ЭТО ПРОВЕРКА НАЙДЕННОЙ ВЫБОРКИ//
                    Console.WriteLine("\nПроверка:");
                    for (int index = 0; index < 5; index++)
                        weighting_coefficients[index] = 0f;
                    int c = 0;
                    while (true)
                    {
                        int total_error_E = one_age_neuron_learning(type_of_function, stored_sample, Funct,
                            "Проверка");
                        c++;
                        if (total_error_E == 0)
                            break;
                    }
                    Console.WriteLine("Количество эпох = {0}", c);
                    break;
                }
            }
        }

        private Int64 factorial(Int64 x)
        {
            if (x < 0)
                throw new ArgumentException();
            if (x == 0 || x == 1)
                return 1;
            if (x > 1)
                return factorial(x - 1) * x;
            return 0;
        }
    }
}
