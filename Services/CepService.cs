using Microsoft.EntityFrameworkCore;
using ClienteAPI_.Context;
using ClienteAPI_.DTOs;
using ClienteAPI_.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClienteAPI_.Services
{
    public class CepService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public CepService(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<ViaCepDto> ConsultarCep(string cep)
        {
           

            var viaCepExistente = await _context.T_VIACEP.FirstOrDefaultAsync(v => v.Cep == cep);

            if (viaCepExistente != null)
            {
                return new ViaCepDto
                {
                    Cep = viaCepExistente.Cep,
                    Logradouro = viaCepExistente.Logradouro,
                    Complemento = viaCepExistente.Complemento,
                    Bairro = viaCepExistente.Bairro,
                    Localidade = viaCepExistente.Localidade,
                    Uf = viaCepExistente.Uf,
                    Ibge = viaCepExistente.Ibge,
                    Gia = viaCepExistente.Gia,
                    Ddd = viaCepExistente.Ddd,
                    Siafi = viaCepExistente.Siafi
                };
            }

            var response = await _httpClient.GetFromJsonAsync<ViaCepDto>($"https://viacep.com.br/ws/{cep}/json/");

            if (response == null || string.IsNullOrEmpty(response.Cep))
            {
                throw new Exception("CEP não encontrado na API do ViaCEP.");
            }

            var novoViaCep = new ViaCep
            {
                Cep = response.Cep,
                Logradouro = response.Logradouro,
                Complemento = response.Complemento,
                Bairro = response.Bairro,
                Localidade = response.Localidade,
                Uf = response.Uf,
                Ibge = response.Ibge,
                Gia = response.Gia,
                Ddd = response.Ddd,
                Siafi = response.Siafi
            };

            _context.T_VIACEP.Add(novoViaCep);
            await _context.SaveChangesAsync();

            return response;
        }
    }
}