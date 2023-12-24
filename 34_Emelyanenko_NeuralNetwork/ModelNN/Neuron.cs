using System;
using static System.Math;


namespace _34_Emelyanenko_NeuralNetwork.ModelNN{

    class Neuron{
        //private double a = 1.7159;
        //private double b = 2 / 3;
        private double a = 1;
        private double b = 1;
        const double alpha = 0.01; // Значение alpha для Leaky ReLU
        // поля
        private NeuroType type;
        private double[] inputs;
        private double[] weights;
        private double derivative;

        private double output;

        // свойства 
        public double[] Inputs
        {
            get { return inputs; }
            set { inputs = value; }
        }

        public double[] Weights
        {
            get { return weights; }
            set { weights = value; }
        }

        public double Derivative
        {
            get { return derivative; }
        }

        public double Output
        {
            get { return output; }
        }

        // конструктор

        public Neuron(double[] ws, NeuroType t){
            type = t;
            weights = ws;
        }

        public void Activator(){
            // первый элемент weights - b (порог)
            double sum = weights[0];
            for (int i = 0; i < inputs.Length; i++){
                sum += inputs[i] * weights[i+1];
            }

            switch (type)
            {
                case NeuroType.Output:
                    output = Exp(sum);
                    break;
                case NeuroType.Hidden:
                    output = Leaky(sum);
                    derivative = LeakyDerivator(sum);
                    break;
                default:
                    break;
            }
        }

        // функция активации (гиперболический тангенс)
        private double Leaky(double x){
            return Math.Max(0.01 * x, x);
        }

        // вычисление производной
        private double LeakyDerivator(double x){
            if (x > 0)
                return 1;
            else return 0.01;
        }
    }
}
