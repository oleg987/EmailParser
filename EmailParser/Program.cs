using EmailParser.DataStorage;
using EmailParser.Services;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddTransient<IEmailReaderService, EmailReaderService>();
builder.Services.AddTransient<IDataStorage, LocalDataStorage>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();