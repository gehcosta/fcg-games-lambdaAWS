using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaSendEmail;

public class EmailRequest
{
    public string From { get; set; } = string.Empty;
    public List<string> To { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsHtml { get; set; } = false;
}

public class Function
{
    private readonly AmazonSimpleEmailServiceClient _sesClient;

    public Function()
    {
        _sesClient = new AmazonSimpleEmailServiceClient(Amazon.RegionEndpoint.USEast1);
    }

    public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            // Validar se o body existe
            if (string.IsNullOrEmpty(request.Body))
            {
                return CreateResponse(400, new { error = "Request body is required" });
            }

            // Deserializar o body da requisição
            var emailRequest = JsonSerializer.Deserialize<EmailRequest>(request.Body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (emailRequest == null)
            {
                return CreateResponse(400, new { error = "Invalid request body" });
            }

            // Validar campos obrigatórios
            if (string.IsNullOrEmpty(emailRequest.From))
            {
                return CreateResponse(400, new { error = "From address is required" });
            }

            if (emailRequest.To == null || emailRequest.To.Count == 0)
            {
                return CreateResponse(400, new { error = "At least one recipient is required" });
            }

            if (string.IsNullOrEmpty(emailRequest.Subject))
            {
                return CreateResponse(400, new { error = "Subject is required" });
            }

            if (string.IsNullOrEmpty(emailRequest.Body))
            {
                return CreateResponse(400, new { error = "Body is required" });
            }

            // Criar a requisição para o SES
            var sendRequest = new SendEmailRequest
            {
                Source = emailRequest.From,
                Destination = new Destination
                {
                    ToAddresses = emailRequest.To
                },
                Message = new Message
                {
                    Subject = new Content(emailRequest.Subject),
                    Body = new Body()
                }
            };

            // Definir o tipo de body (HTML ou texto)
            if (emailRequest.IsHtml)
            {
                sendRequest.Message.Body.Html = new Content(emailRequest.Body);
            }
            else
            {
                sendRequest.Message.Body.Text = new Content(emailRequest.Body);
            }

            // Enviar o email
            var response = await _sesClient.SendEmailAsync(sendRequest);
            
            context.Logger.LogInformation($"Email sent successfully. Message ID: {response.MessageId}");
            
            return CreateResponse(200, new 
            { 
                message = "Email sent successfully",
                messageId = response.MessageId,
                from = emailRequest.From,
                to = emailRequest.To
            });
        }
        catch (JsonException ex)
        {
            context.Logger.LogError($"JSON parsing error: {ex.Message}");
            return CreateResponse(400, new { error = "Invalid JSON format", details = ex.Message });
        }
        catch (AmazonSimpleEmailServiceException ex)
        {
            context.Logger.LogError($"SES error: {ex.Message}");
            return CreateResponse(500, new { error = "Failed to send email", details = ex.Message });
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Unexpected error: {ex.Message}");
            return CreateResponse(500, new { error = "Internal server error", details = ex.Message });
        }
    }

    private APIGatewayProxyResponse CreateResponse(int statusCode, object body)
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = statusCode,
            Body = JsonSerializer.Serialize(body),
            Headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                { "Access-Control-Allow-Origin", "*" },
                { "Access-Control-Allow-Headers", "Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token" },
                { "Access-Control-Allow-Methods", "POST,OPTIONS" }
            }
        };
    }
}
