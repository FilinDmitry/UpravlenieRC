using System.Net;
namespace UpravlenieRC.Server.DTO
{

     public record ResponseWithMessage(HttpStatusCode Code, string message);
    public record AuthServiceController(HttpStatusCode Code, string? message, string? token);

}
