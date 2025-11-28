using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using PencilLibrary;
using RESTPencil;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
const bool useDB = true;
IPencilRepository _repo;
if (useDB)
{
	var optionsBuilder = new DbContextOptionsBuilder<MannazRestAppsDbContext>();
	optionsBuilder.UseSqlServer(SecretDB.ConnectionStringSimply);
	MannazRestAppsDbContext _dbContext = new(optionsBuilder.Options);
	_dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE dbo.Pencils");
	_repo = new PencilRepositoryDB(_dbContext);
}
else
{
	_repo = new PencilRepository();
}

builder.Services.AddSingleton<IPencilRepository>(_repo);
builder.Services.AddCors(options =>
{
	options.AddPolicy("allowAnythingFromZealand",
		builder =>
			builder.WithOrigins("http://zealand.dk")
				.AllowAnyHeader()
				.AllowAnyMethod());
	options.AddPolicy("allowGetPut",
		builder =>
			builder.AllowAnyOrigin()
			.WithMethods("GET", "PUT")
			.AllowAnyHeader());
	options.AddPolicy("allowAnything", // similar to * in Azure
		builder =>
			builder.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());
});

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("allowAnything");

//app.UseCors("allowGetPut");
// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
}*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
