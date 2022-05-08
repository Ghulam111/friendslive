namespace API.DTOs
{
    public class MemberDTO
    {
        public int Id { get; set; } 

        public string UserName { get; set; }

        public string PhotoUrl { get; set; }
        public string knownAs { get; set; }

        public int Age { get; set; }

        public DateTime LastActive  { get; set; }
        public DateTime Createdon { get; set; } 

        public string gender { get; set; }

        public string Interests { get; set; }

        public string Introduction { get; set; }

        public string lookingFor { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<PhotoDTO> photos { get; set; }
    }
}