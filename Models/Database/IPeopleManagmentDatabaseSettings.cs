namespace PeopleManagmentSystem_API.Models.Database
{
    public interface IPeopleManagmentDatabaseSettings
    {
        string CompanyCollectionName { get; set; }
        string UserCollectionName { get; set; }
        string CompanyUserCollectionName { get; set; }
        string ProjectCollectionName { get; set; }
        string TaskCollectionName { get; set; }
        public string TeamCollectionName { get; set; }
        public string TeamRolesCollectionName { get; set; }
        public string PositionsCollectionName { get; set; }
        public string MethodologiesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
