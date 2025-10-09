using PencilLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PencilRepository>();
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
//app.UseCors("allowGetPut");
// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
}*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
