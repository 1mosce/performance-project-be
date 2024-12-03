using Microsoft.ML;
using Microsoft.ML.Data;
using static PerfomanceProject.Shared.Models.ML.ProductivityModel;

namespace PeopleManagmentSystem_API.Services
{
    public class PredictionService
    {
        private readonly MLContext _mlContext;
        private string modelPath;
        private string testDataPath;
        public PredictionService()
        {
            _mlContext = new MLContext();
            modelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../ML/productivity_model.zip");
            testDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../ML/test_productivity_data.csv");
        }

        public virtual float PredictProductivity(ProductivityData inputData)
        {
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"Model file not found at {modelPath}");
            }

            ITransformer trainedModel = _mlContext.Model.Load(modelPath, out _);

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductivityData, ProductivityPrediction>(trainedModel);

            var prediction = predictionEngine.Predict(inputData);
            return prediction.PredictedProductivity;
        }

        public RegressionMetrics? EvaluateModel()
        {
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"Model file not found at {modelPath}");
            }

            ITransformer trainedModel = _mlContext.Model.Load(modelPath, out var modelInputSchema);

            var testDataView = _mlContext.Data.LoadFromTextFile<ProductivityData>(
                testDataPath,
                hasHeader: true,
                separatorChar: ',');

            var predictions = trainedModel.Transform(testDataView);

            var metrics = _mlContext.Regression.Evaluate(predictions);

            Console.WriteLine($"Model Evaluation Metrics:");
            Console.WriteLine($"R^2 Score: {metrics.RSquared:0.##}");
            Console.WriteLine($"Mean Absolute Error (MAE): {metrics.MeanAbsoluteError:#.##}");
            Console.WriteLine($"Root Mean Squared Error (RMSE): {metrics.RootMeanSquaredError:#.##}");

            return metrics;
        }
    }
}
