using System.Linq;
using System.Numerics;
using System.Windows;
using FourierOptions = MathNet.Numerics.IntegralTransforms.FourierOptions;
using Fourier = MathNet.Numerics.IntegralTransforms.Fourier;

namespace FFT_Learn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var sampleRate = 80;
            var sampleLen = sampleRate * 4;
            var data = MathNet.Numerics.Generate.Sinusoidal(sampleLen, sampleRate, 1, 1);
            var data2 = MathNet.Numerics.Generate.Sinusoidal(sampleLen, sampleRate, 2, 2);
            var data3 = MathNet.Numerics.Generate.Sinusoidal(sampleLen, sampleRate, 3, 3);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] += data2[i];
                data[i] += data3[i];
            }

            var dataClone = data.Select(d => new Complex(d, 0.0)).ToArray();

            Fourier.Forward(dataClone, FourierOptions.Default);

            var fft = dataClone.Select(d => d.Magnitude).ToArray();

            var scale = Fourier.FrequencyScale(fft.Length, sampleRate);

            var x = new double[sampleLen];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = i;
            }

            Plot_Origin.Plot.Title("Origin");
            Plot_Origin.Plot.AddScatter(x, data);
            Plot_Origin.Plot.AxisAuto();
            Plot_Origin.Plot.Render();


            Plot_FFT.Plot.Title("FFT");
            Plot_FFT.Plot.AddScatter(scale.Take(40).ToArray(), fft.Take(40).ToArray());
            Plot_FFT.Plot.SetViewLimits(0, 1000);
            Plot_FFT.Plot.Render();
        }

    }
}