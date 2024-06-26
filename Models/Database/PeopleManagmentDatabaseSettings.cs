﻿namespace PeopleManagmentSystem_API.Models.Database
{
    public class PeopleManagmentDatabaseSettings : IPeopleManagmentDatabaseSettings
    {
        public string UserCollectionName { get; set; } = string.Empty;
        public string CompanyCollectionName { get; set; } = string.Empty;
        public string CompanyUserCollectionName { get; set; } = string.Empty;
        public string ProjectCollectionName { get; set; } = string.Empty;
        public string TaskCollectionName { get; set; } = string.Empty;
        public string TeamCollectionName { get; set; } = string.Empty;
        public string TeamUserCollectionName { get; set; } = string.Empty;
        public string CommentCollectionName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;

    }
}
