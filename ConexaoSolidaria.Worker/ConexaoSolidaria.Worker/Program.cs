using ConexaoSolidaria.Worker;
using ConexaoSolidaria.Worker.Configurations;
using ConexaoSolidaria.Worker.Consumers;
using ConexaoSolidaria.Worker.Data;
using ConexaoSolidaria.Worker.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"));
});

builder.Services.AddScoped<IDoacaoProcessor, DoacaoProcessor>();

var rabbitMqSettings = builder.Configuration
    .GetSection("RabbitMq")
    .Get<RabbitMqSettings>();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<DoacaoCriadaConsumer>();

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings!.Host, "/", host =>
        {
            host.Username(rabbitMqSettings.Username);
            host.Password(rabbitMqSettings.Password);
        });

        cfg.ReceiveEndpoint("doacao-criada-queue", endpoint =>
        {
            endpoint.UseMessageRetry(r =>
            {
                r.Interval(3, TimeSpan.FromSeconds(5));
            });

            endpoint.ConfigureConsumer<DoacaoCriadaConsumer>(context);
        });
    });
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();