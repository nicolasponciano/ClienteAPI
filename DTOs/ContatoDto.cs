using System.Text.Json.Serialization;

namespace ClienteAPI_.DTOs
{
    public class ContatoDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }

        public string Tipo { get; set; }
        public string Texto { get; set; }
    }
}
