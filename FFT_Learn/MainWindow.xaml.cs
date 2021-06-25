using System.Linq;
using System.Numerics;
using System.Windows;
using FourierOptions = MathNet.Numerics.IntegralTransforms.FourierOptions;
using Fourier = MathNet.Numerics.IntegralTransforms.Fourier;
using SignalGen = MathNet.Numerics.Generate;

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
            var sampleRate = 64;
            var sampleLen = sampleRate * 4;
            var signal1Hz = SignalGen.Sinusoidal(sampleLen, sampleRate, 1, 1);
            var signal2Hz = SignalGen.Sinusoidal(sampleLen, sampleRate, 2, 1);
            var signal3Hz = SignalGen.Sinusoidal(sampleLen, sampleRate, 3, 1);

            var signalMixed = new double[signal1Hz.Length];

            for (int i = 0; i < signal1Hz.Length; i++)
            {
                signalMixed[i] += signal1Hz[i];
                signalMixed[i] += signal2Hz[i];
                signalMixed[i] += signal3Hz[i];
            }

            var dataClone = signalMixed.Select(d => new Complex(d, 0.0)).ToArray();

            Fourier.Forward(dataClone, FourierOptions.Default);

            var fft = dataClone.Select(d => d.Magnitude).ToArray();

            var scale = Fourier.FrequencyScale(fft.Length, sampleRate);

            var x = new double[sampleLen];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = i;
            }

            Plot_1Hz.Plot.Title("1 Hz");
            Plot_1Hz.Plot.AddScatter(x, signal1Hz);
            Plot_1Hz.Plot.AxisAuto();
            Plot_1Hz.Refresh();

            Plot_2Hz.Plot.Title("2 Hz");
            Plot_2Hz.Plot.AddScatter(x, signal2Hz);
            Plot_2Hz.Plot.AxisAuto();
            Plot_2Hz.Refresh();

            Plot_3Hz.Plot.Title("3 Hz");
            Plot_3Hz.Plot.AddScatter(x, signal3Hz);
            Plot_3Hz.Plot.AxisAuto();
            Plot_3Hz.Refresh();

            Plot_Mix.Plot.Title("Mix");
            Plot_Mix.Plot.AddScatter(x, signalMixed);
            Plot_Mix.Plot.AxisAuto();
            Plot_Mix.Refresh();


            Plot_FFT.Plot.Title("FFT");
            Plot_FFT.Plot.AddScatter(scale.Take(sampleLen/4).ToArray(), fft.Take(sampleLen / 4).ToArray());
            Plot_FFT.Plot.SetOuterViewLimits(0, 1000);
            Plot_FFT.Refresh();
        }

    }
}