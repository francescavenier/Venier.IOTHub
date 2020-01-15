using System;
using System.Collections.Generic;
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
        public string message { get; set; }

        [BindProperty]
        public string deviceId { get; set; }

        public string connectionString { get { return _configuration.GetConnectionString("connectionString"); } }

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var BitMessage = new Message(Encoding.ASCII.GetBytes(message));
                ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(connectionString, s_transportType);
                serviceClient.SendAsync(deviceId, BitMessage).ConfigureAwait(false);
                return RedirectToPage("/Confirm"); // da modificare il redirect
            }
            return RedirectToPage("/error404");

    }
    }
}
