using System.ComponentModel.DataAnnotations;

namespace Propins.Models
{
    public class LoginModel
    {
        public int id { get; set; }
        [Required] public string username { get; set; }
        [Required] public string password { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }

        public string token { get; set; }

        public LoginModel(int id, string username, string password, string name, string lastname, string token)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.name = name;
            this.lastName = lastName;
            this.token = token;
        }

        public LoginModel()
        {
        }
    }
}