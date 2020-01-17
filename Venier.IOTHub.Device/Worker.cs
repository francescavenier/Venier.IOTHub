using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private static TransportType s_transportType = TransportType.Http1;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string deviceConnectionString = _configuration.GetConnectionString("connectionString");
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, s_transportType);


            while (!stoppingToken.IsCancellationRequested)
            {
                var receivedMessage = await deviceClient.ReceiveAsync().ConfigureAwait(false);

                if (receivedMessage != null)
                {
                    var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    Console.WriteLine("\t{0}> Received message: {1}", DateTime.Now.ToLocalTime(), messageData);

                }
                else 
                { 
                    await Task.Delay(2000, stoppingToken);
                }
            }
        }
    }
}
