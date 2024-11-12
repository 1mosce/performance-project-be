using Microsoft.ML;
using MongoDB.Bson;
using MongoDB.Driver;
using PeopleManagmentSystem_API.Models;
using PeopleManagmentSystem_API.Models.Database;
using PeopleManagmentSystem_API.Models.ML;
using PeopleManagmentSystem_API.Services.Interfaces;
using System.Text;
using Task = PeopleManagmentSystem_API.Models.Task;

namespace PeopleManagmentSystem_API.Services
{
    public class TaskService : ITaskService
    {
        private IMongoCollection<Task> _tasks;

        public TaskService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _tasks = database.GetCollection<Task>(settings.TaskCollectionName);
        }

        public async Task<Task> CreateAsync(Task task)
        {
            await _tasks.InsertOneAsync(task);
            return task;
        }

        public async Task<List<Task>> GetAsync()
        {
            return await _tasks.Find(t => true).ToListAsync();
        }

        public async Task<Task> GetAsync(ObjectId id)
        {
            return await _tasks.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Comment>> GetCommentsAsync(ObjectId taskId)
        {
            var task = await _tasks.Find(t => t.Id == taskId).FirstOrDefaultAsync();

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found.");
            }

            return task.Comments;
        }

        public async System.Threading.Tasks.Task RemoveAsync(ObjectId id)
        {
            await _tasks.DeleteOneAsync(t => t.Id == id);
        }

        public async System.Threading.Tasks.Task UpdateAsync(ObjectId id, Task task)
        {
            task.Id = id;
            await _tasks.ReplaceOneAsync(t => t.Id == id, task);
        }

        public async System.Threading.Tasks.Task AddCommentAsync(ObjectId taskId, Comment comment)
        {
            var update = Builders<Task>.Update.Push(t => t.Comments, comment);
            await _tasks.UpdateOneAsync(t => t.Id == taskId, update);
        }

        public async System.Threading.Tasks.Task UpdateCommentAsync(ObjectId taskId, ObjectId commentId, string content)
        {
            var filter = Builders<Task>.Filter.And(
                Builders<Task>.Filter.Eq(t => t.Id, taskId),
                Builders<Task>.Filter.ElemMatch(t => t.Comments, c => c.Id == commentId)
            );
            var update = Builders<Task>.Update.Set(t => t.Comments[-1].Content, content)
                                              .Set(t => t.Comments[-1].SentTime, DateTime.UtcNow);
            await _tasks.UpdateOneAsync(filter, update);
        }

        public async System.Threading.Tasks.Task RemoveCommentAsync(ObjectId taskId, ObjectId commentId)
        {
            var update = Builders<Task>.Update.PullFilter(t => t.Comments, c => c.Id == commentId);
            await _tasks.UpdateOneAsync(t => t.Id == taskId, update);
        }



        //    public double GetProductivity(ObjectId id)
        //    {
        //        // return _tasks.Find(t => t.Id == id).First().CalculateProductivity();
        //        throw new NotImplementedException();
        //    }

        //    private void TrainModel()
        //    {


        //    }

        //    public string GetSkills(string title)
        //    {
        //        var mlContext = new MLContext();

        //        // Створюємо IDataView з наших даних
        //        string dataPath = @"C:\Users\Alyona\source\repos\performance-project-be\Models\ML\task_skills_dataset.csv";

        //        IDataView dataView = mlContext.Data.LoadFromTextFile<TaskData>(dataPath, separatorChar: ',', hasHeader: true, allowQuoting: true);

        //        // 2. Завантажити дані в список TaskData
        //        var taskDataList = mlContext.Data.CreateEnumerable<TaskData>(dataView, reuseRowObject: false).ToList();

        //        // Крок 2. Побудова пайплайну
        //        var modelPipeline = mlContext.Transforms.Text.FeaturizeText("TitleFeaturized", "Title")
        //            .Append(mlContext.Transforms.Text.TokenizeIntoWords("SkillsTokenized", "Skills"))
        //            .Append(mlContext.Transforms.Conversion.MapValueToKey("SkillsKey", "SkillsTokenized", addKeyValueAnnotationsAsText: true))
        //            .Append(mlContext.Transforms.Concatenate("Features", "TitleFeaturized"))
        //            .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("SkillsKey", "Features"))
        //             .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "SkillsKey"));

        //        // Крок 3. Тренування моделі
        //        var model = modelPipeline.Fit(dataView);

        //        // Оцінка моделі
        //        var cvResults = mlContext
        //                            .MulticlassClassification
        //                            .CrossValidate(dataView, modelPipeline, numberOfFolds: 3);

        //        var microAccuracy = cvResults.Average(m => m.Metrics.MicroAccuracy);
        //        var macroAccuracy = cvResults.Average(m => m.Metrics.MacroAccuracy);
        //        var logLossReduction = cvResults.Average(m => m.Metrics.LogLossReduction);

        //        // Збереження моделі
        //        mlContext.Model.Save(model, dataView.Schema, "taskSkillsModel.zip");

        //        // Крок 4. Створення предиктора
        //        var predictor = mlContext.Model.CreatePredictionEngine<TaskData, TaskPrediction>(model);

        //        // Крок 5. Тестування моделі на нових даних
        //        var newTask = new TaskData { Title = title };
        //        var prediction = predictor.Predict(newTask);

        //        var scoreList = prediction.Score
        //.Select((score, index) => new { Index = index, Score = score })
        //.OrderByDescending(x => x.Score)
        //.ToList();

        //        // Виводимо топ-3 найбільш ймовірні скіли
        //        Console.WriteLine("Top 5 predicted skills:");
        //        StringBuilder res = new StringBuilder();
        //        for (int i = 0; i < 5 && i < scoreList.Count; i++)
        //        {
        //            var label = mlContext.Data.CreateEnumerable<TaskData>(dataView, reuseRowObject: false)
        //                .ElementAt(scoreList[i].Index).Skills;
        //            res.AppendLine($"{label} (Confidence: {scoreList[i].Score})");
        //        }

        //        return res.ToString();
        //    }


    }
}
