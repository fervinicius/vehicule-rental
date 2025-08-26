using Microsoft.EntityFrameworkCore;
using rental_challenge.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// LINHA FALTANTE 1: Adiciona os serviços do Swagger ao container.
builder.Services.AddSwaggerGen();

// Configuração do PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RentalDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // LINHA FALTANTE 2: Habilita o middleware para gerar o JSON do Swagger.
    app.UseSwagger();

    // LINHA FALTANTE 3: Habilita o middleware para servir a UI do Swagger.
    app.UseSwaggerUI();
}

// Manter comentado para simplificar o ambiente Docker
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();