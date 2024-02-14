using System.Net;
using AWS.Playground.API;
using AWS.Playground.API.Config;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AWS Playground API", Version = "v1" });
});

builder.Services
        .AddDynamoDB(builder.Configuration)
        .AddServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AWS Playground API v1");
    });
}

app.UseHttpsRedirection();

app.MapGet("/RealEstate", async (IRealEstateRepository repository, CancellationToken cancellationToken) =>
{
    var realEstate = await repository.GetAllRealtyAsync(cancellationToken);
    return Results.Ok(realEstate);
})
.WithName("GetAllRealEstates")
.WithOpenApi()
.Produces<List<Realty>>(StatusCodes.Status200OK);

app.MapGet("/RealEstate/{id}", async (IRealEstateRepository repository, Guid id, CancellationToken cancellationToken) =>
{
    var realEstate = await repository.GetRealtyAsync(id, cancellationToken);
    if (realEstate == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(realEstate);
})
.WithName("GetRealEstateById")
.WithOpenApi()
.Produces(StatusCodes.Status404NotFound)
.Produces<Realty>(StatusCodes.Status200OK);

app.MapPost("/RealEstate", async (IRealEstateRepository repository, Realty realty, CancellationToken cancellationToken) =>
{
    await repository.InsertRealtyAsync(realty, cancellationToken);
    return Results.Created($"/RealEstate/{realty.Id}", realty);
})
.WithName("AddRealEstate")
.WithOpenApi()
.Produces<Realty>(StatusCodes.Status201Created);

app.MapPut("/RealEstate/{id}", async (IRealEstateRepository repository, Guid id, Realty realty, CancellationToken cancellationToken) =>
{
    var existingRealEstate = await repository.GetRealtyAsync(id, cancellationToken);
    if (existingRealEstate == null)
    {
        return Results.NotFound();
    }
    realty.Id = id;
    await repository.UpdateRealtyAsync(realty, cancellationToken);
    return Results.Ok(realty);
})
.WithName("UpdateRealEstate")
.WithOpenApi()
.Produces(StatusCodes.Status404NotFound)
.Produces<Realty>(StatusCodes.Status200OK);


app.MapDelete("/RealEstate/{id}", async (IRealEstateRepository repository, Guid id, CancellationToken cancellationToken) =>
{
    var existingRealEstate = await repository.GetRealtyAsync(id, cancellationToken);
    if (existingRealEstate == null)
    {
        return Results.NotFound();
    }
    await repository.DeleteRealtyAsync(id, cancellationToken);
    return Results.Accepted();
})
.WithName("DeleteRealEstate")
.WithOpenApi()
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status202Accepted);

app.Run();
