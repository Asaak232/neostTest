using BaseSpace.Data;
using BaseSpace.Models;
using Microsoft.EntityFrameworkCore;
using CRUD.actions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//update connection string in appsettings on the first launch

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<DataContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<CRUDClass>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

WebApplication app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapPost("/api/v1/persons", async (Person request, CRUDClass service) => await service.CreatePerson(request));

app.MapGet("/api/v1/persons", async (CRUDClass service) => await service.GetEveryone());

app.MapGet("/api/v1/persons/{id}", async (long id, CRUDClass service) =>
{
    Person result = await service.GetPersonById(id);

    if (result != null)
        return Results.Ok(result);
    else
        return Results.NotFound();
});

app.MapPut("/api/v1/persons/{id}", async (long id, Person request, CRUDClass service) => await service.UpdatePerson(id,request));

app.MapDelete("/api/v1/persons/{id}", async (long id, CRUDClass service) => await service.DeletePerson(id));

app.Run();



