using Newtonsoft.Json;

namespace Propins.Models
{
    public class UsuarioModel
    {
        [JsonProperty("id")]
        public string id{get;set;}
        [JsonProperty("nombre")]
        public string nombre{get;set;}
        [JsonProperty("apellido")]
        public string apellido{get;set;}
        [JsonProperty("profesion")]
        public string profesion{get;set;}
        [JsonProperty("email")]
        public string email{get;set;}

        
        public UsuarioModel(string id,string nombre,string apellido,string profesion,string email)
        {
            this.id = id;
            this.nombre = nombre;
            this.apellido = apellido;
            this.profesion = profesion;
            this.email = email;
        }

        public UsuarioModel()
        {
        }
    }
}