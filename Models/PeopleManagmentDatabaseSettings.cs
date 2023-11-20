namespace PeopleManagmentSystem_API.Models
{
    public class PeopleManagmentDatabaseSettings : IPeopleManagmentDatabaseSettings
    {
        public string CompanyCollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}
