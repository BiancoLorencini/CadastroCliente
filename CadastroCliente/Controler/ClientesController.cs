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
        public async Task<List<cliente>> Get() =>
            await _clienteColecao.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<cliente>> Get(string id)
        {
            var cliente = await _clienteColecao.GetAsync(id);

            if (cliente is null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPost]
        public async Task<IActionResult> Post(cliente cliente)
        {
            await _clienteColecao.CreateAsync(cliente);

            return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, cliente cliente)
        {
            var cliente2 = await _clienteColecao.GetAsync(id);

            if (cliente2 is null)
            {
                return NotFound();
            }

            cliente.Id = cliente2.Id;

            await _clienteColecao.UpdateAsync(id, cliente);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
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
            bool CpfValido = await _clienteColecao.validarCPF(Cpf);
            bool EmailValido = await _clienteColecao.validarEmail(Email);
            if (CpfValido && EmailValido)
                return Accepted("Resultado da busca pelo cliente, positiva!");
            else
                return NoContent();
            
        }    

    }
}
