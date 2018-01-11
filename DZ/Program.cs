using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.Clear();
                Console.WriteLine("Обучение МНС методом обратного распространения ошибки");
                double[] x = { 1, -1 };
                double[] t = {-1, 2, 2 };
                Back_propagation_of_error obj = new Back_propagation_of_error();
                int quit = 0;
                while(true)
                {
                    Console.WriteLine();
                    double[] net1 = new double[1];
                    net1[0] = obj.counting_net_exit("Скрытый", x, 0);
                    double[] out1 = new double[1];
//                    Console.WriteLine("Скрытый слой нейронов:");
                    out1 = obj.function_of_activation(net1);
 //                   Console.WriteLine();
                    double[] x2 = new double[1];
                    x2[0] = out1[0];
                    double[] net2 = new double[3];
                    for (int index = 0; index < net2.Length; index++)
                        net2[index] = obj.counting_net_exit("Выходной", x2, index);
 //                   Console.WriteLine();
                    double[] out2 = new double[3];
                    Console.WriteLine("Выходной слой нейронов:");
                    out2 = obj.function_of_activation(net2);
                    for (int index = 0; index < out2.Length; index++)
                        Console.Write(" {0:f2}", out2[index]);
                    Console.WriteLine();
  //                  for (int i = 0; i < out2.Length; i++)
  //                      Console.WriteLine(out2[i]);
                    double[] delta = obj.counting_delta_error(out2, t);
    //                for (int index = 0; index < delta.Length; index++)
     //                   Console.Write(" {0}", delta[index]);
      //              Console.WriteLine();
                    quit++;
     //               for (int i = 0; i < delta.Length; i++)
      //                  Console.Write(", {0}", delta[i]);
                    Console.WriteLine("Ошибки выходного слоя:");
                    double[] mistake = obj.counting_mistake_out_layer(out2, t, net2);
                    for (int index = 0; index < mistake.Length; index++)
                        Console.Write(" {0:f2}", mistake[index]);
                    Console.WriteLine();
     //               for (int i = 0; i < mistake.Length; i++)
     //                   Console.WriteLine(mistake[i]);
                    Console.WriteLine("Ошибки скрытого слоя:");
                    double[] mistake2 = obj.counting_mistake_hidden_layer(net1, mistake);
                    for (int index = 0; index < mistake2.Length; index++)
                        Console.Write(" {0:f2}", mistake2[index]);
                    Console.WriteLine();
     //               for (int i = 0; i < mistake2.Length; i++)
     //                   Console.WriteLine(mistake2[i]);
                    double[] x_var = {1f, x2[0]};
                    obj.correction_of_weighting_coefficients(x_var, mistake, "Выходной");
                    obj.correction_of_weighting_coefficients(x, mistake2, "Скрытый");
                    Console.WriteLine();
                    if (quit == 2)
                        break;
                }
                Console.WriteLine("Для продолжения нажмите ENTER, для выхода - любую другую клавишу");
                keyInfo = Console.ReadKey();
            } while (keyInfo.Key == ConsoleKey.Enter);
        }
    }
}
