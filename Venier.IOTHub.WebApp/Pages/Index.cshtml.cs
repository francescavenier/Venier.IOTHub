using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Venier.IOTHub.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        //private readonly ServiceClient _serviceClient;
        private static TransportType s_transportType = TransportType.Amqp;

        [BindProperty]
        public InputModel input { get; set; }

        public string[] Devices = new string[] { "device1", "device2" };

        public class InputModel
        {
            [Required]
            public string message { get; set; }
            [Required]
            public string deviceId { get; set; }

        }
        

        public string connectionString { get { return _configuration.GetConnectionString("connectionString"); } }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var ConvertedMessage = new Message(Encoding.ASCII.GetBytes(input.message));
                ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(connectionString, s_transportType);
                await serviceClient.SendAsync(input.deviceId, ConvertedMessage).ConfigureAwait(false);
                return RedirectToPage("/Confirm");
            }
            return RedirectToPage("/error404");

    }
    }
}
