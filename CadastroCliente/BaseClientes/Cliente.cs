using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace CadastroCliente.BaseClientes
{
    public class Cliente
    {
        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]

        [BsonElement("Id")]
        public int Id {  get; set; }

        [BsonElement("Nome")]
        public string Nome { get; set; } = null;

        [BsonElement("CPF")]
        public string Cpf { get; set; } = null;

        [BsonElement("Email")]
        public string Email { get; set; } = null;

        [BsonElement("Endereco")]
        public string Endereco { get; set; } = null;

        [BsonElement("TelContato")]
        public string TelCel { get; set; } = null;

        [BsonElement("Status")]
        public bool Status { get; set; }
    }
}
