namespace DTO.Identity.Responses
{
	public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
}