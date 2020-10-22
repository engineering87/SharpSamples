// (c) 2020 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService.Services
{
    public class EchoService : Echo.EchoBase
    {
        private readonly ILogger<EchoService> _logger;
        public EchoService(ILogger<EchoService> logger)
        {
            _logger = logger;
        }

        public override Task<EchoReply> SendEcho(EchoRequest request, ServerCallContext context)
        {
            return Task.FromResult(new EchoReply
            {
                Message = request.Message
            });
        }
    }
}
