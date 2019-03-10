using System;
using OxyPlot.Series;
using RummikubLib.Statistics;

namespace RummikubGraph
{
    public class ScatterErrorPointConverter
    {
        public ScatterErrorPointConverter()
        {
            ConvertFunc = Convert;
        }

        public Func<object, ScatterErrorPoint> ConvertFunc { get; }

        public ScatterErrorPoint Convert(object obj)
        {
            if (!(obj is IScoreThresholdAnalysis analysis))
            {
                return null;
            }

            double confidenceIntervalLength =
                analysis.Result.ConfidenceInterval.Max - analysis.Result.ConfidenceInterval.Min;
            double confidenceIntervalMidpoint =
                (analysis.Result.ConfidenceInterval.Max + analysis.Result.ConfidenceInterval.Min) / 2.0;
            return new ScatterErrorPoint(analysis.Simulation.TileCount, confidenceIntervalMidpoint, 0,
                confidenceIntervalLength / 2.0);
        }
    }
}