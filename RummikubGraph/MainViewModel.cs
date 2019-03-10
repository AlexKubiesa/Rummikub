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
        readonly object updatePlotLock = new object();

        readonly HighLowSeries highLowSeries;

        readonly ScatterSeries scatterSeries;

        readonly List<Task<IScoreThresholdAnalysis>> tasks;

        readonly Timer timer;

        public MainViewModel()
        {
            Plot = new PlotModel
            {
                Title = "Probability of not being able to lay out tiles in Rummikub",
            };

            Plot.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = 0, Maximum = 1 });
            Plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 106 });

            highLowSeries = new HighLowSeries();
            Plot.Series.Add(highLowSeries);

            scatterSeries = new ScatterSeries();
            Plot.Series.Add(scatterSeries);

            scatterSeries.MarkerType = MarkerType.Cross;
            scatterSeries.MarkerSize = 5;
            scatterSeries.MarkerStrokeThickness = 1;
            scatterSeries.MarkerStroke = OxyColor.FromRgb(0, 0, 0);

            //int maxTileCount = DesignerProperties.GetIsInDesignMode() ? 10 : 30;

            tasks = new List<Task<IScoreThresholdAnalysis>>();
            for (int i = 0; i <= 20; ++i)
            {
                // Declare tileCount inside the loop to prevent the different tasks from accessing the same variable.
                int tileCount = i;
                var task = Task.Run(() => RunAnalysis(tileCount));
                tasks.Add(task);
            }

            timer = new Timer(UpdatePlot, null, 0, 1000);
        }

        public PlotModel Plot { get; }

        static IScoreThresholdAnalysis RunAnalysis(int tileCount)
        {
            const int trialCount = 3000;
            const int threshold = 30;
            const double confidenceLevel = 0.95;

            var analysis = new ScoreThresholdAnalysis(trialCount, tileCount, threshold, confidenceLevel);
            analysis.Run();

            return analysis;
        }

        void UpdatePlot(object state)
        {
            lock (updatePlotLock)
            {
                var completedTasks = tasks.Where(x => x.IsCompleted).ToArray();

                foreach (var task in completedTasks)
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        highLowSeries.Items.Add(GetHighLowItem(task.Result));
                        scatterSeries.Points.Add(GetScatterPoint(task.Result));
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

        static HighLowItem GetHighLowItem(IScoreThresholdAnalysis analysis)
        {
            return new HighLowItem(analysis.Simulation.TileCount,
                analysis.Result.ConfidenceInterval.Max, analysis.Result.ConfidenceInterval.Min);
        }

        static ScatterPoint GetScatterPoint(IScoreThresholdAnalysis analysis)
        {
            return new ScatterPoint(analysis.Simulation.TileCount, (analysis.Result.ConfidenceInterval.Max + analysis.Result.ConfidenceInterval.Min) / 2.0);
        }
    }
}