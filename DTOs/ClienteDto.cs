
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace ClienteAPI_.DTOs
{
    public class ClienteDto
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }

        public string Nome { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? DataCadastro { get; set; }

        public ContatoDto Contato { get; set; }

        public EnderecoDto Endereco { get; set; }
    }
}
