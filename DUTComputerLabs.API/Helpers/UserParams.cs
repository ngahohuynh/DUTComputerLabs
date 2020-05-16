namespace DUTComputerLabs.API.Helpers
{
    public class UserParams : PaginationParams
    {
        public string Name { get; set; }

        public string RoleName { get; set; }
    }
}