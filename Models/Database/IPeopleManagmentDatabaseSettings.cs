namespace PeopleManagmentSystem_API.Models.Database
{
    public interface IPeopleManagmentDatabaseSettings
    {
        string CompanyCollectionName { get; set; }
        string UserCollectionName { get; set; }
        string CompanyUserCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
