using System.Threading.Tasks;
using KvizAPI.Application.DTO;

namespace KvizAPI.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResultDto> LoginAsync(LoginRequestDto request);
    }
}
