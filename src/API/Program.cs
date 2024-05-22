using Application.Common.CacheCommon;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region [ Options ]

builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));

#endregion [ Options ]

#region [ MediatR ]

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

#endregion [ MediatR ]

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));


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
