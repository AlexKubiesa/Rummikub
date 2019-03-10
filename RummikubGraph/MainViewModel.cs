using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using RummikubLib.Statistics;

namespace RummikubGraph
{
    public class MainViewModel
    {
        readonly object updatePlotLock = new object();

        readonly List<Task<IScoreThresholdAnalysis>> tasks;

        readonly Timer timer;

        public MainViewModel()
        {
            AnalysisData = new ObservableCollection<IScoreThresholdAnalysis>();

            tasks = new List<Task<IScoreThresholdAnalysis>>();
            for (int i = 0; i <= 30; ++i)
            {
                // Declare tileCount inside the loop to prevent the different tasks from accessing the same variable.
                int tileCount = i;
                var task = Task.Run(() => RunAnalysis(tileCount));
                tasks.Add(task);
            }

            timer = new Timer(UpdateData, null, 0, 1000);
        }

        public ObservableCollection<IScoreThresholdAnalysis> AnalysisData { get; }

        static IScoreThresholdAnalysis RunAnalysis(int tileCount)
        {
            const int trialCount = 3000;
            const int threshold = 30;
            const double confidenceLevel = 0.95;

            var analysis = new ScoreThresholdAnalysis(trialCount, tileCount, threshold, confidenceLevel);
            analysis.Run();

            return analysis;
        }

        void UpdateData(object state)
        {
            lock (updatePlotLock)
            {
                var completedTasks = tasks.Where(x => x.IsCompleted).ToArray();

                foreach (var task in completedTasks)
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                            AnalysisData.Add(task.Result));

                    }

                    tasks.Remove(task);
                }

                if (tasks.Count == 0)
                {
                    timer.Dispose();
                }
            }
        }
    }
}