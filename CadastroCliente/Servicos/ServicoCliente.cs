using CadastroCliente.BaseClientes;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CadastroCliente.Servicos
{
    public class ServicoCliente
    {
        private readonly IMongoCollection<Cliente> _clienteColecao;

        public ServicoCliente(IOptions<DadosDeConfiguracao> servicoCliente)
        {
            var clienteMongo = new MongoClient(servicoCliente.Value.ConeccaoString);

            var mongoDBase = clienteMongo.GetDatabase(servicoCliente.Value.NomeBancoDados);

            _clienteColecao = mongoDBase.GetCollection<Cliente>(servicoCliente.Value.ClienteColecaoNome);
        }

        public async Task<List<Cliente>> GetAsync() =>
            await _clienteColecao.Find(x => true).ToListAsync();

        public async Task<Cliente> GetAsync(int id) =>              
            await _clienteColecao.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<bool> CreateAsync(Cliente cliente)
        {
            if (!await validarCliente(cliente))
                return false;    
            await _clienteColecao.InsertOneAsync(cliente);
            return true;
        }   
        public async Task<bool> UpdateAsync(int id, Cliente cliente)
        {
            if (!await validarCliente(cliente))
                return false;
            await _clienteColecao.ReplaceOneAsync(x => x.Id == id, cliente);
            return true;
        }        

        public async Task RemoveAsync(int id) =>
            await _clienteColecao.DeleteOneAsync(x => x.Id == id);

        public async Task<bool> validarCPF(string cpf, int id)
        {
            if (String.IsNullOrEmpty(cpf))
                return false;

            if (cpf.Length != 11)
                return false;

            if (!cpf.All(char.IsDigit))
                return false;

            return !await _clienteColecao.Find(x => (String.Equals(x.Cpf, cpf)) && (x.Id != id || id == 0)).AnyAsync();
        }

        public async Task<bool> validarEmail(string email, int id)
        {
            if (String.IsNullOrEmpty(email))
                return false;
            return !await _clienteColecao.Find(x => String.Equals(x.Email, email) && (x.Id != id || id == 0)).AnyAsync();
        }

        public async Task<bool> validarCliente(Cliente cliente)
        {
            bool CpfValido = await validarCPF(cliente.Cpf, cliente.Id);
            bool EmailValido = await validarEmail(cliente.Email, cliente.Id);
            return (CpfValido && EmailValido);
        }

        public async Task<Cliente> GetPorCpf(string cpf) =>
            await _clienteColecao.Find(x => String.Equals(x.Cpf, cpf)).FirstOrDefaultAsync();
    }
}

