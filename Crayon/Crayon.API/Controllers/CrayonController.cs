using Crayon.API.Contracts;
using Crayon.API.Exceptions;
using Crayon.API.Models.Dto;
using Crayon.API.Models.Dto.Input;
using Crayon.API.Util;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Crayon.API.Controllers
{
    [Route("api/crayon/v1")]
    [ApiController]
    public class CrayonController : ControllerBase
    {
        private readonly ICrayonService service;

        public CrayonController(ICrayonService service)
        {
            this.service = Guard.AgainstNull(service, nameof(service));
        }

        [HttpGet("accounts/{userId}/page/{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AccountsPage))]
        public async Task<IActionResult> GetUserAccounts(Guid userId, int pageNumber)
        {
            try
            {
                var accounts = await service.GetAccounts(userId, pageNumber);
                return Ok(accounts);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("available-services/page/{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AvailableSoftwareLicenceDto>))]
        public async Task<IActionResult> GetAvailableServices(int pageNumber)
        {
            var accounts = await service.GetAvailableServices(pageNumber);
            return Ok(accounts);
        }

        [HttpGet("licences/{accountId}/page/{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SoftwareLicenceDto>))]
        public async Task<IActionResult> GetLicences(Guid accountId, int pageNumber)
        {
            var accounts = await service.GetLicencesForAccount(accountId, pageNumber);
            return Ok(accounts);
        }

        [HttpPost("order")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> OrderSoftware([FromBody]OrderSoftwareInputDto orderSoftwareInput)
        {
            var res = await service.OrderSoftware(orderSoftwareInput);
            return Ok(res);
        }

        [HttpPatch("order-quantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityInputDto updateQuantityInput)
        {
            try
            {
                return Ok(await service.ModifyQuantity(updateQuantityInput));

            }
            catch(EntityNotFoundException notFound)
            {
                return NotFound(notFound.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPatch("cancel-licence")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Cancel([FromBody] Guid licenceId)
        {
            try
            {
                return Ok(await service.CancelLicence(licenceId));

            }
            catch (EntityNotFoundException notFound)
            {
                return NotFound(notFound.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpPatch("extend-licence")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Cancel([FromBody] ExtendLicenceInputDto extendLicenceInput)
        {
            try
            {
                return Ok(await service.ExtendLicence(extendLicenceInput.LicenceId, extendLicenceInput.EndDate));

            }
            catch (EntityNotFoundException notFound)
            {
                return NotFound(notFound.Message);
            }
            catch (InvalidDateTimeException invalidExtendDate)
            {
                return BadRequest(invalidExtendDate.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
