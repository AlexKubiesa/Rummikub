namespace RummikubLib.Statistics
{
    public class ScoreThresholdAnalysisResult : IScoreThresholdAnalysisResult
    {
        public ScoreThresholdAnalysisResult(Range confidenceInterval)
        {
            ConfidenceInterval = confidenceInterval;
        }

        public Range ConfidenceInterval { get; }
    }
}