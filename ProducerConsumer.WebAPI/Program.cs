using Microsoft.EntityFrameworkCore;
using ProducerConsumer.BusinessServices.Implementation;
using ProducerConsumer.BusinessServices.Interface;
using ProducerConsumer.Data.Entities;
using ProducerConsumer.Data.Implementation;
using ProducerConsumer.Data.Interface;
using ProducerConsumer.Data.IRepository;
using ProducerConsumer.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration["ConnectionString"];

builder.Services.AddDbContext<ProducerConsumerDbContext>(options => { options.UseSqlServer(connectionString); });

builder.Services.AddScoped<IProducerService, ProducerService>();
builder.Services.AddScoped<IConsumerService, ConsumerService>();
builder.Services.AddScoped<IMessageQueue, MessageQueueWrapper>();
builder.Services.AddScoped<IProducerConsumerDAL, ProducerConsumerDAL>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ProducerConsumerDbContext>();

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
