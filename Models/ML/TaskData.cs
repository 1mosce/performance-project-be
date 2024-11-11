using Microsoft.ML.Data;

namespace PeopleManagmentSystem_API.Models.ML
{
    public class TaskData
    {
        [LoadColumn(0)]
        public string Title { get; set; }
        [LoadColumn(1)]
        public string Skills { get; set; }
    }
    public class TaskPrediction
    {
        [ColumnName("PredictedLabel")] 
        public uint[] SkillsKey { get; set; }
        public string Skill { get; set; }
        public float[] Score { get; set; }  // Ймовірності для кожної навички
    }
}
