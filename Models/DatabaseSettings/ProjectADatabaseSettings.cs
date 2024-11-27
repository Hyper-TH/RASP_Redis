namespace RASP_Redis.Models.DatabaseSettings
{
    public class ProjectADatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string MeetingsCollectionName { get; set; } = null!;
        public string AttendeesCollectionName { get; set; } = null!;
    }
}
