using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shared.Infrastructure.Configs.Payment
{
    public class VnPayConfigSetup : IConfigureOptions<VnPayConfigSetup>
    {
        private readonly IConfiguration _configuration;

        public VnPayConfigSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(VnPayConfigSetup options)
        {
            _configuration.GetSection("VNPay").Bind(options);
        }
    }
}
