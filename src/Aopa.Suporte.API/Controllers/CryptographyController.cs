using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Aopa.Suporte.Business.Application.Interfaces;
using System.IO;

namespace Aopa.Suporte.API.Controllers
{
    [ApiController]
    [Route("api/v1/criptografias")]
    public class CryptographyController : Controller
    {
        private readonly ICriptografiaService _service;

        public CryptographyController(ICriptografiaService service)
        {
            _service = service;
        }

        [Produces("text/plain")]
        [HttpPost("{password}/criptografar")]
        public async Task<IActionResult> EncryptAsync(string password)
        {
            try
            {
                string requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                var response = await _service.CriptografarAsync(password, requestBody);

                return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
            }
            catch (Exception exception)
            {
                return new ObjectResult(exception?.Message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        [Produces("text/plain")]
        [HttpPost("{password}/descriptografar")]
        public async Task<IActionResult> DecryptAsync(string password)
        {
            try
            {
                string requestBody = await new StreamReader(Request.Body).ReadToEndAsync();
                var response = await _service.DescriptografarAsync(password, requestBody);

                return new ObjectResult(response) { StatusCode = StatusCodes.Status200OK };
            }
            catch (Exception exception)
            {
                return new ObjectResult(exception?.Message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
