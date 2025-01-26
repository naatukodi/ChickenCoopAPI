using Microsoft.AspNetCore.Mvc;
using ChickenCoop.Services;
using ChickenCoop.Models;

[ApiController]
[Route("api/whatsapp")]
public class WhatsAppWebhookController : ControllerBase
{
    private readonly IAzureCommunicationService _acsService;

    public WhatsAppWebhookController(IAzureCommunicationService acsService)
    {
        _acsService = acsService;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> HandleWhatsAppMessage([FromBody] WhatsAppMessage message)
    {
        if (message == null || string.IsNullOrEmpty(message.Body))
            return BadRequest("Invalid message received.");

        // Respond with menu options
        string responseText = @"👋 Welcome to Naatukodi by GSR!  
        Please select an option by replying with a number:

        1️⃣ Register as a Farmer (రెజిస్ట్రేషన్)  
        2️⃣ Order Chicks (కోడి పిల్లల ఆర్డర్)  
        3️⃣ Request Vet Support (పశువైద్య సహాయం)  
        4️⃣ Sell Chickens (కోడి అమ్మకం)  
        5️⃣ Check Market Prices (మార్కెట్ ధరలు)";

        if (string.IsNullOrEmpty(message.From))
            return BadRequest("Invalid sender phone number.");

        await _acsService.SendWhatsAppMessage(message.From, responseText);
        return Ok();
    }
}

