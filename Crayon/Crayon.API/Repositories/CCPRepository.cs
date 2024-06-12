using Crayon.API.Contracts;
using Crayon.API.Models.Domain;
using Crayon.API.Settings;
using Crayon.API.Util;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;

namespace Crayon.API.Repositories
{
    public class CCPRepository : ICCPRepository
    {
        private readonly CCPSettings CCPSettings;

        public CCPRepository(IOptions<CCPSettings> CCPSettings)
        {
            this.CCPSettings = Guard.AgainstNull(CCPSettings.Value, nameof(CCPSettings));
        }

        public async Task<IEnumerable<ServiceCCPModel>> GetPage(int pageNumber, int pageSize)
        {
            //SendHttpRequest to CCP API by using http client

            //mocked data
            return new List<ServiceCCPModel>()
            {
                new ServiceCCPModel()
                {
                     Id = Guid.NewGuid(),
                     Name = "MS Office"
                },
                new ServiceCCPModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "MS Excel"
                }
            };
        }

        public async Task<OrderSoftwareCCPModel> OrderSoftware(Guid id, Guid accountId)
        {
            // SendHttpRequest to CCP API by using http client, get order as a response

            return new OrderSoftwareCCPModel()
            {
                Id = Guid.NewGuid(),
                Name = "MS Office",
                Quantity = 5
            };
        }
    }
}
