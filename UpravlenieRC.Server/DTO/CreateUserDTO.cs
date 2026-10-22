namespace UpravlenieRC.Server.DTO
{
    public record CreateUser(string Login, string Password, string? Phone, string? Email, string Role);
}
