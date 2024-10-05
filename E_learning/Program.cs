using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var uploadsFolderPath = Path.Combine(builder.Environment.ContentRootPath, "root/uploadfiles");
if (!Directory.Exists(uploadsFolderPath))
    Directory.CreateDirectory(uploadsFolderPath);

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(uploadsFolderPath));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsFolderPath),
    RequestPath = "/uploads"
});


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
