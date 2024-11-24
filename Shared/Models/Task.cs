using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PerformanceProject.Shared.Models
{
    [CollectionName("tasks")]
    public class Task
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId Id { get; set; }
        [BsonIgnore]
        public string SerializedId { get => Id.ToString(); }

        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string AssigneeId { get; set; } = String.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskStatus Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DifficultyLevel Difficulty { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime? Accepted { get; set; }
        public DateTime? Finished { get; set; }
        public TimeSpan? PlannedTime
        {
            get
            {
                if (Accepted != null)
                {
                    return DueDate - Accepted.Value;
                }
                else
                {
                    return null;
                }
            }
        }
        public TimeSpan? TimeSpent
        {
            get
            {
                if (Finished != null && Accepted != null)
                {
                    return Finished.Value.Subtract(Accepted.Value);
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Comment> Comments { get; set; } = new();

        public List<string> Skills { get; set; } = new();

        [Range(1, 5)]
        public double? Rating { get; set; }

        [Range(0, 1)]
        public double? Engagement { get; set; }

        /*
        // Method to convert difficulty level to numeric value
        private int GetDifficultyLevel()
        {
            return Difficulty.ToLower() switch
            {
                "low" => 1,
                "medium" => 2,
                "high" => 3,
                _ => 1
            };
        }

        // Method to calculate time factor
        private double CalculateTimeFactor()
        {
            double dueTimeHours = (DueDate - Accepted).TotalHours;

            if (Finished <= DueDate)
            {
                // Task finished before or on due date
                double remainingTimeHours = (DueDate - Finished).TotalHours;
                return 1 + (remainingTimeHours / dueTimeHours);
            }
            else
            {
                // Task finished after due date
                double overdueTimeHours = (Finished - DueDate).TotalHours;
                return 1 - (overdueTimeHours / dueTimeHours);
            }
        }
        // Method to calculate productivity
        public double CalculateProductivity()
        {
            if (Status.ToLower() != "completed")
            {
                return 0;
            }

            int difficultyLevel = GetDifficultyLevel();
            double timeSpentHours = TimeSpent.TotalHours;

            // Ensure minimum time threshold for productivity calculation
            const double minTimeThreshold = 1.0; // 1 hour
            if (timeSpentHours < minTimeThreshold)
            {
                timeSpentHours = minTimeThreshold;
            }

            double timeFactor = CalculateTimeFactor();
            return (difficultyLevel / timeSpentHours) * timeFactor;
        }
        */
    }
}
