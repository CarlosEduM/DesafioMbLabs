using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketsManager.Models
{
    public class UserBase
    {
        [Key]
        [Column("UserId")]
        public int Id { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Creates a void UserBase
        /// </summary>
        public UserBase()
        {
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user password</param>
        public UserBase(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public UserBase(int id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
    }
}
