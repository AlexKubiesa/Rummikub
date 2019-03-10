using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using RummikubLib.Simulation;
using RummikubLib.Statistics;

namespace RummikubGraph
{
    public class MainViewModel
    {
        readonly HighLowSeries series;

        readonly List<Task<HighLowItem>> tasks;

        readonly Timer timer;

        public MainViewModel()
        {
            Plot = new PlotModel
            {
                Title = "Example 1",
            };

            Plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 1 });
            Plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 106 });

            series = new HighLowSeries();
            Plot.Series.Add(series);

            tasks = new List<Task<HighLowItem>>();
            for (int i = 0; i <= 30; ++i)
            {
                // Declare tileCount inside the loop to prevent the different tasks from accessing the same variable.
                int tileCount = i;
                var task = Task.Run(() => GetDataPoint(tileCount));
                tasks.Add(task);
            }

            timer = new Timer(UpdatePlot, null, 0, 1000);
        }

        public PlotModel Plot { get; }

        static HighLowItem GetDataPoint(int tileCount)
        {
            const int trialCount = 3000;
            const int threshold = 30;
            const double confidenceLevel = 0.95;

            var simulation = new ScoreThresholdSimulation(trialCount, tileCount, threshold);
            var results = simulation.Run();

            var confidenceInterval = BernoulliConfidenceIntervalProvider.Instance.GetConfidenceInterval(results, confidenceLevel);

            return new HighLowItem(tileCount, confidenceInterval.Max, confidenceInterval.Min);
        }

        void UpdatePlot(object state)
        {
            lock (tasks)
            {
                var completedTasks = tasks.Where(x => x.IsCompleted).ToArray();

                foreach (var task in completedTasks)
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        series.Items.Add(task.Result);
                    }

                    tasks.Remove(task);
                }

                if (tasks.Count == 0)
                {
                    timer.Dispose();
                }

                Plot.InvalidatePlot(true);
            }
        }
    }
}