namespace RummikubLib.Statistics
{
    public interface IScoreThresholdAnalysisResult
    {
        Range ConfidenceInterval { get; }
    }
}