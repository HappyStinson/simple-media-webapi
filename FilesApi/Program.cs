var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Watch for changes to files in our media directory.
builder.Services.AddSingleton<IFileWatcher, FileWatcher>();
builder.Services.AddScoped<IFileStorage, LocalFileStorage>();

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

// do i need this ?
// FileWatcher watcher = new();
// watcher.Start();

app.Run(); // will call FileWatcher.Start() ?
