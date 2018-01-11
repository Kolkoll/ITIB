using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ
{
    class Back_propagation_of_error
    {
        double[] weighting_coefficients_of_hidden_layer = { 0f, 0f };
        double[,] weighting_coefficients_of_out_layer = { { 0f, 0f }, { 0f, 0f }, { 0f, 0f }};

        public double counting_net_exit(string layer, double[] X, int start)
        {
            double net_exit = 0f;
            double sum = 0f;
            if (layer == "Скрытый")
            {
                for (int index = 0; index < weighting_coefficients_of_hidden_layer.Length; index++)
                    sum += weighting_coefficients_of_hidden_layer[index] * X[index];
                net_exit = sum;
                return net_exit;
            }
            if (layer == "Выходной")
            {
                net_exit += weighting_coefficients_of_out_layer[start, 0];
                net_exit += weighting_coefficients_of_out_layer[start, 1] * X[0];
                return net_exit;
            }
            return 0f;
        }

        public double[] function_of_activation(double[] net)
        {
            double[] out_exit = new double[net.Length];
            for (int index = 0; index < net.Length; index++)
            {
                out_exit[index] = 1 / (1 + Math.Exp(-net[index]));
           //     Console.Write(" {0}", out_exit[index]);
            }
            return out_exit;
        }

        public double[] counting_delta_error(double[] Y_exit, double[] T)
        {
            double[] delta = new double[Y_exit.Length];
            for (int index = 0; index < Y_exit.Length; index++)
                delta[index] = Y_exit[index] - T[index];
            return delta;
        }

        public double[] counting_mistake_out_layer(double[] Y_exit, double[] T, double[] net)
        {
            double[] mistake = new double[Y_exit.Length];
            for (int index = 0; index < Y_exit.Length; index++)
                mistake[index] = (T[index] - Y_exit[index]) * (1 / (1 + Math.Exp(-net[index])) *
                    (1 - (1 / (1 + Math.Exp(-net[index])))));
            return mistake;
        }

        public double[] counting_mistake_hidden_layer(double[] net, double[] mistake_out_layer)
        {
            double[] mistake = new double[2];
            double sum = 0f;
            for (int indexx = 0; indexx < mistake.Length; indexx++)
            {
                for (int index = 0; index < weighting_coefficients_of_out_layer.Length / 2; index++)
                    sum += mistake_out_layer[index] * weighting_coefficients_of_out_layer[index, indexx];
                mistake[indexx] = (1 / (1 + Math.Exp(-net[0])) * (1 - (1 / (1 + Math.Exp(-net[0]))))) * sum;
            }
            return mistake;
        }

        public void correction_of_weighting_coefficients(double[] X, double[] delta, string layer)
        {
            if (layer == "Выходной")
            {
          //      Console.WriteLine("Веса выходного слоя:");
                double[,] delta_w = new double[weighting_coefficients_of_out_layer.Length / 2,
                    weighting_coefficients_of_out_layer.Length / 3];
                for (int i1_index = 0; i1_index < weighting_coefficients_of_out_layer.Length / 2; i1_index++)
                {
                    for (int i2_index = 0; i2_index < weighting_coefficients_of_out_layer.Length / 3; i2_index++)
                    {
                        delta_w[i1_index, i2_index] = X[i2_index] * delta[i1_index];
                        weighting_coefficients_of_out_layer[i1_index, i2_index] += delta_w[i1_index, i2_index];
              //          Console.Write(", {0}", weighting_coefficients_of_out_layer[i1_index, i2_index]);
                    }
                //    Console.WriteLine();
                }
            }
            if (layer == "Скрытый")
            {
         //       Console.WriteLine("Веса скрытого слоя:");
                double[] delta_w_c = new double[weighting_coefficients_of_hidden_layer.Length];
                for (int index = 0; index < delta_w_c.Length; index++)
                {
                    delta_w_c[index] = X[index] * delta[index];
                    weighting_coefficients_of_hidden_layer[index] += delta_w_c[index];
             //       Console.Write(", {0}", weighting_coefficients_of_hidden_layer[index]);
                }
            }

        }
    }
}
