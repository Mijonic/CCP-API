using AutoMapper;
using Crayon.API.Contracts;
using Crayon.API.Models.Database;
using Crayon.API.Models.Domain;
using Crayon.API.Models.Dto;
using Crayon.API.Settings;
using Crayon.API.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Crayon.API.Services
{
    public class CrayonService : ICrayonService
    {
        private readonly CrayonDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ICCPRepository ccpRepository;
        private readonly IRedisRepository redisRepository;
        private readonly RedisSettings redisSettings;
        private readonly DatabaseSettings databaseSettings;
        private readonly CCPSettings ccpSettings;

        public CrayonService(CrayonDbContext dbContext, IMapper mapper, IOptions<DatabaseSettings> databaseSettings, IRedisRepository redisRepository, 
            IOptions<RedisSettings> redisSettings, ICCPRepository ccpRepository, IOptions<CCPSettings> ccpSettings)
        {
            this.dbContext = Guard.AgainstNull(dbContext, nameof(dbContext));
            this.mapper = Guard.AgainstNull(mapper, nameof(mapper));
            this.databaseSettings = Guard.AgainstNull(databaseSettings.Value, nameof(databaseSettings));
            this.redisRepository = Guard.AgainstNull(redisRepository, nameof(redisRepository));
            this.redisSettings = Guard.AgainstNull(redisSettings.Value, nameof(redisSettings));
            this.ccpRepository = Guard.AgainstNull(ccpRepository, nameof(ccpRepository));
            this.ccpSettings = Guard.AgainstNull(ccpSettings.Value, nameof(ccpSettings));
        }

        public async Task<AccountsPage> GetAccounts(Guid userId, int pageNumber)
        {

            var cacheKey = GetUserAccountsCacheKey(userId, pageNumber);
            var foundInCache = await redisRepository.GetFromCache<AccountsPage>(cacheKey);

            if(foundInCache != null)
            {
                return foundInCache;
            }


            var pagedData = dbContext.Accounts.Where(x => x.CustomerId == userId);
            var resourceCount = await pagedData.CountAsync();

            var results = await pagedData.Skip(pageNumber * databaseSettings.PageSizes.CustomerAccounts)
                                 .Take(databaseSettings.PageSizes.CustomerAccounts)
                                 .ToListAsync();

            var result =  new AccountsPage()
            {
                Accounts = mapper.Map<List<AccountDto>>(results),
                Total = resourceCount
            };

            await redisRepository.SaveToCache<AccountsPage>(cacheKey, result, redisSettings.CacheTime.UserAccounts);
        
            return result;
        }

        public async Task<IEnumerable<AvailableSoftwareLicenceDto>> GetAvailableServices(int pageNumber)
        {
            var availableService = await ccpRepository.GetPage(pageNumber, ccpSettings.PageSizes.AvailableServices);
            
            return mapper.Map<List<AvailableSoftwareLicenceDto>>(availableService);
        }

        public async Task<OrderSotwareDto> OrderSoftware(OrderSoftwareInputDto orderSoftwareInput)
        {
            var ccpResponse = await ccpRepository.OrderSoftware(orderSoftwareInput.SoftwareId, orderSoftwareInput.AccountId);

            var order = mapper.Map<SoftwareLicence>(ccpResponse);
            order.UpdateOrderedSoftwareLicence(orderSoftwareInput.Quantity, orderSoftwareInput.AccountId);

            dbContext.SoftwareLicences.Add(order);
            await dbContext.SaveChangesAsync();

            return mapper.Map<OrderSotwareDto>(order);

        }

        private string GetUserAccountsCacheKey(Guid userId, int pageNumber) => $"UserAccounts:{userId}-{pageNumber}";
        
    }
}
