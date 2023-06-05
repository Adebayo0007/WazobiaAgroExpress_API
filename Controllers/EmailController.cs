using AgroExpressAPI.Email;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
            
        }
    
        [HttpPost("CreateEmail")]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> CreateEmail([FromForm]EmailRequestModel emailRequestModel)
        {
             if(!ModelState.IsValid)
            {
                string response1 = "Invalid input,check your input very well";
                return BadRequest(new{mesage = response1});
            }
            var result = await _emailSender.SendEmail(emailRequestModel);
            string response;
            if(result == true)
            {
                response = "Email sent!";
               return Ok(new{mesage = response});
            }
               response = "Email not sent!";
               return BadRequest(new{mesage = response});
        }
    }
