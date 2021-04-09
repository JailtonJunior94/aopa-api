using System.Threading.Tasks;
using Aopa.Suporte.Business.Infra.Security;
using Aopa.Suporte.Business.Application.Interfaces;

namespace Aopa.Suporte.Business.Application.Services
{
    public class CriptografiaService : ICriptografiaService
    {
        public async Task<string> CriptografarAsync(string password, string payload)
        {
            return await Task.Run(() => payload.Encrypt(password));
        }

        public async Task<string> DescriptografarAsync(string password, string payload)
        {
            return await Task.Run(() => payload.Decrypt(password));
        }
    }
}
