using CadastroCliente.BaseClientes;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CadastroCliente.Servicos
{
    public class ServicoCliente
    {
        private readonly IMongoCollection<cliente> _clienteColecao;

        public ServicoCliente(IOptions<DadosDeConfiguracao> servicoCliente)
        {
            var clienteMongo = new MongoClient(servicoCliente.Value.ConeccaoString);

            var mongoDBase = clienteMongo.GetDatabase(servicoCliente.Value.NomeBancoDados);

            _clienteColecao = mongoDBase.GetCollection<cliente>(servicoCliente.Value.ClienteColecaoNome);
        }

        public async Task<List<cliente>> GetAsync() =>
            await _clienteColecao.Find(x => true).ToListAsync();

        public async Task<cliente> GetAsync(string id) =>
           await _clienteColecao.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(cliente cliente) =>
            await _clienteColecao.InsertOneAsync(cliente);

        public async Task UpdateAsync(string id, cliente cliente) =>
            await _clienteColecao.ReplaceOneAsync(x => x.Id == id, cliente);

        public async Task RemoveAsync(string id) =>
            await _clienteColecao.DeleteOneAsync(x => x.Id == id);

        public async Task<bool> validarCPF(string Cpf)
        {
            if (String.IsNullOrEmpty(Cpf))
            {
                return false;
            }
            return !await _clienteColecao.Find(x => String.Equals(x.Cpf, Cpf)).AnyAsync();
        }

        public async Task<bool> validarEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                return false;
            return !await _clienteColecao.Find(x => String.Equals(x.Email, email)).AnyAsync();
        }                     
    }
}

