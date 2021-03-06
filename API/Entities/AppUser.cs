using API.Extensions;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        // public int Id { get; set; } 

        // public string UserName { get; set; }

        // public byte[] passwordHash { get; set; }

        // public byte[] passwordSalt { get; set; }

        public string knownAs { get; set; }

        public DateTime DateofBirth { get; set; }

        public DateTime LastActive  { get; set; } = DateTime.Now;

        public DateTime Createdon { get; set; } = DateTime.Now;

        public string gender { get; set; }

        public string Interests { get; set; }

        public string Introduction { get; set; }

        public string lookingFor { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public ICollection<Photo> photos { get; set; }

        public ICollection<UserLike> LikedByUsers { get; set; }

        public ICollection<UserLike> LikedUsers { get; set; }

        public ICollection<Message> MessageSent { get; set; }

        public ICollection<Message> MessageRecieved { get; set; }

        public ICollection<AppUserRole> UserRole { get; set; }

        public int GetAge(){

            return DateofBirth.CalculateAge();

               
        }

    }
}