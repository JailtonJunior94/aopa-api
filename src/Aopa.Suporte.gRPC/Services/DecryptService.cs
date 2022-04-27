using Grpc.Core;
using Aopa.Suporte.gRPC.Crypt;

namespace Aopa.Suporte.gRPC.Services
{
    public class DecryptService : Decrypt.DecryptBase
    {
        public override Task<DecryptResponse> Decrypt(DecryptRequest request, ServerCallContext context)
        {
            string response = CryptExtension.Decrypt(request.Body, request.Key);
            return Task.FromResult(new DecryptResponse { Data = response });
        }

        public override Task<EncryptResponse> Encrypt(EncryptRequest request, ServerCallContext context)
        {
            string response = CryptExtension.Encrypt(request.Body, request.Key);
            return Task.FromResult(new EncryptResponse { Data = response });
        }
    }
}