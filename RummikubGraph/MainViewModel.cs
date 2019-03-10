using System.ComponentModel;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using RummikubLib;
using RummikubLib.Simulation;
using RummikubLib.Statistics;

namespace RummikubGraph
{
    public class MainViewModel
    {
        readonly HighLowSeries series;

        public MainViewModel()
        {
            Plot = new PlotModel
            {
                Title = "Example 1",
            };

            Plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 1 });
            Plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 106 });

            var worker = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };

            const int tileCount = 20;
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            worker.RunWorkerAsync(tileCount);

            series = new HighLowSeries();

            Plot.Series.Add(series);
        }

        static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            const int trialCount = 3000;
            int tileCount = (int) e.Argument;
            const int threshold = 30;
            const double confidenceLevel = 0.95;

            var simulation = new ScoreThresholdSimulation(trialCount, tileCount, threshold);
            var results = simulation.Run();

            var confidenceInterval = BernoulliConfidenceIntervalProvider.Instance.GetConfidenceInterval(results, confidenceLevel);

            e.Result = new HighLowItem(20, confidenceInterval.Max, confidenceInterval.Min);
        }

        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var datum = e.Result as HighLowItem;

            lock (series)
            {
                series.Items.Add(datum);
            }

            Plot.InvalidatePlot(true);
        }

        public PlotModel Plot { get; }
    }
}