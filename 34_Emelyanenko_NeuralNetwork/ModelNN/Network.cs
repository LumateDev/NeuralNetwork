using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace _34_Emelyanenko_NeuralNetwork.ModelNN
{
    class Network
    {
        Form form;
        int epochK = 0;
        //Массив для хранения вектора выходного сигнала нейросети
        public double[] Fact = new double[10];

        //Все слои нейросети
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer1 = new HiddenLayer(70, 15, NeuroType.Hidden, nameof(hidden_layer1));
        private HiddenLayer hidden_layer2 = new HiddenLayer(35, 70, NeuroType.Hidden, nameof(hidden_layer2));
        private OutputLayer output_layer = new OutputLayer(10, 35, NeuroType.Output, nameof(output_layer));

        // Среднее значение энергии ошибки эпохи обучения
        private double e_error_avg;

        //Свойства
        public double E_error_avg
        {
            get { return e_error_avg; }
            set { e_error_avg = value; }
        }

        //Конструктор
        public Network(NetworkMode nm)
        {
            input_layer = new InputLayer(nm);
        }

        //Прямой проход сигнала по нейросети
        public void ForwardPass(Network net, double[] net_input)
        {
            net.hidden_layer1.Data = net_input;
            net.hidden_layer1.Recognize(null, net.hidden_layer2);
            net.hidden_layer2.Recognize(null, net.output_layer);
            net.output_layer.Recognize(net, null);
        }

        private Form createChartForm()
        {
            var errorChartForm = new Form
            {
                Text = "График ошибки",
                Width = 600,
                Height = 400
            };

            Chart errorChart = new Chart
            {
                Dock = DockStyle.Fill
            };
            //errorChart.ChartAreas[0].AxisY.Minimum = 0;
            //errorChart.ChartAreas[0].AxisX.Minimum = 0;

            Series errorSeries = new Series("Ошибка")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2
            };

            errorChart.Series.Add(errorSeries);
            errorChart.ChartAreas.Add(new ChartArea());


            errorChart.ChartAreas[0].AxisX.Title = "Эпоха";
            errorChart.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            errorChart.ChartAreas[0].AxisY.Title = "Энергия ошибки";
            errorChart.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);

            // Добавление Chart на форму
            errorChartForm.Controls.Add(errorChart);

            // Подписка на событие закрытия формы
            errorChartForm.FormClosed += (sender, e) => { form = null; errorChartForm = null; };
            return errorChartForm;
        }

        private void UpdateErrorChartAsync(Form errorChartForm, double error)
        {
            // Вызов метода UpdateChart на форме ErrorChartForm
            errorChartForm.Invoke(new Action(() => UpdateChart(errorChartForm, error)));
        }

        private void UpdateChart(Form form, double error)
        {
            // Получение элемента управления Chart из формы
            Chart errorChart = (Chart)form.Controls[0];

            // Добавление новой точки на график
            errorChart.Series[0].Points.AddXY(epochK++, error);

            // Обновление графика
            errorChart.Update();
        }


        // Обучение
        public async Task Train(Network net)
        {
            if (form == null) { 
                form = createChartForm();
                form.Show();
            }

            int epochs = 100;
            net.input_layer = new InputLayer(NetworkMode.Train);

            double tmpSumError; // Временная переменная суммы ошибок
            double[] errors; //Вектор сигнала ошибки выходного слоя
            double[] tmpGradSums1; //Вектор градиента первого скрытого слоя
            double[] tmpGradSums2; //Вектор градиента второго скрытого слоя

            for (int k = 0; k < epochs; k++)
            {
                e_error_avg = 0; // Обнуляем значение в начале эпохи
                for (int i = 0; i < net.input_layer.TrainSet.Length; i++)
                {
                    //Прямой проход
                    ForwardPass(net, net.input_layer.TrainSet[i].Item1);

                    //Вычисление ошибки по итерации
                    tmpSumError = 0;
                    errors = new double[net.Fact.Length];
                    for (int x = 0; x < errors.Length; x++)
                    {
                        if (x == net.input_layer.TrainSet[i].Item2)
                        {
                            errors[x] = 1.0 - net.Fact[x];
                        }
                        else
                        {
                            errors[x] = -net.Fact[x];
                        }

                        //Собираем энергию ошибки
                        tmpSumError += errors[x] * errors[x] / 2.0;
                    }

                    e_error_avg += tmpSumError / errors.Length; //Суммарное значение энергии оишбки эпох
                    
                    //Обратный проход и коррекция весов
                    tmpGradSums2 = net.output_layer.BackwardPass(errors);
                    tmpGradSums1 = net.hidden_layer2.BackwardPass(tmpGradSums2);
                    net.hidden_layer1.BackwardPass(tmpGradSums1);
                }
                
                e_error_avg /= net.input_layer.TrainSet.Length; //Среднее значение энергии ошибки одной эпохи
                UpdateErrorChartAsync(form, e_error_avg);
                //Console.WriteLine($"Эпоха {k}, ошибка: {e_error_avg}");
            }

            net.input_layer = null; //Обнуление входного слоя

            //Сохранение скорректированных весов
            net.hidden_layer1.WeightsInitialize(MemoryMode.SET);
            net.hidden_layer2.WeightsInitialize(MemoryMode.SET);
            net.output_layer.WeightsInitialize(MemoryMode.SET);

        }


        // Тестирование
        public void Test(Network net)
        {
            net.input_layer = new InputLayer(NetworkMode.Test);
            double m = 0;
            for (int i = 0; i < net.input_layer.TestSet.Length; i++)
            {
                ForwardPass(net, net.input_layer.TestSet[i].Item1);
                m += (Fact.ToList().IndexOf(Fact.Max()) == net.input_layer.TestSet[i].Item2) ? 1: 0;
            }
            MessageBox.Show(
                $"Точность: {m/net.input_layer.TestSet.Length}",
            "Предсказание",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.RightAlign);

        }
    }
}
