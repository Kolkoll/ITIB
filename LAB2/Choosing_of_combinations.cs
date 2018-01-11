using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_2
{
    public class Choosing_of_combinations
    {
        int elementsCount; // Общее число элементов
        int max_amount_elements; // max кол-во одновременно перебираемых элементов
        int[] array_of_indexes; // массив для хранения текущих перебираемых индексов 
        int current_amount_elements; // текущее количество элементов для перебора
        bool sw;
        int tv; // точка внимания

        public Choosing_of_combinations(int min_limit, int max_limit, int totalCount)
        {
            if ((min_limit < 1) || (min_limit > max_limit) || (max_limit > totalCount))
                throw new IndexOutOfRangeException();
            max_amount_elements = max_limit;
            elementsCount = totalCount;
            current_amount_elements = min_limit;
            array_of_indexes = new int[0];
            int tv = current_amount_elements - 1;
            sw = true;
        }

        public bool Get_Indexes(out int[] myArr)
        {// пройти по всем размерностям
            for (; current_amount_elements <= max_amount_elements; current_amount_elements++, sw = true)
            {
                if (array_of_indexes.Length != current_amount_elements)
                {
                    Array.Resize(ref array_of_indexes, current_amount_elements);
                    for (int j = 0; j < current_amount_elements; j++)
                        array_of_indexes[j] = j; // инициализация номеров для перебора
                    tv = current_amount_elements - 1;
                }
                while (true)
                {
                    if (sw)
                    {
                        sw = false;
                        myArr = new int[array_of_indexes.Length];
                        Array.Copy(array_of_indexes, myArr, array_of_indexes.Length);
                        return true;
                    }
                    if (array_of_indexes[tv] < elementsCount - current_amount_elements + tv)
                    {
                        array_of_indexes[tv]++;
                        for (int m = tv + 1; m < current_amount_elements; m++)
                            array_of_indexes[m] = array_of_indexes[m - 1] + 1;
                        tv = current_amount_elements - 1;
                        sw = true;
                    }
                    else
                    {
                        tv--;
                        sw = false;
                        if (tv < 0)
                            break;
                    }
                }
            }
            myArr = new int[0];
            return false;
        }
    }
}
