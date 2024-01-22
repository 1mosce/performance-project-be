namespace PeopleManagmentSystem_API.Models.Database
{
    public class PeopleManagmentDatabaseSettings : IPeopleManagmentDatabaseSettings
    {
        public string EmployeeCollectionName { get; set; } = string.Empty;
        public string CompanyCollectionName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;

    }
}
