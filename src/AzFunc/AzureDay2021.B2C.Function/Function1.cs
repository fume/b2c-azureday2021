using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureDay2021.B2C.Function
{
    public static class Function1
    {
        [FunctionName("Login")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string email = data.email;
            string password = data.password;

            if(password == Environment.GetEnvironmentVariable("blockingPassword"))
			{
                return new ConflictObjectResult(new ResponseContent() { userMessage = $"From API: Invalid Credentials for user {email}" });
			}

            return new OkObjectResult(new { loginSuccess = true });

        }
    }

    public class ResponseContent
    {
        public string version { get; set; } = "1.0.0";
        public int status { get; set; } = 409;
        public string code { get; set; }
        public string userMessage { get; set; }
        public string developerMessage { get; set; }
        public string requestId { get; set; }
        public string moreInfo { get; set; }
    }
}
