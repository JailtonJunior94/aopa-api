using System.Threading.Tasks;

namespace Aopa.Suporte.Business.Application.Interfaces
{
    public interface ICriptografiaService
    {
        Task<string> CriptografarAsync(string password, string payload);
        Task<string> DescriptografarAsync(string password, string payload);
    }
}
