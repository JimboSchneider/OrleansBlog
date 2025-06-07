using System.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Microsoft.Data.Sqlite;

try
{
	using IHost host = await StartSiloAsync();
	
	// Don't wait for input when running in background
	if (bool.TryParse(Environment.GetEnvironmentVariable("ORLEANS_NO_WAIT"), out var noWait) && noWait)
	{
		await host.WaitForShutdownAsync();
	}
	else
	{
		Console.WriteLine("\n\n Press Enter to terminate...\n\n");
		Console.ReadLine();
		await host.StopAsync();
	}

	return 0;
}
catch (Exception ex)
{
	Console.WriteLine(ex);
	return 1;
}

static async Task<IHost> StartSiloAsync()
{
	var builder = Host
		.CreateDefaultBuilder()
		.UseOrleans((context, silo) =>
		{
			silo
				.UseLocalhostClustering()
				.Configure<EndpointOptions>(options => 
				{
					options.AdvertisedIPAddress = IPAddress.Loopback;
					options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Loopback, 30000);
					options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Loopback, 11111);
				})
				.ConfigureLogging(logging => logging.AddConsole());
		});

	var host = builder.Build();
	await host.StartAsync();

	return host;
}

