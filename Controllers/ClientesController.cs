using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClienteAPI_.Context;
using ClienteAPI_.DTOs;
using ClienteAPI_.Models;
using ClienteAPI_.Services;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly CepService _cepService;

    public ClientesController(AppDbContext context, IMapper mapper, CepService cepService)
    {
        _context = context;
        _mapper = mapper;
        _cepService = cepService;
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> CadastrarCliente(ClienteDto clienteDto)
    {
        try
        {
            if (string.IsNullOrEmpty(clienteDto.Endereco.Cep) || string.IsNullOrEmpty(clienteDto.Endereco.Numero))
            {
                return BadRequest(new { message = "CEP e Número são obrigatórios." });
            }

            if (!IsValidCep(clienteDto.Endereco.Cep))
            {
                return BadRequest(new { message = "O CEP informado é inválido. O formato numérico correto é XXXXX-XXX." });
            }



            var clienteExistentePorContato = _context.Clientes.FirstOrDefault(c => c.Contato.Texto == clienteDto.Contato.Texto);
            if (clienteExistentePorContato != null)
            {
                return Conflict(new { message = "Já existe um cliente cadastrado com este contato." });
            }

            var enderecoViaCep = await _cepService.ConsultarCep(clienteDto.Endereco.Cep);

            clienteDto.Endereco.Logradouro = enderecoViaCep.Logradouro;
            clienteDto.Endereco.Cidade = enderecoViaCep.Localidade;

            var cliente = _mapper.Map<Cliente>(clienteDto);


            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ConsultarClientePorId), new { id = cliente.Id }, cliente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao cadastrar cliente.", details = ex.Message });
        }
    }

    private bool IsValidCep(string cep)
    {
        return !string.IsNullOrEmpty(cep) && System.Text.RegularExpressions.Regex.IsMatch(cep, @"^\d{5}-\d{3}$");
    }

    [HttpGet("dados-cep/{cep}")]
    public async Task<IActionResult> ConsultarCep(string cep)
    {
        try
        {
            if (string.IsNullOrEmpty(cep))
            {
                return BadRequest(new { message = "O campo CEP é obrigatório." });
            }

            if (!IsValidCep(cep))
            {
                return BadRequest(new { message = "O CEP informado é inválido. O formato correto é XXXXX-XXX." });
            }

            var viaCepExistente = await _context.T_VIACEP.FirstOrDefaultAsync(v => v.Cep == cep);
            bool veioDaApi = viaCepExistente == null;

            var enderecoViaCep = await _cepService.ConsultarCep(cep);

            if (enderecoViaCep == null || string.IsNullOrEmpty(enderecoViaCep.Cep))
            {
                return NotFound(new { message = "CEP não encontrado na API do ViaCEP." });
            }

            string mensagemEspecial = "";
            if (cep == "08780-060")
            {
                mensagemEspecial = " Este CEP é onde está sediada à Muralis, uma empresa de tecnologia e inovação. Visite: https://www.muralis.com.br/";
            }

            return Ok(new
            {
                message = (veioDaApi
                    ? "CEP não encontrado no banco de dados. Dados inseridos a partir da API do ViaCEP."
                    : "CEP encontrado no banco de dados.") + mensagemEspecial,
                cep = enderecoViaCep.Cep,
                logradouro = enderecoViaCep.Logradouro,
                complemento = enderecoViaCep.Complemento,
                bairro = enderecoViaCep.Bairro,
                localidade = enderecoViaCep.Localidade,
                uf = enderecoViaCep.Uf,
                ibge = enderecoViaCep.Ibge,
                gia = enderecoViaCep.Gia,
                ddd = enderecoViaCep.Ddd,
                siafi = enderecoViaCep.Siafi
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao consultar CEP.", details = ex.Message });
        }
    }

    [HttpGet("pesquisa-cliente-cep")]
    public IActionResult PesquisarClientesPorCep([FromQuery] string cep)
    {
        if (string.IsNullOrEmpty(cep))
        {
            return BadRequest(new { message = "O campo CEP é obrigatório para a pesquisa." });
        }

        if (!IsValidCep(cep))
        {
            return BadRequest(new { message = "O CEP informado é inválido. O formato correto é XXXXX-XXX." });
        }

        var clientes = _context.Clientes
            .Include(c => c.Contato)
            .Include(c => c.Endereco)
            .Where(c => c.Endereco.Cep == cep)
            .ToList();

        if (!clientes.Any())
        {
            return NotFound(new { message = "Nenhum cliente encontrado com o CEP informado." });
        }

        return Ok(_mapper.Map<List<ClienteDto>>(clientes));
    }

    [HttpGet("listar-todos")]
    public IActionResult ListarClientes()
    {
        var clientes = _context.Clientes
            .Include(c => c.Contato)
            .Include(c => c.Endereco)
            .ToList();

        return Ok(_mapper.Map<List<ClienteDto>>(clientes));
    }

    [HttpGet("consulta-cliente-id/{id}")]
    public IActionResult ConsultarClientePorId(int id)
    {
        var cliente = _context.Clientes
            .Include(c => c.Contato)
            .Include(c => c.Endereco)
            .FirstOrDefault(c => c.Id == id);

        if (cliente == null)
        {
            return NotFound(new
            {
                StatusCode = 404,
                Message = "Cliente não encontrado",
                Details = $"Nenhum cliente com o ID {id} foi localizado em nossa base de dados",
                RequestedId = id,
                Timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")
            });
        }

        return Ok(_mapper.Map<ClienteDto>(cliente));
    }

    [HttpPut("atualizar/{id}")]
    public async Task<IActionResult> AtualizarCliente(int id, ClienteDto clienteDto)
    {
        try
        {
            var cliente = await _context.Clientes
                .Include(c => c.Contato)
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado." });
            }

            if (string.IsNullOrEmpty(clienteDto.Endereco.Cep))
            {
                return BadRequest(new { message = "O campo CEP é obrigatório." });
            }

            if (!IsValidCep(clienteDto.Endereco.Cep))
            {
                return BadRequest(new { message = "O CEP informado é inválido. O formato correto é XXXXX-XXX." });
            }

            var enderecoViaCep = await _cepService.ConsultarCep(clienteDto.Endereco.Cep);

            var camposAlterados = new List<string>();

            if (cliente.Nome != clienteDto.Nome)
            {
                camposAlterados.Add($"Nome alterado de '{cliente.Nome}' para '{clienteDto.Nome}'");
                cliente.Nome = clienteDto.Nome;
            }

            if (cliente.Contato.Tipo != clienteDto.Contato.Tipo)
            {
                camposAlterados.Add($"Tipo de contato alterado de '{cliente.Contato.Tipo}' para '{clienteDto.Contato.Tipo}'");
                cliente.Contato.Tipo = clienteDto.Contato.Tipo;
            }

            if (cliente.Contato.Texto != clienteDto.Contato.Texto)
            {
                camposAlterados.Add($"Texto de contato alterado de '{cliente.Contato.Texto}' para '{clienteDto.Contato.Texto}'");
                cliente.Contato.Texto = clienteDto.Contato.Texto;
            }

            if (cliente.Endereco.Cep != clienteDto.Endereco.Cep)
            {
                camposAlterados.Add($"CEP alterado de '{cliente.Endereco.Cep}' para '{clienteDto.Endereco.Cep}'");
                cliente.Endereco.Cep = enderecoViaCep.Cep;
            }

            if (cliente.Endereco.Logradouro != enderecoViaCep.Logradouro)
            {
                camposAlterados.Add($"Logradouro alterado de '{cliente.Endereco.Logradouro}' para '{enderecoViaCep.Logradouro}'");
                cliente.Endereco.Logradouro = enderecoViaCep.Logradouro;
            }

            if (cliente.Endereco.Cidade != enderecoViaCep.Localidade)
            {
                camposAlterados.Add($"Cidade alterada de '{cliente.Endereco.Cidade}' para '{enderecoViaCep.Localidade}'");
                cliente.Endereco.Cidade = enderecoViaCep.Localidade;
            }

            if (cliente.Endereco.Numero != clienteDto.Endereco.Numero)
            {
                camposAlterados.Add($"Número alterado de '{cliente.Endereco.Numero}' para '{clienteDto.Endereco.Numero}'");
                cliente.Endereco.Numero = clienteDto.Endereco.Numero;
            }

            if (cliente.Endereco.Complemento != clienteDto.Endereco.Complemento)
            {
                camposAlterados.Add($"Complemento alterado de '{cliente.Endereco.Complemento}' para '{clienteDto.Endereco.Complemento}'");
                cliente.Endereco.Complemento = clienteDto.Endereco.Complemento;
            }

            await _context.SaveChangesAsync();

            var mensagemSucesso = camposAlterados.Any()
                ? $"Cliente com ID {id} foi atualizado com sucesso. Alterações: {string.Join("; ", camposAlterados)}"
                : $"Cliente com ID {id} não teve alterações nos dados.";

            return Ok(new { message = mensagemSucesso });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao atualizar cliente.", details = ex.Message });
        }
    }

    [HttpDelete("excluir/{id}")]
    public async Task<IActionResult> ExcluirCliente(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Contato)
            .Include(c => c.Endereco)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
        {
            return NotFound(new { message = "Cliente não encontrado." });
        }

        var clienteExcluido = new
        {
            id = cliente.Id,
            nome = cliente.Nome,
            dataCadastro = cliente.DataCadastro,
            contato = new
            {
                tipo = cliente.Contato.Tipo,
                texto = cliente.Contato.Texto
            },
            endereco = new
            {
                cep = cliente.Endereco.Cep,
                logradouro = cliente.Endereco.Logradouro,
                cidade = cliente.Endereco.Cidade,
                numero = cliente.Endereco.Numero,
                complemento = cliente.Endereco.Complemento
            }
        };

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Cliente excluído com sucesso!",
            clienteExcluido
        });
    }
}