using Application.Abstractions.Services;
using Application.Wrapper;
using DTO.Identity.Requests;
using DTO.Identity.Responses;
using System.Threading.Tasks;

namespace Application.Abstractions.Services.Identity
{
    public interface ITokenService : ITransientService
    {
        Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress);

        Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
    }
}