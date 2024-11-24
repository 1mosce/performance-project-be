using Microsoft.ML.Data;
using Microsoft.ML;

namespace PerfomanceProject.Shared.Models.ML
{
    public class ProductivityModel
    {
        public class ProductivityData
        {
            [LoadColumn(0)]
            public float TaskDifficulty { get; set; }

            [LoadColumn(1)]
            public float TaskRating { get; set; }

            [LoadColumn(2)] 
            public float PlannedTime { get; set; }

            [LoadColumn(3)]
            public float RealTime { get; set; }

            [LoadColumn(4)]
            public float Engagement { get; set; }

            [LoadColumn(5)]
            public float TasksPerPeriod { get; set; }

            [LoadColumn(6), ColumnName("Label")]
            public float Productivity { get; set; }
        }


        public class ProductivityPrediction
        {
            [ColumnName("Score")]
            public float PredictedProductivity { get; set; }
        }

        public class UserProductivity
        {
            public string UserId { get; set; }
            public double TotalProductivity { get; set; }
        }
    }
}
