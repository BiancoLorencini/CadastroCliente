using CadastroCliente.BaseClientes;
using CadastroCliente.Servicos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CadastroCliente.Controler
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ServicoCliente _clienteColecao;

        public ClientesController(ServicoCliente servicoCliente) =>
            _clienteColecao = servicoCliente;

        [HttpGet]
        public async Task<List<Cliente>> Get() =>
            await _clienteColecao.GetAsync();

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            var cliente = await _clienteColecao.GetAsync(id);

            if (cliente is null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Cliente cliente)
        {
            if (await _clienteColecao.CreateAsync(cliente))
                return Ok();
            else
                return NoContent();            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Cliente cliente)
        {
            var cliente2 = await _clienteColecao.GetAsync(id);

            if (cliente2 is null)
            {
                return NotFound();
            }

            cliente.Id = cliente2.Id;            

            if (await _clienteColecao.UpdateAsync(id, cliente))
                return Ok();
            else
                return NoContent();
            
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteColecao.GetAsync(id);

            if (cliente is null)
            {
                return NotFound();
            }

            await _clienteColecao.RemoveAsync(id);

            return NoContent();
        }

        [HttpGet("validar")]
        public async Task<IActionResult> validar(string Cpf,string Email)
        {
            bool CpfValido = await _clienteColecao.validarCPF(Cpf, 0);
            bool EmailValido = await _clienteColecao.validarEmail(Email, 0);
            if (CpfValido && EmailValido)
                return Accepted();
            else
                return NoContent(); 
        }

        [HttpGet("Cpf")]
        public async Task<ActionResult<Cliente>> Get(string cpf)
        {
            var cliente = await _clienteColecao.GetPorCpf(cpf);

            if (cliente is null)
            {
                return NotFound();
            }

            return cliente;
        }
    }
}
