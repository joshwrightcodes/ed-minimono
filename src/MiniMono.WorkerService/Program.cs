using MiniMono.WorkerService;
using Wright.Demo.MiniMono.Application;
using Wright.Demo.MiniMono.Infrastructure;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((host, services) =>
	{
		services.AddApplicationLayer();
		services.AddInfrastructureLayer(host.Configuration);
		services.AddHostedService<Worker>();
	})
	.Build();

await host.RunAsync();