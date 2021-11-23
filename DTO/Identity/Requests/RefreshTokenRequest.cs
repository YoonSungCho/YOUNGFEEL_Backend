namespace DTO.Identity.Requests
{
    public record RefreshTokenRequest(string Token, string RefreshToken);
}