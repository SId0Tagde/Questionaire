using Form.Data;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<CosmosClient>((sp) =>
{
    var endpointUri = builder.Configuration.GetSection("CosmosDb:EndpointUri").Value;
    var primaryKey = builder.Configuration.GetSection("CosmosDb:Primarykey").Value;
    var cosmosClient = new CosmosClient(endpointUri, primaryKey);
    cosmosClient.CreateDatabaseIfNotExistsAsync("QuestionaireDatabase").Wait();
    cosmosClient.GetDatabase("QuestionaireDatabase").CreateContainerIfNotExistsAsync("QuestionaireTable", "/id").Wait();
    cosmosClient.GetDatabase("QuestionaireDatabase").CreateContainerIfNotExistsAsync("ResponseTable", "/id").Wait();
    return cosmosClient;
});
builder.Services.AddScoped<CosmosDBContext>();
builder.Services.AddAutoMapper(typeof(Program));
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
