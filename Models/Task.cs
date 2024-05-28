using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace PeopleManagmentSystem_API.Models
{
    [CollectionName("tasks")]
    public class Task
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string SerializedId { get => Id.ToString(); }

        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssigneeId { get; set; } = String.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProjectId { get; set; } = String.Empty;

        [BsonIgnoreIfNull]
        public string Status { get; set; } = String.Empty;
        [BsonIgnoreIfNull]
        public string Difficulty { get; set; } = String.Empty;

        public DateTime DueDate { get; set; }
        public DateTime Accepted { get; set; }
        public DateTime Finished { get; set; }

        public TimeSpan TimeSpent { get => Finished.Subtract(Accepted); }

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

        //[BsonRepresentation(BsonType.ObjectId)]
        //[BsonIgnoreIfNull]
        //public string StatusId { get; set; } = String.Empty;
        //[BsonRepresentation(BsonType.ObjectId)]
        //[BsonIgnoreIfNull]
        //public string PriorityId { get; set; } = String.Empty;
        //public double Rating { get; set; }
    }
}
