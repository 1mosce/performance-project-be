using Microsoft.ML;
using MongoDB.Bson;
using MongoDB.Driver;
using PerformanceProject.Shared.Models;
using PerformanceProject.Shared.Models.Database;
using PerformanceProject.Shared.Models.ML;
using PeopleManagmentSystem_API.Services.Interfaces;
using System.Text;
using Task = PerformanceProject.Shared.Models.Task;

namespace PeopleManagmentSystem_API.Services
{
    public class TaskService : ITaskService
    {
        private IMongoCollection<Task> _tasks;
        private readonly IProjectService _projectService;
        public TaskService(IPeopleManagmentDatabaseSettings settings, IMongoClient mongoClient, IProjectService projectService)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _tasks = database.GetCollection<Task>(settings.TaskCollectionName);
            _projectService = projectService;
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

        public async System.Threading.Tasks.Task RemoveAsync(ObjectId id)
        {
            await _tasks.DeleteOneAsync(t => t.Id == id);
        }

        public async System.Threading.Tasks.Task UpdateAsync(ObjectId id, Task task)
        {
            task.Id = id;
            await _tasks.ReplaceOneAsync(t => t.Id == id, task);
        }

        // Comments
        public async Task<List<Comment>> GetCommentsAsync(ObjectId projectId, ObjectId taskId)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            return task.Comments;
        }

        public async Task<Comment> GetCommentAsync(ObjectId projectId, ObjectId taskId, ObjectId commentId)
        {
            var comments = await GetCommentsAsync(projectId, taskId);
            var comment = comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
                throw new KeyNotFoundException($"Comment with Id '{commentId}' not found in Task '{taskId}'.");

            return comment;
        }

        public async System.Threading.Tasks.Task AddCommentAsync(ObjectId projectId, ObjectId taskId, Comment comment)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            comment.Id = ObjectId.GenerateNewId();
            comment.SentTime = DateTime.UtcNow;
            task.Comments.Add(comment);

            await _projectService.UpdateAsync(projectId, project);
        }

        public async System.Threading.Tasks.Task UpdateCommentAsync(ObjectId projectId, ObjectId taskId, ObjectId commentId, string content)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            var comment = task.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
                throw new KeyNotFoundException($"Comment with Id '{commentId}' not found in Task '{taskId}'.");

            comment.Content = content;
            comment.SentTime = DateTime.UtcNow;

            await _projectService.UpdateAsync(projectId, project);
        }

        public async System.Threading.Tasks.Task RemoveCommentAsync(ObjectId projectId, ObjectId taskId, ObjectId commentId)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            task.Comments.RemoveAll(c => c.Id == commentId);

            await _projectService.UpdateAsync(projectId, project);
        }


        // Assignee
        public async System.Threading.Tasks.Task AssignUserAsync(ObjectId projectId, ObjectId taskId, ObjectId userId)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            task.AssigneeId = userId.ToString();

            await _projectService.UpdateAsync(projectId, project);
        }

        public async System.Threading.Tasks.Task RemoveAssigneeAsync(ObjectId projectId, ObjectId taskId)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            task.AssigneeId = string.Empty;

            await _projectService.UpdateAsync(projectId, project);
        }

        // Skills
        public async System.Threading.Tasks.Task AddSkillAsync(ObjectId projectId, ObjectId taskId, string skill)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            if (!task.Skills.Contains(skill))
                task.Skills.Add(skill);

            await _projectService.UpdateAsync(projectId, project);
        }

        public async System.Threading.Tasks.Task RemoveSkillAsync(ObjectId projectId, ObjectId taskId, string skill)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            task.Skills.Remove(skill);

            await _projectService.UpdateAsync(projectId, project);
        }

        public async Task<List<string>> GetSkillsAsync(ObjectId projectId, ObjectId taskId)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            return task.Skills;
        }

        // Status
        public async System.Threading.Tasks.Task UpdateStatusAsync(ObjectId projectId, ObjectId taskId, PerformanceProject.Shared.Models.TaskStatus status)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            task.Status = status;

            await _projectService.UpdateAsync(projectId, project);
        }

        // Difficulty
        public async System.Threading.Tasks.Task UpdateDifficultyAsync(ObjectId projectId, ObjectId taskId, DifficultyLevel difficulty)
        {
            var project = await _projectService.GetAsync(projectId);
            if (project == null)
                throw new KeyNotFoundException($"Project with Id '{projectId}' not found.");

            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with Id '{taskId}' not found in Project '{projectId}'.");

            task.Difficulty = difficulty;

            await _projectService.UpdateAsync(projectId, project);
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
        //        string dataPath = @"C:\Users\Alyona\source\repos\performance-project-be\PerformanceProject.Shared.Models\ML\task_skills_dataset.csv";

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
