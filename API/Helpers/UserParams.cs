namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
       

        public string currentUsername { get; set; }

        public string gender { get; set; }

        public int minAge { get; set; } = 18;

        public int maxAge { get; set; } = 50;

        public string OrderBy { get; set; } = "lastActive";

        
    }
}