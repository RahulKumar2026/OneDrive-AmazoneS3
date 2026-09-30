using Amazon.S3;
using Microsoft.Extensions.Options;
using OneDriveAmazoneS3.Service;
using OneDriveAmazoneS3.Service.Interface;
using OneDriveAmazoneS3.Settings;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure AmazoneS3Settings from appsettings.json
builder.Services.Configure<AmazoneS3Settings>(builder.Configuration.GetSection("S3Settings"));

// Register AmazonS3 client with dependency injection
builder.Services.AddSingleton<IAmazonS3>(sp => {
    var settings = sp.GetRequiredService<IOptions<AmazoneS3Settings>>().Value;
    return new AmazonS3Client(
        settings.AccessKey,
        settings.SecretKey,
        Amazon.RegionEndpoint.GetBySystemName(settings.RegionName));
});

//Configure OneDriveSettings from appsettings.json
builder.Services.Configure<MicrosoftGraphSettings>(builder.Configuration.GetSection("MicrosoftGraph"));

// Register MicrosoftGraphSettings with dependency injection
builder.Services.AddSingleton<IGraphClientFactory, GraphClientFactory>();

//Dependncy Injection for OneDriveSettings
builder.Services.AddScoped<IAmazoneCrud, AmazoneCrud>();
builder.Services.AddScoped<IOneDriveCrud, OneDriveCrud>();

//Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
