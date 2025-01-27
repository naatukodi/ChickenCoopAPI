using Microsoft.AspNetCore.Mvc;
using ChickenCoop.Services;
using ChickenCoop.Models;
using System.Text.Json;

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
    public async Task<IActionResult> HandleWhatsAppMessage([FromBody] JsonElement payload)
    {
        // Check if the request is a SubscriptionValidationEvent
        if (payload.TryGetProperty("eventType", out var eventTypeProp) &&
            eventTypeProp.GetString() == "Microsoft.EventGrid.SubscriptionValidationEvent")
        {
            var validationCode = payload.GetProperty("data").GetProperty("validationCode").GetString();
            return Ok(new { validationResponse = validationCode });
        }

        // Deserialize to WhatsAppMessage
        WhatsAppMessage message;
        try
        {
            message = JsonSerializer.Deserialize<WhatsAppMessage>(payload.GetRawText());
        }
        catch
        {
            return BadRequest("Invalid message format.");
        }

        // Validate message
        if (message == null || string.IsNullOrEmpty(message.Body) || string.IsNullOrEmpty(message.From))
            return BadRequest("Invalid message received.");

        // Respond with menu options
        string responseText = @"👋 Welcome to Naatukodi by GSR!  
        Please select an option by replying with a number:

        1️⃣ Register as a Farmer (రెజిస్ట్రేషన్)  
        2️⃣ Order Chicks (కోడి పిల్లల ఆర్డర్)  
        3️⃣ Request Vet Support (పశువైద్య సహాయం)  
        4️⃣ Sell Chickens (కోడి అమ్మకం)  
        5️⃣ Check Market Prices (మార్కెట్ ధరలు)";

        await _acsService.SendWhatsAppMessage(message.From, responseText);
        return Ok();
    }
}
