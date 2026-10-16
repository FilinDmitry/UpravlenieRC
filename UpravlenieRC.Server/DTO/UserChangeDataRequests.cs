namespace UpravlenieRC.Server.DTO
{
    public record ChangePasswordRequest(string OldPassword, string NewPassword);
    public record ChangeEmailRequest(string Password, string NewEmail);
    public record ChangePhoneRequest(string OldPassword, string NewPhone);
}
