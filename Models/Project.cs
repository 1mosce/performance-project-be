﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace PeopleManagmentSystem_API.Models
{
    public class Project
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string SerializedId { get => Id.ToString(); }

        public string Name { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        [BsonIgnoreIfNull]
        public Methodology? MainMethodology { get; set; } 

        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }


        [BsonIgnoreIfNull]
        public Team? Team { get; set; }

        public List<Task> Tasks { get; set; } = new();

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectStatus Status { get; set; }
    }
}
