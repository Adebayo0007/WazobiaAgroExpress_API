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
            var result = await _emailSender.SendEmail(emailRequestModel);
            string response;
            if(result == true)
            {
                response = "Email sent!";
               return Ok(response);
            }
               response = "Email not sent!";
               return BadRequest(response);
        }
    }
