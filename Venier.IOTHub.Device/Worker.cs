using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Venier.IOTHub.Device
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        
        private static TransportType s_transportType = TransportType.Amqp;
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string deviceConnectionString = _configuration.GetConnectionString("DeviceConnectionString");
            while (!stoppingToken.IsCancellationRequested)
            {

                if (string.IsNullOrEmpty(deviceConnectionString) && args.Length > 0)
                {
                    deviceConnectionString = args[0];
                }

                DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, s_transportType);

                if (deviceClient == null)
                {
                    Console.WriteLine("Failed to create DeviceClient!");
                }

                var sample = new MessageSample(deviceClient);
                sample.RunSampleAsync().GetAwaiter().GetResult();

                Console.WriteLine("Done.\n");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
