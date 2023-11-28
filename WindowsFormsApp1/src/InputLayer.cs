using System;
using System.IO;


namespace WindowsFormsApp1.src
{
    class InputLayer
    {

        private Random random = new Random();

        // поля 

        private (double[], int)[] trainSet = new (double[], int)[100]; 


        public (double[], int)[] TrainSet { get => trainSet; }

        // конструктор
        public InputLayer(NetworkMode nm) {
            // Вернёмся позже
        }
    }
}
