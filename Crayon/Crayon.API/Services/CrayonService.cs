using AutoMapper;
using Crayon.API.Contracts;
using Crayon.API.Controllers;
using Crayon.API.Exceptions;
using Crayon.API.Models.Database;
using Crayon.API.Models.Domain;
using Crayon.API.Models.Dto;
using Crayon.API.Models.Dto.Input;
using Crayon.API.Settings;
using Crayon.API.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

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
        private readonly ILogger<CrayonService> logger;

        public CrayonService(CrayonDbContext dbContext, IMapper mapper, IOptions<DatabaseSettings> databaseSettings, IRedisRepository redisRepository,
            IOptions<RedisSettings> redisSettings, ICCPRepository ccpRepository, IOptions<CCPSettings> ccpSettings, ILogger<CrayonService> logger)
        {
            this.dbContext = Guard.AgainstNull(dbContext, nameof(dbContext));
            this.mapper = Guard.AgainstNull(mapper, nameof(mapper));
            this.databaseSettings = Guard.AgainstNull(databaseSettings.Value, nameof(databaseSettings));
            this.redisRepository = Guard.AgainstNull(redisRepository, nameof(redisRepository));
            this.redisSettings = Guard.AgainstNull(redisSettings.Value, nameof(redisSettings));
            this.ccpRepository = Guard.AgainstNull(ccpRepository, nameof(ccpRepository));
            this.ccpSettings = Guard.AgainstNull(ccpSettings.Value, nameof(ccpSettings));
            this.logger = Guard.AgainstNull(logger, nameof(logger));
        }



        public async Task<AccountsPage> GetAccounts(Guid userId, int pageNumber)
        {

            logger.LogInformation($"[{nameof(CrayonService)}::{nameof(GetAccounts)}] Getting customer accounts.");

            var cacheKey = GetUserAccountsCacheKey(userId, pageNumber);
            var foundInCache = await redisRepository.GetFromCache<AccountsPage>(cacheKey);

            if(foundInCache != null)
            {
                logger.LogInformation($"[{nameof(CrayonService)}::{nameof(GetAccounts)}] Accounts page found in cache {JsonSerializer.Serialize(foundInCache)}");
                return foundInCache;
            }


            logger.LogInformation($"[{nameof(CrayonService)}::{nameof(GetAccounts)}] Getting accounts page from database.");

            var pagedData = dbContext.Accounts.Where(x => x.CustomerId == userId);
            var resourceCount = await pagedData.CountAsync();

            var results = await pagedData.Skip(pageNumber * databaseSettings.PageSizes.CustomerAccounts)
                                 .Take(databaseSettings.PageSizes.CustomerAccounts)
                                 .ToListAsync();


            logger.LogInformation($"[{nameof(CrayonService)}::{nameof(GetAccounts)}] Completed getting data from database - {JsonSerializer.Serialize(results)}");

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

        public async Task<IEnumerable<SoftwareLicenceDto>> GetLicencesForAccount(Guid accountId, int pageNumber)
        {

            var pagedData = dbContext.SoftwareLicences.Where(x => x.AccountId == accountId).Include(x => x.Account);

            var results = await pagedData.Skip(pageNumber * databaseSettings.PageSizes.AccountLicences)
                                 .Take(databaseSettings.PageSizes.CustomerAccounts)
                                 .ToListAsync();

            return mapper.Map<List<SoftwareLicenceDto>>(results);

        }

        public async Task<SoftwareLicenceDto> ModifyQuantity(UpdateQuantityInputDto updateQuantityInput)
        {
            updateQuantityInput.Validate();

            var licence = await dbContext.SoftwareLicences.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == updateQuantityInput.LicenceId);

            if (licence == null)
            {
                throw new EntityNotFoundException($"Could not find licence with {updateQuantityInput.LicenceId}");
            }

            licence.Quantity = updateQuantityInput.Quantity;

            dbContext.SoftwareLicences.Update(licence);
            await dbContext.SaveChangesAsync();

            return mapper.Map<SoftwareLicenceDto>(licence);

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

        public async Task<SoftwareLicenceDto> CancelLicence(Guid id)
        {
            var licence = await dbContext.SoftwareLicences.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == id);

            if (licence == null)
            {
                throw new EntityNotFoundException($"Could not find licence with {id}");
            }

            licence.State = Models.Enums.SoftwareLicenceState.Canceled;

            dbContext.SoftwareLicences.Update(licence);
            await dbContext.SaveChangesAsync();

            return mapper.Map<SoftwareLicenceDto>(licence);
        }

        private string GetUserAccountsCacheKey(Guid userId, int pageNumber) => $"UserAccounts:{userId}-{pageNumber}";

        public async Task<SoftwareLicenceDto> ExtendLicence(Guid id, DateTimeOffset extendedDate)
        {
            var licence = await dbContext.SoftwareLicences.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == id);

            if (licence == null)
            {
                throw new EntityNotFoundException($"Could not find licence with {id}");
            }

            if(extendedDate <= licence.SubscriptionEndDate)
            {
                throw new InvalidDateTimeException("Cannot reduce the licence expiration date.");
            }

            licence.SubscriptionEndDate = extendedDate;

            dbContext.SoftwareLicences.Update(licence);
            await dbContext.SaveChangesAsync();

            return mapper.Map<SoftwareLicenceDto>(licence);
        }
    }
}
