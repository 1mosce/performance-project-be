namespace PeopleManagmentSystem_API.Models.Database
{
    public interface IPeopleManagmentDatabaseSettings
    {
        string CompanyCollectionName { get; set; }
        string EmployeeCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
