namespace ClienteAPI_.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string DataCadastro { get; private set; } 

        public Contato Contato { get; set; }

        public Endereco Endereco { get; set; }

        public Cliente()
        {
            DataCadastro = DateTime.Today.ToString("yyyy-MM-dd"); 
        }
    }
}
