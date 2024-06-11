using Crayon.API.Contracts;
using Crayon.API.Models.Dto;
using Crayon.API.Util;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Crayon.API.Controllers
{
    [Route("api/crayon")]
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
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }

         
        }
    }
}
