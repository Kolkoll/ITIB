using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2
{
    class Neuron
    {
        private double[,] phi_exit = new double[0, 0];
        private double[] weighting_coefficients = { 0f, 0f, 0f, 0f };

        public void gauss_funtion_of_activation(double[,] training_vectors, double[,] RBF_neurons)
        {
            phi_exit = new double[training_vectors.Length / 4 , 4];
            for (int index = 0; index < training_vectors.Length / 4; index++)
            {
                phi_exit[index, 0] = 1f;
                for (int i1_index = 0; i1_index < 3; i1_index++)
                {
                    double sum = 0;
                    for (int i2_index = 0; i2_index < 4; i2_index++)
                        sum += Math.Pow((training_vectors[index, i2_index] - RBF_neurons[i1_index, i2_index]), 2);
                    phi_exit[index, i1_index + 1] = Math.Exp(-sum);
                }
            }
        }

        private double[] counting_net_exit()
        {
            double[] net_exit = new double[phi_exit.Length / 4];
            for (int i1_index = 0; i1_index < phi_exit.Length / 4; i1_index++)
            {
                double sum = 0;
                for (int i2_index = 0; i2_index < 4; i2_index++)
                    sum += weighting_coefficients[i2_index] * phi_exit[i1_index, i2_index];
                net_exit[i1_index] = sum;
            }
            return net_exit;
        }

        private int one_age_neuron_learning(double[] Function, string action)
        {
            double[] net_exit = counting_net_exit();
            double[] Y_real_exit = new double[net_exit.Length];
            for (int index = 0; index < Y_real_exit.Length; index++)
            {
                if (net_exit[index] >= 0)
                    Y_real_exit[index] = 1;
                else
                    Y_real_exit[index] = 0;
            }
            double[] delta_error = new double[Y_real_exit.Length];
            for (int index = 0; index < Y_real_exit.Length; index++)
                delta_error[index] = Function[index] - Y_real_exit[index];
            int total_error_E = 0;
            for (int index = 0; index < Function.Length; index++)
                total_error_E = total_error_E + ((int)Function[index] ^ (int)Y_real_exit[index]);
            if (action == "Полная выборка" || action == "Проверка")
            {
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
                    weight_change += (0.3f * delta_error[i2_index] * phi_exit[i2_index, i1_index]);
                weighting_coefficients[i1_index] = weighting_coefficients[i1_index] + weight_change;
            }
            return total_error_E;
        }

        public int neuron_learning(bool full_sample, double[] Function)
        {
            if (full_sample == true)
            {
                int count = 0;
                int total_error_E = 0;
                while(count < 200)
                {
                    total_error_E = one_age_neuron_learning(Function, "Полная выборка");
                    count++;
                    if (total_error_E == 0)
                        break;
                }
                Console.WriteLine("Количество эпох = {0}", count);
                return 0;
            }
            if (full_sample == false)
                return one_age_neuron_learning(Function, "");
            return 0;
        }

        public void choose_set_of_training_vectors(double[] Function, double[,] set_of_training_vectors,
            double[,] RBF_neurons)
        {
            bool indicator = false;
            double[,] stored_sample = new double[0, 0];//Инициализация хранилища минимальной выборки
            double[] Funct = new double[0];
            for (int capacity = 1; capacity < 16; capacity++)//Цикл смены разрядности сочетаний
            {
                int[] indexes; // массив для получения результатов
                bool flag; int count_of_combinations = 0; int count_2 = 0;
                Choosing_of_combinations choice = new Choosing_of_combinations(capacity, capacity, 16);
                while (choice.Get_Indexes(out indexes) == true) // получили значения индексов
                {
                    double[,] not_full_training_vectors = new double[16 - indexes.Length, 4];
                    double[,] testing_vectors = new double[indexes.Length, 4];
                    double[] not_full_Function = new double[16 - indexes.Length];
                    double[] testing_Function = new double[indexes.Length];
                    int index = 0; int index_1 = 0;
                    for (int i1_index = 0; i1_index < 16; i1_index++)//Формируем в цикле новую выборку и учителя
                    {
                        flag = false;
                        for (int i2_index = 0; i2_index < indexes.Length; i2_index++)//Новая выборка
                        {
                            if (indexes[i2_index] == i1_index)
                            {
                                for (int i3_index = 0; i3_index < 4; i3_index++)
                                    testing_vectors[index_1, i3_index] = 
                                        set_of_training_vectors[i1_index, i3_index];
                                testing_Function[index_1] = Function[i1_index];
                                index_1++;
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            for (int i3_index = 0; i3_index < 4; i3_index++)
                                not_full_training_vectors[index, i3_index] = 
                                    set_of_training_vectors[i1_index, i3_index];
                            not_full_Function[index] = Function[i1_index];
                            index++;
                        }
                    }//Выборка сформирована
                    for (int iindex = 0; iindex < 4; iindex++)//Обнуляем весовые коэффициенты
                        weighting_coefficients[iindex] = 0f;
                    gauss_funtion_of_activation(not_full_training_vectors, RBF_neurons);
                    int max_possible_ages = 0;
                    while (max_possible_ages < 200)//Предполагаем, что обучится за 100 эпох
                    {
                        int total_error_E = neuron_learning(false, not_full_Function);
                        max_possible_ages++;
                        if (total_error_E == 0)
                            break;
                    }
                    count_of_combinations++;//Счетчик числа комбинаций
                    if (max_possible_ages == 200)//Не обучился
                    {
                        if (count_of_combinations == factorial(16) / (factorial(capacity) * 
                            factorial(16 - capacity)))
                        {
                            indicator = true;
                            break;
                        }
                        else
                            continue;
                    }
                    if (max_possible_ages < 200)//Обучился
                    {//Проверяем на тестовой комбинации полученные синаптические веса
                        gauss_funtion_of_activation(testing_vectors, RBF_neurons);
                        int total_error_E = neuron_learning(false, testing_Function);
                        count_2++;
                        if (total_error_E == 0)//Если тест пройден, выборка верна
                        {
                            stored_sample = new double[16 - indexes.Length, 4];
                            //Сохраняем текущую выборку
                            Array.Copy(not_full_training_vectors, stored_sample, (16 - indexes.Length) * 4);
                            Funct = new double[16 - indexes.Length];
                            Array.Copy(not_full_Function, Funct, 16 - indexes.Length);
                            break;
                        }
                        else//Если тест не пройден, выборка не подходит
                        {
                            if (count_of_combinations == factorial(16) / (factorial(capacity) * 
                                factorial(16 - capacity)))
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
                    for (int index = 0; index < stored_sample.Length / 4; index++)
                    {
                        for (int i2_index = 0; i2_index < 4; i2_index++)
                            Console.Write(stored_sample[index, i2_index]);
                        Console.Write(", ");
                    }
                    //ЭТО ПРОВЕРКА НАЙДЕННОЙ ВЫБОРКИ//
                    Console.WriteLine("\nПроверка:");
                    for (int index = 0; index < 4; index++)
                        weighting_coefficients[index] = 0f;
                    int c = 0;
                    gauss_funtion_of_activation(stored_sample, RBF_neurons);
                    while (true)
                    {
                        int total_error_E = one_age_neuron_learning(Funct, "Проверка");
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
