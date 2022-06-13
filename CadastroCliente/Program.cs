using CadastroCliente.BaseClientes;
using CadastroCliente.Servicos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DadosDeConfiguracao>
    (builder.Configuration.GetSection("ClientStoreDatabase"));
builder.Services.AddSingleton<ServicoCliente>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

