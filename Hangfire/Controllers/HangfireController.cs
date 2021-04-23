using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hangfire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfireController : ControllerBase
    {

        [HttpPost]
        [Route("[action]")]

        public IActionResult Welcome()
        {
            var jobid = BackgroundJob.Enqueue(() => SendwelcomeEmail("Welcome to our app"));

            return Ok($" job ID: {jobid}, Welcome email sent to user");
        }

        [HttpPost]
        [Route("[action]")]

        public IActionResult Discount()
        {
            int timeInSeconds = 30;
            var jobid = BackgroundJob.Schedule(() => SendwelcomeEmail("Welcome to our app"), TimeSpan.FromSeconds(timeInSeconds));

            return Ok($" job ID: {jobid}, A Discount email will be sent to user in {timeInSeconds} seconds!");
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DatabaseUpdate()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Database update"), Cron.Minutely);
            return Ok("Database check job initiated!");

        }

        [HttpPost]
        [Route("[action]")]

        public IActionResult Confirm()
        {
            int timeInseconds = 30;
            var parentjobId = BackgroundJob.Schedule(() => Console.WriteLine("You asked to unsubcribe"), TimeSpan.FromSeconds(timeInseconds));

            BackgroundJob.ContinueJobWith(parentjobId, () => Console.WriteLine("You are unsubscribed"));

            return Ok(" Confirmation Job created!");
        }

        public void SendwelcomeEmail(string text)
        {
            Console.WriteLine(text);  
        }
    }
}
