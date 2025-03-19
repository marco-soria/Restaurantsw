var builder = DistributedApplication.CreateBuilder(args);

// Agregar la API como un servicio en Aspire
var api = builder.AddProject<Projects.Restaurants_API>("Restaurants-api");

builder.Build().Run();
