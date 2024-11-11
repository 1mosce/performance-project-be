using Amazon.Auth.AccessControlPolicy;
using Microsoft.ML;
using Microsoft.ML.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Models.ML;
using PeopleManagmentSystem_API.Services.Interfaces;
using System.Text;
using System.Xml.Linq;
using static PeopleManagmentSystem_API.TaskClassification;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Task = PeopleManagmentSystem_API.Models.Task;

namespace PeopleManagmentSystem_API.Services
{
    public class TaskService : ITaskService
    {
        private IMongoCollection<Task> _tasks;
        private IMongoCollection<Comment> _comments;

        public TaskService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _tasks = database.GetCollection<Task>(settings.TaskCollectionName);
            _comments = database.GetCollection<Comment>(settings.CommentCollectionName);
        }

        //add ID existence check based on relationships
        public Task Create(Task task)
        {
            _tasks.InsertOne(task);
            return task;
        }

        public List<Task> Get()
        {
            return _tasks.Find(t => true).ToList();
        }

        public Task Get(ObjectId id)
        {
            return _tasks.Find(t => t.Id == id).FirstOrDefault();
        }

        public double GetProductivity(ObjectId id)
        {
            return _tasks.Find(t => t.Id == id).First().CalculateProductivity();
        }

        private void TrainModel()
        {
            
           
        }

        public string GetSkills(string title)
        {
            var mlContext = new MLContext();

            // Створюємо IDataView з наших даних
            string dataPath = @"C:\Users\Alyona\source\repos\performance-project-be\Models\ML\task_skills_dataset.csv";

            IDataView dataView = mlContext.Data.LoadFromTextFile<TaskData>(dataPath, separatorChar: ',', hasHeader: true, allowQuoting: true);

            // 2. Завантажити дані в список TaskData
            var taskDataList = mlContext.Data.CreateEnumerable<TaskData>(dataView, reuseRowObject: false).ToList();

            // Крок 2. Побудова пайплайну
            var modelPipeline = mlContext.Transforms.Text.FeaturizeText("TitleFeaturized", "Title")
                .Append(mlContext.Transforms.Text.TokenizeIntoWords("SkillsTokenized", "Skills"))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("SkillsKey", "SkillsTokenized", addKeyValueAnnotationsAsText: true))
                .Append(mlContext.Transforms.Concatenate("Features", "TitleFeaturized"))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("SkillsKey", "Features"))
                 .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "SkillsKey"));

            // Крок 3. Тренування моделі
            var model = modelPipeline.Fit(dataView);

            // Оцінка моделі
            var cvResults = mlContext
                                .MulticlassClassification
                                .CrossValidate(dataView, modelPipeline, numberOfFolds: 3);

            var microAccuracy = cvResults.Average(m => m.Metrics.MicroAccuracy);
            var macroAccuracy = cvResults.Average(m => m.Metrics.MacroAccuracy);
            var logLossReduction = cvResults.Average(m => m.Metrics.LogLossReduction);

            // Збереження моделі
            mlContext.Model.Save(model, dataView.Schema, "taskSkillsModel.zip");

            // Крок 4. Створення предиктора
            var predictor = mlContext.Model.CreatePredictionEngine<TaskData, TaskPrediction>(model);

            // Крок 5. Тестування моделі на нових даних
            var newTask = new TaskData { Title = title };
            var prediction = predictor.Predict(newTask);

            var scoreList = prediction.Score
    .Select((score, index) => new { Index = index, Score = score })
    .OrderByDescending(x => x.Score)
    .ToList();

            // Виводимо топ-3 найбільш ймовірні скіли
            Console.WriteLine("Top 5 predicted skills:");
            StringBuilder res = new StringBuilder();
            for (int i = 0; i < 5 && i < scoreList.Count; i++)
            {
                var label = mlContext.Data.CreateEnumerable<TaskData>(dataView, reuseRowObject: false)
                    .ElementAt(scoreList[i].Index).Skills;
                res.AppendLine($"{label} (Confidence: {scoreList[i].Score})");
            }

            return res.ToString();
        }


        public List<Comment> GetComments(ObjectId id)
        {
            _tasks.Find(c => c.Id == id).FirstOrDefault();

            return _comments
                .Find(c => c.TaskId == id.ToString())
                .ToList();
        }

        public void Remove(ObjectId id)
        {
            _tasks.DeleteOne(t => t.Id == id);
        }

        public void Update(ObjectId id, Task task)
        {
            task.Id = id;
            _tasks.ReplaceOne(t => t.Id == id, task);
        }
    }
}
