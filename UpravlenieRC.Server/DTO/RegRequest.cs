namespace UpravlenieRC.Server.DTO
{
    public record RegRequest(string Login, string Password, string INN, string message);
    public record ExistingRegRequest(int id, string PartnerName, string Login, string Description, DateTime CreatedAt);
}
