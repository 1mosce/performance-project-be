﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PeopleManagmentSystem_API.Models
{
    public class Methodology
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; } = String.Empty;
       // public List<string> Phases { get; set; } = new List<string>();
    }
}
