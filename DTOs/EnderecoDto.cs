using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClienteAPI_.DTOs
{
    public class EnderecoDto
    {
        [Required(ErrorMessage = "O campo CEP é obrigatório.")]
        public string Cep { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Logradouro { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Cidade { get; set; }

        public string Numero { get; set; }
        public string Complemento { get; set; }
    }
}
