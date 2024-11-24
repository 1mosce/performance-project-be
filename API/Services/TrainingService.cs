using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.LightGbm;
using static PerfomanceProject.Shared.Models.ML.ProductivityModel;

namespace PeopleManagmentSystem_API.Services
{
    public class TrainingService
    {
        private readonly MLContext _mlContext;
        private string modelPath;
        private string csvFilePath;

        public TrainingService()
        {
            _mlContext = new MLContext();
            modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../ML/productivity_model.zip");
            csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../ML/synthetic_productivity_data.csv");
        }

        public RegressionMetrics TrainModel()
        {
            if (!File.Exists(csvFilePath))
            {
                throw new FileNotFoundException($"File {csvFilePath} not found.");
            }

            var dataView = _mlContext.Data.LoadFromTextFile<ProductivityData>(
                csvFilePath,
                hasHeader: true,
                separatorChar: ',');

            var split = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

            var pipeline = _mlContext.Transforms.Concatenate("Features",
                nameof(ProductivityData.TaskDifficulty),
                nameof(ProductivityData.TaskRating),
                nameof(ProductivityData.PlannedTime),
                nameof(ProductivityData.RealTime),
                nameof(ProductivityData.Engagement),
                nameof(ProductivityData.TasksPerPeriod))
                .Append(_mlContext.Regression.Trainers.LightGbm(
                new LightGbmRegressionTrainer.Options
                {
                    NumberOfLeaves = 15, // Reducing model complexity
                    MinimumExampleCountPerLeaf = 10,
                    LearningRate = 0.05,
                    L2CategoricalRegularization = 1.0
                }))
                .Append(_mlContext.Transforms.NormalizeMeanVariance("Features"));

    //        var labels = _mlContext.Data.CreateEnumerable<ProductivityData>(dataView, reuseRowObject: false)
    //.Select(d => d.Productivity);
    //        Console.WriteLine($"Min: {labels.Min()}, Max: {labels.Max()}, Avg: {labels.Average()}");


            // Cross-validation
            var cvResults = _mlContext.Regression.CrossValidate(
                data: dataView,
                estimator: pipeline,
                numberOfFolds: 5,
                labelColumnName: "Label");

            // Print cross-validation metrics
            var avgR2 = cvResults.Average(fold => fold.Metrics.RSquared);
            var avgMSE = cvResults.Average(fold => fold.Metrics.MeanSquaredError);

            Console.WriteLine("Cross-Validation Results:");
            Console.WriteLine($"Average R²: {avgR2}");
            Console.WriteLine($"Average MSE: {avgMSE}");

            var model = pipeline.Fit(split.TrainSet);

            var predictions = model.Transform(split.TestSet);
            var metrics = _mlContext.Regression.Evaluate(predictions);

            Console.WriteLine($"R²: {metrics.RSquared}");
            Console.WriteLine($"Mean Absolute Error: {metrics.MeanAbsoluteError}");
            Console.WriteLine($"Mean Squared Error: {metrics.MeanSquaredError}");
            Console.WriteLine($"Root Mean Squared Error: {metrics.RootMeanSquaredError}");

            _mlContext.Model.Save(model, split.TrainSet.Schema, modelPath);
            Console.WriteLine($"Model saved to {modelPath}");

            return metrics;
        }
    }
}
