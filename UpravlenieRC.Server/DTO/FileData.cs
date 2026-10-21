namespace UpravlenieRC.Server.DTO
{
    public record FileData(byte[] content, string? filename, string type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
}
