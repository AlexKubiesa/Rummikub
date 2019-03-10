using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using RummikubLib.Statistics;

namespace RummikubGraph
{
    public class MainViewModel
    {
        readonly List<Task<IScoreThresholdAnalysis>> tasks;

        readonly Timer timer;

        public MainViewModel()
        {
            AnalysisData = new ObservableCollection<IScoreThresholdAnalysis>();

            const int maxTileCount = 106;

            tasks = new List<Task<IScoreThresholdAnalysis>>();
            for (int i = 0; i <= maxTileCount; ++i)
            {
                // Declare tileCount inside the loop to prevent the different tasks from accessing the same variable.
                int tileCount = i;
                var task = Task.Run(() => RunAnalysis(tileCount));
                tasks.Add(task);
            }

            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
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

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
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